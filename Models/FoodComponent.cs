using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salat.Models
{
    public class FoodComponent
    {
        [Key]
        public int Id { get; set; }

        public int FoodId { get; set; }
        public Food? Food { get; set; }

        public int FoodItemId { get; set; }
        public FoodItem? FoodItem { get; set; }

        [Required]
        public double QuantityGrams { get; set; }

        // Расчёт макросов по весу
        public double ProteinGrams(double scale = 1.0)
        {
            decimal pct = (decimal)(FoodItem?.ProteinPct ?? 0);  // Преобразуем double? в decimal
            return Math.Round((double)(pct / 100 * (decimal)QuantityGrams * (decimal)scale), 3); // Приводим к double для результата
        }

        public double FatGrams(double scale = 1.0)
        {
            decimal pct = (decimal)(FoodItem?.FatPct ?? 0); // Преобразуем double? в decimal
            return Math.Round((double)(pct / 100 * (decimal)QuantityGrams * (decimal)scale), 3); // Приводим к double для результата
        }

        public double CarbsGrams(double scale = 1.0)
        {
            decimal pct = (decimal)(FoodItem?.CarbsPct ?? 0); // Преобразуем double? в decimal
            return Math.Round((double)(pct / 100 * (decimal)QuantityGrams * (decimal)scale), 3); // Приводим к double для результата
        }

        public double WeightByScale(double targetWeight, double totalWeight)
        {
            if (totalWeight == 0) return 0;
            double factor = targetWeight / totalWeight;
            return Math.Round(QuantityGrams * factor, 3);
        }
    }
}
