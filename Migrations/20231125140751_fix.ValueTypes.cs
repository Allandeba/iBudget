using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iBudget.Migrations
{
    /// <inheritdoc />
    public partial class fixValueTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProposalContentJSON_ProposalContentItems",
                table: "ProposalHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProposalContent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision"
            );

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Proposal",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 18,
                oldScale: 2
            );

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "LoginLog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<string>(
                name: "RemoteIpAddress",
                table: "LoginLog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "LoginLog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageFile",
                table: "ItemImage",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea"
            );

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Item",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 18,
                oldScale: 2
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProposalContentJSON_ProposalContentItems",
                table: "ProposalHistory",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "ProposalContent",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Proposal",
                type: "double precision",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2
            );

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "LoginLog",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "RemoteIpAddress",
                table: "LoginLog",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "LoginLog",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageFile",
                table: "ItemImage",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Item",
                type: "double precision",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2
            );
        }
    }
}
