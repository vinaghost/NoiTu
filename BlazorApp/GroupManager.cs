using BlazorApp.Models;

namespace BlazorApp
{
    public class GroupManager
    {
        private readonly Dictionary<string, List<User>> _groups = [];

        public string CreateGroup()
        {
            var groupName = KeyGenerator.GetUniqueKey(6);
            _groups.Add(groupName, []);
            return groupName;
        }

        public void JoinGroup(string groupName, User user)
        {
            if (!_groups.TryGetValue(groupName, out List<User>? value))
            {
                return;
            }

            value.Add(user);
        }

        public void LeaveGroup(string groupName, string connectionId)
        {
            if (!_groups.TryGetValue(groupName, out List<User>? value))
            {
                return;
            }

            var user = value.Find(x => x.ConnectionId == connectionId);
            if (user is null) return;

            value.Remove(user);
            if (value.Count == 0)
            {
                _groups.Remove(groupName);
            }
        }

        public List<User> GetGroupMembers(string groupName)
        {
            return _groups.TryGetValue(groupName, out List<User>? value) ? value : [];
        }

        public bool IsGroupExists(string groupName)
        {
            return _groups.ContainsKey(groupName);
        }
    }
}