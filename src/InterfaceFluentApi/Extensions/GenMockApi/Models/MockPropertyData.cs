using System;
using System.Linq.Expressions;
using System.Reflection;
using ChanceNET;

namespace InterfaceFluentApi.Extensions.GenMockApi.Models
{
    public class PropertyMockData
    {
        public PropertyInfo Property { get; set; }
        public Expression<Func<Chance, object>> ValueExpression { get; set; }
    }
}
