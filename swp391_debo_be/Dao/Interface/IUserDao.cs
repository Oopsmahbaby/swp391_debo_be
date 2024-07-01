﻿using Microsoft.Owin.Security;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Dao.Interface
{
    public interface IUserDao
    {
        public User GetUserById(Guid id);

        public User GetUserByEmail(string email);
        public User GetUserByAvt(string avt);

        public User CreateUser(User user);

        public User UpdateUser(User user);

        public User DeleteUser(User user);

        public User GetUserByPhone(string phone);

        public List<User> GetUsers(); 

        public string[] GetRolesName(User user);

        public bool SaveRefreshToken(Guid userId, string refreshToken);

        public bool DeleteRefreshToken(Guid userId);

        public bool IsPasswordExist(string password, User user);
        public bool IsRefreshTokenExist(User user);

        public Task<Guid> CreateNewStaff(EmployeeDto employee);
        public Task<Guid> CreateNewDentist(EmployeeDto employee);
        public Task<Guid> CreateNewManager(EmployeeDto employee);

        public Task<List<EmployeeDto>> ViewStaffList(int page, int limit);
        public Task<List<EmployeeDto>> ViewDentList(int page, int limit);
        public Task<List<EmployeeDto>> ViewManagerList(int page, int limit);
        public Task<List<EmployeeDto>> ViewCustomerList(int page, int limit);
        public Task<List<EmployeeDto>> AvailableManager(int page, int limit);
        public Task<EmployeeDto> GetUserById2(Guid id);
        public Task UpdateUser(Guid id, EmployeeDto emp);
        public Task UploadAvatarUser(Guid id, EmployeeDto emp);
        public Task UploadMedRecPatient(Guid id, EmployeeDto emp);
        public Task UpdatePassword(Guid id, EmployeeDto emp);
        public object firstTimeBooking(Guid userId);

    }
}
