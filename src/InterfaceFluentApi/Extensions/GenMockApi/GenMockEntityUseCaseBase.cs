using System;

namespace InterfaceFluentApi.Extensions.GenMockApi
{
    public abstract class GenMockEntityUseCaseBase<TEntity> where TEntity : class, new()
    {
        public readonly Type type = typeof(TEntity);
        public abstract GenMockUseCaseRunFromEnum FromEnum { get; }
        public abstract string UseCaseTitle { get; }
        public abstract string UseCaseDescription { get; }
        public abstract void DefineUseCase(GenMockEntityUseCaseBuilder<TEntity> builder);

        public virtual void RunUseCase()
        {

        }
    }
}
