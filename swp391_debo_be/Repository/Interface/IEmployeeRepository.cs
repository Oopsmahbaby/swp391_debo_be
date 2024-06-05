using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IEmployeeRepository
    {
        public List<User> GetDentistBasedOnTreamentId(int treatmentId);
    }
}
