using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JsonTest2.Model
{
    
    public class MyMessage
    {
        public Header Header { get; set; }

        [JsonConverter(typeof(PersonTypeConverter))]
        public IPerson Person { get; set; }

      
    }
}
