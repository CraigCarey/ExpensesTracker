using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Marvin.HttpCache;

namespace ExpenseTracker.MobileClient.Helpers
{
    public static class ExpenseTrackerHttpClient
    {
        // static as we reuse a single client
        private static HttpClient currentClient = null;

        public static HttpClient GetClient()
        {
            if (currentClient == null)
            {
                currentClient = new HttpClient(new Marvin.HttpCache.HttpCacheHandler()
                {
                    InnerHandler = new HttpClientHandler()
                });

                currentClient.BaseAddress = new Uri(ExpenseTrackerConstants.ExpenseTrackerAPI);

                currentClient.DefaultRequestHeaders.Accept.Clear();
                currentClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             }

            return currentClient;
        }         
    }     
}