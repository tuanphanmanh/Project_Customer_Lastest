using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatalengthROITblEsignRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ROI",
                table: "EsignRequest",
                type: "decimal(6,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,3)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ROI",
                table: "EsignRequest",
                type: "decimal(3,3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,3)",
                oldNullable: true);
        }
    }
}
