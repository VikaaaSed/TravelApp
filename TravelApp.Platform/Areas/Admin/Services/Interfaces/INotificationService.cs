namespace TravelApp.Platform.Areas.Admin.Services.Interfaces
{
    public interface INotificationService
    {
        public void Success(string message);
        public void Error(string message);
        public void Info(string message);
        public void Warning(string message);
    }
}
