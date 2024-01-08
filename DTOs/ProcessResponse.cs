using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework1Development.DTOs
{
    internal class ProcessResponse(bool success, string message)
    {
        private bool _success = success;
        private string _message = message;
    }
}
