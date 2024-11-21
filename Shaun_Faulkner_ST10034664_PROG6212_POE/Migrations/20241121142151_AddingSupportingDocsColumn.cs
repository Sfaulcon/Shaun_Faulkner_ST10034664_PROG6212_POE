using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class AddingSupportingDocsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupportingDocuments",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportingDocuments",
                table: "Claims");
        }
    }
}
