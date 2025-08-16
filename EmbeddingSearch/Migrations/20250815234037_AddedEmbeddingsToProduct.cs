using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace EmbeddingSearch.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmbeddingsToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.AddColumn<Vector>(
                name: "Embeddings",
                table: "Products",
                type: "vector",
                precision: 1536,
                nullable: true,
                comment: "Embeddings for product search");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Embeddings",
                table: "Products");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:vector", ",,");
        }
    }
}
