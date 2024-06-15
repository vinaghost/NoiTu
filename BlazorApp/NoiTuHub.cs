using Microsoft.AspNetCore.SignalR;

namespace BlazorApp
{
    public interface INoiTuHub
    {
        Task RoomJoined(string groupName);

        Task RoomNotFound(string groupName);

        Task UserJoined(string userName);

        Task UserLeave(string userName);
    }

    public class NoiTuHub(RoomManager roomManager) : Hub<INoiTuHub>
    {
        private readonly RoomManager _roomManager = roomManager;

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var (roomName, userName) = _roomManager.GetInfo(Context.ConnectionId);
            if (string.IsNullOrEmpty(roomName)) return;
            await Clients.GroupExcept(roomName, Context.ConnectionId).UserLeave(userName);

            _roomManager.LeavelRoom(Context.ConnectionId);
        }

        public async Task CreateRoom(string username)
        {
            var groupName = _roomManager.CreateRoom();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _roomManager.JoinRoom(groupName, new(Context.ConnectionId, username));

            await Clients.Caller.RoomJoined(groupName);
        }

        public async Task JoinRoom(string roomName, string username)
        {
            if (!_roomManager.IsRoomExists(roomName))
            {
                await Clients.Caller.RoomNotFound(roomName);
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            _roomManager.JoinRoom(roomName, new(Context.ConnectionId, username));

            await Clients.Caller.RoomJoined(roomName);
            await Clients.GroupExcept(roomName, Context.ConnectionId).UserJoined(username);
        }
    }
}