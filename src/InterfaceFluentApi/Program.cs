using System;
using System.Reflection;
using InterfaceFluentApi.Entities;
using InterfaceFluentApi.Extensions;

namespace InterfaceFluentApi
{
    class Program
    {
        static void Main(string[] args)
        {

            Type type = typeof(User);

            User userInstance = new User();

            GenEntityExtender<User> builder = new GenEntityExtender<User>();

            type.InvokeMember(nameof(IGenEntityExtender<User>.ExtendGenEntity), BindingFlags.InvokeMethod, null, userInstance, new[] { builder });

            Console.WriteLine("Hello World!");
        }
    }
}
