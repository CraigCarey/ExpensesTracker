using ExpenseTracker.DTO;
using ExpenseTracker.WebClient.Helpers;
using PagedList;
using System.Collections.Generic;

namespace ExpenseTracker.WebClient.Models
{
    public class ExpenseGroupsViewModel
    {
        public IPagedList<ExpenseGroup> ExpenseGroups { get; set; }

        public IEnumerable<ExpenseGroupStatus> ExpenseGroupStatusses { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}