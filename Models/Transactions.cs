using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1Development.Models
{
    internal class Transactions
    {

        public Guid Id { get; set; }
        public string CustomerName {  get; set; }
        public float TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public List<EachOrderProductDetail> OrderProductDetails { get; set; }
        public List<EachOrderProductDetail> OrderedAddOnsDetails { get; set; }

    }
}
