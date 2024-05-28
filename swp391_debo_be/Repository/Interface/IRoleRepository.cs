using swp391_debo_be.Models;

namespace swp391_debo_be.Repository.Interface
{
    public interface IRoleRepository
    {
        public Role GetRoleById(int roleId);
    }
}
