using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class updateDecimalToDoubleValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "ProposalContent",
                type: "double",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Proposal",
                type: "double",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2
            );

            _ = migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Item",
                type: "double",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2
            );

            _ = migrationBuilder
                .AlterColumn<string>(
                    name: "Phone",
                    table: "Contact",
                    type: "varchar(25)",
                    maxLength: 25,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(15)",
                    oldMaxLength: 15
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder
                .AlterColumn<string>(
                    name: "Phone",
                    table: "Company",
                    type: "varchar(25)",
                    maxLength: 25,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(15)",
                    oldMaxLength: 15
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder
                .AlterColumn<string>(
                    name: "CNPJ",
                    table: "Company",
                    type: "varchar(20)",
                    maxLength: 20,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(50)",
                    oldMaxLength: 50
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProposalContent",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double"
            );

            _ = migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Proposal",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double",
                oldPrecision: 18,
                oldScale: 2
            );

            _ = migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Item",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double",
                oldPrecision: 18,
                oldScale: 2
            );

            _ = migrationBuilder
                .AlterColumn<string>(
                    name: "Phone",
                    table: "Contact",
                    type: "varchar(15)",
                    maxLength: 15,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(25)",
                    oldMaxLength: 25
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder
                .AlterColumn<string>(
                    name: "Phone",
                    table: "Company",
                    type: "varchar(15)",
                    maxLength: 15,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(25)",
                    oldMaxLength: 25
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder
                .AlterColumn<string>(
                    name: "CNPJ",
                    table: "Company",
                    type: "varchar(50)",
                    maxLength: 50,
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "varchar(20)",
                    oldMaxLength: 20
                )
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
