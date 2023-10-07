using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class addTableProposal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<byte[]>(
                name: "ImageFile",
                table: "ItemImage",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob"
            );

            _ = migrationBuilder
                .CreateTable(
                    name: "Proposal",
                    columns: table =>
                        new
                        {
                            ProposalId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            ModificationDate = table
                                .Column<DateTime>(
                                    type: "timestamp",
                                    nullable: false,
                                    defaultValueSql: "CURRENT_TIMESTAMP"
                                )
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.ComputedColumn
                                ),
                            PersonId = table.Column<int>(type: "int", nullable: true)
                        },
                    constraints: table =>
                    {
                        _ = table.PrimaryKey("PK_Proposal", x => x.ProposalId);
                        _ = table.ForeignKey(
                            name: "FK_Proposal_Person_PersonId",
                            column: x => x.PersonId,
                            principalTable: "Person",
                            principalColumn: "PersonId"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder
                .CreateTable(
                    name: "ProposalContent",
                    columns: table =>
                        new
                        {
                            ProposalContentId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            ProposalId = table.Column<int>(type: "int", nullable: true),
                            ItemId = table.Column<int>(type: "int", nullable: true)
                        },
                    constraints: table =>
                    {
                        _ = table.PrimaryKey("PK_ProposalContent", x => x.ProposalContentId);
                        _ = table.ForeignKey(
                            name: "FK_ProposalContent_Item_ItemId",
                            column: x => x.ItemId,
                            principalTable: "Item",
                            principalColumn: "ItemId"
                        );
                        _ = table.ForeignKey(
                            name: "FK_ProposalContent_Proposal_ProposalId",
                            column: x => x.ProposalId,
                            principalTable: "Proposal",
                            principalColumn: "ProposalId"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Proposal_PersonId",
                table: "Proposal",
                column: "PersonId"
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ItemId",
                table: "ProposalContent",
                column: "ItemId"
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ProposalId",
                table: "ProposalContent",
                column: "ProposalId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(name: "ProposalContent");

            _ = migrationBuilder.DropTable(name: "Proposal");

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
        }
    }
}
