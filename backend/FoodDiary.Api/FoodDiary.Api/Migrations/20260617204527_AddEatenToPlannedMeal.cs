using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDiary.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEatenToPlannedMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eaten",
                table: "PlannedMeals",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eaten",
                table: "PlannedMeals");
        }
    }
}
