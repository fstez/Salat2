using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salat.Data;
using Salat.Models;

namespace Salat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalatController : ControllerBase
    {
        private readonly SalatDbContext _db;
        public SalatController(SalatDbContext db) => _db = db;

        [HttpGet("foods")]
        public async Task<IEnumerable<Food>> GetFoods() =>
            await _db.Foods.Include(f => f.Components).ThenInclude(c => c.FoodItem).ToListAsync();

        [HttpPost("fooditem")]
        public async Task<IActionResult> AddFoodItem(FoodItem item)
        {
            if (item.ProteinPct + item.FatPct + item.CarbsPct > 100)
                return BadRequest("Protsentide summa ei tohi ületada 100%.");

            if (await _db.FoodItems.AnyAsync(x => x.Name == item.Name))
                return Conflict($"FoodItem '{item.Name}' already exists.");

            _db.FoodItems.Add(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPost("food")]
        public async Task<IActionResult> CreateFood([FromBody] Food food)
        {
            if (string.IsNullOrWhiteSpace(food.Name))
                return BadRequest("Name is required.");

            if (await _db.Foods.AnyAsync(x => x.Name == food.Name))
                return Conflict($"Food with name '{food.Name}' already exists.");

            _db.Foods.Add(food);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFoods), new { id = food.Id }, food);
        }

        [HttpPost("food/{foodId}/component")]
        public async Task<IActionResult> AddComponent(int foodId, [FromBody] FoodComponent comp)
        {
            if (!await _db.Foods.AnyAsync(f => f.Id == foodId)) return NotFound("Toit ei leitud");
            if (!await _db.FoodItems.AnyAsync(f => f.Id == comp.FoodItemId)) return NotFound("Toiduaine ei leitud");

            comp.FoodId = foodId;
            _db.FoodComponents.Add(comp);
            await _db.SaveChangesAsync();

            return Ok(await _db.Foods.Include(f => f.Components).ThenInclude(c => c.FoodItem)
                                     .FirstAsync(f => f.Id == foodId));
        }

        [HttpGet("food/{id}/macros")]
        public async Task<IActionResult> Macros(int id, [FromQuery] double scale = 1000)
        {
            var f = await _db.Foods
                .Include(x => x.Components)
                .ThenInclude(c => c.FoodItem)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (f == null) return NotFound();

            var total = f.Macros(scale);
            var list = f.ScaleTo(scale).ToList();

            return Ok(new
            {
                totalWeight = scale,
                macros = new
                {
                    protein = total.protein,
                    fat = total.fat,
                    carbs = total.carbs
                },
                components = list.Select(x => new { item = x.item, grams = x.grams })
            });
        }


        [HttpGet("fooditems")]
        public async Task<IEnumerable<FoodItem>> GetFoodItems() =>
            await _db.FoodItems.OrderBy(x => x.Name).ToListAsync();
    }
            
}


