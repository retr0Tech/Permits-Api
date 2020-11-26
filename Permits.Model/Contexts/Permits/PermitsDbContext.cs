using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Permits.Model.Entities;
using Permits.Core.Models;

namespace Permits.Model.Contexts.Permits
{
    public class PermitsDbContext : BaseDbContext, IPermitsDbContext
    {

        public PermitsDbContext(DbContextOptions<PermitsDbContext> options) : base(options)
        {
        }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<PermitType> PermitTypes { get; set; }
        public DbSet<T> GetDbSet<T>() where T : class, IBaseEntity => Set<T>();

        public override EntityEntry<T> Entry<T>(T entity)
        {
            return base.Entry(entity);
        }

    }
}
