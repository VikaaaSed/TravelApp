using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllCityInformation
    {
        public City City { get; set; }
        public List<LocationInCity> Locations { get; set; }
        public AllCityInformation(City city, List<LocationInCity> locations)
        {
            City = city;
            Locations = locations;
        }
    }
}
