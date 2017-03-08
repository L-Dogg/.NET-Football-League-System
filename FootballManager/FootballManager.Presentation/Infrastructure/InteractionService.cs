using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FootballManager.Presentation.Infrastructure
{
	public interface IInteractionService
	{
		Task ShowMessageBox(string title, string message);
		Task ShowConfirmationMessage(string title, string message, Action onConfirm);
	}

	public class InteractionService : IInteractionService
	{
		/// <summary>
		/// Shows message box with given title and message
		/// </summary>
		/// <param name="title">Title of message box</param>
		/// <param name="message">Message of message box</param>
		/// <returns>Task</returns>
		public async Task ShowMessageBox(string title, string message)
		{
			var window = Application.Current.MainWindow as MetroWindow;
			await window.ShowMessageAsync(title, message);
		}

		/// <summary>
		/// Shows confirmation message box with given title, message, and confirm action.
		/// </summary>
		/// <param name="title">Title of message box</param>
		/// <param name="message">Message of message box</param>
		/// <param name="onConfirm">Action to perform when user confirms</param>
		/// <returns>Task</returns>
		public async Task ShowConfirmationMessage(string title, string message, Action onConfirm)
		{
			var window = Application.Current.MainWindow as MetroWindow;
			var dialogResult = await window.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative);
			if(dialogResult == MessageDialogResult.Affirmative)
			{
				onConfirm();
			}
		}
	}
}
