using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RethinkLogs
{
    public class LogEvent
    {
        
            [JsonProperty("id")]
            public Guid Id;

            [JsonProperty("timestamp")]
            public DateTimeOffset Timestamp;
            
            [JsonProperty("level")]
            public LogEventLevel Level;

            [JsonProperty("message")]
            public string Message;

            [JsonProperty("messageTemplate")]
            public string MessageTemplate;
        
            [JsonProperty("props")]
            public Dictionary<string, object> Props;

            [JsonProperty("exception")]
            public string Exception;
        
    }
}