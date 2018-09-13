using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ChanceNET;

namespace InterfaceFluentApi.Extensions
{
    public sealed class GenMockEntityDefinitionBuilder<TEntity> where TEntity : class, new()
    {
        public readonly Type entity = typeof(TEntity);
        public readonly TEntity entityInstance = new TEntity();
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
        public GenMockEntityDefinitionBuilder<TEntity> Property<TProperty, TValue>(Expression<Func<TEntity, TProperty>> property, Func<Chance, TValue> chanceFunction)
        {
            MemberExpression body = property.Body as MemberExpression;
            PropertyInfo propertyInfo = body.Member as PropertyInfo;
            TValue value = chanceFunction(chance);

            if (propertyInfo.PropertyType.IsArray) // TODO: Modify this to be able to identitfy normal properties.
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` is a navigation property, please use `Navigation` method instead.");
            }

            if (propertyDefinitions.TryAdd(propertyInfo, new PropertyMockData {
                Type = typeof(TValue),
                Value = value
            })) {
                var en = entityInstance.GetType();
                en.GetProperty(propertyInfo.Name).SetValue(entityInstance, value);
            }
            else
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` already Mocked, please remove duplicates.");
            }

            return this;
        }

        public GenMockEntityDefinitionBuilder<TEntity> Property<TProperty, TValue>(Expression<Func<TEntity, TProperty>> property, TValue value)
        {
            return Property(property, c => value);
        }
    }

    public class PropertyMockData
    {
        public Type Type { get; set; }
        public object Value { get; set; }
    }
}
