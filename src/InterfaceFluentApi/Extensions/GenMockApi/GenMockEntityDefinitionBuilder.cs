using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ChanceNET;
using InterfaceFluentApi.Extensions.GenMockApi.Models;

namespace InterfaceFluentApi.Extensions.GenMockApi
{
    public sealed class GenMockEntityDefinitionBuilder<TEntity> where TEntity : class, new()
    {
        public readonly Chance chance;
        private readonly MockEntity mockEntity = new MockEntity();

        public GenMockEntityDefinitionBuilder(Chance chance)
        {
            this.chance = chance;
            this.mockEntity.Type = typeof(TEntity);
            this.mockEntity.MockProperties = new Dictionary<string, PropertyMockData>();
        }

        /// <summary>
        /// Property to add a chance definition
        /// </summary>
        /// <param name="property"></param>
        /// <param name="chanceFunction">Using https://chancejs.com/index.html</param>
        public PropertyConfig Property<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            MemberExpression body = property.Body as MemberExpression;
            PropertyInfo propertyInfo = body.Member as PropertyInfo;

            if (propertyInfo.PropertyType.IsArray) // TODO: Modify this to be able to identitfy normal properties.
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{mockEntity.Type.Name}` is a navigation property, please use this builder only for properties and not for navigation properties, for this last ones use `UseCaseBuilder`.");
            }

            return new PropertyConfig(chance, mockEntity, propertyInfo, body, this);
        }

        public class PropertyConfig
        {
            private readonly Chance chance;
            private readonly MockEntity mockEntity;
            private readonly PropertyInfo propertyInfo;
            private readonly MemberExpression body;
            private readonly GenMockEntityDefinitionBuilder<TEntity> builder;

            public PropertyConfig(
                Chance chance,
                MockEntity mockEntity,
                PropertyInfo propertyInfo,
                MemberExpression body,
                GenMockEntityDefinitionBuilder<TEntity> builder
            )
            {
                this.chance = chance;
                this.mockEntity = mockEntity;
                this.propertyInfo = propertyInfo;
                this.body = body;
                this.builder = builder;
            }

            public GenMockEntityDefinitionBuilder<TEntity> Value(Expression<Func<Chance, object>> chanceExpression)
            {

                PropertyMockData propertyMockData = new PropertyMockData
                {
                    Property = propertyInfo,
                    ValueExpression = chanceExpression
                };

                if (!mockEntity.MockProperties.TryAdd(propertyInfo.Name, propertyMockData))
                {
                    throw new Exception($"Property `{body.Member.Name}` on Entity `{mockEntity.Type.Name}` already Mocked, please remove duplicates.");
                }

                return this.builder;
            }

        }

        // TODO: Remove this from the API
        public TEntity GetEntityInstance()
        {
            TEntity entity = new TEntity();

            foreach (KeyValuePair<string, PropertyMockData> item in this.mockEntity.MockProperties)
            {
                item.Value.Property.SetValue(entity, item.Value.ValueExpression.Compile().Invoke(chance));
            }

            return entity;
        }

    }
}
