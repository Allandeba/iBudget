using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class ProposalGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<Guid>(
                name: "GUID",
                table: "Proposal",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci"
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_Proposal_GUID",
                table: "Proposal",
                column: "GUID",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(name: "IX_Proposal_GUID", table: "Proposal");

            _ = migrationBuilder.DropColumn(name: "GUID", table: "Proposal");
        }
    }
}
