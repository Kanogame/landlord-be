
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models {
    public class Property {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        // Owner
        [Required]
        [ForeignKey("User")]
        public int OwnerId {get; set;}

        // Property name
        [Required]
        [StringLength(255)]
        public string Name {get; set;}

        public string Desc {get; set;}

        [Required]
        [StringLength(255)]
        public string Address {get; set;}

        [Required]
        public int Area {get; set;}

        public int ImageLinkId {get; set;}

        // relations
        public User User {get; set;}
    }
}