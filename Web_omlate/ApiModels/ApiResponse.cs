using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_omlate.ApiModels
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Data = null;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}