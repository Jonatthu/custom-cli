using System;
using System.Collections.Generic;
using ChanceNET;
using InterfaceFluentApi.Entities;
using InterfaceFluentApi.Extensions;
using InterfaceFluentApi.Extensions.GenMockApi;

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
                    .Generate(20);

            builder
                .Navigation(x => x.MainPost)
                    .Redifine(b =>
                    {
                        b.Property(x => x.Message, c => "Hello World");
                    })
                    .Generate();

            builder
                .Property(x => x.Birthdate, c => new DateTime(2018, 2, 2))
                .Property(x => x.FirstName, c => "Jonathan")
                .Generate(20)
            ;
        }
    }
}
