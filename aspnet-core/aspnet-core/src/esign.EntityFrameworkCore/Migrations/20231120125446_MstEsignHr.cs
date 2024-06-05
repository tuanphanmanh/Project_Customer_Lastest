using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class MstEsignHr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstEsignHrOrgStructure",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Published = table.Column<int>(type: "int", nullable: false),
                    Orgstructuretypename = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Orgstructuretypecode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Parentid = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstEsignHrOrgStructure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstEsignHrPosition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstEsignHrPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstEsignHrTitles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstEsignHrTitles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstEsignHrOrgStructure_IsActive",
                table: "MstEsignHrOrgStructure",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MstEsignHrPosition_Code",
                table: "MstEsignHrPosition",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_MstEsignHrPosition_IsActive",
                table: "MstEsignHrPosition",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MstEsignHrTitles_Code",
                table: "MstEsignHrTitles",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_MstEsignHrTitles_IsActive",
                table: "MstEsignHrTitles",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstEsignHrOrgStructure");

            migrationBuilder.DropTable(
                name: "MstEsignHrPosition");

            migrationBuilder.DropTable(
                name: "MstEsignHrTitles");
        }
    }
}
