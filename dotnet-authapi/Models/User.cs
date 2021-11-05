using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace dotnet_authapi.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string Password { get; set; }
    }
}
