using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using landlord_be.Data;
using landlord_be.Models;
using landlord_be.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

namespace landlord_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly Random rd;

    public UserController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        rd = new Random();
    }

    [HttpPost("verification_number")]
    public async Task<ActionResult<VerificationNumberRespDTO>> VerificationNumber(
        VerificationNumberReqDTO req
    )
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(req.Number));
        string numberHash = BitConverter.ToString(hash).Replace("-", String.Empty);

        int code = rd.Next(100000, 999999);
        Console.WriteLine(code);

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

    [HttpPost("verification_code")]
    public async Task<ActionResult<VerificationCodeRespDTO>> VerificationCode(
        VerificationCodeReqDTO req
    )
    {
        bool codeExists = await _context.VerificationPendings.AnyAsync(v =>
            v.Id == req.VerificationId
        );
        if (!codeExists)
        {
            return BadRequest(new BadRequestMessage("No such verification"));
        }

        var record = await _context.VerificationPendings.FirstAsync(v =>
            v.Id == req.VerificationId
        );
        if (record.VerificationCode != req.Code)
        {
            return BadRequest(new BadRequestMessage("No such verification"));
        }

        record.Verified = true;
        _context.VerificationPendings.Update(record);
        await _context.SaveChangesAsync();

        return Ok(new VerificationCodeRespDTO { Success = true });
    }

    [HttpPost("verification_personal")]
    public async Task<ActionResult<VerificationPersonalRespDTO>> VerificationPersonal(
        VerificationPersonalReqDTO req
    )
    {
        bool codeExists = await _context.VerificationPendings.AnyAsync(v =>
            v.Id == req.VerificationId
        );
        if (!codeExists)
        {
            return BadRequest(new BadRequestMessage("No such verification"));
        }

        var record = await _context.VerificationPendings.FirstAsync(v =>
            v.Id == req.VerificationId
        );

        if (!record.Verified)
        {
            return BadRequest(new BadRequestMessage("Verification not completed"));
        }

        var personal = new Personal
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Patronym = req.Patronym,
        };

        _context.Personals.Add(personal);
        await _context.SaveChangesAsync();

        DateTime time = DateTime.UtcNow;

        var newUser = new User
        {
            PersonalId = personal.Id,
            NumberHash = record.NumberHash,
            Email = req.Email,
            Token = "null",
            RegisterDate = time,
            UpdateDate = time,
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
            new(ClaimTypes.Name, req.FirstName),
            new(ClaimTypes.Email, req.Email),
            new(ClaimTypes.Role, "User"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = GetToken(authClaims);
        var tokenHandler = new JwtSecurityTokenHandler();

        newUser.Token = tokenHandler.WriteToken(token);
        _context.Users.Update(newUser);

        _context.VerificationPendings.Remove(record);
        await _context.SaveChangesAsync();

        return Ok(
            new VerificationPersonalRespDTO
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
            }
        );
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(
                authSigningKey,
                SecurityAlgorithms.HmacSha256
            )
        );

        return token;
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out int userId))
        {
            return userId;
        }
        return null;
    }
}
