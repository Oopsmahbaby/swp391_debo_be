using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;
using Microsoft.EntityFrameworkCore;

namespace swp391_debo_be.Dao.Implement
{
    public class BranchDao : IBranchDao
    {
        private readonly DeboDev02Context _context = new DeboDev02Context(new DbContextOptions<DeboDev02Context>());

        public BranchDao() 
        {
        }
        public BranchDao(DeboDev02Context context)
        {
            _context = context;
        }
        public async Task<int> addBranchAsync(BranchDto branch)
        {
            var newBranch = new ClinicBranch
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                Avt = branch.Avt,
            };
            _context.ClinicBranches.Add(newBranch);
            await _context.SaveChangesAsync();
            return newBranch.Id;
        }

        public async Task deleteBranchAsync(int id)
        {
            var deleteBranch = _context.ClinicBranches!.SingleOrDefault(x => x.Id == id);
            if (deleteBranch != null) 
            {
                _context.ClinicBranches.Remove(deleteBranch);
                await _context.SaveChangesAsync() ;
            }
        }

        public async Task<List<BranchDto>> getAllBranchAsync(int page, int limit)
        {
            var branchs = new List<ClinicBranch>();
            if (limit < 0)
            {
                branchs = await _context.ClinicBranches.ToListAsync();
            }
            else
            {
                branchs = await _context.ClinicBranches
                                           .Skip((page - 1) * limit)
                                           .Take(limit)
                                           .ToListAsync();
            }
            var branchsDto = branchs.Select(b => new BranchDto
            {
                Id=b.Id,
                Address = b.Address,
            }).ToList();
            return branchsDto;
        }

        public async Task<BranchDto> getBranchAsync(int id)
        {
            var branch = await _context.ClinicBranches.FindAsync(id);
            if (branch == null)
            {
                return null;
            }
            return new BranchDto
            {
                Id = branch.Id,
                Address = branch.Address,
            };

        }

        public async Task updateBranchAsync(int id, BranchDto branch)
        {
            if (id == branch.Id)
            {
                var existingBranch = await _context.ClinicBranches.FindAsync(id);
                if (existingBranch != null)
                {
                    existingBranch.Address = branch.Address;
                    _context.ClinicBranches.Update(existingBranch);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
