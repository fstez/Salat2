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

        // --- НОВОЕ поле: пропорция ---
        [Required]
        public double Ratio { get; set; }


        // Расчёт веса компонента при данном итоговом весе
        public double Grams(double totalWeight, double sumRatio)
        {
            if (sumRatio == 0) return 0;
            return Math.Round(totalWeight * (Ratio / sumRatio), 3);
        }

        // Макросы для итогового веса
        public double ProteinGrams(double grams)
        {
            decimal pct = (decimal)(FoodItem?.ProteinPct ?? 0);
            return Math.Round((double)(pct / 100 * (decimal)grams), 3);
        }

        public double FatGrams(double grams)
        {
            decimal pct = (decimal)(FoodItem?.FatPct ?? 0);
            return Math.Round((double)(pct / 100 * (decimal)grams), 3);
        }

        public double CarbsGrams(double grams)
        {
            decimal pct = (decimal)(FoodItem?.CarbsPct ?? 0);
            return Math.Round((double)(pct / 100 * (decimal)grams), 3);
        }
    }
}
