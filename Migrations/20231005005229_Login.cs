using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class Login : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder
                .CreateTable(
                    name: "Login",
                    columns: table =>
                        new
                        {
                            LoginId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            Username = table
                                .Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4"),
                            Password = table
                                .Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4")
                        },
                    constraints: table =>
                    {
                        _ = table.PrimaryKey("PK_Login", x => x.LoginId);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Login_Username",
                table: "Login",
                column: "Username",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(name: "Login");
        }
    }
}
