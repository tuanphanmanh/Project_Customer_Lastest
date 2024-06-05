using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class EsignSignAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "UserSignature",
                table: "EsignPosition",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AffiliateId",
                table: "AbpUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSignature",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "AffiliateId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "AbpUsers");
        }
    }
}
