using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models {
    public class ImageLink {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]
        public int PropertyId {get; set;}

        [Required]
        [StringLength(255)]
        public string Link {get; set;}
    }
}