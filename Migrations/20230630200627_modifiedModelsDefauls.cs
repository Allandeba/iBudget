using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class modifiedModelsDefauls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(name: "FK_ItemImage_Item_ItemId", table: "ItemImage");

            _ = migrationBuilder.DropForeignKey(name: "FK_Proposal_Person_PersonId", table: "Proposal");

            _ = migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Proposal",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemImage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true
            );

            _ = migrationBuilder.AlterColumn<byte[]>(
                name: "ImageFile",
                table: "ItemImage",
                type: "longblob",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ItemImage_Item_ItemId",
                table: "ItemImage",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(name: "FK_ItemImage_Item_ItemId", table: "ItemImage");

            _ = migrationBuilder.DropForeignKey(name: "FK_Proposal_Person_PersonId", table: "Proposal");

            _ = migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Proposal",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int"
            );

            _ = migrationBuilder.AlterColumn<byte[]>(
                name: "ImageFile",
                table: "ItemImage",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob"
            );

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ItemImage_Item_ItemId",
                table: "ItemImage",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId"
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
