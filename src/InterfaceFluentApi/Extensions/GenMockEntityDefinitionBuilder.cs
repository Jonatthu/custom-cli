using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ChanceNET;

namespace InterfaceFluentApi.Extensions
{
    public sealed class GenMockEntityDefinitionBuilder<TEntity> where TEntity : class, new()
    {
        public readonly Type entity = typeof(TEntity);
        public readonly TEntity instance = new TEntity();
        public readonly Dictionary<PropertyInfo, PropertyMockData> propertyDefinitions = new Dictionary<PropertyInfo, PropertyMockData>();
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
            Func<Chance, TProperty> chanceFunction
        )
        {
            MemberExpression body = property.Body as MemberExpression;
            PropertyInfo propertyInfo = body.Member as PropertyInfo;

            if (propertyInfo.PropertyType.IsArray) // TODO: Modify this to be able to identitfy normal properties.
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` is a navigation property, please use this builder only for properties and not for navigation properties, for this last ones use `UseCaseBuilder`.");
            }

            TProperty value = chanceFunction(chance);

            PropertyMockData propertyMockData = new PropertyMockData
            {
                ValueType = typeof(TProperty),
                Value = value,
            };

            if (!propertyDefinitions.TryAdd(propertyInfo, propertyMockData))
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` already Mocked, please remove duplicates.");
            }

            propertyInfo.SetValue(instance, value);

            return this;
        }
    }

    public class PropertyMockData
    {
        public Type ValueType { get; set; }
        public object Value { get; set; }
    }
}
