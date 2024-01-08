using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1Development.Models
{
    internal class Customer
    {

        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }

        public bool isRegular {  get; set; }

        public DateTime regularBecameDate { get; set; }

    }
}
