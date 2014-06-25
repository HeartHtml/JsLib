namespace JS.Entities.GeocodingEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class GeocodeObject
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GeocodeAddress Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RawXmlResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GeocodeObject()
        {
            Latitude = default(decimal);
            Longitude = default(decimal);
            Address = new GeocodeAddress();
            RawXmlResult = default(string);
            IsValid = default(bool);
        }


    }
}
