﻿using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Dao.Interface
{
    public interface ITreatmentDao
    {
        public Task<List<TreatmentDto>> getAllTreatmentAsync(int pageNumber, int pageSize);
        public Task<TreatmentDto> getTreatmentAsync(int id);
        public Task<int> addTreatmentAsync(TreatmentDto treatment);
        public Task updateTreatmentAsync(int id, TreatmentDto treatment);
        public Task deleteTreatmentAsync(int id);
        public Task activeTreatmentAsync(int id);
        List<ClinicTreatment> GetDentistsBasedOnBranchId(int branchId);
    }
}
