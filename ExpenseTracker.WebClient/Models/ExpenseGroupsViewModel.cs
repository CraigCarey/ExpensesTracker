using ExpenseTracker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseTracker.WebClient.Models
{
    public class ExpenseGroupsViewModel
    {
        public IEnumerable<ExpenseGroup> ExpenseGroups { get; set; }

        public IEnumerable<ExpenseGroupStatus> ExpenseGroupStatusses { get; set; }
    }
}