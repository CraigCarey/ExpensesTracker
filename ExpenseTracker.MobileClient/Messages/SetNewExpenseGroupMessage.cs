using ExpenseTracker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MobileClient.Messages
{
    public class SetNewExpenseGroupMessage
    {
        public ExpenseGroup ExpenseGroup { get; set; }

        public SetNewExpenseGroupMessage (ExpenseGroup expenseGroup)
	    {
                ExpenseGroup = expenseGroup;
	    }
    }
}
