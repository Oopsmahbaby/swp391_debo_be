using swp391_debo_be.Dao.Implement;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Repository.Implement
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly TreatmentDao _treatmentDao;

        public TreatmentRepository()
        {
            _treatmentDao = new TreatmentDao();
        }
        public Task<int> addTreatmentAsync(TreatmentDto treatment)
        {
            return _treatmentDao.addTreatmentAsync(treatment);
        }

        public Task deleteTreatmentAsync(int id)
        {
            return _treatmentDao.deleteTreatmentAsync(id);
        }

        public Task<List<TreatmentDto>> getAllTreatmentAsync()
        {
            return _treatmentDao.getAllTreatmentAsync();
        }

        public Task<TreatmentDto> getTreatmentAsync(int id)
        {
            return _treatmentDao.getTreatmentAsync(id);
        }

        public Task updateTreatmentAsync(int id, TreatmentDto treatment)
        {
            return _treatmentDao.updateTreatmentAsync(id, treatment);
        }
    }
}
