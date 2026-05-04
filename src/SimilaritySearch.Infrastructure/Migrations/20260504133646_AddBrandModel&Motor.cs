using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimilaritySearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandModelMotor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandModel",
                table: "Ad",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Motor",
                table: "Ad",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandModel",
                table: "Ad");

            migrationBuilder.DropColumn(
                name: "Motor",
                table: "Ad");
        }
    }
}
