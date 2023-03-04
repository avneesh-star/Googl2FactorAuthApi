using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace google2fa.API.Models
{
    public class UserMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TfaSecretKEy { get; set; } = string.Empty;
        public bool TfaEnabled { get; set; }

    }
}