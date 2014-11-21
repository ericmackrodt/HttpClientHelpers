using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientHelpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ODataRouteAttribute : Attribute
    {
        public string Route { get; set; }

        public ODataRouteAttribute(string route) 
        {
            Route = route;
        }
    }
}
