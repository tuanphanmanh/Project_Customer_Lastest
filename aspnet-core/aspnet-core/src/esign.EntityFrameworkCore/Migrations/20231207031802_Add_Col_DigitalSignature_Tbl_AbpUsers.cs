using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class AddColDigitalSignatureTblAbpUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DigitalSignaturePin",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DigitalSignatureUuid",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDigitalSignature",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDigitalSignatureOtp",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DigitalSignaturePin",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DigitalSignatureUuid",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsDigitalSignature",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsDigitalSignatureOtp",
                table: "AbpUsers");
        }
    }
}
