using ExpenseTracker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MobileClient.Messages
{
    public class SetExpenseToEditMessage
    {

           public Expense Expense { get; set; }

           public SetExpenseToEditMessage(Expense expense)
	    {
            Expense = expense;
	    }
    }
}
