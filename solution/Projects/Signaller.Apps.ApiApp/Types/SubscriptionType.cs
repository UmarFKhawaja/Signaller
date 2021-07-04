using System;
using System.Collections.Generic;
using GraphQL.Types;
using Signaller.Apps.ApiApp.Models;

namespace Signaller.Apps.ApiApp.Types
{
    public class SubscriptionType : ObjectGraphType<Subscription>
    {
        public SubscriptionType()
            : base()
        {
            Field<UserType>
            (
                "doNothing",
                resolve: (context) =>
                {
                    return new User
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = "Delta Omega"
                    };
                }
            );
        }
    }
}