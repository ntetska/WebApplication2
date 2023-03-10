using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdeiesApplication.Migrations
{
    /// <inheritdoc />
    public partial class RestOfVacation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestOfVacation",
                table: "Vacation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestOfVacation",
                table: "Vacation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
