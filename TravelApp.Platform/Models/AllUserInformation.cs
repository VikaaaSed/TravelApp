using TravelApp.API.Models;

namespace TravelApp.Platform.Models
{
    public class AllUserInformation
    {
        public User User;
        public List<Feedback> Feedbacks;
        public AllUserInformation(User user, List<Feedback> feedbacks)
        {
            User = user;
            Feedbacks = feedbacks;
        }
    }
}
