using ExpenseTracker.DTO;
using ExpenseTracker.MobileClient.Common;
using ExpenseTracker.MobileClient.Messages;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ExpenseTracker.MobileClient.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExpensesView : Page
    {
        private NavigationHelper navigationHelper;
     
        public ExpensesView()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
          
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
   

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // we can come here from either the groups page, or from the 
            // "add/edit expense" page.  
            // 
            // If the first is the case, we want to 
            // ensure the ExpenseGroup is set and the Expenses are loaded.
            // 
            // If the second is the case, we want to refresh the expenses

            if (e.NavigationMode != NavigationMode.Back && e.Parameter != null)
            {
                
                    // expensegroup or bool? 
                    if ((e.Parameter as ExpenseGroup) != null)
                    {
                        // the parameter contains the ExpenseGroup.  Pass this to the
                        // ViewModel.
                        Messenger.Default.Send<SetNewExpenseGroupMessage>(
                            new SetNewExpenseGroupMessage(e.Parameter as ExpenseGroup));
                    }
                    else
                    {
                        bool isOk = false;
                        if (bool.TryParse(e.Parameter.ToString(), out isOk))
                        {
                            Messenger.Default.Send<RefreshExpensesMessage>(
                            new RefreshExpensesMessage());
                        }
                    }
               
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
       
        }

        #endregion

        private void ListViewExpenses_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(EditExpenseView), e.ClickedItem);
        }
         
    }
}
