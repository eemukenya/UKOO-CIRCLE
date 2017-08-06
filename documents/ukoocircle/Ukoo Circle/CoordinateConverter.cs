using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Ukoo_Circle
{
    class CoordinateConverter
    {
        public static Geopoint Point;

        public static GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
        {


            return new GeoCoordinate
                (
                Point.Position.Latitude,
                Point.Position.Longitude,
                Point.Position.Altitude,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }
    }
}
