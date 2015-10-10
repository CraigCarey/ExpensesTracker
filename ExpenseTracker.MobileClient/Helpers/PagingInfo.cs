using System;
using System.Collections.Generic;
using System.Linq;
 

namespace ExpenseTracker.MobileClient.Helpers
{
    public class PagingInfo
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }


        public PagingInfo(int totalCount, int totalPages, int currentPage, int pageSize, string previousPageLink, string nextPageLink)
        {
            this.TotalCount = totalCount;
            this.TotalPages = totalPages;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.PreviousPageLink = previousPageLink;
            this.NextPageLink = nextPageLink;
        }
    }
}