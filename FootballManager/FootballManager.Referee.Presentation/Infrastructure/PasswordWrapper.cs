using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Referee.Presentation.Infrastructure
{
	/// <summary>
	/// "Proxy" class used to properly bind PasswordBoxes to other controls. 
	/// Instances of PasswordWrapper should be used as a DataContext for those controls.
	/// </summary>
	class PasswordWrapper : INotifyPropertyChanged
	{
		private bool nonZero = false;
		private string pass = "";

		public string Password
		{
			get
			{
				return pass;
			}
			set
			{
				pass = value;
				Length = (pass.Length != 0);
				OnPropertyChanged("Password");
			}
		}

		/// <summary>
		/// Returns true if database file was opened and password length is nonzero.
		/// Otherwise returns false;
		/// </summary>
		public bool Length
		{
			get
			{
				return nonZero;
			}
			set
			{
				nonZero = value;
				OnPropertyChanged("Length");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
