using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getQuote.Migrations
{
    /// <inheritdoc />
    public partial class addProposalHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder
                .CreateTable(
                    name: "ProposalHistory",
                    columns: table =>
                        new
                        {
                            ProposalHistoryId = table
                                .Column<int>(type: "int", nullable: false)
                                .Annotation(
                                    "MySql:ValueGenerationStrategy",
                                    MySqlValueGenerationStrategy.IdentityColumn
                                ),
                            ModificationDate = table.Column<DateTime>(
                                type: "datetime(6)",
                                nullable: false
                            ),
                            Discount = table.Column<decimal>(
                                type: "decimal(18,2)",
                                precision: 18,
                                scale: 2,
                                nullable: false
                            ),
                            ProposalId = table.Column<int>(type: "int", nullable: false),
                            PersonId = table.Column<int>(type: "int", nullable: false),
                            ProposalContentArray = table
                                .Column<string>(type: "longtext", nullable: false)
                                .Annotation("MySql:CharSet", "utf8mb4")
                        },
                    constraints: table =>
                    {
                        _ = table.PrimaryKey("PK_ProposalHistory", x => x.ProposalHistoryId);
                        _ = table.ForeignKey(
                            name: "FK_ProposalHistory_Person_PersonId",
                            column: x => x.PersonId,
                            principalTable: "Person",
                            principalColumn: "PersonId",
                            onDelete: ReferentialAction.Cascade
                        );
                        _ = table.ForeignKey(
                            name: "FK_ProposalHistory_Proposal_ProposalId",
                            column: x => x.ProposalId,
                            principalTable: "Proposal",
                            principalColumn: "ProposalId",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            _ = migrationBuilder.CreateIndex(
                name: "IX_ProposalHistory_PersonId",
                table: "ProposalHistory",
                column: "PersonId"
            );

            _ = migrationBuilder.CreateIndex(
                name: "IX_ProposalHistory_ProposalId",
                table: "ProposalHistory",
                column: "ProposalId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(name: "ProposalHistory");
        }
    }
}
