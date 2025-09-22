using Microsoft.EntityFrameworkCore;
using UniversalSolution.Domain.Entities;

namespace UniversalSolution.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts) { }

    public DbSet<User> Users { get; set; } = null!;
}