using System.ComponentModel.DataAnnotations;
using InterfaceFluentApi.Extensions;
using InterfaceFluentApi.Extensions.GenMockApi;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterfaceFluentApi.Entities
{
    public class Post : IGenEntityModelBuilder<Post>, IGenMockEntity<Post>
    {
        public int Id { get; set; }
        public string Message { get; set; }

        [Required]
        public int? UserId { get; set; }
        public User User { get; set; }

        public void GenMockEntity(GenMockEntityDefinitionBuilder<Post> builder)
        {
            builder
                .Property(x => x.Message, c => c.Sentence(58, 0, false, false, '.'))
            ;
        }

        public void ModelBuildGenEntity(EntityTypeBuilder<Post> modelBuilder)
        {

            modelBuilder
                .Property(x => x.UserId)
                .IsRequired(false);

        }
    }
}
