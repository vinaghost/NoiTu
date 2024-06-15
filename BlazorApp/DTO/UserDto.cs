using BlazorApp.Enitites;

namespace BlazorApp.DTO
{
    public record UserDto(string ConnectionId, string Name);

    public static class UserDtoExtension
    {
        public static User ToEntity(this UserDto userDto)
        {
            return new User
            {
                ConnectionId = userDto.ConnectionId,
                Name = userDto.Name
            };
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto(user.ConnectionId, user.Name);
        }
    }
}