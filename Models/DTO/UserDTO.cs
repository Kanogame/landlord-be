using System.ComponentModel.DataAnnotations;
using landlord_be.Models.DTO;

public class EditUserReqDTO
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = "";

    [StringLength(50, ErrorMessage = "Patronym cannot exceed 50 characters")]
    public string? Patronym { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string? Email { get; set; }
}

public class EditUserRespDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public UserInfoDTO? User { get; set; }
}

public class GetCurrentUserRespDTO
{
    public bool Success { get; set; }
    public CurrentUserDTO? User { get; set; }
}

public class CurrentUserDTO
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public DateTime RegisterDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string ProfileLink { get; set; } = "";
    public PersonalDTO Personal { get; set; } = null!;
    public UserStatisticsDTO Statistics { get; set; } = null!;
}

public class PersonalDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Patronym { get; set; } = "";
    public string FullName { get; set; } = "";
}

public class UserStatisticsDTO
{
    public int ActivePropertiesCount { get; set; }
    public int TotalPropertiesCount { get; set; }
    public int RentedSoldRentEndingCount { get; set; }
    public string Experience { get; set; } = "";
}
