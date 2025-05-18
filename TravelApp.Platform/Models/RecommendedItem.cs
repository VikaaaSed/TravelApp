namespace TravelApp.Platform.Models
{
    public class RecommendedItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        public string LinkImage { get; set; }
        public double Rating { get; set; }

        public RecommendedItem(int id, string title, string pageName, string linkImage, double rating)
        {
            Id = id;
            Title = title;
            PageName = pageName;
            LinkImage = linkImage;
            Rating = rating;
        }
    }
}
