using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientHelpers
{
    [DataContract]
    public class ODataResponse<T>
    {
        [DataMember(Name = "value")]
        public T Value { get; set; }
    }
}
