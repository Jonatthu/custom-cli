using System.ComponentModel.DataAnnotations;
using InterfaceFluentApi.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterfaceFluentApi.Entities
{
    public class Post : IGenEntityModelBuilder<Post>
    {
        public int Id { get; set; }
        public string Message { get; set; }

        [Required]
        public int? UserId { get; set; }
        public User User { get; set; }

        public void ModelBuildGenEntity(EntityTypeBuilder<Post> modelBuilder)
        {

            modelBuilder
                .Property(x => x.UserId)
                .IsRequired(false);

        }
    }
}
