using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEsignSignature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignatureUrl",
                table: "EsignSignature",
                newName: "SignaturePath");

            migrationBuilder.AddColumn<int>(
                name: "SignatureHeight",
                table: "EsignSignature",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SignatureWidth",
                table: "EsignSignature",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureHeight",
                table: "EsignSignature");

            migrationBuilder.DropColumn(
                name: "SignatureWidth",
                table: "EsignSignature");

            migrationBuilder.RenameColumn(
                name: "SignaturePath",
                table: "EsignSignature",
                newName: "SignatureUrl");
        }
    }
}
