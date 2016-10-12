using System.ComponentModel.DataAnnotations;

namespace A4U3.Domain.Models
{
    public class Picture
    {
        public int PictureId { get; set; }
        public int PropertyId { get; set; }

        /// <summary>
        /// For use in img title property
        /// </summary>
        [Required]
        public string Description { get; set; }

        public string OriginalName { get; set; }
        /// <summary>
        /// Combination of propertyId, original name and  a guid to make it unique
        /// </summary>
        public string UniqueName { get; set; }
        public string ThumbName { get; set; }

    }
}