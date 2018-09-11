using System;
using System.Collections.Generic;
using System.Linq;

namespace InterfaceFluentApi.Extensions
{
    public class GenEntityExtender<TEntity> where TEntity : class
    {
        private readonly Type entity = typeof(TEntity);
        private readonly Dictionary<object, Parameter[]> propertyParameters = new Dictionary<object, Parameter[]>();
        private readonly List<ComputedProperty> computedRequestProperties = new List<ComputedProperty>();
        private readonly List<ComputedProperty> computedResponseProperties = new List<ComputedProperty>();
        private readonly Dictionary<ComputedProperty, Parameter[]> computedResponsePropertyParameters = new Dictionary<ComputedProperty, Parameter[]>();

        public GenEntityExtender() { }

        public GenEntityExtender<TEntity> AddPropertyParameters(Func<TEntity, object> property, params Parameter[] parameters)
        {
            _checkUniqueness(parameters);

            if (propertyParameters.TryGetValue(property, out Parameter[] outParameters))
            {
                List<Parameter> newParameters = parameters.ToList();
                newParameters.AddRange(outParameters);
                outParameters = newParameters.ToArray();
            }
            else
            {
                propertyParameters.Add(property, parameters);
            }

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

            _checkUniqueness(parameters);

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

        private void _checkUniqueness(Parameter[] parameters)
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
