using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllCityInformation
    {
        public City City { get; set; }
        public List<Location> Locations { get; set; }
        public AllCityInformation(City city, List<Location> locations)
        {
            City = city;
            Locations = locations;
        }
    }
}
