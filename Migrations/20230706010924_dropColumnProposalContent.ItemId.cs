using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class dropColumnProposalContentItemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent"
            );

            _ = migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "ProposalContent",
                newName: "ItemId1"
            );

            _ = migrationBuilder.RenameIndex(
                name: "IX_ProposalContent_ItemId",
                table: "ProposalContent",
                newName: "IX_ProposalContent_ItemId1"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId1",
                table: "ProposalContent",
                column: "ItemId1",
                principalTable: "Item",
                principalColumn: "ItemId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId1",
                table: "ProposalContent"
            );

            _ = migrationBuilder.RenameColumn(
                name: "ItemId1",
                table: "ProposalContent",
                newName: "ItemId"
            );

            _ = migrationBuilder.RenameIndex(
                name: "IX_ProposalContent_ItemId1",
                table: "ProposalContent",
                newName: "IX_ProposalContent_ItemId"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId"
            );
        }
    }
}
