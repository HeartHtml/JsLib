using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using JS.Entities.ExtensionMethods;
using JS.Entities.GeocodingEngine;

namespace JS.Business.GeocodingEngine
{
    /// <summary>
    /// Encapsulates all logic for geocoding functionality
    /// </summary>
    public static class GeocodeManager
    {
        private static string GeocodeUrlTemplate
        {
            get
            {
                return "http://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false";
            }
        }

        private static string ReverseGeocodeUrlTemplate
        {
            get
            {
                return "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0}&sensor=false";
            }
        }

        /// <summary>
        /// Geocodes a complete street address
        /// </summary>
        /// <param name="fullAddress"></param>
        /// <returns>A Geocode object</returns>
        public static GeocodeObject Geocode(string fullAddress)
        {
            return Parse(fullAddress);
        }

        /// <summary>
        /// Geocodes a street address as components
        /// </summary>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <returns>A Geocode object</returns>
        public static GeocodeObject Geocode(string street,
                                                   string city,
                                                   string state,
                                                   string zip)
        {
            string fullAddress = string.Format("{0} {1},{2} {3}", street, city, state, zip).TrimSafely();

            return Parse(fullAddress);
        }

        /// <summary>
        /// Converts latitude and longitude into a human readable string
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>A Geocode object</returns>
        public static GeocodeObject ReverseGeocode(decimal latitude,
                                                   decimal longitude)
        {
            return Parse(latitude, longitude);
        }

        /// <summary>
        /// Converts latitude and longitude into a human readable string
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>A Geocode object</returns>
        public static GeocodeObject ReverseGeocode(double latitude,
                                                   double longitude)
        {
            return ReverseGeocode(Convert.ToDecimal(latitude),
                                  Convert.ToDecimal(longitude));
        }

        #region HelperMethods

        private static string GetGeocodeUrl(string fullAddress)
        {
            return string.Format(GeocodeUrlTemplate, HttpUtility.UrlEncode(fullAddress));
        }

        private static string GetReverseGeocodeUrl(decimal latitude,
                                                   decimal longitude)
        {
            string latLongString = string.Format("{0},{1}", latitude, longitude);

            return string.Format(ReverseGeocodeUrlTemplate, HttpUtility.UrlEncode(latLongString));
        }

        private static GeocodeObject Parse(string fullAddress)
        {
            string url = GetGeocodeUrl(fullAddress);

            XElement results = XElement.Load(url);

            return ParseResponse(results.ToString());
        }

        private static GeocodeObject Parse(decimal latitude,
                                           decimal longitude)
        {
            string url = GetReverseGeocodeUrl(latitude, longitude);

            XElement results = XElement.Load(url);

            return ParseResponse(results.ToString());
        }

        private static GeocodeObject ParseResponse(string xml)
        {
            GeocodeObject obj = ParseLatLong(xml);

            obj.RawXmlResult = xml;

            obj.Address = ParseAddress(xml);

            obj.IsValid = obj.IsValid && obj.Address.IsValid;

            return obj;
        }

        //LINQ to XML consumes all
        private static GeocodeObject ParseLatLong(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            XElement latlngElement = doc.XPathSelectElements("/GeocodeResponse/result[1]/geometry/location[1]").FirstOrDefault();

            GeocodeObject obj = new GeocodeObject();

            if (latlngElement != null)
            {
                XElement latElement = latlngElement.Element("lat");
                if (latElement != null)
                {
                    obj.Latitude = Convert.ToDecimal(latElement.Value);
                }

                XElement lngElement = latlngElement.Element("lng");
                if (lngElement != null)
                {
                    obj.Longitude = Convert.ToDecimal(lngElement.Value);
                }
            }

            obj.IsValid = obj.Latitude != default(decimal) && obj.Longitude != default(decimal);

            return obj;
        }

        private static GeocodeAddress ParseAddress(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            List<XElement> addressComponents = doc.XPathSelectElements("/GeocodeResponse/result[1]/address_component").ToList();

            string streetNumber = string.Empty;

            string routeNumber = string.Empty;

            string city = string.Empty;

            string stateCode = string.Empty;

            string stateName = string.Empty;

            string zip = string.Empty;

            string country = string.Empty;

            foreach (XElement component in addressComponents)
            {
                XElement xElement = component.Element("type");

                if (xElement != null)
                {
                    if (xElement.Value.Equals("street_number"))
                    {
                        XElement streetValue = component.Element("long_name");

                        if (streetValue != null)
                        {
                            streetNumber = streetValue.Value;
                        }
                    }
                    else if (xElement.Value.Equals("route"))
                    {
                        XElement routeValue = component.Element("long_name");

                        if (routeValue != null)
                        {
                            routeNumber = routeValue.Value;
                        }
                    }
                    else if (xElement.Value.Equals("administrative_area_level_1"))
                    {
                        XElement stateCodeValue = component.Element("short_name");

                        if (stateCodeValue != null)
                        {
                            stateCode = stateCodeValue.Value;
                        }

                        XElement stateNameValue = component.Element("long_name");

                        if (stateNameValue != null)
                        {
                            stateName = stateNameValue.Value;
                        }
                    }
                    else if (xElement.Value.Equals("locality"))
                    {
                        XElement cityValue = component.Element("short_name");

                        if (cityValue != null)
                        {
                            city = cityValue.Value;
                        }
                    }
                    else if (xElement.Value.Equals("postal_code"))
                    {
                        XElement zipValue = component.Element("short_name");

                        if (zipValue != null)
                        {
                            zip = zipValue.Value;
                        }
                    }
                    else if (xElement.Value.Equals("country"))
                    {
                        XElement countryValue = component.Element("long_name");

                        if (countryValue != null)
                        {
                            country = countryValue.Value;
                        }
                    }
                }
            }

            string formattedAddress = string.Format("{0} {1}, {2}, {3} {4}",
                                                    streetNumber,
                                                    routeNumber,
                                                    city,
                                                    stateCode,
                                                    zip);

            GeocodeAddress address = new GeocodeAddress
            {
                FormattedAddress = formattedAddress,
                City = city,
                StateCode = stateCode,
                StateName = stateName,
                Zip = zip,
                AddressLine1 = string.Format("{0} {1}", streetNumber, routeNumber),
                Country = country,
                IsValid = !formattedAddress.Replace(",", string.Empty).TrimSafely().IsNullOrWhiteSpace(),
            };

            return address;
        }

        #endregion

    }
}
