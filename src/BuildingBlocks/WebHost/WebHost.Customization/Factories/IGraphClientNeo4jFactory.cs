using System;
using Neo4jClient;

namespace Microsoft.AspNetCore.Hosting.Factories
{
    public interface IGraphClientNeo4jFactory
    {
        IGraphClientFactory Factory { get; }
    }
}
