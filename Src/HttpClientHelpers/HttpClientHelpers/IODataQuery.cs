using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientHelpers
{
    public interface IODataQuery<T>
    {
        IODataQuery<T> Filter(Expression<Func<T, object>> expression);
        IODataQuery<T> Expand(params Func<T, object>[] properties);
        IODataQuery<T> Select(params Func<T, object>[] properties);
        IODataQuery<T> Select(object obj);
        Task<IEnumerable<T>> GetAsync();
    }
}
