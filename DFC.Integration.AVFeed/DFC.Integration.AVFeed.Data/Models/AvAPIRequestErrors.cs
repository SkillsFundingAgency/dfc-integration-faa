using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Models
{
    class AVAPIRequestErrors
    {
        public List<RequestError> RequestErrors { get; set; }
    }

    public class RequestError
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

  
}
