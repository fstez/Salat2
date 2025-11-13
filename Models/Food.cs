using System.ComponentModel.DataAnnotations;

namespace Salat.Models
{
    public class Food
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public List<FoodComponent> Components { get; set; } = new();

        // Общий вес и макросы
        public double TotalWeightGrams() => Components.Sum(c => c.QuantityGrams);
        public double TotalProteinGrams() => Components.Sum(c => c.ProteinGrams());
        public double TotalFatGrams() => Components.Sum(c => c.FatGrams());
        public double TotalCarbsGrams() => Components.Sum(c => c.CarbsGrams());



        // Масштабирование компонентов
        public (double factor, IEnumerable<(string item, double grams)> list) ScaleTo(double targetGrams)
        {
            double total = TotalWeightGrams();
            double factor = total == 0 ? 0 : targetGrams / total;

            var list = Components.Select(c =>
                (c.FoodItem?.Name ?? $"#{c.FoodItemId}", Math.Round(c.QuantityGrams * factor, 3)));

            return (factor, list);
        }

    }
}
