using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimilaritySearch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Ad",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Ad");
        }
    }
}
