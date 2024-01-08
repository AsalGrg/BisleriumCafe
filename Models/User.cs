using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1Development.Models
{
    internal class User
    {
        public Guid id = new();
        public string name { get; set; }
        public string password{ get; set; }
        public string role { get; set; }
    }
}
