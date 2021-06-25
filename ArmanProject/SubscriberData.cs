using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmanProject
{
    class SubscriberData
    {
        [JsonProperty("name")]
        public string SubNumber;
        [JsonProperty("number")]
        public string SubName;
        [JsonProperty("description")]
        public string Discript;
        [JsonProperty("value")]
        public string value_key;
        [JsonProperty("visible")]
        public bool value_visible;
        [JsonProperty("id")]
        public string eventId;
    }



}
