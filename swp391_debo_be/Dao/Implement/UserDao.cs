﻿using swp391_debo_be.Dao.Interface;
using swp391_debo_be.DBContext;
using swp391_debo_be.Models;

namespace swp391_debo_be.Dao.Implement
{
    public class UserDao : IUserDao
    {
        private readonly Debo_dev_02Context _context = new Debo_dev_02Context (new Microsoft.EntityFrameworkCore.DbContextOptions<Debo_dev_02Context>());

        public UserDao()
        {
        }

        public UserDao(Debo_dev_02Context context)
        {
            this._context = context;
        }

        public User CreateUser(User user)
        { 
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();

            return user;
        }

        public string[] GetRolesName(User user)
        {
            int? roleId = user.Role;

            Role? role = _context.Roles.Find(roleId);

            if (role == null)
            {
                return new string[0];
            }

            return new string[] { role.Name };
        }

        public User GetUserByEmail(string email)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Email == email);

            return user;
        }

        public User GetUserById(Guid id)
        {
            User? user = _context.Users.Find(id);
            return user;
        }

        public User GetUserByPhone(string phone)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Phone == phone);

            return user;
        }

        public bool IsRefreshTokenExist(User user)
        {
            User foundUser = GetUserById(user.Id);

            if (foundUser.RefreshToken == null)
            {
                return false;
            }

            return true;
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }
    }
}
