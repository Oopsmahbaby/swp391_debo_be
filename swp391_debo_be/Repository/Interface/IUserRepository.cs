using Microsoft.Owin.Security;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IUserRepository
    {
        public User GetUserById(Guid id);

        public User GetUserByEmail(string email);

        public User CreateUser(User user);

        public User UpdateUser(User user);

        public User DeleteUser(User user);

        public User GetUserByPhone(string phone);

        public List<User> GetUsers();

        public string[] getRolesName(User user);

        public bool IsRefreshTokenExist(User user);

        public bool SaveRefreshToken(Guid userId, string refreshToken);
        public bool DeleteRefreshToken(Guid userId);
        public bool IsPasswordExist(string password, User user);

        public Task<Guid> CreateNewStaff(EmployeeDto employee);
        public Task<Guid> CreateNewDent(EmployeeDto employee);
        public Task<Guid> CreateNewManager(EmployeeDto employee);
    }
}
