using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Net;

namespace swp391_debo_be.Services.Implements
{
    public class BranchService : IBranchService
    {
        private readonly CBranch _cBranch;

        public BranchService(CBranch cBranch)
        {
            _cBranch = cBranch;
        }

        public async Task<ApiRespone> activeBranchAsync(int id)
        {
            try
            {
                var existingBranchs = await CBranch.getBranchAsync(id);
                if (existingBranchs != null)
                {
                    await CBranch.activeBranchAsync(id);
                    return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = existingBranchs, Message = "Branch data is activated successfully.", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Branch not found.", Success = false };
                }

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> addBranchAsync(BranchDto branch)
        {
            try
            {
                var existingBranch = await CBranch.getAllBranchAsync(1, -1);
                if (existingBranch.Any(t => t.Id == branch.Id))
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = "ID cannot be duplicated", Success = false };
                }
                var newBranch = await CBranch.addBranchAsync(branch);
                var branchs = await CBranch.getBranchAsync(newBranch);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = branchs, Message = "Branch data is added successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> deleteBranchAsync(int id)
        {
            try
            {
                var existingBranch = await CBranch.getBranchAsync(id);
                if (existingBranch != null)
                {
                    await CBranch.deleteBranchAsync(id);
                    return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = existingBranch, Message = "Branch data is deleted successfully.", Success = true };
                }
                else
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Branch not found.", Success = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> getAllBranchAsync(int page, int limit)
        {
            try
            {
                var data = await CBranch.getAllBranchAsync(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Branch data is retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> getBranchAsync(int id)
        {
            try
            {
                var data = await CBranch.getBranchAsync(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Branch data is retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> getManagerBranchAsync(Guid id)
        {
            try
            {
                var data = await CBranch.getManagerBranchAsync(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Branch of Manager is retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> updateBranchAsync(int id, BranchDto branch)
        {
            try
            {
                if (id != branch.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Branch ID mismatch.", Success = false };
                }
                var data = await CBranch.getBranchAsync(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Branch not found or inactive.", Success = false };
                }
                await CBranch.updateBranchAsync(id, branch);
                var updBranch = await CBranch.getBranchAsync(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updBranch, Message = "Branch data is updated successfully.", Success = true };

            }
            catch (InvalidOperationException ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> UploadPicBranch(int id, BranchDto branch)
        {
            try
            {
                if (id != branch.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Branch ID mismatch.", Success = false };
                }
                var data = await CBranch.getBranchAsync(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "Branch not found or inactive.", Success = false };
                }
                await CBranch.UploadPicBranch(id, branch);
                var updBranch = await CBranch.getBranchAsync(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updBranch, Message = "Branch data is updated successfully.", Success = true };

            }
            catch (InvalidOperationException ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = ex.Message, Success = false };
            }
        }
    }
}
