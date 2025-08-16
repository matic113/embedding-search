using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace EmbeddingSearch.Migrations
{
    /// <inheritdoc />
    public partial class AddedHNSWIndexToProductsEmbeddings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Vector>(
                name: "Embeddings",
                table: "Products",
                type: "vector(1536)",
                precision: 1536,
                nullable: true,
                comment: "Embeddings for product search",
                oldClrType: typeof(Vector),
                oldType: "vector",
                oldPrecision: 1536,
                oldNullable: true,
                oldComment: "Embeddings for product search");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Embeddings",
                table: "Products",
                column: "Embeddings")
                .Annotation("Npgsql:IndexMethod", "hnsw")
                .Annotation("Npgsql:IndexOperators", new[] { "vector_cosine_ops" })
                .Annotation("Npgsql:StorageParameter:ef_construction", 64)
                .Annotation("Npgsql:StorageParameter:m", 16);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Embeddings",
                table: "Products");

            migrationBuilder.AlterColumn<Vector>(
                name: "Embeddings",
                table: "Products",
                type: "vector",
                precision: 1536,
                nullable: true,
                comment: "Embeddings for product search",
                oldClrType: typeof(Vector),
                oldType: "vector(1536)",
                oldPrecision: 1536,
                oldNullable: true,
                oldComment: "Embeddings for product search");
        }
    }
}
