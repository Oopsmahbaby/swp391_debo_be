using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Dao.Interface
{
    public interface IEmployeeDao
    {
        public List<User> GetDentistBasedOnTreamentId(int treatmentId);
    }
}
