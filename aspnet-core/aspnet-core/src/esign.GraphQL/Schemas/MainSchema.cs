using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using esign.Queries.Container;
using System;

namespace esign.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}