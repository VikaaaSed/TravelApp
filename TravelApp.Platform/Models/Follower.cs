namespace TravelApp.Platform.Models
{
    public class Follower
    {
        public int id { get; set; }
        public int FollowerId { get; set; }
        public string? FollowerFirstName { get; set; }
        public string? FollowerLastName { get; set; }
        public string? AvatarLink { get; set; }

        public Follower(int id, int followerId, string? followerFirstName,
            string? followerLastName, string? avatarLink)
        {
            this.id = id;
            FollowerId = followerId;
            FollowerFirstName = followerFirstName;
            FollowerLastName = followerLastName;
            AvatarLink = avatarLink;
        }
    }
}
