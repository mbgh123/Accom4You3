using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.Spatial;  // 1/12/15 not yet in EF7
using System.Linq;

namespace A4U3.Domain.Models
{
    public class Property
    {
        public int PropertyId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy ddd}")]
        [Display(Name = "Next Availability")]
        public DateTime? NextAvailability { get; set; }
        
        [Required]
        public string Address { get; set; }

        public string PostCode { get; set; }

        //public DbGeography Location { get; set; }  // Not yet in EF7 as of feb 2016
        //I innitially used decimal, but discovered a C# decimal fild was mapped
        //to a SQL server decimal type with only 2 decimals places.
        //SQL server decimal precision can be defined, but EF7 is in a state of flux
        //Lets just store it as a string, nad convert later.
        public string Latitude { get; set; }
        public string Longitude { get; set; }


        public Furnishing Furnishing { get; set; }
        
        [Required]
        public string Description { get; set; }

        public int Bedrooms { get; set; }
        
        [Required]       
        public Decimal RatePCM { get; set; }
        
        public ICollection<Feature> Features { get; set; }        
        public ICollection<Picture> Pictures { get; set; }

        /// <summary>
        /// Shown on website
        /// </summary>
        public bool isEnabled { get; set; }

        /// <summary>
        /// A maximum of 8 pictures is allowed
        /// </summary>
        [NotMapped]
        public bool hasMaxPictures => Pictures != null && Pictures.Count() > 7; 

    }
}