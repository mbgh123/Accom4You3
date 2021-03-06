﻿using System.ComponentModel;

namespace A4U3.Domain.Models
{
    public enum Furnishing 
    {
        [Description("Unfurnished")]
        Unfurnished = 0,
        
        [Description("Furnished")]
        Furnished = 1,

        [Description("Part Furnished")] 
        PartFurnished = 2
    }

    public enum Bedrooms
    {
        [Description("Bedrooms: Any")]
        Any = 0,
        [Description("1 bedroom")]
        OneOnly = 1,
        [Description("2+ bedrooms")]
        TwoPlus = 2,
        [Description("3+ bedrooms")]
        ThreePlus = 3
    }

    public enum SortOrder
    {
        [Description("Default sort order")]
        Default = 0,
        [Description("Price lowest first")]
        PriceLowest = 1,
        [Description("Price highest first")]
        PriceHighest = 2
    }
}