using System;
using Microsoft.AspNetCore.Hosting.Factories;
using Microsoft.Extensions.Options;
using Neo4jClient;
//using WebHost.Customization.Factories;

namespace Pages.API.Infrastructure
{
    public class PagesContext : IGraphClientNeo4jFactory
    {
        public PagesContext(IGraphClientFactory factory)
        {
            Factory = factory;
        }

        public IGraphClientFactory Factory { get; }
    }
}
