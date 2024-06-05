using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class mstaffrefid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AffiliateReferenceId",
                table: "EsignStatusSignerHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AffiliateReferenceId",
                table: "EsignComments",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AffiliateReferenceId",
                table: "EsignStatusSignerHistory");

            migrationBuilder.DropColumn(
                name: "AffiliateReferenceId",
                table: "EsignComments");
        }
    }
}
