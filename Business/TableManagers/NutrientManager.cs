using Microsoft.EntityFrameworkCore;
using Nutrition.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nutrition.Business.TableManagers
{
    public class NutrientManager
    {
        private readonly NutritionContext _context;
        public NutrientManager(NutritionContext context)
        {
            _context = context;
        }

        public Task<Nutrient> CreateNutrientAsync(Nutrient newNutrient)
        {
            try
            {
                // Add a new help desk ticket.
                _context.Nutrient.Add(newNutrient);
                _context.SaveChanges();
                return Task.FromResult(newNutrient);
            }
            catch (Exception ex)
            {
                DetachAllEntities();
                throw ex;
            }
        }

        public Task<bool> DeleteNutrientAsync(Nutrient nutrientToDelete)
        {
            // Get the existing record.
            var ExistingNutrient = _context.Nutrient
                .FirstOrDefault(nutrient => nutrient.NutrientId == nutrientToDelete.NutrientId);

            if (ExistingNutrient != null)
            {
                // Delete the help desk ticket.
                _context.Nutrient.Remove(ExistingNutrient);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public void DetachAllEntities()
        {
            // When we have an error, we need
            // to remove EF Core change tracking.
            var changedEntriesCopy = _context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted)
            .ToList();
            foreach (var entry in changedEntriesCopy)
            {
                entry.State = EntityState.Detached;
            }
        }

        public IQueryable<Nutrient> GetAllNutrients()
        {
            return _context.Nutrient.AsNoTracking();
        }

        public async Task<Nutrient> GetNutrientByGUID(Guid NutrientGUID)
        {
            var ExistingNutrient = await _context.Nutrient
            .Where(NutrientSet => NutrientSet.NutrientGuid == NutrientGUID)
            .AsNoTracking()
            .FirstOrDefaultAsync();
            return ExistingNutrient;
        }

        public Task<bool> UpdateNutrientAsync(Nutrient updatedNutrient)
        {
            try
            {
                // Get the existing record.
                var ExistingNutrient = _context.Nutrient
                    .FirstOrDefault(nutrient => nutrient.NutrientId == updatedNutrient.NutrientId);
                if (ExistingNutrient != null)
                {
                    ExistingNutrient.NutrientName = updatedNutrient.NutrientName;
                    ExistingNutrient.NutrientDescription = updatedNutrient.NutrientDescription;
                    _context.SaveChanges();
                }
                else
                {
                    return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                DetachAllEntities();
                throw ex;
            }
        }
    }
}
