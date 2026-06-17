using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDiary.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodCategoryToFoodAlternative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodCategory",
                table: "FoodAlternatives",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodCategory",
                table: "FoodAlternatives");
        }
    }
}
