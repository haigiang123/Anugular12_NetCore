using System;
using System.Collections.Generic;
using System.Text;

namespace WebUltilities
{
    public class WebAPIException : Exception
    {
        public WebAPIException() : base()
        {

        }
        
        public WebAPIException(string name) : base(name)
        {

        }

        public WebAPIException(string name, Exception inner):base(name, inner)
        {

        }
    }
}
