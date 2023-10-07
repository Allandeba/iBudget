using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class refactoryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(name: "FK_Contact_Person_PersonId", table: "Contact");

            _ = migrationBuilder.DropForeignKey(name: "FK_Document_Person_PersonId", table: "Document");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal"
            );

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId1",
                table: "ProposalContent"
            );

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent"
            );

            _ = migrationBuilder.DropIndex(
                name: "IX_ProposalContent_ItemId1",
                table: "ProposalContent"
            );

            _ = migrationBuilder.DropIndex(name: "IX_Proposal_PersonId1", table: "Proposal");

            _ = migrationBuilder.DropColumn(name: "ItemId1", table: "ProposalContent");

            _ = migrationBuilder.DropColumn(name: "PersonId1", table: "Proposal");

            _ = migrationBuilder.AlterColumn<int>(
                name: "ProposalId",
                table: "ProposalContent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            _ = migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "ProposalContent",
                type: "int",
                nullable: false,
                defaultValue: 0
            );

            _ = migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Proposal",
                type: "int",
                nullable: false,
                defaultValue: 0
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Document",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ItemId",
                table: "ProposalContent",
                column: "ItemId"
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_Proposal_PersonId",
                table: "Proposal",
                column: "PersonId"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Contact_Person_PersonId",
                table: "Contact",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Document_Person_PersonId",
                table: "Document",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId",
                table: "Proposal",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent",
                column: "ProposalId",
                principalTable: "Proposal",
                principalColumn: "ProposalId",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(name: "FK_Contact_Person_PersonId", table: "Contact");

            _ = migrationBuilder.DropForeignKey(name: "FK_Document_Person_PersonId", table: "Document");

            _ = migrationBuilder.DropForeignKey(name: "FK_Proposal_Person_PersonId", table: "Proposal");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent"
            );

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent"
            );

            _ = migrationBuilder.DropIndex(name: "IX_ProposalContent_ItemId", table: "ProposalContent");

            _ = migrationBuilder.DropIndex(name: "IX_Proposal_PersonId", table: "Proposal");

            _ = migrationBuilder.DropColumn(name: "ItemId", table: "ProposalContent");

            _ = migrationBuilder.DropColumn(name: "PersonId", table: "Proposal");

            _ = migrationBuilder.AlterColumn<int>(
                name: "ProposalId",
                table: "ProposalContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.AddColumn<int>(
                name: "ItemId1",
                table: "ProposalContent",
                type: "int",
                nullable: true
            );

            _ = migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "Proposal",
                type: "int",
                nullable: true
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Document",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Contact",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ItemId1",
                table: "ProposalContent",
                column: "ItemId1"
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_Proposal_PersonId1",
                table: "Proposal",
                column: "PersonId1"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Contact_Person_PersonId",
                table: "Contact",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Document_Person_PersonId",
                table: "Document",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId1",
                table: "Proposal",
                column: "PersonId1",
                principalTable: "Person",
                principalColumn: "PersonId"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId1",
                table: "ProposalContent",
                column: "ItemId1",
                principalTable: "Item",
                principalColumn: "ItemId"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent",
                column: "ProposalId",
                principalTable: "Proposal",
                principalColumn: "ProposalId"
            );
        }
    }
}
