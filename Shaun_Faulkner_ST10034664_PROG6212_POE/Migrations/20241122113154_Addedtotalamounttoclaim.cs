using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class Addedtotalamounttoclaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Claims",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Claims");
        }
    }
}
