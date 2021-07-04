using System;
using System.Collections.Generic;
using GraphQL.Types;
using Signaller.Apps.ApiApp.Models;

namespace Signaller.Apps.ApiApp.Types
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
            : base()
        {
            Field
            (
                "id",
                (user) => user.ID
            );

            Field
            (
                "name",
                (user) => user.Name
            );

            Field<ListGraphType<PostType>>
            (
                "posts",
                resolve: (context) =>
                {
                    return new Post[]
                    {
                    };
                }
            );
        }
    }
}
