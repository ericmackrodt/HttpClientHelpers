using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpClientHelpers.Tests
{
    [TestClass]
    public class ODataQueryTests
    {
        [TestMethod]
        public async void CreateBasicQuery()
        {
            string teste = "afk";

            var query = await new ODataQuery<DummyDTO>("http://www.google.com")
                .Filter(o => o.Id == 1 && (o.Name == teste || o.Name == "teste"))
                .GetAsync();
        }

        public class DummyDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
