//using Flurl.Http.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;

//namespace Diplom.Mobile
//{
//    class UntrustedCertClientFactory : DefaultHttpClientFactory
//    {
//        public override HttpMessageHandler CreateMessageHandler()
//        {
//            return new HttpClientHandler
//            {
//                ServerCertificateCustomValidationCallback = (a, b, c, d) => true
//            };
//        }   
//    }
//}
