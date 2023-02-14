using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class cascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_ApplicantId",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_ApplicantId",
                table: "Request",
                column: "ApplicantId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_User_ApplicantId",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_User_ApplicantId",
                table: "Request",
                column: "ApplicantId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
