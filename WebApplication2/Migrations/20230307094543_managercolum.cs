using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class managercolum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_managerIdId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "managerIdId",
                table: "User",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_User_managerIdId",
                table: "User",
                newName: "IX_User_ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ManagerId",
                table: "User",
                column: "ManagerId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ManagerId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "User",
                newName: "managerIdId");

            migrationBuilder.RenameIndex(
                name: "IX_User_ManagerId",
                table: "User",
                newName: "IX_User_managerIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_managerIdId",
                table: "User",
                column: "managerIdId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
