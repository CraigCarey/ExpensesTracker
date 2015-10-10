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
    public sealed partial class ExpenseGroupsView : Page
    {
        private NavigationHelper navigationHelper;
 

        public ExpenseGroupsView()
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

   
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // load the expense groups
            Messenger.Default.Send<LoadExpenseGroupsMessage>(new LoadExpenseGroupsMessage());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
             
        }

        #endregion

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ExpensesView), e.ClickedItem);
        }
    }
}
