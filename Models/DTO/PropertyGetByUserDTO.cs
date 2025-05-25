using System.ComponentModel.DataAnnotations;

namespace landlord_be.Models.DTO {
    public class PropertyGetByUserDTO {
        [Required(ErrorMessage ="ID is required")]
        public int UserId {get; set;}
    }
}