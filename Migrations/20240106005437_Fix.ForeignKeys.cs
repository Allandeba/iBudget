using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace iBudget.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemImage_Item_ItemImageId",
                table: "ItemImage"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Person_ProposalId",
                table: "Proposal"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ProposalContentId",
                table: "ProposalContent"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalHistory_Proposal_ProposalHistoryId",
                table: "ProposalHistory"
            );

            migrationBuilder
                .AlterColumn<int>(
                    name: "ProposalHistoryId",
                    table: "ProposalHistory",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder.AddColumn<int>(
                name: "ProposalId",
                table: "ProposalHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder
                .AlterColumn<int>(
                    name: "ProposalContentId",
                    table: "ProposalContent",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder
                .AlterColumn<int>(
                    name: "ProposalId",
                    table: "Proposal",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder
                .AlterColumn<int>(
                    name: "ItemImageId",
                    table: "ItemImage",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalHistory_ProposalId",
                table: "ProposalHistory",
                column: "ProposalId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ItemId",
                table: "ProposalContent",
                column: "ItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_PersonId",
                table: "Proposal",
                column: "PersonId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ItemImage_ItemId",
                table: "ItemImage",
                column: "ItemId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImage_Item_ItemId",
                table: "ItemImage",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_PersonId",
                table: "Proposal",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalHistory_Proposal_ProposalId",
                table: "ProposalHistory",
                column: "ProposalId",
                principalTable: "Proposal",
                principalColumn: "ProposalId",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_ItemImage_Item_ItemId", table: "ItemImage");

            migrationBuilder.DropForeignKey(name: "FK_Proposal_Person_PersonId", table: "Proposal");

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ProposalHistory_Proposal_ProposalId",
                table: "ProposalHistory"
            );

            migrationBuilder.DropIndex(
                name: "IX_ProposalHistory_ProposalId",
                table: "ProposalHistory"
            );

            migrationBuilder.DropIndex(name: "IX_ProposalContent_ItemId", table: "ProposalContent");

            migrationBuilder.DropIndex(name: "IX_Proposal_PersonId", table: "Proposal");

            migrationBuilder.DropIndex(name: "IX_ItemImage_ItemId", table: "ItemImage");

            migrationBuilder.DropColumn(name: "ProposalId", table: "ProposalHistory");

            migrationBuilder
                .AlterColumn<int>(
                    name: "ProposalHistoryId",
                    table: "ProposalHistory",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .OldAnnotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder
                .AlterColumn<int>(
                    name: "ProposalContentId",
                    table: "ProposalContent",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .OldAnnotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder
                .AlterColumn<int>(
                    name: "ProposalId",
                    table: "Proposal",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .OldAnnotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder
                .AlterColumn<int>(
                    name: "ItemImageId",
                    table: "ItemImage",
                    type: "integer",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "integer"
                )
                .OldAnnotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                );

            migrationBuilder.AddForeignKey(
                name: "FK_ItemImage_Item_ItemImageId",
                table: "ItemImage",
                column: "ItemImageId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Person_ProposalId",
                table: "Proposal",
                column: "ProposalId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ProposalContentId",
                table: "ProposalContent",
                column: "ProposalContentId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ProposalHistory_Proposal_ProposalHistoryId",
                table: "ProposalHistory",
                column: "ProposalHistoryId",
                principalTable: "Proposal",
                principalColumn: "ProposalId",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
