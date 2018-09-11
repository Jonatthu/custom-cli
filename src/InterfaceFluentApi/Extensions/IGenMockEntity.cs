namespace InterfaceFluentApi.Extensions
{
    public interface IGenMockEntity<TEntity> where TEntity : class
    {
        GenMockEntity<TEntity> GenMockEntity(GenMockEntity<TEntity> builder);
    }
}
