using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace landlord_be.Models
{
    public class PropertyAttribute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        [Required]
        [StringLength(100)]
        public string AttributeName { get; set; } = "";

        [StringLength(500)]
        public string AttributeValue { get; set; } = "";

        [Required]
        public PropertyAttributeType AttributeType { get; set; }

        public bool IsSearchable { get; set; } = true;

        // Relations
        public Property? Property { get; set; }
    }

    public enum PropertyAttributeType
    {
        Text,
        Number,
        Boolean,
    }
}
