using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientHelpers
{
    public class ODataQuery<T> : IODataQuery<T>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _requestUrl;
        private Expression<Func<T, object>> _filter;

        public ODataQuery(string baseUrl)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
        }

        public IODataQuery<T> Filter(Expression<Func<T, object>> expression)
        {
            _filter = expression;
            return this;
        }

        public IODataQuery<T> Expand(params Func<T, object>[] properties)
        {
            throw new NotImplementedException();
        }

        public IODataQuery<T> Select(params Func<T, object>[] properties)
        {
            throw new NotImplementedException();
        }

        public IODataQuery<T> Select(object obj)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            var odataString = GetODataString();
            var response = await _client.GetAsync(_requestUrl + "?" + odataString);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private string GetODataString()
        {
            var visitor = new ODataExpressionVisitor();
            visitor.Visit(_filter);
            var filter = visitor.ToString();
            return filter;
        }
    }
}
