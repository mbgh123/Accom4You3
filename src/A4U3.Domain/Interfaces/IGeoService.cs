using A4U3.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4U3.Domain.Interfaces
{
    public interface IGeoService
    {
        GeoLocation GetGeoPoint(string postcode);
    }
}
