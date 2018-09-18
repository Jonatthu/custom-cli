using System;
using System.Collections.Generic;
using ChanceNET;
using InterfaceFluentApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterfaceFluentApi.Entities
{
    public class User : IGenEntityExtender<User>, IGenMockEntity<User>, IGenEntityModelBuilder<User>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public DateTime? Birthdate { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public Post MainPost { get; set; }

        public void ExtendGenEntity(GenEntityExtenderBuilder<User> builder)
        {

            builder
                .AddPropertyParameters(x => x.Id,
                    new Parameter<bool>("isName"),
                    new Parameter<string>("anotherOne")
                )
                .AddPropertyParameters(x => x.Username,
                    new Parameter<int>("numberOfCharacters")
                )
                .AddRequestProperty<string>("fullNameSearch")
                .AddRequestProperty<string[]>("fullNameValues")
                .AddResponseProperty<string>("fullName",
                    new Parameter<bool>("onlyInitials"),
                    new Parameter<bool>("onlyFirstNameAndLastNameInitial")
                );

        }

        public void GenMockEntity(GenMockEntityDefinitionBuilder<User> builder)
        {
            string firstName = builder.chance.FirstName();

            builder
                .Property(x => x.Username, c => firstName.ToLower())
                .Property(x => x.FirstName, c => firstName)
                .Property(x => x.Birthdate, c => c.Birthday(AgeRanges.Adult))
            ;
        }

        public void ModelBuildGenEntity(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder
                .HasIndex(x => x.Username)
                .IsUnique();

            modelBuilder
                .Property(x => x.Username)
                .IsRequired(false);

        }
    }
}
