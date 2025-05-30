using landlord_be.Models;

namespace landlord_be.Models.DTO
{
    public class PropertySearchAttributeDTO
    {
        public string AttributeName { get; set; } = "";
        public PropertyAttributeType AttributeType { get; set; }
        public List<string> PossibleValues { get; set; } = new List<string>();
    }
}
