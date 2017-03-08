using FootballManager.BusinessLayer;
using FootballManager.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FootballManager.Presentation.ViewModels;
using FootballManager.Presentation.Infrastructure;

namespace FootballManager.Presentation.Views
{
	/// <summary>
	/// Interaction logic for SelectEditItem.xaml
	/// Search queries are delayed for VALIDATION_DELAY miliseconds.
	/// If user pressed Enter or Return key, query is performed immediately.
	/// </summary>
	public partial class SelectEditItem : UserControl
    {
		/// <summary>
		/// Delay time after which search query will be performed.
		/// </summary>
		private static int VALIDATION_DELAY = 1500;
		/// <summary>
		/// Timer for delayed queries.
		/// </summary>
		private Timer timer = null;

        public SelectEditItem()
        {
            InitializeComponent();
	        this.searchTextbox.Focusable = true;
	        //Keyboard.Focus(this.searchTextbox);
        }

	    #region Delayed Search Methods
		/// <summary>
		/// Event for text changes in searchtextbox.
		/// </summary>
		private void SearchTextbox_OnTextChanged(object sender, TextChangedEventArgs e)
	    {
			var origin = sender as TextBox;
			if (!origin.IsFocused)
				return;

			DisposeTimer();
			this.okButton.IsEnabled = false;
			this.cancelButton.IsEnabled = false;
			GlobalCommands.BlockWindowButtons();
			timer = new Timer(TimerElapsed, null, VALIDATION_DELAY, VALIDATION_DELAY);
		}

		/// <summary>
		/// Performs search query when timer time has elapsed.
		/// </summary>
		/// <param name="obj"></param>
		private void TimerElapsed(object obj)
		{
			this.Dispatcher.Invoke(PerformSearch);
			DisposeTimer();
		}

		/// <summary>
		/// Dispoes timer.
		/// </summary>
		private void DisposeTimer()
		{
			if (timer == null) return;
			timer.Dispose();
			timer = null;
		}

		/// <summary>
		/// Searches the database for text in searchtextbox.
		/// Blocks buttons whilst searching.
		/// </summary>
		private async void PerformSearch()
		{
			var vm = (this.DataContext) as SelectEditItemViewModel;
			await Dispatcher.InvokeAsync(() => vm.Search(this.searchTextbox.Text));

			this.cancelButton.IsEnabled = true;
			this.okButton.IsEnabled = true;
		}

		/// <summary>
		/// If Enter/Return is pressed, query is performed immediately.
		/// </summary>
	    private void SearchTextbox_OnKeyDown(object sender, KeyEventArgs e)
	    {
		    if (e.Key != Key.Enter && e.Key != Key.Return) return;
		    PerformSearch();
	    }
    }
	#endregion
}

