using System;
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
        public DateTime Birthdate { get; set; }

        public GenEntityExtender<User> ExtendGenEntity(GenEntityExtender<User> builder)
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

            return builder;
        }

        public GenMockEntity<User> GenMockEntity(GenMockEntity<User> builder)
        {
            builder
                .Property(x => x.Username, "username()")
                .Property(x => x.Birthdate, "date(1986, 2015)")
                .Property(x => x.FirstName, "name()")
                ;

            return builder;
        }

        public EntityTypeBuilder<User> ModelBuildGenEntity(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder
                .HasIndex(x => x.Username)
                .IsUnique();

            modelBuilder
                .Property(x => x.Username)
                .IsRequired(false);

            return modelBuilder;
        }
    }
}