using ExpenseTracker.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MobileClient.Messages
{
    public class SetNewExpenseToAddMessage
    {

        public int ExpenseGroupId { get; set; }

        public SetNewExpenseToAddMessage(int expenseGroupId)
        {
            ExpenseGroupId = expenseGroupId;
        }
    }
}
