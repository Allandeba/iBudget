﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class modifiedModelsDefauls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent"
            );

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent"
            );

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

            _ = migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ProposalContent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
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
            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent"
            );

            _ = migrationBuilder.DropForeignKey(
                name: "FK_ProposalContent_Proposal_ProposalId",
                table: "ProposalContent"
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "ProposalId",
                table: "ProposalContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ProposalContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ProposalContent_Item_ItemId",
                table: "ProposalContent",
                column: "ItemId",
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
