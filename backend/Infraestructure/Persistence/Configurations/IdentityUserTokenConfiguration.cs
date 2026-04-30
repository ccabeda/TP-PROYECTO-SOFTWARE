using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TP_PROYECTO_SOFTWARE.Infraestructure.Persistence.Configurations;

public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<int>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<int>> builder)
    {
        builder.ToTable("IDENTITY_USER_TOKEN");
    }
}
