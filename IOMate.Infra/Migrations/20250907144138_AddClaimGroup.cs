using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOMate.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Resource = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    ClaimGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceClaims_ClaimGroups_ClaimGroupId",
                        column: x => x.ClaimGroupId,
                        principalTable: "ClaimGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaimGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaimGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaimGroups_ClaimGroups_ClaimGroupId",
                        column: x => x.ClaimGroupId,
                        principalTable: "ClaimGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClaimGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceClaims_ClaimGroupId",
                table: "ResourceClaims",
                column: "ClaimGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimGroups_ClaimGroupId",
                table: "UserClaimGroups",
                column: "ClaimGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimGroups_UserId_ClaimGroupId",
                table: "UserClaimGroups",
                columns: new[] { "UserId", "ClaimGroupId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceClaims");

            migrationBuilder.DropTable(
                name: "UserClaimGroups");

            migrationBuilder.DropTable(
                name: "ClaimGroups");
        }
    }
}
