using System.Collections.Generic;
//using System.Data.Entity.Spatial;
using A4U3.Domain.Models;
using A4U3.Web.Helpers;

namespace A4U3.Web.Models.ViewModel
{
    public class PropertyVM
    {
        private int _shortDescriptionLenth;

        public PropertyVM(int shortDescriptionLength = 60)
        {
            _shortDescriptionLenth = shortDescriptionLength;
        }

        public bool isEnabled { get; set; }

        public int PropertyId { get; set; }
        public string Address { get; set; }

        public string PostCode { get; set; }
        //public DbGeography Location { get; set; }
        public GeoLocation Location { get; set; }

        public string Furnishing { get; set; }
        public string Description { get; set; }
        public string DescriptionWithBreaks => Description.AddBreaks(); //C#6
        
        public string DescriptionShort => Description.Shorten(_shortDescriptionLenth); //C#6

        public decimal RatePCM { get; set; }

        public int Bedrooms { get; set; }
        public ICollection<Feature> Features { get; set; }
        public int FeatureToHighlight { get; set; }
        public int PictureToHighlight { get; set; }
        public ICollection<Picture> Pictures { get; set; }

        public int PictureCount => Pictures.Count;
    }
}