using System;
using JS.Business.GeocodingEngine;
using JS.Entities.GeocodingEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JS.Test.GeocodingEngine
{
    [TestClass]
    public class GeocodingManagerTest
    {
        [TestMethod]
        public void GeocodingTest()
        {
            GeocodeObject obj = GeocodeManager.Geocode("704 NW 128th PL Miami FL 33182");

            Assert.IsNotNull(obj);

            Assert.IsTrue(obj.IsValid);
        }
    }
}
