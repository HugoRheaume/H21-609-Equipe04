using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class HelloWorldController : ApiController
    {
        public class HelloWorldObj
        {
            public string Image;
            public string Text;
        }

        [HttpGet]
        public HelloWorldObj HelloWorld()
        {
            HelloWorldObj helloWorldObj = new HelloWorldObj
            {
                Image = "https://d1y8sb8igg2f8e.cloudfront.net/images/shutterstock_1375463840.width-800.jpg",
                Text = "Hello World"
            };

            return helloWorldObj;
        }
    }
}
