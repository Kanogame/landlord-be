
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public class Personal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
	[StringLength(255)]
        public string Name { get; set; }

        [Required]
	[StringLength(255)]
        public string Surname { get; set; }

	[Required]
	[StringLength(255)]
        public string Patronym { get; set; }

        [NotMapped]
        public string FullName { get {
		return $"{Name} {Surname} {Patronym}";
	    }
	}
    }
}
