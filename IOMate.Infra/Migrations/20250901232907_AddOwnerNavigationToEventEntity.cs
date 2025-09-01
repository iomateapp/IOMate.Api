using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOMate.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerNavigationToEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_OwnerId",
                table: "UserEvents",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_Users_OwnerId",
                table: "UserEvents",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_Users_OwnerId",
                table: "UserEvents");

            migrationBuilder.DropIndex(
                name: "IX_UserEvents_OwnerId",
                table: "UserEvents");
        }
    }
}
