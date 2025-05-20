using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Common;

namespace Template.Domain
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options)
          : base(options)
        {
            base.SavingChanges += OnSavingChanges;
        }

        private void OnSavingChanges(object sender, SavingChangesEventArgs e)
        {
            ConfigureEntityDates();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

        private void ConfigureEntityDates()
        {
            var updatedEntities = ChangeTracker.Entries().Where(x =>
                x.Entity is ITimeModification && x.State == EntityState.Modified).Select(x => x.Entity as ITimeModification);

            var addedEntities = ChangeTracker.Entries().Where(x =>
                x.Entity is ITimeModification && x.State == EntityState.Added).Select(x => x.Entity as ITimeModification);

            foreach (var entity in updatedEntities)
            {
                if (entity != null)
                {
                    entity.ModifiedDate = DateTime.Now;
                }
            }

            foreach (var entity in addedEntities)
            {
                if (entity != null)
                {
                    entity.CreatedTime = DateTime.Now;
                    entity.ModifiedDate = null;
                }
            }
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void RegisterAllEntities<BaseType>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseType).IsAssignableFrom(c));

            foreach (Type type in types)
                modelBuilder.Entity(type);
        }
    }
}

