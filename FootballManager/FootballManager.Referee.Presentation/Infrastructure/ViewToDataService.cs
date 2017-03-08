using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FootballManager.Referee.Presentation.Infrastructure
{
	public interface IViewToDataService
	{
		object DataGridToObject(object sender, MouseEventArgs e);
	}
	public class ViewToDataService : IViewToDataService
	{
		/// <summary>
		/// Convert data from view to data object
		/// </summary>
		/// <param name="sender">Original sender of event</param>
		/// <param name="e">Original parameters of event</param>
		/// <returns>Data object</returns>
		public object DataGridToObject(object sender, MouseEventArgs e)
		{
			var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
			return row?.DataContext;
		}
	}
}
