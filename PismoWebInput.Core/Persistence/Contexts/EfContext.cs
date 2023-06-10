using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PismoWebInput.Core.Infrastructure.Common.Timing;
using PismoWebInput.Core.Infrastructure.Domain.Common;
using PismoWebInput.Core.Infrastructure.Domain.Entities;
using PismoWebInput.Core.Infrastructure.Models.Tuition;
using PismoWebInput.Core.Infrastructure.Services;
using PismoWebInput.Core.Persistence.Extensions;

namespace PismoWebInput.Core.Persistence.Contexts;

public class EfContext : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    private readonly ICurrentUserService _currentUser;

    public EfContext(DbContextOptions<EfContext> options) : base(options)
    {
    }

    public EfContext(DbContextOptions options, ICurrentUserService currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<SubjectClass> SubjectClasses { get; set; }
    public DbSet<RegisCourse> RegisCourses { get; set; }
    public DbSet<Bill> Bills { get; set; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetAuditProperties();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        SetAuditProperties();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void SetAuditProperties()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is ITimeAudited timeEntity)
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Property(nameof(ITimeAudited.CreatedOn)).IsModified = false;
                        timeEntity.ModifiedOn = Clock.Now;
                        break;
                    case EntityState.Added:
                        timeEntity.CreatedOn = Clock.Now;
                        //timeEntity.ModifiedOn = Clock.Now;
                        break;
                }

            if (entry.Entity is ICreationAudited creationEntity)
                switch (entry.State)
                {
                    case EntityState.Added:
                        creationEntity.CreatedOn = Clock.Now;
                        creationEntity.CreatedBy = _currentUser.UserId;
                        break;
                }

            if (entry.Entity is IAudited entity)
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedBy = _currentUser.UserId;
                        //entity.ModifiedBy = _currentUser.UserId;
                        break;
                    case EntityState.Modified:
                        if (string.IsNullOrEmpty(_currentUser.UserId)) break;

                        entity.CreatedBy ??= _currentUser.UserId;
                        entity.ModifiedBy = _currentUser.UserId;
                        break;
                }
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(b =>
        {
            b.HasMany(e => e.UserRoles).WithOne(e => e.User).HasForeignKey(ur => ur.UserId).IsRequired();
        });

        builder.Entity<AppRole>(b =>
        {
            b.HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
        });

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName != null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        builder.UseDefaultConventions();

        builder.Entity<ActivityLog>(b =>
        {
            b.Property(x => x.Detail).HasColumnType("nvarchar(max)");
        });

        builder.HasDbFunction(() => SqlFunctions.JsonValue(default, default));
    }
}