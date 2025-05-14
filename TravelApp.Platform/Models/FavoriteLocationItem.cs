namespace TravelApp.Platform.Models
{
    public class FavoriteLocationItem
    {
        public int Id { get; set; }
        public int IdLocation { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }

        public FavoriteLocationItem(int id, int idLocation, string pageName, string title)
        {
            Id = id;
            IdLocation = idLocation;
            PageName = pageName;
            Title = title;
        }
    }
}
