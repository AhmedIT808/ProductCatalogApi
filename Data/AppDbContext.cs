
using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // تحديد دقة السعر: 18 رقمًا إجمالًا، ورقمان بعد العلامة العشرية
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        // تطبيق Soft Delete: إخفاء المنتجات المحذوفة تلقائيًا
        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.IsDeleted);

        // استخدام RowVersion لاكتشاف تعارض التعديلات
        modelBuilder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

        // إنشاء Index لتحسين عمليات البحث المناسبة باسم المنتج
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name);
    }
}
