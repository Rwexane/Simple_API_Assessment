using Microsoft.EntityFrameworkCore;
using SimpleA.Data;
using SimpleA.Models;

namespace Simple_API_Assessment.Data.Repository
{
    public class ApplicantRepo : IApplicantRepository
    {
        private readonly DataContext _context;

        public ApplicantRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Applicant>> GetAllApplicantsAsync()
        {
            return await _context.Applicants
                .Include(a => a.Skills)
                .ToListAsync();
        }

        public async Task<Applicant> GetApplicantByIdAsync(int id)
        {
            return await _context.Applicants
                .Include(a => a.Skills)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> AddApplicantAsync(Applicant applicant)
        {
            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();
            return applicant.Id;
        }

        public async Task UpdateApplicantAsync(Applicant applicant)
        {
            _context.Entry(applicant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteApplicantAsync(int id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant != null)
            {
                _context.Applicants.Remove(applicant);
                await _context.SaveChangesAsync();
            }
        }
    }
}
