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

    }
}
