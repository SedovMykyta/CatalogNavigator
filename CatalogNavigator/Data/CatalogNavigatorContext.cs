using CatalogNavigator.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogNavigator.Data;

public class CatalogNavigatorContext : DbContext
{
    public CatalogNavigatorContext(DbContextOptions<CatalogNavigatorContext> options) : base(options)
    {
    }

    public DbSet<Folder> Folders { get; set; }
}