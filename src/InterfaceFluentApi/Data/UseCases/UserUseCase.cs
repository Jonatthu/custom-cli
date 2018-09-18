using System.Collections.Generic;
using ChanceNET;
using InterfaceFluentApi.Entities;
using InterfaceFluentApi.Extensions;

namespace InterfaceFluentApi.Data.UseCases
{
    public class UserUseCase : GenMockEntityUseCaseBase<User>
    {
        public override string UseCaseTitle => "USER_POSTS";
        public override string UseCaseDescription => "A user can have multiple posts.";
        public override GenMockUseCaseRunFromEnum FromEnum => GenMockUseCaseRunFromEnum.DB;

        public override void DefineUseCase(GenMockEntityUseCaseBuilder<User> builder)
        {
            builder
                .Navigation(x => x.Posts)
                    .Generate(20)
                .Navigation(x => x.MainPost)
                    .Redifine(b => {
                        b.Property(x => x.Message, c => "Hello World");
                    })
                    .Generate()
                .Property(x => x.Birthdate, c => c.Date(2015, Month.March, 12))
                .Generate(20)
            ;
        }
    }
}
