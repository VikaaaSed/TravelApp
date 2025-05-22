using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IFeedBackUser
    {
        public Task<List<Feedback>> GetFeedbackByUserId(int UserId);
    }
}
