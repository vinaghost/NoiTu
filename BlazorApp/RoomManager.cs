using BlazorApp.DTO;
using BlazorApp.Enitites;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp
{
    public class RoomManager(IDbContextFactory<AppDbContext> contextFactory)
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory = contextFactory;

        public string CreateRoom()
        {
            var roomName = KeyGenerator.GetUniqueKey(6);

            using var context = _contextFactory.CreateDbContext();
            context.Rooms.Add(new Room { Name = roomName });
            context.SaveChanges();

            return roomName;
        }

        public void JoinRoom(string roomName, UserDto user)
        {
            using var context = _contextFactory.CreateDbContext();
            var room = context.Rooms.FirstOrDefault(x => x.Name == roomName);
            if (room is null) return;

            var userEntity = user.ToEntity();
            context.Users.Add(userEntity);
            context.SaveChanges();
            context.Add(new Member { RoomId = room.Id, UserId = userEntity.Id });
            context.SaveChanges();
        }

        public void LeavelRoom(string connectionId)
        {
            using var context = _contextFactory.CreateDbContext();

            var user = context.Users.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (user is null) return;

            context.Members
                .Where(x => x.UserId == user.Id)
                .ExecuteDelete();
        }

        public (string, string) GetInfo(string connectionId)
        {
            using var context = _contextFactory.CreateDbContext();

            var user = context.Users.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (user is null) return ("", "");

            var room = context.Members
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Room)
                .FirstOrDefault();

            return (room?.Name ?? "", user.Name);
        }

        public List<UserDto> GetRoomMembers(string roomName)
        {
            using var context = _contextFactory.CreateDbContext();
            var room = context.Rooms.FirstOrDefault(x => x.Name == roomName);
            if (room is null) return [];

            var users = context.Members
                .Where(x => x.RoomId == room.Id)
                .Select(x => x.User)
                .ToList();

            return users.Where(x => x is not null).Select(x => x!.ToDto()).ToList();
        }

        public bool IsRoomExists(string roomName)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Rooms.Any(x => x.Name == roomName);
        }
    }
}