using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Repository.Implement;
using swp391_debo_be.Repository.Interface;

namespace swp391_debo_be.Cores
{
    public class CTreatment
    {
        private readonly TreatmentRepository _treatmentRepo;

        public CTreatment()
        {
            _treatmentRepo = new TreatmentRepository();
        }

        public Task<int> addTreatmentAsync(TreatmentDto treatment)
        {
            return _treatmentRepo.addTreatmentAsync(treatment);
        }

        public Task deleteTreatmentAsync(int id)
        {
            return _treatmentRepo.deleteTreatmentAsync(id);
        }

        public Task<List<TreatmentDto>> getAllTreatmentAsync()
        {
            return _treatmentRepo.getAllTreatmentAsync();
        }

        public Task<TreatmentDto> getTreatmentAsync(int id)
        {
            return _treatmentRepo.getTreatmentAsync(id);
        }

        public Task updateTreatmentAsync(int id, TreatmentDto treatment)
        {
            return _treatmentRepo.updateTreatmentAsync(id, treatment);
        }
    }
}
