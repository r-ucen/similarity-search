using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimilaritySearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReadyToBePresentedToAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReadyToBePresented",
                table: "Ad",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadyToBePresented",
                table: "Ad");
        }
    }
}
