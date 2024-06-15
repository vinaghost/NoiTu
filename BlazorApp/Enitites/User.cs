using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Enitites
{
    [Index(nameof(ConnectionId), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public required string ConnectionId { get; set; }
        public required string Name { get; set; }
    }
}