using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using landlord_be.Data;
using landlord_be.Models;
using landlord_be.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace landlord_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ApplicationDbContext context, IConfiguration configuration)
    : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly Random rd = new Random();

    [HttpPost("get_user_info")]
    public async Task<ActionResult<GetUserInfoRespDTO>> GetUserInfo(GetUserInfoReqDTO req)
    {
        var user = await _context
            .Users.Include(u => u.Personal)
            .FirstOrDefaultAsync(u => u.Id == req.UserId);

        if (user == null)
        {
            return BadRequest(new BadRequestMessage($"User with id {req.UserId} not found"));
        }

        // Get property counts by status
        var activePropertiesCount = await _context.Properties.CountAsync(p =>
            p.OwnerId == req.UserId && p.Status == PropertyStatus.Active
        );

        var rentedSoldRentEndingCount = await _context.Properties.CountAsync(p =>
            p.OwnerId == req.UserId
            && (
                p.Status == PropertyStatus.Rented
                || p.Status == PropertyStatus.Sold
                || p.Status == PropertyStatus.RentEnding
            )
        );

        // Calculate time since registration
        var timeSinceRegistration = DateTime.UtcNow - user.RegisterDate;
        var experienceYears = (int)(timeSinceRegistration.TotalDays / 365.25);
        var experienceMonths = (int)((timeSinceRegistration.TotalDays % 365.25) / 30.44);

        string experience;
        if (experienceYears > 0)
        {
            experience =
                experienceMonths > 0
                    ? $"{experienceYears} лет {experienceMonths} месяцев"
                    : $"{experienceYears} лет";
        }
        else
        {
            experience = experienceMonths > 0 ? $"{experienceMonths} месяцев" : "Менее месяца";
        }

        var fullName = $"{user.Personal?.FirstName} {user.Personal?.LastName}".Trim();

        return Ok(
            new GetUserInfoRespDTO
            {
                FullName = fullName,
                ActivePropertiesCount = activePropertiesCount,
                RentedSoldRentEndingCount = rentedSoldRentEndingCount,
                Experience = experience,
            }
        );
    }

    // REGISTRATION ENDPOINTS
    [HttpPost("register/send-code")]
    public async Task<ActionResult<VerificationNumberRespDTO>> RegisterSendCode(
        VerificationNumberReqDTO req
    )
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(req.Number))
        {
            return BadRequest(new BadRequestMessage("Phone number is required"));
        }

        // Basic phone number validation (you might want to add more sophisticated validation)
        if (req.Number.Length < 10)
        {
            return BadRequest(new BadRequestMessage("Invalid phone number format"));
        }

        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(req.Number));
        string numberHash = BitConverter.ToString(hash).Replace("-", String.Empty);

        // Check if user already exists with this phone number
        bool userExists = await _context.Users.AnyAsync(u => u.NumberHash == numberHash);
        if (userExists)
        {
            return BadRequest(new BadRequestMessage("User with this phone number already exists"));
        }

        // Remove any existing pending verifications for this number (cleanup old attempts)
        var existingPending = _context.VerificationPendings.Where(v => v.NumberHash == numberHash);
        _context.VerificationPendings.RemoveRange(existingPending);

        int code = rd.Next(100000, 999999);
        Console.WriteLine($"Registration code for {req.Number}: {code}");

        var record = new VerificationPending
        {
            VerificationCode = code,
            Verified = false,
            NumberHash = numberHash,
        };

        _context.VerificationPendings.Add(record);
        await _context.SaveChangesAsync();

        return Ok(new VerificationNumberRespDTO { Success = true, VerificationId = record.Id });
    }

    [HttpPost("register/verify-code")]
    public async Task<ActionResult<VerificationCodeRespDTO>> RegisterVerifyCode(
        VerificationCodeReqDTO req
    )
    {
        var record = await _context.VerificationPendings.FirstOrDefaultAsync(v =>
            v.Id == req.VerificationId
        );

        if (record == null)
        {
            return BadRequest(new BadRequestMessage("Invalid verification ID"));
        }

        if (record.VerificationCode != req.Code)
        {
            return BadRequest(new BadRequestMessage("Invalid verification code"));
        }

        // Check if verification is already completed
        if (record.Verified)
        {
            return BadRequest(new BadRequestMessage("Verification code already used"));
        }

        // Double-check that user doesn't exist (race condition protection)
        bool userExists = await _context.Users.AnyAsync(u => u.NumberHash == record.NumberHash);
        if (userExists)
        {
            _context.VerificationPendings.Remove(record);
            await _context.SaveChangesAsync();
            return BadRequest(new BadRequestMessage("User with this phone number already exists"));
        }

        record.Verified = true;
        _context.VerificationPendings.Update(record);
        await _context.SaveChangesAsync();

        return Ok(new VerificationCodeRespDTO { Success = true });
    }

    [HttpPost("register/complete")]
    public async Task<ActionResult<VerificationPersonalRespDTO>> RegisterComplete(
        VerificationPersonalReqDTO req
    )
    {
        // Validate input
        if (
            string.IsNullOrWhiteSpace(req.FirstName)
            || string.IsNullOrWhiteSpace(req.LastName)
            || string.IsNullOrWhiteSpace(req.Email)
        )
        {
            return BadRequest(
                new BadRequestMessage("First name, last name, and email are required")
            );
        }

        // Basic email validation
        if (!IsValidEmail(req.Email))
        {
            return BadRequest(new BadRequestMessage("Invalid email format"));
        }

        var record = await _context.VerificationPendings.FirstOrDefaultAsync(v =>
            v.Id == req.VerificationId
        );

        if (record == null)
        {
            return BadRequest(new BadRequestMessage("Invalid verification ID"));
        }

        if (!record.Verified)
        {
            return BadRequest(new BadRequestMessage("Phone number not verified"));
        }

        // Check if email already exists (case-insensitive)
        bool emailExists = await _context.Users.AnyAsync(u =>
            u.Email.ToLower() == req.Email.ToLower()
        );
        if (emailExists)
        {
            return BadRequest(new BadRequestMessage("User with this email already exists"));
        }

        // Double-check that user doesn't exist with this phone number (race condition protection)
        bool userExists = await _context.Users.AnyAsync(u => u.NumberHash == record.NumberHash);
        if (userExists)
        {
            _context.VerificationPendings.Remove(record);
            await _context.SaveChangesAsync();
            return BadRequest(new BadRequestMessage("User with this phone number already exists"));
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var personal = new Personal
            {
                FirstName = req.FirstName.Trim(),
                LastName = req.LastName.Trim(),
                Patronym = req.Patronym?.Trim() ?? "",
            };

            _context.Personals.Add(personal);
            await _context.SaveChangesAsync();

            DateTime time = DateTime.UtcNow;

            var newUser = new User
            {
                PersonalId = personal.Id,
                NumberHash = record.NumberHash,
                Email = req.Email.ToLower().Trim(), // Store email in lowercase
                Token = "null",
                RegisterDate = time,
                UpdateDate = time,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(newUser, req.FirstName, req.Email);

            newUser.Token = token;
            _context.Users.Update(newUser);

            _context.VerificationPendings.Remove(record);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new VerificationPersonalRespDTO { Success = true, Token = token });
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new BadRequestMessage("Registration failed. Please try again."));
        }
    }

    // LOGIN ENDPOINTS
    [HttpPost("login/send-code")]
    public async Task<ActionResult<LoginSendCodeRespDTO>> LoginSendCode(LoginSendCodeReqDTO req)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(req.Number))
        {
            return BadRequest(new BadRequestMessage("Phone number is required"));
        }

        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(req.Number));
        string numberHash = BitConverter.ToString(hash).Replace("-", String.Empty);

        // Check if user exists
        var user = await _context
            .Users.Include(u => u.Personal)
            .FirstOrDefaultAsync(u => u.NumberHash == numberHash);

        if (user == null)
        {
            return BadRequest(new BadRequestMessage("User with this phone number not found"));
        }

        int code = rd.Next(100000, 999999);
        Console.WriteLine($"Login code for {req.Number}: {code}");

        // Remove any existing verification records for this user
        var existingRecords = _context.VerificationPendings.Where(v => v.NumberHash == numberHash);
        _context.VerificationPendings.RemoveRange(existingRecords);

        var record = new VerificationPending
        {
            VerificationCode = code,
            Verified = false,
            NumberHash = numberHash,
        };

        _context.VerificationPendings.Add(record);
        await _context.SaveChangesAsync();

        return Ok(new LoginSendCodeRespDTO { Success = true, VerificationId = record.Id });
    }

    [HttpPost("login/verify-code")]
    public async Task<ActionResult<LoginVerifyCodeRespDTO>> LoginVerifyCode(
        LoginVerifyCodeReqDTO req
    )
    {
        var record = await _context.VerificationPendings.FirstOrDefaultAsync(v =>
            v.Id == req.VerificationId
        );

        if (record == null)
        {
            return BadRequest(new BadRequestMessage("Invalid verification ID"));
        }

        if (record.VerificationCode != req.Code)
        {
            return BadRequest(new BadRequestMessage("Invalid verification code"));
        }

        // Get user data
        var user = await _context
            .Users.Include(u => u.Personal)
            .FirstOrDefaultAsync(u => u.NumberHash == record.NumberHash);

        if (user == null)
        {
            _context.VerificationPendings.Remove(record);
            await _context.SaveChangesAsync();
            return BadRequest(new BadRequestMessage("User not found"));
        }

        var token = GenerateJwtToken(user, user.Personal?.FirstName ?? "", user.Email);

        user.Token = token;
        user.UpdateDate = DateTime.UtcNow;
        _context.Users.Update(user);

        _context.VerificationPendings.Remove(record);
        await _context.SaveChangesAsync();

        return Ok(
            new LoginVerifyCodeRespDTO
            {
                Success = true,
                Token = token,
                User = new UserInfoDTO
                {
                    Id = user.Id,
                    FirstName = user.Personal?.FirstName ?? "",
                    LastName = user.Personal?.LastName ?? "",
                    Patronym = user.Personal?.Patronym ?? "",
                    Email = user.Email,
                },
            }
        );
    }

    private string GenerateJwtToken(User user, string firstName, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ?? "");

        // Get expiration time from configuration (default to 60 minutes if not specified)
        var expirationMinutes = _configuration.GetValue<int>("JWT:TokenExpirationMinutes", 60);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, firstName),
                    new Claim(ClaimTypes.Email, email),
                    new Claim("userId", user.Id.ToString()),
                ]
            ),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = _configuration["JWT:ValidIssuer"],
            Audience = _configuration["JWT:ValidAudience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
