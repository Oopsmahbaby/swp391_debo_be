using Microsoft.EntityFrameworkCore;
using swp391_debo_be.Dao.Interface;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Entity.Implement;

namespace swp391_debo_be.Dao.Implement
{
    public class TreatmentDao : ITreatmentDao
    {
        private readonly DeboDev02Context _context = new DeboDev02Context(new DbContextOptions<DeboDev02Context>());

        public TreatmentDao()
        {
        }
        public TreatmentDao(DeboDev02Context context)
        {
            _context = context;
        }

        public async Task<int> addTreatmentAsync(TreatmentDto treatment)
        {
            var newTreat = new ClinicTreatment
            {
                Id = treatment.Id,
                Category = treatment.Category,
                Name = treatment.Name,
                Description = treatment.Description,
                Price = treatment.Price,
            };
            _context.ClinicTreatments.Add(newTreat);
            await _context.SaveChangesAsync();
            return newTreat.Id;
        }

        public async Task deleteTreatmentAsync(int id)
        {
            var deleteTreat = _context.ClinicTreatments!.SingleOrDefault(u => u.Id == id);
            if (deleteTreat != null)
            {
                _context.ClinicTreatments.Remove(deleteTreat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TreatmentDto>> getAllTreatmentAsync(int pageNumber, int pageSize)
        {
            var treatments = await _context.ClinicTreatments
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            var treatmentDtos = treatments.Select(t => new TreatmentDto
            {
                Id = t.Id,
                Category = t.Category,
                Name = t.Name,
                Description = t.Description,
                Price = t.Price,
            }).ToList();

            return treatmentDtos;
        }


        public async Task<TreatmentDto> getTreatmentAsync(int id)
        {
            var treatment = await _context.ClinicTreatments.FindAsync(id);
            if (treatment == null)
            {
                return null; // or throw an appropriate exception
            }

            return new TreatmentDto
            {
                Id = treatment.Id,
                Category = treatment.Category,
                Name = treatment.Name,
                Description = treatment.Description,
                Price = treatment.Price,
            };
        }

        public async Task updateTreatmentAsync(int id, TreatmentDto treatment)
        {
            if (id == treatment.Id)
            {
                var existingTreat = await _context.ClinicTreatments.FindAsync(id);
                if (existingTreat != null)
                {
                    existingTreat.Category = treatment.Category;
                    existingTreat.Name = treatment.Name;
                    existingTreat.Description = treatment.Description;
                    existingTreat.Price = treatment.Price;

                    _context.ClinicTreatments.Update(existingTreat);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
