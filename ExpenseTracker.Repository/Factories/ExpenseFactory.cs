using ExpenseTracker.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Repository.Factories
{
    public class ExpenseFactory
    {

        public ExpenseFactory()
        {

        }

        public DTO.Expense CreateExpense(Expense expense)
        {
            return new DTO.Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }

        public Expense CreateExpense(DTO.Expense expense)
        {
            return new Expense()
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseGroupId = expense.ExpenseGroupId,
                Id = expense.Id
            };
        }

        public object CreateDataShapedObject(Expense expense, List<string> lstOfFields)
        {
            // pass through from entity to DTO
            return CreateDataShapedObject(CreateExpense(expense), lstOfFields);
        }

        public object CreateDataShapedObject(DTO.Expense expense, List<string> lstOfFields)
        {
            if (!lstOfFields.Any())
            {
                return expense;
            }
            else
            {
                // an object whose members can be dynamically removed and added at run-time
                ExpandoObject objectToReturn = new ExpandoObject();

                foreach (var field in lstOfFields)
                {
                    // reflection
                    var fieldValue = expense
                            .GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                            .GetValue(expense, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);
                }
                
                return objectToReturn;
            }
        }         
    }
}
