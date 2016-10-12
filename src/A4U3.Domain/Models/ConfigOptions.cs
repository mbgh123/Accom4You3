using System;

namespace A4U3.Domain.Models
{
    public class ConfigOptions
    {
        public string Robots { get; set; }
        public bool ShowSearch { get; set; }

        /// <summary>
        /// If this is set, then instad of using the connection string to a real DB
        /// the EF InMemomry backing store will be used. Mostly used by unit tests.
        /// </summary>
        public bool UseInMemoryStore { get; set; }
        public string ConnectionString { get; set; }

        public string GoogleMapKey2013 { get; set; }
        public string GoogleMapKeyA4U2AzureDomain { get; set; }
        /// <summary>
        /// Created google map key for www.accom4you.com
        /// </summary>
        public string GoogleMapKey2016 { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTel { get; set; }


    }
}
