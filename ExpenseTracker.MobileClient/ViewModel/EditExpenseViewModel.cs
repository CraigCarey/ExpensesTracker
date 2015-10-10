using ExpenseTracker.DTO;
using ExpenseTracker.MobileClient.Common;
using ExpenseTracker.MobileClient.Helpers;
using ExpenseTracker.MobileClient.Messages;
using ExpenseTracker.MobileClient.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Marvin.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MobileClient.ViewModel
{
    public class EditExpenseViewModel : ViewModelBase
    {

        private RelayCommand _saveExpenseCommand;
        public RelayCommand SaveExpenseCommand
        {
            get
            {
                if (_saveExpenseCommand == null)
                {
                    _saveExpenseCommand = new RelayCommand(() => SaveExpense());
                }
                return _saveExpenseCommand;
            }
        }





        private Expense _expense = null;
        public Expense Expense
        {
            get
            {
                return _expense;
            }
            set
            {
                if (_expense != value)
                {
                    _expense = value;
                    RaisePropertyChanged();
                }
            }
        }


        public EditExpenseViewModel()
        {
            Messenger.Default.Register<SetExpenseToEditMessage>(this, (msg) => SetExpense(msg.Expense));
        }

        private void SetExpense(DTO.Expense expense)
        {
            // We should not use the same expense instance, but a new one

            Expense = new Expense()
                {
                    Amount = expense.Amount,
                     Date = expense.Date,
                      Description = expense.Description,
                     ExpenseGroupId = expense.ExpenseGroupId,
                      Id = expense.Id
                };
        }


        private async Task SaveExpense()
        {
            // partial update

            var client = ExpenseTrackerHttpClient.GetClient();

            // create a patch document, containing the (possible) changes to the
            // expense DTO
 
            JsonPatchDocument<DTO.Expense> patchDoc = new JsonPatchDocument<DTO.Expense>();
            patchDoc.Replace(e => e.Description, Expense.Description);

            // serialize & PATCH
            var serializedItemToUpdate = JsonConvert.SerializeObject(patchDoc);


            var response = client.PatchAsync("api/expenses/" + Expense.Id,
                new StringContent(serializedItemToUpdate,
                System.Text.Encoding.Unicode, "application/json")).Result;

            if (response.IsSuccessStatusCode)
            {
                // return to the expense list
                App.RootFrame.Navigate(typeof(ExpensesView), true);

            }
            else
            {
                // error: handle, log, ...
            }
            
        }
    }
}
