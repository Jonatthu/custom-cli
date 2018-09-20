using System;
using System.Collections.Generic;

namespace InterfaceFluentApi.Extensions.GenMockApi.Models
{
    public class MockEntity
    {
        public Type Type { get; set; }
        public Dictionary<string, PropertyMockData> MockProperties { get; set; }
    }
}
