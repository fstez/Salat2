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

        // Сумма пропорций
        public double TotalRatio() => Components.Sum(c => c.Ratio);


        // Вычисление макросов при заданном итоговом весе
        public (double totalWeight, double protein, double fat, double carbs) Macros(double targetWeight)
        {
            double sumRatio = TotalRatio();
            if (sumRatio == 0)
                return (0, 0, 0, 0);

            double protein = 0, fat = 0, carbs = 0;

            foreach (var c in Components)
            {
                double grams = c.Grams(targetWeight, sumRatio);
                protein += c.ProteinGrams(grams);
                fat += c.FatGrams(grams);
                carbs += c.CarbsGrams(grams);
            }

            return (targetWeight,
                    Math.Round(protein, 3),
                    Math.Round(fat, 3),
                    Math.Round(carbs, 3));
        }


        // Список ингредиентов в граммах при scale
        public IEnumerable<(string item, double grams)> ScaleTo(double targetWeight)
        {
            double sumRatio = TotalRatio();
            foreach (var c in Components)
                yield return (c.FoodItem?.Name ?? $"#{c.FoodItemId}", c.Grams(targetWeight, sumRatio));
        }
    }
}
