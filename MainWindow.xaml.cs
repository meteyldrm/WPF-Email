using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmailWPF {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			var lc = new UC.LoginControl();
			lc.DoLogin += new EventHandler<EventArgs>(OpenMainScreen);
			BorderMain.Child = lc;
		}

		void OpenLoginScreen(object sender, EventArgs e) {
			BorderMain.Child = new UC.LoginControl();
		}

		void OpenMainScreen(object sender, EventArgs e) {
			BorderMain.Child = new UC.MainControl();
		}
	}
}
