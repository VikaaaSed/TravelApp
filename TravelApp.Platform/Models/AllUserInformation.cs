using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllUserInformation
    {
        public User User;
        public List<UserFeedback> Feedbacks;
        public AllUserInformation(User user, List<UserFeedback> feedbacks)
        {
            User = user;
            Feedbacks = feedbacks;
        }
    }
}
