using ExpenseTracker.DTO;
using ExpenseTracker.MobileClient.Helpers;
using ExpenseTracker.MobileClient.Messages;
using ExpenseTracker.MobileClient.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MobileClient.ViewModel
{
    public class AddExpenseViewModel : ViewModelBase
    {


        private RelayCommand _addExpenseCommand;
        public RelayCommand AddExpenseCommand
        {
            get
            {
                if (_addExpenseCommand == null)
                {
                    _addExpenseCommand = new RelayCommand(() => AddExpense());
                }
                return _addExpenseCommand;
            }
        }




        private Expense _expense = new Expense();
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

        public AddExpenseViewModel()
        {
            Messenger.Default.Register<SetNewExpenseToAddMessage>(this, msg => SetNewExpense(msg.ExpenseGroupId));
        }

        private void SetNewExpense(int expenseGroupId)
        {
            Expense = new Expense()
                {
                    ExpenseGroupId = expenseGroupId
                };
        }



        private async Task AddExpense()
        {
            // create an expense 

            var client = ExpenseTrackerHttpClient.GetClient();

            // serialize & POST
            var serializedItemToCreate = JsonConvert.SerializeObject(Expense);

            var response = client.PostAsync("api/expenses",
                new StringContent(serializedItemToCreate,
                System.Text.Encoding.Unicode, "application/json")).Result;

      
            if (response.IsSuccessStatusCode)
            {
                // return to the expense overview
                App.RootFrame.Navigate(typeof(ExpensesView), true);
            }
            else
            {
                // handle, log, ...
            }
        }
        
    }
}
