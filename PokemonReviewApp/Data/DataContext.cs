using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext:IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {

        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Pokemon> Pokemons { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategeroy> PokemonCategeroies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            modelBuilder.Entity < IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<PokemonCategeroy>()
                 .HasKey(pc => new { pc.PokemonId, pc.CategeroyId});
            modelBuilder.Entity<PokemonCategeroy>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategeroies)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategeroy>()
               .HasOne(p => p.Category)
               .WithMany(pc => pc.PokemonCategeroies)
               .HasForeignKey(c => c.CategeroyId);


            modelBuilder.Entity<PokemonOwner>()
                 .HasKey(po => new { po.OwnerId, po.PokemonId });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
               .HasOne(p => p.Owner)
               .WithMany(pc => pc.PokemonOwners)
               .HasForeignKey(p => p.OwnerId);


        }

    }
}


