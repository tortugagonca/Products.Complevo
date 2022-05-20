using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Products.Complevo.Infrastructure.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ManageyMigrationsOnStartupExtensions
    {
        public static bool AllMigrationsApplied(this DbContext context)
        {
            var migrationService = context.GetService<IHistoryRepository>();
            var migrationServiceAssembly = context.GetService<IMigrationsAssembly>();

            var applied = migrationService
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var migrations = migrationServiceAssembly
                .Migrations
                .Select(m => m.Key);

            return !migrations.Except(applied).Any();
        }

        public static bool DatabaseInMemory(this DbContext context)
            => context.Database?.IsInMemory() == true;
    }
}
