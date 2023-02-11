using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Users.API.Core
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.FirstName);
            entityBuilder.Property(x => x.IsActive);
            entityBuilder.Property(x => x.LastName);
            entityBuilder.Property(x => x.Email);
        }
    }
}
