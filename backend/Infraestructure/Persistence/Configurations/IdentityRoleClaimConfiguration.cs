using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Configurations;

public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder)
    {
        builder.ToTable("IDENTITY_ROLE_CLAIM");
    }
}
