using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Signaller.Apps.ApiApp.Types
{
    public class SignallerSchema : Schema
    {
        public SignallerSchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<QueryType>();
            Mutation = serviceProvider.GetRequiredService<MutationType>();
            Subscription = serviceProvider.GetRequiredService<SubscriptionType>();
        }
    }
}