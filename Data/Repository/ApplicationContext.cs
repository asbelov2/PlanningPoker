using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
  public class ApplicationContext : DbContext
  {
    public DbSet<Deck> Decks { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Round> Rounds { get; set; }

    public DbSet<User> Users { get; set; }

    public ApplicationContext()
    {
      Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PlanningPokerdb;Trusted_Connection=True;");
    }
  }
}
