using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class AddColSignerUserIdTextPropertyTblEsignPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "BackGroundColor",
                table: "EsignPosition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "EsignPosition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontFamily",
                table: "EsignPosition",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FontSize",
                table: "EsignPosition",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBold",
                table: "EsignPosition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsItalic",
                table: "EsignPosition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnderline",
                table: "EsignPosition",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "SingerUserId",
                table: "EsignPosition",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextAlignment",
                table: "EsignPosition",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackGroundColor",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "FontFamily",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "FontSize",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "IsBold",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "IsItalic",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "IsUnderline",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "SingerUserId",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "TextAlignment",
                table: "EsignPosition");
        }
    }
}
