using core.entity;

namespace core.interfaces;

public interface IUserRepository
{
    Task<List<User>> CreateAndListUsersAsync(User u);
}
