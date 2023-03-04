using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace google2fa.API.DTOs
{
    public class LogInResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("tfaKey")]
        public string SecretKey { get; set; }
        [JsonPropertyName("expiresOn")]
        public DateTime ExpiresOn { get; set; }
        [JsonPropertyName("tfaEnabled")]
        public bool Tfa { get; set; }

    }
}