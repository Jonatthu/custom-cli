using System;

namespace InterfaceFluentApi.Extensions
{

    public abstract class Parameter
    {
        public Type type { get; set; }
        public string name { get; set; }
    }

    public class Parameter<TType> : Parameter
    {
        public Parameter(string Name)
        {
            type = typeof(TType);
            name = Name;
        }

    }
}
