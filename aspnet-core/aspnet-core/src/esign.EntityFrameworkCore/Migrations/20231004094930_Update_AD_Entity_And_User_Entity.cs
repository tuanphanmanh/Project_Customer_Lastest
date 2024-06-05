using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class UpdateADEntityAndUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFromAD",
                table: "AbpUsers");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "AbpUsers",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MstEsignActiveDirectory",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "AbpUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ADId",
                table: "AbpUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "AbpUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AbpUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Office",
                table: "AbpUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AbpUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPhone",
                table: "AbpUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MstEsignActiveDirectory");

            migrationBuilder.DropColumn(
                name: "ADId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "WorkPhone",
                table: "AbpUsers");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AbpUsers",
                newName: "EmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFromAD",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
