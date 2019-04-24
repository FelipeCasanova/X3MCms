using System;
using Microsoft.Extensions.Options;
using Neo4jClient;

namespace Pages.API.Infrastructure
{
    public class PageDataContext
    {
        private readonly IGraphClientFactory _factory;

        public PageDataContext(IGraphClientFactory factory)
        {
            _factory = factory;
        }

        public IGraphClientFactory Factory
        {
            get
            {
                return _factory;
            }
        }
    }
}
