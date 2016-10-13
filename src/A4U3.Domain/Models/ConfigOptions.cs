using System;

namespace A4U3.Domain.Models
{
    public class ConfigOptions
    {
        public string Robots { get; set; }
        public bool ShowSearch { get; set; }
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
