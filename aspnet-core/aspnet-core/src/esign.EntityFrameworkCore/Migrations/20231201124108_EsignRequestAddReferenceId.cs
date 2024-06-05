using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class EsignRequestAddReferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReferenceId",
                table: "EsignSignerList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReferenceId",
                table: "EsignPosition",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReferenceId",
                table: "EsignDocumentList",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "EsignSignerList");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "EsignPosition");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "EsignDocumentList");
        }
    }
}
