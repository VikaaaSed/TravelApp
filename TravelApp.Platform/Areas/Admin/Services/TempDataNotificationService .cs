using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;

namespace TravelApp.Platform.Areas.Admin.Services
{
    public class TempDataNotificationService : INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public TempDataNotificationService(
            IHttpContextAccessor httpContextAccessor,
            ITempDataDictionaryFactory tempDataFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataFactory = tempDataFactory;
        }

        private ITempDataDictionary TempData =>
            _tempDataFactory.GetTempData(_httpContextAccessor.HttpContext!);

        public void Success(string message) => AddMessage("Success", message);
        public void Error(string message) => AddMessage("Error", message);
        public void Info(string message) => AddMessage("Info", message);
        public void Warning(string message) => AddMessage("Warning", message);

        private void AddMessage(string type, string message) => TempData[type] = message;
    }
}
