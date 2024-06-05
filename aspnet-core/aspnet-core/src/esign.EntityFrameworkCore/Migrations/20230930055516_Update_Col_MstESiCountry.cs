using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColMstESiCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "MstESiCountry",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "MstESiCountry",
                newName: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MstESiCountry",
                newName: "CountryName");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "MstESiCountry",
                newName: "CountryCode");
        }
    }
}
