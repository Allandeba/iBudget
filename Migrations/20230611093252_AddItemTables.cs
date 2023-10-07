using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class AddItemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder
                .CreateTable(
                    name: "Item",
                    columns: table =>
                        new
                        {
                            ItemId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            ItemName = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Value = table.Column<decimal>(
                                type: "decimal(18,2)",
                                precision: 18,
                                scale: 2,
                                nullable: false
                            )
                        },
                    constraints: table =>
                    {
                        _ = table.PrimaryKey("PK_Item", x => x.ItemId);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder
                .CreateTable(
                    name: "ItemImage",
                    columns: table =>
                        new
                        {
                            ItemImageId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            Principal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                            FileName = table
                                .Column<string>(
                                    type: "varchar(100)",
                                    maxLength: 100,
                                    nullable: false
                                )
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            ImageFile = table.Column<byte[]>(type: "longblob", nullable: false),
                            ItemId = table.Column<int>(type: "int", nullable: true)
                        },
                    constraints: table =>
                    {
                        _ = table.PrimaryKey("PK_ItemImage", x => x.ItemImageId);
                        _ = table.ForeignKey(
                            name: "FK_ItemImage_Item_ItemId",
                            column: x => x.ItemId,
                            principalTable: "Item",
                            principalColumn: "ItemId"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ItemImage_ItemId",
                table: "ItemImage",
                column: "ItemId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(name: "ItemImage");

            _ = migrationBuilder.DropTable(name: "Item");
        }
    }
}
