using TravelApp.API.Models;

namespace TravelApp.Platform.Areas.Admin.Services.Interfaces
{
    public interface IAdminFeedbackService
    {
        public Task<List<Feedback>> GetFeedbacksAsync();
    }

}
