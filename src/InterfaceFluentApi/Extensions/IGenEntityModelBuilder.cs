using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterfaceFluentApi.Extensions
{
    public interface IGenEntityModelBuilder<TEntity> where TEntity : class
    {
        void ModelBuildGenEntity(EntityTypeBuilder<TEntity> modelBuilder);
    }
}
