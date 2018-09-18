using System;
using System.Collections.Generic;
using InterfaceFluentApi.Entities;

namespace InterfaceFluentApi.Extensions
{
    public class MockDataDirector
    {

        public readonly List<Type> entities = new List<Type>();

        public MockDataDirector()
        {
            entities.AddRange(new[] {
                typeof(User),
                typeof(Post)
            });
        }

        public void ExecuteGeneration()
        {
            throw new System.NotImplementedException();
        }

        public void InsertToDB()
        {
            throw new System.NotImplementedException();
        }
    }
}
