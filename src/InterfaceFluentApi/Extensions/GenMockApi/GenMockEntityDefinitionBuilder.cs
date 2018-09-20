using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ChanceNET;

namespace InterfaceFluentApi.Extensions.GenMockApi
{
    public sealed class GenMockEntityDefinitionBuilder<TEntity> where TEntity : class, new()
    {
        public readonly Type entity = typeof(TEntity);
        public readonly Dictionary<string, PropertyMockData> propertyDefinitions = new Dictionary<string, PropertyMockData>();
        public readonly Chance chance;

        public GenMockEntityDefinitionBuilder(Chance chance)
        {
            this.chance = chance;
        }

        /// <summary>
        /// Property to add a chance definition
        /// </summary>
        /// <param name="property"></param>
        /// <param name="chanceFunction">Using https://chancejs.com/index.html</param>
        /// <returns></returns>
        public GenMockEntityDefinitionBuilder<TEntity> Property<TProperty>(
            Expression<Func<TEntity, TProperty>> property,
            Expression<Func<Chance, object>> chanceExpression
        )
        {
            MemberExpression body = property.Body as MemberExpression;
            PropertyInfo propertyInfo = body.Member as PropertyInfo;

            if (propertyInfo.PropertyType.IsArray) // TODO: Modify this to be able to identitfy normal properties.
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` is a navigation property, please use this builder only for properties and not for navigation properties, for this last ones use `UseCaseBuilder`.");
            }

            PropertyMockData propertyMockData = new PropertyMockData
            {
                Property = propertyInfo,
                ValueExpression = chanceExpression
            };

            if (!propertyDefinitions.TryAdd(propertyInfo.Name, propertyMockData))
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` already Mocked, please remove duplicates.");
            }


            return this;
        }

        public TEntity GetEntityInstance()
        {
            TEntity entity = new TEntity();

            foreach(KeyValuePair<string, PropertyMockData> item in this.propertyDefinitions)
            {
                item.Value.Property.SetValue(entity, item.Value.ValueExpression.Compile().Invoke(chance));
            }

            return entity;
        }
    }

    public class PropertyMockData
    {
        public PropertyInfo Property { get; set; }
        public Expression<Func<Chance, object>> ValueExpression { get; set; }
    }
}
