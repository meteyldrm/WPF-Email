using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EmailWPF {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public static new App Current => Application.Current as App;

		private void App_Startup(object sender, StartupEventArgs e) {
			MainWindow w = new MainWindow();
			w.Show();
		}

		public List<UserAddress> users = new List<UserAddress>();
		public void addUser(UserAddress addr) {
			users.Add(addr);
		}
	}
}
