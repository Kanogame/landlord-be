using System.ComponentModel.DataAnnotations;
using landlord_be.Models;

public class PropertyAttributeTemplate
{
    [Key]
    public int Id { get; set; }

    [Required]
    public PropertyType PropertyType { get; set; }

    [Required]
    [StringLength(100)]
    public string AttributeName { get; set; } = "";

    [Required]
    public PropertyAttributeType AttributeType { get; set; }

    public string? PossibleValues { get; set; }
    public bool IsRequired { get; set; } = false;
    public string? DefaultValue { get; set; }
    public string? Description { get; set; }
}
