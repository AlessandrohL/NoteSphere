using Application.Abstractions;
using Application.Identity;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Infrastructure.Identity;
using Infrastructure.Identity.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<UserAuth>
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly Guid _tenant;
        private bool filtersDisabled = false;
        private bool tenantFilterEnabled = true;
        private bool softDeleteFilterEnabled = true;

        public DbSet<Notebook> NoteBooks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoteTag> NoteTags { get; set; }
        //public DbSet<Todo> Todos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
            _tenant = tenantProvider.GetTenantId();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AssignTenantToCreationEntities();
            UpdateModifiedTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateModifiedTimestamps()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                if (entry.Entity is BaseEntity trackableEntity)
                {
                    trackableEntity.SetModified();
                }
            }
        }

        private void AssignTenantToCreationEntities()
        {
            var createdEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in createdEntities)
            { 
                if (entry.Entity is ITenantEntity trackableTenantEntity 
                    && _tenant != Guid.Empty && trackableTenantEntity.TenantId == Guid.Empty)
                {
                    trackableTenantEntity.AssignTenant(_tenant!);
                }
            }
        }

        //private void ConfigureTenantEntity<TEntity>(ModelBuilder modelBuilder)
        //    where TEntity : class, ITenantEntity
        //{
        //    modelBuilder.Entity<TEntity>(builder =>
        //    {
        //        builder
        //            .Property(t => t.TenantId)
        //            .IsRequired();

        //        builder.HasIndex(u => u.TenantId);
        //    });
        //}

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.ApplyConfiguration(new UserConfiguration());
            modelbuilder.ApplyConfiguration(new UserClaimConfiguration());
            modelbuilder.ApplyConfiguration(new RolesConfiguration());
            modelbuilder.ApplyConfiguration(new RoleClaimsConfiguration());

            modelbuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelbuilder.ApplyConfiguration(new NotebookConfiguration());
            modelbuilder.ApplyConfiguration(new NoteConfiguration());
            modelbuilder.ApplyConfiguration(new TagConfiguration());
            modelbuilder.ApplyConfiguration(new NoteTagConfiguration());

            ApplyQueryFilters(modelbuilder);

            //builder.ApplyConfiguration(new TodoConfiguration());

            // Seed
            //modelbuilder.ApplyConfiguration(new NotesSeed());
        }

        private void ApplyQueryFilters(ModelBuilder modelBuilder)
        {
            var clrTypes = modelBuilder.Model.GetEntityTypes()
                .Select(et => et.ClrType)
                .ToList();

            var baseFilter = (Expression<Func<BaseEntity, bool>>)(_ => filtersDisabled);
            var tenantFilter = (Expression<Func<ITenantEntity, bool>>)(e => !tenantFilterEnabled || e.TenantId == _tenant);
            var softDeleteFilter = (Expression<Func<ISoftDeleteEntity, bool>>)(e => !softDeleteFilterEnabled || e.IsDeleted == false);

            foreach (var type in clrTypes)
            {
                var filters = new List<LambdaExpression>();

                if (typeof(ITenantEntity).IsAssignableFrom(type))
                    filters.Add(tenantFilter);
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(type))
                    filters.Add(softDeleteFilter);

                var queryFilter = CombineQueryFilters(type, baseFilter, filters);
                modelBuilder.Entity(type).HasQueryFilter(queryFilter);
            }
        }

        private LambdaExpression CombineQueryFilters(
            Type entityType,
            LambdaExpression baseFilter, 
            IEnumerable<LambdaExpression> andAlsoExpressions)
        {
            var newParam = Expression.Parameter(entityType);

            var andAlsoExprBase = (Expression<Func<BaseEntity, bool>>)(_ => true);
            var andAlsoExpr = ReplacingExpressionVisitor.Replace(andAlsoExprBase.Parameters.Single(), newParam, andAlsoExprBase.Body);
            foreach (var expressionBase in andAlsoExpressions)
            {
                var expression = ReplacingExpressionVisitor.Replace(expressionBase.Parameters.Single(), newParam, expressionBase.Body);
                andAlsoExpr = Expression.AndAlso(andAlsoExpr, expression);
            }

            var baseExp = ReplacingExpressionVisitor.Replace(baseFilter.Parameters.Single(), newParam, baseFilter.Body);
            var exp = Expression.OrElse(baseExp, andAlsoExpr);

            return Expression.Lambda(exp, newParam);
        }
    }
}
