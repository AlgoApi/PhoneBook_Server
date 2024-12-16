using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain.Entities;

namespace PhoneBook.Domain
{
    public class DataContext: DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Phonebook> Phonebook { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи "один ко многим"
            modelBuilder.Entity<Phonebook>()
                .HasOne(p => p.User)
                .WithMany(u => u.Phonebook)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление

            modelBuilder.Entity<Phonebook>()
                .Navigation(p => p.User)
                .IsRequired(false);
        }

        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
