using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Signaller.Models;

namespace Signaller.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ConfigureRoleEntity()
                .ConfigureRoleClaimEntity()
                .ConfigureUserEntity()
                .ConfigureUserClaimEntity()
                .ConfigureUserLoginEntity()
                .ConfigureUserRoleEntity();
        }
    }
    public static class IdentityDbContextExtensions
    {
        public static ModelBuilder ConfigureRoleEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Role>()
                .ToTable("Role");

            modelBuilder
                .Entity<Role>()
                .Property((role) => role.Id)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<Role>()
                .Property((role) => role.Name)
                .IsRequired();

            modelBuilder
                .Entity<Role>()
                .Property((role) => role.NormalizedName)
                .IsRequired();

            modelBuilder
                .Entity<Role>()
                .Property((role) => role.ConcurrencyStamp)
                .IsRequired();

            modelBuilder
                .Entity<Role>()
                .HasKey((role) => role.Id);

            return modelBuilder;
        }

        public static ModelBuilder ConfigureRoleClaimEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<RoleClaim>()
                .ToTable("RoleClaim");

            modelBuilder
                .Entity<RoleClaim>()
                .Property((roleClaim) => roleClaim.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder
                .Entity<RoleClaim>()
                .Property((roleClaim) => roleClaim.RoleId)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<RoleClaim>()
                .Property((roleClaim) => roleClaim.ClaimType)
                .IsRequired();

            modelBuilder
                .Entity<RoleClaim>()
                .Property((roleClaim) => roleClaim.ClaimValue);

            modelBuilder
                .Entity<RoleClaim>()
                .HasKey((roleClaim) => roleClaim.Id);

            modelBuilder
                .Entity<RoleClaim>()
                .HasOne((roleClaim) => roleClaim.Role)
                .WithMany((role) => role.RoleClaims);

            return modelBuilder;
        }

        public static ModelBuilder ConfigureUserEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .ToTable("User");

            modelBuilder
                .Entity<User>()
                .Property((user) => user.Id)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.UserName)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.NormalizedUserName)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.Email)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.NormalizedEmail)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.EmailConfirmed)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.PasswordHash);

            modelBuilder
                .Entity<User>()
                .Property((user) => user.SecurityStamp);

            modelBuilder
                .Entity<User>()
                .Property((user) => user.ConcurrencyStamp);

            modelBuilder
                .Entity<User>()
                .Property((user) => user.PhoneNumber);

            modelBuilder
                .Entity<User>()
                .Property((user) => user.PhoneNumberConfirmed)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.TwoFactorEnabled)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.LockoutEnd);

            modelBuilder
                .Entity<User>()
                .Property((user) => user.LockoutEnabled)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .Property((user) => user.AccessFailedCount)
                .IsRequired();

            modelBuilder
                .Entity<User>()
                .HasKey((user) => user.Id);

            modelBuilder
                .Entity<User>()
                .HasIndex((user) => user.UserName)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasIndex((user) => user.NormalizedUserName)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasIndex((user) => user.Email)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasIndex((user) => user.NormalizedEmail)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasIndex((user) => user.PhoneNumber)
                .IsUnique();

            return modelBuilder;
        }

        public static ModelBuilder ConfigureUserClaimEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserClaim>()
                .ToTable("UserClaim");
        
            modelBuilder
                .Entity<UserClaim>()
                .Property((userClaim) => userClaim.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
        
            modelBuilder
                .Entity<UserClaim>()
                .Property((userClaim) => userClaim.UserId)
                .HasMaxLength(256)
                .IsRequired();
        
            modelBuilder
                .Entity<UserClaim>()
                .Property((userClaim) => userClaim.ClaimType)
                .IsRequired();
        
            modelBuilder
                .Entity<UserClaim>()
                .Property((userClaim) => userClaim.ClaimValue);
        
            modelBuilder
                .Entity<UserClaim>()
                .HasKey((userClaim) => userClaim.Id);
        
            modelBuilder
                .Entity<UserClaim>()
                .HasOne((userClaim) => userClaim.User)
                .WithMany((user) => user.UserClaims);
        
            return modelBuilder;
        }

        public static ModelBuilder ConfigureUserLoginEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserLogin>()
                .ToTable("UserLogin");

            modelBuilder
                .Entity<UserLogin>()
                .Property((userLogin) => userLogin.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder
                .Entity<UserLogin>()
                .Property((userLogin) => userLogin.LoginProvider)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<UserLogin>()
                .Property((userLogin) => userLogin.ProviderKey)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<UserLogin>()
                .Property((userLogin) => userLogin.ProviderDisplayName)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<UserLogin>()
                .Property((userLogin) => userLogin.UserId)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<UserLogin>()
                .HasKey((userLogin) => userLogin.Id);

            modelBuilder
                .Entity<UserLogin>()
                .HasIndex((userLogin) => new { userLogin.LoginProvider, userLogin.ProviderKey })
                .IsUnique();

            modelBuilder
                .Entity<UserLogin>()
                .HasOne((userLogin) => userLogin.User)
                .WithMany((user) => user.UserLogins);

            return modelBuilder;
        }

        public static ModelBuilder ConfigureUserRoleEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserRole>()
                .ToTable("UserRole");

            modelBuilder
                .Entity<UserRole>()
                .Property((userRole) => userRole.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            modelBuilder
                .Entity<UserRole>()
                .Property((userRole) => userRole.UserId)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<UserRole>()
                .Property((userRole) => userRole.RoleId)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder
                .Entity<UserRole>()
                .HasKey((userRole) => userRole.Id);

            modelBuilder
                .Entity<UserRole>()
                .HasIndex((userRole) => new { userRole.UserId, userRole.RoleId })
                .IsUnique();

            modelBuilder
                .Entity<UserRole>()
                .HasOne((userRole) => userRole.User)
                .WithMany((user) => user.UserRoles);

            modelBuilder
                .Entity<UserRole>()
                .HasOne((userRole) => userRole.Role)
                .WithMany((role) => role.UserRoles);

            return modelBuilder;
        }
    }
}
