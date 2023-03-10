using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdeiesApplication.Migrations
{
    /// <inheritdoc />
    public partial class petitioner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacation_User_PetitionerId",
                table: "Vacation");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacation_User_PetitionerId",
                table: "Vacation",
                column: "PetitionerId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacation_User_PetitionerId",
                table: "Vacation");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacation_User_PetitionerId",
                table: "Vacation",
                column: "PetitionerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
