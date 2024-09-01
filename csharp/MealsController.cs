using Microsoft.AspNetCore.Mvc;
using MealApiBackend.Services;
using MealApiBackend.Models;
using System.Threading.Tasks;
using System.Collections.Generic; 

namespace MealApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealsController : ControllerBase
    {
        private readonly MealService _mealService;

        public MealsController(MealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet("{mealName}")]
        public async Task<IActionResult> GetMealByName(string mealName)
        {
            var meal = await _mealService.GetMealByNameAsync(mealName);

            if (meal == null)
            {
                return NotFound(new { message = "Meal not found." });
            }

            return Ok(meal);
        }

        [HttpGet("recipes")]
        public async Task<IActionResult> GetAllRecipes([FromQuery] int limit = 40)
        {
            var recipes = await _mealService.GetAllRecipesAsync(limit);

            if (recipes == null || !recipes.Any())
            {
                return NotFound(new { message = "No recipes found." });
            }

            return Ok(recipes);
        }

    }
}
