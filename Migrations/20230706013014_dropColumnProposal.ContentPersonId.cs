using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class dropColumnProposalContentPersonId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(name: "FK_Proposal_Person_PersonId", table: "Proposal");

            _ = migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Proposal",
                newName: "PersonId1"
            );

            _ = migrationBuilder.RenameIndex(
                name: "IX_Proposal_PersonId",
                table: "Proposal",
                newName: "IX_Proposal_PersonId1"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal",
                column: "PersonId1",
                principalTable: "Person",
                principalColumn: "PersonId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal"
            );

            _ = migrationBuilder.RenameColumn(
                name: "PersonId1",
                table: "Proposal",
                newName: "PersonId"
            );

            _ = migrationBuilder.RenameIndex(
                name: "IX_Proposal_PersonId1",
                table: "Proposal",
                newName: "IX_Proposal_PersonId"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId",
                table: "Proposal",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId"
            );
        }
    }
}
