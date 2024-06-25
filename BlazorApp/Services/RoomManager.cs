using BlazorApp.Dto;
using BlazorApp.Enitites;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services
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

            var member = context.Members.FirstOrDefault(x => x.Username == user.Username);
            if (member != null)
            {
                member.RoomId = room.Id;
                member.JoinTime = DateTime.Now;
                member.ConnectionId = user.ConnectionId;
                context.Update(member);
            }
            else
            {
                context.Add(new Member { RoomId = room.Id, Username = user.Username, ConnectionId = user.ConnectionId, JoinTime = DateTime.Now });
            }

            context.SaveChanges();
        }

        public void LeaveRoom(string connectionId)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Members
                .Where(x => x.ConnectionId == connectionId)
                .ExecuteDelete();
        }

        public MemberDto? GetMember(string connectionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var member = context.Members
                .Where(x => x.ConnectionId == connectionId)
                .Select(x => new { x.ConnectionId, x.Username })
                .FirstOrDefault();
            return member is null ? null : new MemberDto(member.ConnectionId, member.Username);
        }

        public List<UserDto> GetRoomMembers(string roomName)
        {
            using var context = _contextFactory.CreateDbContext();
            var room = context.Rooms
                .Where(x => x.Name == roomName)
                .Select(x => x.Id)
                .FirstOrDefault();
            if (room == default) return [];

            var users = context.Members
                .Where(x => x.RoomId == room)
                .OrderBy(x => x.JoinTime)
                .Select(x => new { x.ConnectionId, x.Username })
                .AsEnumerable()
                .Select(x => new UserDto(x.ConnectionId, x.Username))
                .ToList();

            return users;
        }

        public bool IsRoomExists(string roomName)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Rooms.Any(x => x.Name == roomName);
        }
    }
}