using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdeiesApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddColum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vacation_PetitionerId",
                table: "Vacation");

            migrationBuilder.AddColumn<int>(
                name: "RestOfVacation",
                table: "Vacation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Vacation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VacationDays",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_PetitionerId",
                table: "Vacation",
                column: "PetitionerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vacation_PetitionerId",
                table: "Vacation");

            migrationBuilder.DropColumn(
                name: "RestOfVacation",
                table: "Vacation");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Vacation");

            migrationBuilder.DropColumn(
                name: "VacationDays",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_PetitionerId",
                table: "Vacation",
                column: "PetitionerId",
                unique: true);
        }
    }
}
