using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using Signaller.Apps.ApiApp.Models;

namespace Signaller.Apps.ApiApp.Types
{
    public class QueryType : ObjectGraphType<Query>
    {
        public QueryType()
            : base()
        {
            Field<PostType>
            (
                "getPost",
                arguments: new QueryArguments
                (
                    new QueryArgument<IntGraphType>
                    {
                        Name = "id"
                    }
                ),
                resolve: (context) =>
                {
                    var id = context.GetArgument<string>("id");

                    return new Post
                    {
                        ID = id,
                        Subject = "Lorem Ipsum",
                        Text = "Lorem ipsum dolor"
                    };
                }
            );
            Field<UserType>
            (
                "getUser",
                arguments: new QueryArguments
                (
                    new QueryArgument<StringGraphType>
                    {
                        Name = "id"
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "name"
                    }
                ),
                resolve: (context) =>
                {
                    var id = context.GetArgument<string>("id");
                    var name = context.GetArgument<string>("name");

                    return new User
                    {
                        ID = id ?? Guid.NewGuid().ToString(),
                        Name = name ?? "Delta Omega"
                    };
                }
            );
        }
    }
}