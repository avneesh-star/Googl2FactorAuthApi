using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace google2fa.API.DTOs
{
    public class Response
    {
        public Response(bool success, string message, object result)
        {
            Success = success;
            Message = message;
            Result = result;
        }

        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("result")]
        public object Result { get; set; }
    }
}