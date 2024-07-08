﻿using swp391_debo_be.Dto.Implement;

namespace swp391_debo_be.Repository.Interface
{
    public interface IBranchRepository
    {
        public Task<List<BranchDto>> getAllBranchAsync(int page, int limit);
        public Task<BranchDto> getBranchAsync(int id);
        public Task<BranchDto> getManagerBranchAsync(Guid id);
        public Task<object> getAppointmentBranchAsync(Guid id);
        public Task<int> addBranchAsync(BranchDto branch);
        public Task updateBranchAsync(int id, BranchDto branch);
        public Task deleteBranchAsync(int id);
        public Task UploadPicBranch(int id, BranchDto branch);
    }
}

