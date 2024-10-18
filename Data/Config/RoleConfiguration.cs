using BlogProject.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace BlogProject.Data.Config
{
	public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
	{
		private rolesconfigs _roles;
        public RoleConfiguration(IOptions<rolesconfigs> _roles)
        {
			this._roles=_roles.Value;
		}
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.HasData(LoadRoles());
		}
		private List<IdentityRole> LoadRoles()
		{
		
		return new List<IdentityRole>
		{
			 new IdentityRole {Id= Guid.NewGuid().ToString(),ConcurrencyStamp= Guid.NewGuid().ToString(), Name = _roles._user.ToUpper(), NormalizedName = _roles._user.ToUpper() },
			 new IdentityRole {Id= Guid.NewGuid().ToString(),ConcurrencyStamp= Guid.NewGuid().ToString(), Name = _roles._admin.ToUpper(), NormalizedName = _roles._admin.ToUpper() },
			 new IdentityRole { Id= Guid.NewGuid().ToString(),ConcurrencyStamp= Guid.NewGuid().ToString(),Name = _roles._moderator.ToUpper(), NormalizedName = _roles._moderator.ToUpper() },
		};
		}
	}
}
