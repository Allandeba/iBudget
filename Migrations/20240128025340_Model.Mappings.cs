using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace iBudget.Migrations
{
    /// <inheritdoc />
    public partial class ModelMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Company");

            migrationBuilder.DropTable(name: "Contact");

            migrationBuilder.DropTable(name: "Document");

            migrationBuilder.DropTable(name: "ItemImage");

            migrationBuilder.DropTable(name: "Login");

            migrationBuilder.DropTable(name: "LoginLog");

            migrationBuilder.DropTable(name: "ProposalContent");

            migrationBuilder.DropTable(name: "ProposalHistory");

            migrationBuilder.DropTable(name: "Item");

            migrationBuilder.DropTable(name: "Proposal");

            migrationBuilder.DropTable(name: "Person");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table =>
                    new
                    {
                        CompanyId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        CompanyName = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        CNPJ = table.Column<string>(
                            type: "character varying(20)",
                            maxLength: 20,
                            nullable: false
                        ),
                        Address = table.Column<string>(
                            type: "character varying(150)",
                            maxLength: 150,
                            nullable: false
                        ),
                        Email = table.Column<string>(
                            type: "VARCHAR(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Phone = table.Column<string>(
                            type: "VARCHAR(25)",
                            maxLength: 25,
                            nullable: false
                        ),
                        ImageFile = table.Column<byte[]>(type: "bytea", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table =>
                    new
                    {
                        ItemId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        ItemName = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Value = table.Column<decimal>(
                            type: "numeric(18,2)",
                            precision: 18,
                            scale: 2,
                            nullable: false
                        ),
                        Description = table.Column<string>(
                            type: "character varying(250)",
                            maxLength: 250,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                }
            );

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table =>
                    new
                    {
                        LoginLogId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Username = table.Column<string>(type: "text", nullable: false),
                        Password = table.Column<string>(type: "text", nullable: false),
                        RemoteIpAddress = table.Column<string>(type: "text", nullable: true),
                        DateTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                        Status = table.Column<int>(
                            type: "integer",
                            nullable: false,
                            comment: "0-None,1-Failed,2-Success"
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => x.LoginLogId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table =>
                    new
                    {
                        LoginId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Username = table.Column<string>(
                            type: "character varying(25)",
                            maxLength: 25,
                            nullable: false
                        ),
                        Password = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.LoginId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table =>
                    new
                    {
                        PersonId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        FirstName = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        LastName = table.Column<string>(
                            type: "character varying(100)",
                            maxLength: 100,
                            nullable: false
                        ),
                        CreationDate = table.Column<DateTime>(
                            type: "timestamp",
                            nullable: false,
                            defaultValueSql: "CURRENT_TIMESTAMP"
                        ),
                        DocumentId = table.Column<int>(type: "integer", nullable: false),
                        ContactId = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                }
            );

            migrationBuilder.CreateTable(
                name: "ItemImages",
                columns: table =>
                    new
                    {
                        ItemImageId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Main = table.Column<bool>(type: "boolean", nullable: false),
                        FileName = table.Column<string>(
                            type: "character varying(100)",
                            maxLength: 100,
                            nullable: false
                        ),
                        ImageFile = table.Column<byte[]>(type: "bytea", nullable: false),
                        ItemId = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImages", x => x.ItemImageId);
                    table.ForeignKey(
                        name: "FK_ItemImages_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table =>
                    new
                    {
                        ContactId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Email = table.Column<string>(
                            type: "VARCHAR(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Phone = table.Column<string>(
                            type: "VARCHAR(25)",
                            maxLength: 25,
                            nullable: false
                        ),
                        PersonId = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contacts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table =>
                    new
                    {
                        DocumentId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        DocumentType = table.Column<int>(type: "integer", nullable: false),
                        Document = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        PersonId = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table =>
                    new
                    {
                        ProposalId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        ModificationDate = table.Column<DateTime>(
                            type: "timestamp",
                            nullable: false,
                            defaultValueSql: "CURRENT_TIMESTAMP"
                        ),
                        Discount = table.Column<decimal>(
                            type: "numeric(18,2)",
                            precision: 18,
                            scale: 2,
                            nullable: false
                        ),
                        GUID = table.Column<Guid>(type: "uuid", nullable: false),
                        PersonId = table.Column<int>(type: "integer", nullable: false),
                        PersonModelPersonId = table.Column<int>(type: "integer", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.ProposalId);
                    table.ForeignKey(
                        name: "FK_Proposals_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_Proposals_Persons_PersonModelPersonId",
                        column: x => x.PersonModelPersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ProposalContents",
                columns: table =>
                    new
                    {
                        ProposalContentId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Quantity = table.Column<int>(type: "integer", nullable: false),
                        ProposalId = table.Column<int>(type: "integer", nullable: false),
                        ItemId = table.Column<int>(type: "integer", nullable: false),
                        ItemModelItemId = table.Column<int>(type: "integer", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalContents", x => x.ProposalContentId);
                    table.ForeignKey(
                        name: "FK_ProposalContents_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_ProposalContents_Items_ItemModelItemId",
                        column: x => x.ItemModelItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId"
                    );
                    table.ForeignKey(
                        name: "FK_ProposalContents_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "ProposalId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ProposalHistories",
                columns: table =>
                    new
                    {
                        ProposalHistoryId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        ModificationDate = table.Column<DateTime>(
                            type: "timestamp",
                            nullable: false
                        ),
                        Discount = table.Column<decimal>(
                            type: "numeric(18,2)",
                            precision: 18,
                            scale: 2,
                            nullable: false
                        ),
                        ProposalId = table.Column<int>(type: "integer", nullable: false),
                        PersonId = table.Column<int>(type: "integer", nullable: false),
                        ProposalContentJSON_ProposalContentItems = table.Column<string>(
                            type: "text",
                            nullable: true
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalHistories", x => x.ProposalHistoryId);
                    table.ForeignKey(
                        name: "FK_ProposalHistories_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_ProposalHistories_Proposals_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposals",
                        principalColumn: "ProposalId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PersonId",
                table: "Contacts",
                column: "PersonId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Phone",
                table: "Contacts",
                column: "Phone",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Document",
                table: "Documents",
                column: "Document",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PersonId",
                table: "Documents",
                column: "PersonId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_ItemId",
                table: "ItemImages",
                column: "ItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemName",
                table: "Items",
                column: "ItemName",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Logins_Username",
                table: "Logins",
                column: "Username",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContents_ItemId",
                table: "ProposalContents",
                column: "ItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContents_ItemModelItemId",
                table: "ProposalContents",
                column: "ItemModelItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContents_ProposalId",
                table: "ProposalContents",
                column: "ProposalId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalHistories_PersonId",
                table: "ProposalHistories",
                column: "PersonId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalHistories_ProposalId",
                table: "ProposalHistories",
                column: "ProposalId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_GUID",
                table: "Proposals",
                column: "GUID",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_PersonId",
                table: "Proposals",
                column: "PersonId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_PersonModelPersonId",
                table: "Proposals",
                column: "PersonModelPersonId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Companies");

            migrationBuilder.DropTable(name: "Contacts");

            migrationBuilder.DropTable(name: "Documents");

            migrationBuilder.DropTable(name: "ItemImages");

            migrationBuilder.DropTable(name: "LoginLogs");

            migrationBuilder.DropTable(name: "Logins");

            migrationBuilder.DropTable(name: "ProposalContents");

            migrationBuilder.DropTable(name: "ProposalHistories");

            migrationBuilder.DropTable(name: "Items");

            migrationBuilder.DropTable(name: "Proposals");

            migrationBuilder.DropTable(name: "Persons");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table =>
                    new
                    {
                        CompanyId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Address = table.Column<string>(
                            type: "character varying(150)",
                            maxLength: 150,
                            nullable: false
                        ),
                        CNPJ = table.Column<string>(
                            type: "character varying(20)",
                            maxLength: 20,
                            nullable: false
                        ),
                        CompanyName = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Email = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        ImageFile = table.Column<byte[]>(type: "bytea", nullable: true),
                        Phone = table.Column<string>(
                            type: "character varying(25)",
                            maxLength: 25,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table =>
                    new
                    {
                        ItemId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Description = table.Column<string>(
                            type: "character varying(250)",
                            maxLength: 250,
                            nullable: false
                        ),
                        ItemName = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Value = table.Column<decimal>(
                            type: "numeric(18,2)",
                            precision: 18,
                            scale: 2,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table =>
                    new
                    {
                        LoginId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        Password = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Username = table.Column<string>(
                            type: "character varying(25)",
                            maxLength: 25,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.LoginId);
                }
            );

            migrationBuilder.CreateTable(
                name: "LoginLog",
                columns: table =>
                    new
                    {
                        LoginLogId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        DateTime = table.Column<DateTime>(
                            type: "timestamp without time zone",
                            nullable: false
                        ),
                        Password = table.Column<string>(type: "text", nullable: true),
                        RemoteIpAddress = table.Column<string>(type: "text", nullable: true),
                        Status = table.Column<int>(
                            type: "integer",
                            nullable: false,
                            comment: "0-None,1-Failed,2-Success"
                        ),
                        Username = table.Column<string>(type: "text", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLog", x => x.LoginLogId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table =>
                    new
                    {
                        PersonId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        CreationDate = table.Column<DateTime>(
                            type: "timestamp",
                            nullable: false,
                            defaultValueSql: "CURRENT_TIMESTAMP"
                        ),
                        FirstName = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        LastName = table.Column<string>(
                            type: "character varying(100)",
                            maxLength: 100,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonId);
                }
            );

            migrationBuilder.CreateTable(
                name: "ItemImage",
                columns: table =>
                    new
                    {
                        ItemImageId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        ItemId = table.Column<int>(type: "integer", nullable: false),
                        FileName = table.Column<string>(
                            type: "character varying(100)",
                            maxLength: 100,
                            nullable: false
                        ),
                        ImageFile = table.Column<byte[]>(type: "bytea", nullable: true),
                        Main = table.Column<bool>(type: "boolean", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImage", x => x.ItemImageId);
                    table.ForeignKey(
                        name: "FK_ItemImage_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table =>
                    new
                    {
                        ContactId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        PersonId = table.Column<int>(type: "integer", nullable: false),
                        Email = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Phone = table.Column<string>(
                            type: "character varying(25)",
                            maxLength: 25,
                            nullable: false
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_Contact_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table =>
                    new
                    {
                        DocumentId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        PersonId = table.Column<int>(type: "integer", nullable: false),
                        Document = table.Column<string>(
                            type: "character varying(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        DocumentType = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Document_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Proposal",
                columns: table =>
                    new
                    {
                        ProposalId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        PersonId = table.Column<int>(type: "integer", nullable: false),
                        Discount = table.Column<decimal>(
                            type: "numeric(18,2)",
                            precision: 18,
                            scale: 2,
                            nullable: false
                        ),
                        GUID = table.Column<Guid>(type: "uuid", nullable: false),
                        ModificationDate = table.Column<DateTime>(
                            type: "timestamp",
                            nullable: false,
                            defaultValueSql: "CURRENT_TIMESTAMP"
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposal", x => x.ProposalId);
                    table.ForeignKey(
                        name: "FK_Proposal_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ProposalContent",
                columns: table =>
                    new
                    {
                        ProposalContentId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        ItemId = table.Column<int>(type: "integer", nullable: false),
                        ProposalId = table.Column<int>(type: "integer", nullable: false),
                        Quantity = table.Column<int>(type: "integer", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalContent", x => x.ProposalContentId);
                    table.ForeignKey(
                        name: "FK_ProposalContent_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ProposalContent_Proposal_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposal",
                        principalColumn: "ProposalId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ProposalHistory",
                columns: table =>
                    new
                    {
                        ProposalHistoryId = table
                            .Column<int>(type: "integer", nullable: false)
                            .Annotation(
                                "Npgsql:ValueGenerationStrategy",
                                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                            ),
                        PersonId = table.Column<int>(type: "integer", nullable: false),
                        ProposalId = table.Column<int>(type: "integer", nullable: false),
                        Discount = table.Column<decimal>(
                            type: "numeric(18,2)",
                            precision: 18,
                            scale: 2,
                            nullable: false
                        ),
                        ModificationDate = table.Column<DateTime>(
                            type: "timestamp without time zone",
                            nullable: false
                        ),
                        ProposalContentJSON_ProposalContentItems = table.Column<string>(
                            type: "text",
                            nullable: true
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalHistory", x => x.ProposalHistoryId);
                    table.ForeignKey(
                        name: "FK_ProposalHistory_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ProposalHistory_Proposal_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposal",
                        principalColumn: "ProposalId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Contact_PersonId",
                table: "Contact",
                column: "PersonId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Document_PersonId",
                table: "Document",
                column: "PersonId",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemName",
                table: "Item",
                column: "ItemName",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ItemImage_ItemId",
                table: "ItemImage",
                column: "ItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Login_Username",
                table: "Login",
                column: "Username",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_GUID",
                table: "Proposal",
                column: "GUID",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_PersonId",
                table: "Proposal",
                column: "PersonId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ItemId",
                table: "ProposalContent",
                column: "ItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalContent_ProposalId",
                table: "ProposalContent",
                column: "ProposalId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalHistory_PersonId",
                table: "ProposalHistory",
                column: "PersonId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ProposalHistory_ProposalId",
                table: "ProposalHistory",
                column: "ProposalId"
            );
        }
    }
}
