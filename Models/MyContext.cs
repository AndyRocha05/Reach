using Microsoft.EntityFrameworkCore;
namespace Reach.Models{

public class MyContext : DbContext{

    public MyContext(DbContextOptions Options) : base(Options){}

    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }

    public DbSet<Church> Churches { get; set; }
    public DbSet<Organization> Organizations { get; set; }


}
}