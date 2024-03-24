using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iBudget.Migrations
{
    /// <inheritdoc />
    public partial class loginLogHostnameremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Hostname", table: "LoginLog");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ProposalHistory",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "LoginLog",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ProposalHistory",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "LoginLog",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone"
            );

            migrationBuilder.AddColumn<string>(
                name: "Hostname",
                table: "LoginLog",
                type: "text",
                nullable: false,
                defaultValue: ""
            );
        }
    }
}
