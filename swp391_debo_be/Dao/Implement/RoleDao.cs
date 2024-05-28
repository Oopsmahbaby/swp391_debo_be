using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Models;

namespace swp391_debo_be.Dao.Implement
{
    public class RoleDao : IRoleDao
    {
        private readonly Debo_dev_02Context _context = new Debo_dev_02Context(new Microsoft.EntityFrameworkCore.DbContextOptions<Debo_dev_02Context>());

        public RoleDao()
        {
        }

        public Role GetRoleById(int roleId)
        {
            Role role = _context.Roles.Find(roleId);

            if (role == null)
            {
                return new Role();
            }

            return role;
        }
    }
}
