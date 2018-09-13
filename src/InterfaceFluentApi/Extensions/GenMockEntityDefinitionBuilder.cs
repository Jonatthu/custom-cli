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
        public GenMockEntityDefinitionBuilder<TEntity> Property<TProperty, TValue>(
            Expression<Func<TEntity, TProperty>> property,
            Func<Chance, TValue> chanceFunction,
            Func<TValue, TValue> modifyFunc = null
        )
        {
            MemberExpression body = property.Body as MemberExpression;
            PropertyInfo propertyInfo = body.Member as PropertyInfo;

            if (propertyInfo.PropertyType.IsArray) // TODO: Modify this to be able to identitfy normal properties.
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` is a navigation property, please use `Navigation` method instead.");
            }

            PropertyMockData propertyMockData = new PropertyMockData
            {
                ValueType = typeof(TValue),
                ValueFunc = _parseChanceToValueFunc(chanceFunction),
                ModifyFunc = _parseValueToValueFunc(modifyFunc)
            };

            if (!propertyDefinitions.TryAdd(propertyInfo, propertyMockData))
            {
                throw new Exception($"Property `{body.Member.Name}` on Entity `{entity.Name}` already Mocked, please remove duplicates.");
            }

            return this;
        }

        private Func<Chance, object> _parseChanceToValueFunc<TValue>(Func<Chance, TValue> from)
        {
            Func<Chance, object> parsed = c => from(c);
            return parsed;
        }

        private Func<object, object> _parseValueToValueFunc<TValue>(Func<TValue, TValue> from) where TValue : class
        {
            Func<object, object> parsed = c => from(c as TValue);
            return parsed;
        }
        // public GenMockEntityDefinitionBuilder<TEntity> Property<TProperty, TValue>(Expression<Func<TEntity, TProperty>> property, TValue value)
        // {
        //     return Property(property, c => value);
        // }
    }

    public class PropertyMockData
    {
        public Type ValueType { get; set; }
        public Func<Chance, object> ValueFunc { get; set; }
        public Func<object, object> ModifyFunc { get; set; }
    }
}
