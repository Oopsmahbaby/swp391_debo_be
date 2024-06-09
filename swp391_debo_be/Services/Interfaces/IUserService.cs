using swp391_debo_be.Constants;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Services.Interfaces
{
    public interface IUserService
    {
        public bool IsRefreshToken(User user);
        public ApiRespone CreateUser(CreateUserDto createUserDto);
        public List<User> GetUsers();
        object? GetProfile(string? userId);

        public Task<ApiRespone> CreateNewStaff(EmployeeDto employee);
        public Task<ApiRespone> CreateNewDent(EmployeeDto employee);
        public Task<ApiRespone> CreateNewManager(EmployeeDto employee);
        public Task<ApiRespone> ViewStaffList(int page, int limit);
        public Task<ApiRespone> ViewDentList(int page, int limit);
        public Task<ApiRespone> ViewManagerList(int page, int limit);
    }
}
