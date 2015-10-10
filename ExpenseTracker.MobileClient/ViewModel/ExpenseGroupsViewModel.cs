using ExpenseTracker.DTO;
using ExpenseTracker.MobileClient.Helpers;
using ExpenseTracker.MobileClient.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
 

namespace ExpenseTracker.MobileClient.ViewModel
{
 
    public class ExpenseGroupsViewModel : ViewModelBase
    {

        private ObservableCollection<ExpenseGroup> _expenseGroups = null;
        public ObservableCollection<ExpenseGroup> ExpenseGroups
        {
            get
            {
               
                return _expenseGroups;
            }
            set
            {
                if (_expenseGroups != value)
                {
                    _expenseGroups = value;
                    RaisePropertyChanged();
                }

            }
        }


        public ExpenseGroupsViewModel()
        {
            Messenger.Default.Register<LoadExpenseGroupsMessage>(this, (msg) => GetExpenseGroups());
        }

        private async Task GetExpenseGroups()
        {
             // load open expense groups
            var client = ExpenseTrackerHttpClient.GetClient();

            HttpResponseMessage response = await client
                .GetAsync("api/expensegroups?fields=id,title,description");
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var lstEG = JsonConvert.DeserializeObject<IEnumerable<ExpenseGroup>>(content);
                ExpenseGroups = new ObservableCollection<ExpenseGroup>(lstEG);  
            }
            else
            {
                // something went wrong, log this, handle this, show message, ...
            }

        }
    }
}