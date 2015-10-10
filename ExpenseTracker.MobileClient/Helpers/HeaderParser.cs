using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
 

namespace ExpenseTracker.MobileClient.Helpers
{
    public static class HeaderParser
    {

        public static PagingInfo FindAndParsePagingInfo(HttpResponseHeaders responseHeaders)
        {
            // find the "X-Pagination" info in header
            if (responseHeaders.Contains("X-Pagination"))
            {
                var xPag = responseHeaders.First(ph => ph.Key == "X-Pagination").Value;
                
                // parse the value - this is a JSON-string.
                return JsonConvert.DeserializeObject<PagingInfo>(xPag.First());
            }

        
            return null;
        }
    }
}