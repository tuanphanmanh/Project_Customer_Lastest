using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class DocumentListAddEncryptionIsInput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUserPassInput",
                table: "EsignDocumentListTemp",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUserPassInput",
                table: "EsignDocumentList",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUserPassInput",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "IsUserPassInput",
                table: "EsignDocumentList");
        }
    }
}
