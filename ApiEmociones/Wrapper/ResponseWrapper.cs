using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ApiEmociones.Wrapper
{
    public class ResponseWrapper<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}