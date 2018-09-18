using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ChanceNET;

namespace InterfaceFluentApi.Extensions
{
    public sealed class GenMockEntityUseCaseBuilder<TEntity> where TEntity : class, new()
    {
        private readonly Chance chance;
        public readonly Type entity = typeof(TEntity);
        public readonly TEntity entityInstance = new TEntity();
        public readonly List<TEntity> entityInstances = new List<TEntity>();
        private readonly GenMockEntityDefinitionBuilder<TEntity> entityDefinitionBuilder;
        private readonly List<string> modifiedProperties = new List<string>();

        public GenMockEntityUseCaseBuilder(
            Chance chance,
            GenMockEntityDefinitionBuilder<TEntity> entityDefinitionBuilder
        )
        {
            this.chance = chance;
            this.entityDefinitionBuilder = entityDefinitionBuilder;
        }

        public NavigationCollectionConfig<TPropertyType> Navigation<TPropertyType>(
            Expression<Func<TEntity, IEnumerable<TPropertyType>>> property
        ) where TPropertyType : class, new()
        {
            GenMockEntityDefinitionBuilder<TPropertyType> mockDefinition = new GenMockEntityDefinitionBuilder<TPropertyType>(chance);
            return new NavigationCollectionConfig<TPropertyType>(this, property, chance, entityInstance, entity, modifiedProperties);
        }

        public NavigationConfig<TPropertyType> Navigation<TPropertyType>(
            Expression<Func<TEntity, TPropertyType>> property
        ) where TPropertyType : class, new()
        {
            GenMockEntityDefinitionBuilder<TPropertyType> mockDefinition = new GenMockEntityDefinitionBuilder<TPropertyType>(chance);
            return new NavigationConfig<TPropertyType>(this, property, chance, entityInstance, entity, modifiedProperties);
        }

        public GenMockEntityUseCaseBuilder<TEntity> Property<TPropertyType>(
            Expression<Func<TEntity, TPropertyType>> property,
            Func<Chance, TPropertyType> chanceFunction
        )
        {

            return this;
        }

        public void Generate(int numberOfElements)
        {

        }

        public class NavigationConfig<TPropertyType> where TPropertyType : class, new()
        {
            private readonly Type entity;
            private readonly List<string> modifiedProperties;
            private readonly Chance chance;
            private readonly TEntity entityInstance;
            private readonly Expression<Func<TEntity, TPropertyType>> property;
            private readonly GenMockEntityUseCaseBuilder<TEntity> useCaseBuilder;
            private GenMockEntityDefinitionBuilder<TPropertyType> mockDefinitionBuilder;
            private readonly MemberInfo memberInfo;

            public NavigationConfig(
                GenMockEntityUseCaseBuilder<TEntity> useCaseBuilder,
                Expression<Func<TEntity, TPropertyType>> property,
                Chance chance,
                TEntity entityInstance,
                Type entity,
                List<string> modifiedProperties
            )
            {
                this.useCaseBuilder = useCaseBuilder;
                this.property = property;
                this.chance = chance;
                this.entityInstance = entityInstance;
                this.entity = entity;
                this.modifiedProperties = modifiedProperties;

                MemberExpression body = property.Body as MemberExpression;

                if (body == null)
                {
                    UnaryExpression ubody = (UnaryExpression)property.Body;
                    body = ubody.Operand as MemberExpression;
                }

                MemberInfo memberInfo = body.Member;

                this.memberInfo = memberInfo;

            }

            public NavigationConfig<TPropertyType> Redifine(Action<GenMockEntityDefinitionBuilder<TPropertyType>> definition)
            {

                if (modifiedProperties.Any(x => x == memberInfo.Name))
                {
                    throw new Exception($"Property `{memberInfo.Name}` on Entity `{entity.Name}` has been already configured, please remove duplicate configs.");
                }

                modifiedProperties.Add(memberInfo.Name);

                mockDefinitionBuilder = new GenMockEntityDefinitionBuilder<TPropertyType>(chance);

                if (definition != null)
                {
                    definition(mockDefinitionBuilder);
                }

                return this;
            }

            public GenMockEntityUseCaseBuilder<TEntity> Generate()
            {

                //entity.GetProperty(memberInfo.Name).SetValue(entityInstance, )
                // Activator.CreateInstance()

                return useCaseBuilder;
            }
        }

        public class NavigationCollectionConfig<TPropertyType> where TPropertyType : class, new()
        {
            private readonly Type entity;
            private readonly List<string> modifiedProperties;
            private readonly Chance chance;
            private readonly TEntity entityInstance;
            private readonly Expression<Func<TEntity, IEnumerable<TPropertyType>>> property;
            private readonly GenMockEntityUseCaseBuilder<TEntity> useCaseBuilder;
            private GenMockEntityDefinitionBuilder<TPropertyType> mockDefinitionBuilder;
            private readonly MemberInfo memberInfo;

            public NavigationCollectionConfig(
                GenMockEntityUseCaseBuilder<TEntity> useCaseBuilder,
                Expression<Func<TEntity, IEnumerable<TPropertyType>>> property,
                Chance chance,
                TEntity entityInstance,
                Type entity,
                List<string> modifiedProperties
            )
            {
                this.useCaseBuilder = useCaseBuilder;
                this.property = property;
                this.chance = chance;
                this.entityInstance = entityInstance;
                this.entity = entity;
                this.modifiedProperties = modifiedProperties;

                MemberExpression body = property.Body as MemberExpression;

                if (body == null)
                {
                    UnaryExpression ubody = (UnaryExpression)property.Body;
                    body = ubody.Operand as MemberExpression;
                }

                MemberInfo memberInfo = body.Member;

                this.memberInfo = memberInfo;

            }

            public NavigationCollectionConfig<TPropertyType> Redifine(Action<GenMockEntityDefinitionBuilder<TPropertyType>> definition)
            {

                if (modifiedProperties.Any(x => x == memberInfo.Name))
                {
                    throw new Exception($"Property `{memberInfo.Name}` on Entity `{entity.Name}` has been already configured, please remove duplicate configs.");
                }

                modifiedProperties.Add(memberInfo.Name);

                mockDefinitionBuilder = new GenMockEntityDefinitionBuilder<TPropertyType>(chance);

                if (definition != null)
                {
                    definition(mockDefinitionBuilder);
                }

                return this;
            }

            public GenMockEntityUseCaseBuilder<TEntity> Generate(int numberOfElements)
            {

                //entity.GetProperty(memberInfo.Name).SetValue(entityInstance, )
                // Activator.CreateInstance()

                return useCaseBuilder;
            }
        }
    }
}
