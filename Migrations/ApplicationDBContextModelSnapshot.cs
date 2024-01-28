﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using iBudget.DAO;

#nullable disable

namespace iBudget.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("iBudget.DAO.Entities.CompanyModel", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("Address");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("CNPJ");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("CompanyName");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnName("Email");

                    b.Property<byte[]>("ImageFile")
                        .HasColumnType("bytea")
                        .HasColumnName("ImageFile");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("VARCHAR(25)")
                        .HasColumnName("Phone");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ContactModel", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ContactId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR(50)")
                        .HasColumnName("Email");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("VARCHAR(25)")
                        .HasColumnName("Phone");

                    b.HasKey("ContactId");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Contacts", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.DocumentModel", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DocumentId"));

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Document");

                    b.Property<int>("DocumentType")
                        .HasColumnType("integer")
                        .HasColumnName("DocumentType");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.HasKey("DocumentId");

                    b.HasIndex("Document")
                        .IsUnique();

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Documents", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ItemImageModel", b =>
                {
                    b.Property<int>("ItemImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ItemImageId"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("FileName");

                    b.Property<byte[]>("ImageFile")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("ImageFile");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<bool>("Main")
                        .HasColumnType("boolean")
                        .HasColumnName("Main");

                    b.HasKey("ItemImageId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemImages", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ItemModel", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ItemId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("Description");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("ItemName");

                    b.Property<decimal>("Value")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Value");

                    b.HasKey("ItemId");

                    b.HasIndex("ItemName")
                        .IsUnique();

                    b.ToTable("Items", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.LoginLogModel", b =>
                {
                    b.Property<int>("LoginLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LoginLogId"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp")
                        .HasColumnName("DateTime");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Password");

                    b.Property<string>("RemoteIpAddress")
                        .HasColumnType("text")
                        .HasColumnName("RemoteIpAddress");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("Status")
                        .HasComment("0-None,1-Failed,2-Success");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Username");

                    b.HasKey("LoginLogId");

                    b.ToTable("LoginLogs", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.LoginModel", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LoginId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("Username");

                    b.HasKey("LoginId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Logins", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.PersonModel", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PersonId"));

                    b.Property<int>("ContactId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("DocumentId")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("LastName");

                    b.HasKey("PersonId");

                    b.ToTable("Persons", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ProposalContentModel", b =>
                {
                    b.Property<int>("ProposalContentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProposalContentId"));

                    b.Property<int>("ItemId")
                        .HasColumnType("integer")
                        .HasColumnName("ItemId");

                    b.Property<int?>("ItemModelItemId")
                        .HasColumnType("integer");

                    b.Property<int>("ProposalId")
                        .HasColumnType("integer")
                        .HasColumnName("ProposalId");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("Quantity");

                    b.HasKey("ProposalContentId");

                    b.HasIndex("ItemId");

                    b.HasIndex("ItemModelItemId");

                    b.HasIndex("ProposalId");

                    b.ToTable("ProposalContents", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ProposalHistoryModel", b =>
                {
                    b.Property<int>("ProposalHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProposalHistoryId"));

                    b.Property<decimal>("Discount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Discount");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("timestamp")
                        .HasColumnName("ModificationDate");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer")
                        .HasColumnName("PersonId");

                    b.Property<int>("ProposalId")
                        .HasColumnType("integer")
                        .HasColumnName("ProposalId");

                    b.HasKey("ProposalHistoryId");

                    b.HasIndex("PersonId");

                    b.HasIndex("ProposalId");

                    b.ToTable("ProposalHistories", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ProposalModel", b =>
                {
                    b.Property<int>("ProposalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProposalId"));

                    b.Property<decimal>("Discount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Discount");

                    b.Property<Guid>("GUID")
                        .HasColumnType("uuid")
                        .HasColumnName("GUID");

                    b.Property<DateTime>("ModificationDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasColumnName("ModificationDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int?>("PersonModelPersonId")
                        .HasColumnType("integer");

                    b.HasKey("ProposalId");

                    b.HasIndex("GUID")
                        .IsUnique();

                    b.HasIndex("PersonId");

                    b.HasIndex("PersonModelPersonId");

                    b.ToTable("Proposals", (string)null);
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ContactModel", b =>
                {
                    b.HasOne("iBudget.DAO.Entities.PersonModel", "Person")
                        .WithOne("Contact")
                        .HasForeignKey("iBudget.DAO.Entities.ContactModel", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("iBudget.DAO.Entities.DocumentModel", b =>
                {
                    b.HasOne("iBudget.DAO.Entities.PersonModel", "Person")
                        .WithOne("Document")
                        .HasForeignKey("iBudget.DAO.Entities.DocumentModel", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ItemImageModel", b =>
                {
                    b.HasOne("iBudget.DAO.Entities.ItemModel", "Item")
                        .WithMany("ItemImageList")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ProposalContentModel", b =>
                {
                    b.HasOne("iBudget.DAO.Entities.ItemModel", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("iBudget.DAO.Entities.ItemModel", null)
                        .WithMany("ProposalContent")
                        .HasForeignKey("ItemModelItemId");

                    b.HasOne("iBudget.DAO.Entities.ProposalModel", "Proposal")
                        .WithMany("ProposalContent")
                        .HasForeignKey("ProposalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Proposal");
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ProposalHistoryModel", b =>
                {
                    b.HasOne("iBudget.DAO.Entities.PersonModel", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("iBudget.DAO.Entities.ProposalModel", "Proposal")
                        .WithMany("ProposalHistory")
                        .HasForeignKey("ProposalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("iBudget.DAO.Entities.ProposalContentJSON", "ProposalContentJSON", b1 =>
                        {
                            b1.Property<int>("ProposalHistoryModelProposalHistoryId")
                                .HasColumnType("integer");

                            b1.Property<string>("ProposalContentItems")
                                .HasColumnType("text");

                            b1.HasKey("ProposalHistoryModelProposalHistoryId");

                            b1.ToTable("ProposalHistories");

                            b1.WithOwner()
                                .HasForeignKey("ProposalHistoryModelProposalHistoryId");
                        });

                    b.Navigation("Person");

                    b.Navigation("Proposal");

                    b.Navigation("ProposalContentJSON")
                        .IsRequired();
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ProposalModel", b =>
                {
                    b.HasOne("iBudget.DAO.Entities.PersonModel", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("iBudget.DAO.Entities.PersonModel", null)
                        .WithMany("Proposal")
                        .HasForeignKey("PersonModelPersonId");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ItemModel", b =>
                {
                    b.Navigation("ItemImageList");

                    b.Navigation("ProposalContent");
                });

            modelBuilder.Entity("iBudget.DAO.Entities.PersonModel", b =>
                {
                    b.Navigation("Contact")
                        .IsRequired();

                    b.Navigation("Document")
                        .IsRequired();

                    b.Navigation("Proposal");
                });

            modelBuilder.Entity("iBudget.DAO.Entities.ProposalModel", b =>
                {
                    b.Navigation("ProposalContent");

                    b.Navigation("ProposalHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
