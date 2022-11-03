using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GES.Inside.Data.DataContexts
{
    public class GesRefreshDbContext : IdentityDbContext<GesUser, GesRole, string, GesUserLogin, GesUserRole, GesUserClaim>
    {
        private const string SchemaName = "ges";

        public IDbSet<Glossary> Glossaries { get; set; }
        public IDbSet<GesUserRole> UserRoles { get; set; }
        public IDbSet<GesForm> GesForm { get; set; }
        public IDbSet<GesRolePermission> GesRolePermission { get; set; }
        public IDbSet<PersonalSettingCategories> PersonalSettingCategories { get; set; }
        public IDbSet<PersonalSettings> PersonalSettings { get; set; }

        public GesRefreshDbContext() : base("GesRefresh")
        {

        }

        public static GesRefreshDbContext Create()
        {
            return new GesRefreshDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GesUser>()
                .ToTable("Users", SchemaName);

            modelBuilder.Entity<GesRole>()
                .ToTable("Roles", SchemaName);

            modelBuilder.Entity<GesUserRole>()
                .ToTable("UserRoles", SchemaName);

            modelBuilder.Entity<GesUserClaim>()
                .ToTable("UserClaims", SchemaName);

            modelBuilder.Entity<GesUserLogin>()
                .ToTable("UserLogins", SchemaName);

            modelBuilder.Entity<GesRolePermission>()
                .ToTable("GesRolePermission", SchemaName);

            modelBuilder.Entity<GesRolePermission>()
                .HasRequired(d => d.GesRole)
                .WithMany()
                .HasForeignKey(d => d.GesRoleId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<GesRolePermission>()
                .HasRequired(d=>d.GesForm)
                .WithMany()
                .HasForeignKey(d=>d.GesFormId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<GesForm>()
                .ToTable("GesForm", SchemaName);

            modelBuilder.Entity<Glossary>()
                .ToTable("Glossaries", SchemaName);

            modelBuilder.Entity<PersonalSettingCategories>()
                .ToTable("PersonalSettingCategories", SchemaName);

            modelBuilder.Entity<PersonalSettings>()
                .ToTable("PersonalSettings", SchemaName);

            modelBuilder.Entity<PersonalSettings>()
                .HasRequired(d => d.PersonalSettingCategories)
                .WithMany()
                .HasForeignKey(d => d.PersonalSettingCategoryId)
                .WillCascadeOnDelete(true);

        }
    }
}