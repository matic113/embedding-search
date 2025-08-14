using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmbeddingSearch.Migrations
{
    /// <inheritdoc />
    public partial class AddFTSIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_Description",
                table: "Products",
                columns: new[] { "Name", "Description" })
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name_Description",
                table: "Products");
        }
    }
}
