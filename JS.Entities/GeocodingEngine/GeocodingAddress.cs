namespace JS.Entities.GeocodingEngine
{
    /// <summary>
    /// Geocodes a street address using google
    /// </summary>
    public class GeocodeAddress
    {
        /// <summary>
        /// 
        /// </summary>
        public string FormattedAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GeocodeAddress()
        {
            FormattedAddress = default(string);
            AddressLine1 = default(string);
            City = default(string);
            StateCode = default(string);
            Zip = default(string);
        }

    }
}
