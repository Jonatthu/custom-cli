using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterfaceFluentApi.Extensions
{
    public interface IGenEntityModelBuilder<TEntity> where TEntity : class
    {
        EntityTypeBuilder<TEntity> ModelBuildGenEntity(EntityTypeBuilder<TEntity> modelBuilder);
    }
}
