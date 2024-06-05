using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class AddColTblAbpUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AbpUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "AbpUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "AbpUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "AbpUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ext",
                table: "AbpUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "AbpUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HrOrgStructureId",
                table: "AbpUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HrPositionId",
                table: "AbpUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HrTitlesId",
                table: "AbpUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "JobBand",
                table: "AbpUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "AbpUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionName",
                table: "AbpUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SectionName",
                table: "AbpUsers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SexName",
                table: "AbpUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TMVEmail",
                table: "AbpUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFullName",
                table: "AbpUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnSignName",
                table: "AbpUsers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Ext",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "HrOrgStructureId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "HrPositionId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "HrTitlesId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "JobBand",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "PositionName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "SectionName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "SexName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "TMVEmail",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "TitleFullName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "UnSignName",
                table: "AbpUsers");
        }
    }
}
