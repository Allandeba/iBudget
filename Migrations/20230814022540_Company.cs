using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class Company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder
                .CreateTable(
                    name: "Company",
                    columns: table =>
                        new
                        {
                            CompanyId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            CompanyName = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            CNPJ = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Address = table
                                .Column<string>(
                                    type: "varchar(150)",
                                    maxLength: 150,
                                    nullable: false
                                )
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Email = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Phone = table
                                .Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            ImageFile = table.Column<byte[]>(type: "longblob", nullable: true)
                        },
                    constraints: table =>
                    {
                        _ = table.PrimaryKey("PK_Company", x => x.CompanyId);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(name: "Company");
        }
    }
}
