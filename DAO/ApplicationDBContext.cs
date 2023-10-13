using iBudget.Framework;
using iBudget.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace iBudget.DAO
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<PersonModel> Person { get; set; }
        public DbSet<DocumentModel> Document { get; set; }
        public DbSet<ContactModel> Contact { get; set; }
        public DbSet<ItemModel> Item { get; set; }
        public DbSet<ItemImageModel> ItemImage { get; set; }
        public DbSet<ProposalModel> Proposal { get; set; }
        public DbSet<ProposalContentModel> ProposalContent { get; set; }
        public DbSet<ProposalHistoryModel> ProposalHistory { get; set; }
        public DbSet<CompanyModel> Company { get; set; }
        public DbSet<LoginModel> Login { get; set; }
        public DbSet<LoginLogModel> LoginLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            PersonInformation(modelBuilder);
            DocumentInformation(modelBuilder);
            ContactInformation(modelBuilder);
            ProposalInformation(modelBuilder);
            ItemInformation(modelBuilder);
            ProposalHistoryInformation(modelBuilder);
            LoginInformation(modelBuilder);
            LoginLogInformation(modelBuilder);
        }

        private void PersonInformation(ModelBuilder modelBuilder)
        {
            _ = modelBuilder
                .Entity<PersonModel>()
                .Property(p => p.CreationDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            _ = modelBuilder
                .Entity<PersonModel>()
                .HasOne(PersonModel => PersonModel.Document)
                .WithOne(d => d.Person)
                .HasForeignKey<DocumentModel>(d => d.PersonId);

            _ = modelBuilder
                .Entity<PersonModel>()
                .HasOne(PersonModel => PersonModel.Contact)
                .WithOne(c => c.Person)
                .HasForeignKey<ContactModel>(c => c.PersonId);
        }

        private void DocumentInformation(ModelBuilder modelBuilder)
        {
            _ = modelBuilder
                .Entity<DocumentModel>()
                .HasOne(d => d.Person)
                .WithOne(p => p.Document)
                .HasForeignKey<DocumentModel>(d => d.PersonId);
        }

        private void ContactInformation(ModelBuilder modelBuilder)
        {
            _ = modelBuilder
                .Entity<ContactModel>()
                .HasOne(d => d.Person)
                .WithOne(p => p.Contact)
                .HasForeignKey<ContactModel>(d => d.PersonId);
        }

        private void ProposalInformation(ModelBuilder modelBuilder)
        {
            _ = modelBuilder
                .Entity<ProposalModel>()
                .Property(p => p.ModificationDate)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            _ = modelBuilder.Entity<ProposalModel>().HasIndex(p => p.GUID).IsUnique();
        }

        private void ItemInformation(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<ItemModel>().HasIndex(p => p.ItemName).IsUnique();
        }

        private void ProposalHistoryInformation(ModelBuilder modelBuilder)
        {
            _ = modelBuilder
                .Entity<ProposalHistoryModel>()
                .OwnsOne(
                    ph => ph.ProposalContentJSON,
                    ownedNavigationBuilder =>
                    {
                        _ = ownedNavigationBuilder
                            .Property(pc => pc.ProposalContentItems)
                            .HasConversion(
                                v => Newtonsoft.Json.JsonConvert.SerializeObject(v),
                                v =>
                                    Newtonsoft.Json.JsonConvert.DeserializeObject<
                                        List<ProposalContentItems>
                                    >(v),
                                new ValueComparer<List<ProposalContentItems>>(
                                    (c1, c2) =>
                                        Newtonsoft.Json.JsonConvert.SerializeObject(c1)
                                        == Newtonsoft.Json.JsonConvert.SerializeObject(c2),
                                    c =>
                                        c == null
                                            ? 0
                                            : Newtonsoft.Json.JsonConvert
                                                .SerializeObject(c)
                                                .GetHashCode(),
                                    c =>
                                        Newtonsoft.Json.JsonConvert.DeserializeObject<
                                            List<ProposalContentItems>
                                        >(Newtonsoft.Json.JsonConvert.SerializeObject(c))
                                )
                            );
                    }
                );
        }

        private void LoginInformation(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<LoginModel>().HasIndex(p => p.Username).IsUnique();
        }

        private void LoginLogInformation(ModelBuilder modelBuilder)
        {
            List<string> enumValues = Enum.GetValues(typeof(LoginLogStatus))
                .Cast<LoginLogStatus>()
                .Select(e => $"{(int)e}-{e}")
                .ToList();

            string comment = string.Join(",", enumValues);

            _ = modelBuilder.Entity<LoginLogModel>().Property(l => l.Status).HasComment(comment);
        }
    }
}
