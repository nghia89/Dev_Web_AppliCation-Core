using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using WebAppCore.Data.EF.Configurations;
using WebAppCore.Data.EF.Extensions;
using WebAppCore.Data.Entities;

namespace WebAppCore.Data.EF.BuilderExtensions
{
	public static class BuilderExtensions
	{
		public static void ConfigModelBuilder(this ModelBuilder modelBuilder)
		{

			#region Identity Config

			modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

			modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
				.HasKey(x => x.Id);

			modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

			modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
				.HasKey(x => new { x.RoleId,x.UserId });

			modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
			   .HasKey(x => new { x.UserId });

			#endregion Identity Config

			modelBuilder.AddConfiguration(new AdvertistmentPositionConfiguration());
			modelBuilder.AddConfiguration(new BlogTagConfiguration());
			modelBuilder.AddConfiguration(new ContactDetailConfiguration());
			modelBuilder.AddConfiguration(new FooterConfiguration());
			modelBuilder.AddConfiguration(new FunctionConfiguration());
			modelBuilder.AddConfiguration(new PageConfiguration());
			modelBuilder.AddConfiguration(new ProductTagConfiguration());
			modelBuilder.AddConfiguration(new TagConfiguration());
			modelBuilder.AddConfiguration(new AnnouncementConfiguration());
			modelBuilder.AddConfiguration(new AdvertistmentPageConfiguration());


			//base.OnModelCreating(builder);

			modelBuilder.Entity<Product>(entity => {
				entity.ToTable(nameof(Product));
				entity.Property(x => x.Name).HasMaxLength(255).IsRequired();
				entity.HasIndex(x => x.Id).IsUnique();
				entity.HasIndex(x => new { x.Name});
			});

		}
	}
}
