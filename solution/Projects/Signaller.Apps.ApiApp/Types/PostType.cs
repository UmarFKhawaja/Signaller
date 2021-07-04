using System;
using System.Collections.Generic;
using GraphQL.Types;
using Signaller.Apps.ApiApp.Models;

namespace Signaller.Apps.ApiApp.Types
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType()
            : base()
        {
            Field
            (
                "id",
                (post) => post.ID
            );

            Field
            (
                "subject",
                (post) => post.Subject
            );

            Field
            (
                "text",
                (post) => post.Text
            );

            Field<UserType>
            (
                "user",
                resolve: (context) =>
                {
                    return null;
                }
            );
        }
    }
}
