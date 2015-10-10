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
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MobileClient.ViewModel
{
    public class ExpensesViewModel : ViewModelBase
    {

        private ExpenseGroup _expenseGroup = null;
        public ExpenseGroup ExpenseGroup
        {
            get
            {
                return _expenseGroup;
            }
            set
            {
                if (_expenseGroup != value)
                {
                    _expenseGroup = value;
                    RaisePropertyChanged();
                }
            }
        }


        private ObservableCollection<Expense> _expenses = new ObservableCollection<Expense>();
        public ObservableCollection<Expense> Expenses
        {
            get
            {
               
                return _expenses;
            }
            set
            {
                if (_expenses != value)
                {
                    _expenses = value;
                    RaisePropertyChanged();
                }
            }
        }

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



        public ExpensesViewModel()
        {
            Messenger.Default.Register<SetNewExpenseGroupMessage>(this, 
                (msg) => SetNewExpenseGroup(msg.ExpenseGroup));

            Messenger.Default.Register<RefreshExpensesMessage>(this,
                (msg) => RefreshExpenses());
        }

        private async Task RefreshExpenses()
        {

            Expenses.Clear();

            // load expenses for group
            var client = ExpenseTrackerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync("api/expensegroups/"
                + ExpenseGroup.Id + "/expenses?fields=id,date,description,amount");
              
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var lstExpenses = JsonConvert.DeserializeObject<IEnumerable<Expense>>(content);
                Expenses = new ObservableCollection<Expense>(lstExpenses);
            }
            else
            {
                // something went wrong, log this, handle this, show message, ...
            }
        }



        private async void SetNewExpenseGroup(DTO.ExpenseGroup expenseGroup)
        {
            // a new expense group has been clicked.  Set the expense group, and
            // load the expenses for this expense group.
            ExpenseGroup = expenseGroup;

            // load expenses for group
            Expenses.Clear();

            var client = ExpenseTrackerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync("api/expensegroups/"
                + ExpenseGroup.Id + "/expenses?fields=id,date,description,amount");
      

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var lstExpenses = JsonConvert.DeserializeObject<IEnumerable<Expense>>(content);
                Expenses = new ObservableCollection<Expense>(lstExpenses);
            }
            else
            {
                // something went wrong, log this, handle this, show message, ...
            }
        }



        private void AddExpense()
        {
              App.RootFrame.Navigate(typeof(AddExpenseView), ExpenseGroup.Id);
        }

        

         
    }
}
