using System;
using System.Collections.Generic;

namespace InterfaceFluentApi.Extensions
{
    public class GenMockEntity<TEntity> where TEntity : class
    {
        private readonly Dictionary<object, string> propertyDefinitions = new Dictionary<object, string>();

        /// <summary>
        /// Property to add a chance definition
        /// </summary>
        /// <param name="property"></param>
        /// <param name="chanceFunction">Using https://chancejs.com/index.html</param>
        /// <returns></returns>
        public GenMockEntity<TEntity> Property(Func<TEntity, object> property, string chanceFunction) {



            return this;
        }

    }
}
