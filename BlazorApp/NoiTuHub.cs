using Microsoft.AspNetCore.SignalR;

namespace BlazorApp
{
    public interface INoiTuHub
    {
        Task GroupCreated(string groupName);

        Task GroupJoined(string groupName);

        Task GroupNotFound(string groupName);

        Task UserJoined(string groupName, string userName);
    }

    public class NoiTuHub(GroupManager groupManager) : Hub<INoiTuHub>
    {
        private readonly GroupManager _groupManager = groupManager;

        public async Task CreateGroup(string username)
        {
            var groupName = _groupManager.CreateGroup();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _groupManager.JoinGroup(groupName, new(Context.ConnectionId, username));

            await Clients.Caller.GroupCreated(groupName);
        }

        public async Task JoinGroup(string groupName, string username)
        {
            if (!_groupManager.IsGroupExists(groupName))
            {
                await Clients.Caller.GroupNotFound(groupName);
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _groupManager.JoinGroup(groupName, new(Context.ConnectionId, username));

            await Clients.Caller.GroupJoined(groupName);
            await Clients.GroupExcept(groupName, Context.ConnectionId).UserJoined(groupName, username);
        }
    }
}