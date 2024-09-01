namespace MealApiBackend.Models
{
    public class Meal
    {
        public string Id { get; set; } = string.Empty; // Cambiar a string
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
