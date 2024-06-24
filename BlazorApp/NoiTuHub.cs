using BlazorApp.Services;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp
{
    public interface INoiTuHub
    {
        Task RoomJoined(List<string> users);

        Task UserJoined(string userName);

        Task UserLeave(string userName);

        Task UserInput(string username, string content);
    }

    public class NoiTuHub(RoomManager roomManager) : Hub<INoiTuHub>
    {
        private readonly RoomManager _roomManager = roomManager;

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var member = _roomManager.GetMember(Context.ConnectionId);
            if (member is null) return;
            await Clients.GroupExcept(member.RoomName, Context.ConnectionId).UserLeave(member.UserName);

            _roomManager.LeaveRoom(Context.ConnectionId);
        }

        public async Task JoinRoom(string roomName, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            await Clients.GroupExcept(roomName, Context.ConnectionId).UserJoined(username);

            _roomManager.JoinRoom(roomName, new(Context.ConnectionId, username));

            await Clients.Caller.RoomJoined(_roomManager.GetRoomMembers(roomName).Select(x => x.Username).ToList());
        }

        public async Task LeaveRoom(string roomName, string username)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

            await Clients.GroupExcept(roomName, Context.ConnectionId).UserLeave(username);

            _roomManager.LeaveRoom(Context.ConnectionId);
        }

        public async Task Submit(string roomName, string username, string content)
        {
            await Clients.Group(roomName).UserInput(username, content);
        }
    }
}