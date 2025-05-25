namespace TravelApp.Platform.Models
{
    public class RecommendationModel
    {
        public int Id;
        public List<int> MyCityId;
        public Dictionary<int, List<int>> MyFriendIdCity;

        public RecommendationModel()
        {
            Id = 0;
            MyCityId = [];
            MyFriendIdCity = [];
        }
        public RecommendationModel(int id, List<int> myCityId)
        {
            Id = id;
            MyCityId = myCityId;
            MyFriendIdCity = [];
        }
        public RecommendationModel(int id, List<int> myCityId, Dictionary<int, List<int>> myFriendIdCity)
        {
            Id = id;
            MyCityId = myCityId;
            MyFriendIdCity = myFriendIdCity;
        }
    }
}
