using System;

namespace InterfaceFluentApi.Extensions
{
    public interface IGenEntityExtender<TEntity> where TEntity : class
    {
        void ExtendGenEntity(GenEntityExtenderBuilder<TEntity> builder);
    }
}
