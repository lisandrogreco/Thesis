using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Legacy_Todo.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class User : IEntityBase
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface IEntityBase
    {
        string Id { get; set; }
    }
}
