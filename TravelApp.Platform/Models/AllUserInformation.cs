using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllUserInformation
    {
        public User User;
        public List<UserFeedback> Feedbacks;
        public List<FavoriteLocationItem> FavoriteLocations;
        public List<Follower> Subscriptions;
        public AllUserInformation(User user, List<UserFeedback> feedbacks, 
            List<FavoriteLocationItem> favoriteLocations, List<Follower> subscriptions)
        {
            User = user;
            Feedbacks = feedbacks;
            FavoriteLocations = favoriteLocations;
            Subscriptions = subscriptions;
        }
    }
}
