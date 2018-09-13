using System.Collections.Generic;
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

                .NavigationCollection<Post>(x => x.Posts)
                    .RedifineMockEntity(b => {
                        b.Property(x => x.Message, c => c.Phone());
                    })
                    .NumberOfElements(234)
            ;
        }
    }
}
