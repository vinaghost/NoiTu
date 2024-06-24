using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Enitites
{
    [Index(nameof(ConnectionId))]
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string ConnectionId { get; set; }
        public required string Username { get; set; }
        public required int RoomId { get; set; }
        public required DateTime JoinTime { get; set; }

        public Room? Room { get; set; }
    }
}