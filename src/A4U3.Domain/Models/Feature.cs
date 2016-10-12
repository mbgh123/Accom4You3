using System.ComponentModel.DataAnnotations;

namespace A4U3.Domain.Models
{
    /// <summary>
    /// Holds feature descriptions to be used in bullet points.
    /// </summary>
    public class Feature
    {
        public int FeatureId { get; set; }
        public int PropertyId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}