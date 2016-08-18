using Nancy.Json;
using System;

namespace API.Model
{
    public class APIModel
    {
        [ScriptIgnore]
        public String Id { get; set; }

        public String Url { get; set; }

        public DateTime CreateDate { get; set; }
    }
}