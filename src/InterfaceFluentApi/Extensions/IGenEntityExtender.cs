using System;

namespace InterfaceFluentApi.Extensions
{
    public interface IGenEntityExtender<TEntity> where TEntity : class
    {
        GenEntityExtender<TEntity> ExtendGenEntity(GenEntityExtender<TEntity> builder);
    }
}
