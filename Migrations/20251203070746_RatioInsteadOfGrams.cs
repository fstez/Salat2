using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salat.Migrations
{
    /// <inheritdoc />
    public partial class RatioInsteadOfGrams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityGrams",
                table: "FoodComponents",
                newName: "Ratio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ratio",
                table: "FoodComponents",
                newName: "QuantityGrams");
        }
    }
}
