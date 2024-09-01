using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MealApiBackend.Models;
using System.Collections.Generic;
using System.Linq;

namespace MealApiBackend.Services
{
    public class MealService
    {
        private readonly HttpClient _httpClient;

        public MealService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Meal> GetMealByNameAsync(string mealName)
        {
            string url = $"https://www.themealdb.com/api/json/v1/1/search.php?s={mealName}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseBody);
                var mealData = jsonDocument.RootElement.GetProperty("meals");

                if (mealData.ValueKind == JsonValueKind.Array && mealData.GetArrayLength() > 0)
                {
                    var firstMeal = mealData[0];
                    return new Meal
                    {
                        Id = firstMeal.GetProperty("idMeal").GetString(),
                        Name = firstMeal.GetProperty("strMeal").GetString(),
                        Description = firstMeal.GetProperty("strInstructions").GetString(),
                        ImageUrl = firstMeal.GetProperty("strMealThumb").GetString()
                    };
                }
            }

            return null;
        }

        public async Task<IEnumerable<Meal>> GetAllRecipesAsync(int limit = 40)
            {
                string url = "https://www.themealdb.com/api/json/v1/1/search.php?s="; // URL para buscar recetas
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(responseBody);
                    var mealsData = jsonDocument.RootElement.GetProperty("meals");

                    // Convertir el JSON a una lista de recetas
                    var recipes = new List<Meal>();

                    foreach (var meal in mealsData.EnumerateArray())
                    {
                        recipes.Add(new Meal
                        {
                            Id = meal.GetProperty("idMeal").GetString(),
                            Name = meal.GetProperty("strMeal").GetString(),
                            Description = meal.GetProperty("strInstructions").GetString(),
                            ImageUrl = meal.GetProperty("strMealThumb").GetString()
                        });

                        if (recipes.Count >= limit) break;
                    }

                    return recipes;
                }

                return Enumerable.Empty<Meal>();
            }

    }
}
