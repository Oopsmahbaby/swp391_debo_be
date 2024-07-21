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

        public async Task activeBranchAsync(int id)
        {
            var activeBranch = _context.ClinicBranches!.SingleOrDefault(u => u.Id == id);
            if (activeBranch != null)
            {
                activeBranch.Status = true;
                _context.ClinicBranches.Update(activeBranch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> addBranchAsync(BranchDto branch)
        {
            var newBranch = new ClinicBranch
            {
                Id = branch.Id,
                MngId = branch.MngId,
                Name = branch.Name,
                Address = branch.Address,
                Phone = branch.Phone,
                Email = branch.Email,
                Status = true,
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
                deleteBranch.MngId = null;
                deleteBranch.Status = false;
                _context.ClinicBranches.Update(deleteBranch);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task<List<BranchDto>> getAllBranchAsync(int page, int limit)
        //{
        //    IQueryable<ClinicBranch> query = _context.ClinicBranches
        //                                           .Where(b => b.Status == true);


        //    if (limit > 0)
        //    {
        //        query = query.Skip(page * limit)
        //                     .Take(limit);
        //    }

        //    var branchs = await query.ToListAsync();

        //    var branchDto = branchs.Select(b => new BranchDto
        //    {
        //        Id = b.Id,
        //        MngId = b.MngId,
        //        Name = b.Name,
        //        Address = b.Address,
        //        Phone = b.Phone,
        //        Email = b.Email,
        //        Avt = b.Avt,
        //    }).ToList();

        //    return branchDto;
        //}
        public async Task<List<BranchDto>> getAllBranchAsync(int page, int limit)
        {
            IQueryable<BranchDto> query = _context.ClinicBranches
                                                  .AsNoTracking()
                                                  .Where(b => b.Status == true)
                                                  .Select(b => new BranchDto
                                                  {
                                                      Id = b.Id,
                                                      MngId = b.MngId,
                                                      Name = b.Name,
                                                      Address = b.Address,
                                                      Phone = b.Phone,
                                                      Email = b.Email,
                                                      Avt = b.Avt,
                                                  });

            if (limit > 0)
            {
                query = query.Skip(page * limit)
                             .Take(limit);
            }

            return await query.ToListAsync();
        }

        //public async Task<object> getAppointmentBranchAsync(Guid id)
        //{
        //    var result = await (from a in _context.Appointments
        //                        join e in _context.Employees on a.DentId equals e.Id
        //                        join cb in _context.ClinicBranches on e.BrId equals cb.Id
        //                        where a.Id == id
        //                        select new
        //                        {
        //                            AppointmentId = a.Id,
        //                            a.DentId,
        //                            a.TempDentId,
        //                            a.CreatedDate,
        //                            a.StartDate,
        //                            a.TimeSlot,
        //                            a.Status,
        //                            BranchId = e.BrId,
        //                            BranchName = cb.Name
        //                        }).FirstOrDefaultAsync();

        //    return result;
        //}

        public async Task<List<object>> getAppointmentBranchAsync(int branchId)
        {
            var query = from appointment in _context.Appointments
                        join employee in _context.Employees on appointment.DentId equals employee.Id
                        join clinictreatment in _context.ClinicTreatments on appointment.TreatId equals clinictreatment.Id
                        join dentist in _context.Users on appointment.DentId equals dentist.Id into dentistGroup
                        from dent in dentistGroup.DefaultIfEmpty()
                        join tempDentist in _context.Users on appointment.TempDentId equals tempDentist.Id into tempDentistGroup
                        from tempDent in tempDentistGroup.DefaultIfEmpty()
                        join customer in _context.Users on appointment.CusId equals customer.Id into customerGroup
                        from cus in customerGroup.DefaultIfEmpty()
                        where employee.BrId == branchId
                        select new
                        {
                            appointment.Id,
                            appointment.TreatId,
                            clinictreatment.Name,
                            appointment.PaymentId,
                            appointment.DentId,
                            DentistFullName = (dent.FirstName + " " + dent.LastName),
                            appointment.TempDentId,
                            TempDentistFullName = (tempDent.FirstName + " " + tempDent.LastName),
                            appointment.CusId,
                            CustomerFullName = (cus.FirstName + " " + cus.LastName),
                            appointment.CreatorId,
                            appointment.CreatedDate,
                            appointment.StartDate,
                            appointment.TimeSlot,
                            appointment.Description,
                            appointment.Note,
                        };

            var result = await query.ToListAsync();
            return result.Cast<object>().ToList();
        }


        public async Task<BranchDto> getBranchAsync(int id)
        {
            var branch = await _context.ClinicBranches.FindAsync(id);
            if (branch == null)
            {
                return null;
            }
            var query = from cb in _context.ClinicBranches
                        join u in _context.Users on cb.MngId equals u.Id into cb_u
                        from u in cb_u.DefaultIfEmpty()
                        where cb.Id == id
                        select new BranchDto
                        {
                            Id = cb.Id,
                            MngId = cb.MngId,
                            MngName = cb.MngId != null ? (u.FirstName + " " + u.LastName) : null,
                            Name = cb.Name,
                            Address = cb.Address,
                            Phone = cb.Phone,
                            Email = cb.Email,
                            Avt = cb.Avt,
                        };
            var branchDto = await query.FirstOrDefaultAsync();

            return branchDto;
        }

        public async Task<BranchDto> getManagerBranchAsync(Guid id)
        {
            var branch = await _context.ClinicBranches
                        .Where(cb => cb.MngId == id)
                        .Select(cb => new BranchDto
                        {
                            Id = cb.Id,
                            MngId = cb.MngId,
                            Name = cb.Name,
                            Address = cb.Address,
                            Phone = cb.Phone,
                            Email = cb.Email,
                            Avt = cb.Avt
                        })
                        .FirstOrDefaultAsync();

            if (branch == null)
            {
                throw new KeyNotFoundException($"No branch found for manager ID: {id}");
            }

            return branch;
        }

        public async Task updateBranchAsync(int id, BranchDto branch)
        {
            var existingBranch = await _context.ClinicBranches.FindAsync(id);
            if (existingBranch == null || existingBranch.Status != true || id != branch.Id)
            {
                throw new InvalidOperationException("Branch not found, inactive, or ID mismatch.");

            }
            else
            {
                if (branch.Phone.Length > 10 || branch.Phone.Any(char.IsLetter))
                {
                    throw new InvalidOperationException("Phone number cannot be more than 10 digits or contain alphabetic characters.");
                }
                existingBranch.Name = branch.Name;
                existingBranch.MngId = branch.MngId;
                existingBranch.Address = branch.Address;
                existingBranch.Phone = branch.Phone;
                existingBranch.Email = branch.Email;

                _context.ClinicBranches.Update(existingBranch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UploadPicBranch(int id, BranchDto branch)
        {
            var existingBranch = await _context.ClinicBranches.FindAsync(id);
            if (existingBranch == null || existingBranch.Status != true || id != branch.Id)
            {
                throw new InvalidOperationException("Branch not found, inactive, or ID mismatch.");

            }
            else
            {
                existingBranch.Avt = branch.Avt;
                _context.ClinicBranches.Update(existingBranch);
                await _context.SaveChangesAsync();
            }
        }
    }
}
