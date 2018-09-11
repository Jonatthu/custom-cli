using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InterfaceFluentApi.Extensions
{
    public class GenEntityExtender<TEntity> where TEntity : class
    {
        private readonly Type entity = typeof(TEntity);
        private readonly Dictionary<MemberInfo, Parameter[]> propertyParameters = new Dictionary<MemberInfo, Parameter[]>();
        private readonly List<ComputedProperty> computedRequestProperties = new List<ComputedProperty>();
        private readonly List<ComputedProperty> computedResponseProperties = new List<ComputedProperty>();
        private readonly Dictionary<ComputedProperty, Parameter[]> computedResponsePropertyParameters = new Dictionary<ComputedProperty, Parameter[]>();

        public GenEntityExtender() { }

        public GenEntityExtender<TEntity> AddPropertyParameters(Expression<Func<TEntity, object>> property, params Parameter[] parameters)
        {
            _checkParametersUniqueness(parameters);

            MemberExpression body = property.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)property.Body;
                body = ubody.Operand as MemberExpression;
            }

            MemberInfo memberInfo = body.Member;

            if (propertyParameters.Keys.Any(x => x.Name == memberInfo.Name))
            {
                throw new Exception($"Property `{memberInfo.Name}` on Entity `{entity.Name}` has been already configured, please try to remove repeated configs.");
            }

            propertyParameters.Add(memberInfo, parameters);

            return this;
        }

        public GenEntityExtender<TEntity> AddRequestProperty<TPropertyType>(string name)
        {

            if (computedRequestProperties.Any(x => x.Name == name))
            {
                throw new Exception($"Adding request property `{name}` on Entity `{entity.Name}` has failed, the name of the request property should be unique.");
            }

            computedRequestProperties.Add(new ComputedProperty
            {
                Name = name,
                Type = typeof(TPropertyType)
            });

            return this;
        }

        public GenEntityExtender<TEntity> AddResponseProperty<TPropertyType>(string name, params Parameter[] parameters)
        {

            _checkParametersUniqueness(parameters);

            if (computedResponseProperties.Any(x => x.Name == name))
            {
                throw new Exception($"Adding response property `{name}` on Entity `{entity.Name}` has failed, the name of the response property should be unique.");
            }

            ComputedProperty computedProperty = new ComputedProperty
            {
                Name = name,
                Type = typeof(TPropertyType)
            };

            computedResponseProperties.Add(computedProperty);

            computedResponsePropertyParameters.TryAdd(computedProperty, parameters);

            return this;
        }

        private void _checkParametersUniqueness(Parameter[] parameters)
        {
            List<Parameter> computedLocalParameters = new List<Parameter>();

            foreach (var item in parameters)
            {
                if (computedLocalParameters.Any(x => x.name == item.name))
                {
                    throw new Exception($"Adding response property `{item.name}` on Entity `{entity.Name}` has failed, the name of the response property should be unique.");
                }
                computedLocalParameters.Add(item);
            }
        }

    }
}
