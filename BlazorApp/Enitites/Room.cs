using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Enitites
{
    [Index(nameof(Name), IsUnique = true)]
    public class Room
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}