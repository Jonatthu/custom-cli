﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ChanceNET;
using InterfaceFluentApi.Data;
using InterfaceFluentApi.Entities;
using InterfaceFluentApi.Extensions;
using InterfaceFluentApi.Extensions.GenMockApi;
using InterfaceFluentApi.Extensions.GenMockApi.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace InterfaceFluentApi
{
    class Program
    {

        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] {
            new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information, true)
        });

        static void Main(string[] args)
        {

            Type type = typeof(User);

            User userInstance = new User();

            GenEntityExtenderBuilder<User> builder = new GenEntityExtenderBuilder<User>();

            type.InvokeMember(nameof(IGenEntityExtender<User>.ExtendGenEntity), BindingFlags.InvokeMethod, null, userInstance, new[] { builder });


            Chance chance = new Chance();
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            var igenMockEntityImplementors = types.SelectMany(x => x.GetInterfaces()).Where(x => x.Name == typeof(IGenMockEntity<>).Name);
            var entities = igenMockEntityImplementors.Select(x => x.GenericTypeArguments.First());
            int numberOfGens = 10;
            Dictionary<string, MockEntityValued> mockedData = new Dictionary<string, MockEntityValued>();

            for(int i = 0; i < entities.Count(); i++)
            {
                Type entity = entities.ElementAt(i);
                object entityInstance = Activator.CreateInstance(entity);
                Type builderMock = typeof(GenMockEntityDefinitionBuilder<>).MakeGenericType(entity);
                object[] data = new object[numberOfGens];

                for (int j = 0; j < numberOfGens; j++)
                {
                    dynamic instanceBuilderMock = Activator.CreateInstance(builderMock, new object[] { chance });
                    entity.InvokeMember(nameof(IGenMockEntity<object>.GenMockEntity), BindingFlags.InvokeMethod, null, entityInstance, new object[] { instanceBuilderMock });
                    data[j] = instanceBuilderMock.GetEntityInstance();
                }

                mockedData.Add(entity.Name, new MockEntityValued {
                    Type = entity,
                    Data = data
                });

            }

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<MyDbContext>()
                 .UseLoggerFactory(MyLoggerFactory)
                 .UseSqlite(connection)
                 .Options;

            using (var dbContext = new MyDbContext(options))
            {
                dbContext.Database.EnsureCreated();
            }

            using (var dbContext = new MyDbContext(options))
            {
                //dbContext.AddRange(users);
                dbContext.SaveChanges();
            }

            using (var dbContext = new MyDbContext(options))
            {
                var list = dbContext.User.Include(x => x.Posts).ToList();

                var post = new Post
                {
                    Message = "I am a post without an owner"
                };

                var context = new ValidationContext(post, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(post, context, results);

                if (!isValid)
                {
                    foreach (var validationResult in results)
                    {
                        Console.WriteLine(validationResult.ErrorMessage);
                    }
                }

                dbContext.Post.Add(post);

                dbContext.SaveChanges();
            }

            using (var dbContext = new MyDbContext(options))
            {
                var post = dbContext.Post.AsNoTracking().Where(x => x.Id == 5).FirstOrDefault();

                post.UserId = 1;

                var dbEntityEntry = dbContext.Entry(post);

                dbEntityEntry.Property(x => x.UserId).IsModified = true;

                dbContext.SaveChanges();
            }

        }
    }
}
