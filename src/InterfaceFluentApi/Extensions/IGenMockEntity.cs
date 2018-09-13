using ChanceNET;

namespace InterfaceFluentApi.Extensions
{
    public interface IGenMockEntity<TEntity> where TEntity : class, new()
    {
        void GenMockEntity(GenMockEntityDefinitionBuilder<TEntity> builder);
    }
}
