namespace BlazorApp.Enitites
{
    public class Member
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public User? User { get; set; }
        public required int RoomId { get; set; }
        public Room? Room { get; set; }
    }
}