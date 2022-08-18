using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend
{
    internal class User
    {
        public string username { get; set; }
        public string password { get; set; }

        public User(string u, string p)
        {
            this.username = u;
            this.password = p;
        }
    }
}
