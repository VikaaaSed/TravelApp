using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllUserInformation
    {
        public User User;
        public List<UserFeedback> Feedbacks;
        public List<FavoriteLocationItem> FavoriteLocations;
        public List<Follower> Subscriptions;
        public List<Follower> Followers;
        public List<RecommendedItem> Recommendations;

        public AllUserInformation(User user, List<UserFeedback> feedbacks,
            List<FavoriteLocationItem> favoriteLocations, List<Follower> subscriptions, List<Follower> followers,
            List<RecommendedItem> recommendations)
        {
            User = user;
            Feedbacks = feedbacks;
            FavoriteLocations = favoriteLocations;
            Subscriptions = subscriptions;
            Followers = followers;
            Recommendations = recommendations;
        }

        public AllUserInformation(User user, List<UserFeedback> feedbacks,
            List<FavoriteLocationItem> favoriteLocations, List<Follower> subscriptions, List<Follower> followers)
        {
            User = user;
            Feedbacks = feedbacks;
            FavoriteLocations = favoriteLocations;
            Subscriptions = subscriptions;
            Followers = followers;
            Recommendations = [];
        }
    }
}
