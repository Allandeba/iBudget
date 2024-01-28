using iBudget.DAO.Mappings;
using iBudget.DAO.Entities;
using Microsoft.EntityFrameworkCore;

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
            modelBuilder.ApplyConfiguration(new PersonMap());
            modelBuilder.ApplyConfiguration(new DocumentMap());
            modelBuilder.ApplyConfiguration(new ContactMap());
            modelBuilder.ApplyConfiguration(new ItemMap());
            modelBuilder.ApplyConfiguration(new ItemImageMap());
            modelBuilder.ApplyConfiguration(new ProposalMap());
            modelBuilder.ApplyConfiguration(new ProposalContentMap());
            modelBuilder.ApplyConfiguration(new ProposalHistoryMap());
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new LoginMap());
            modelBuilder.ApplyConfiguration(new LoginLogMap());
        }
    }
}
