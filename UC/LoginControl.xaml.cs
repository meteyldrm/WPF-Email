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
using System.Security.Cryptography;

namespace EmailWPF.UC {
	/// <summary>
	/// Interaction logic for LoginControl.xaml
	/// </summary>
	public partial class LoginControl : UserControl {
		public LoginControl() {
			InitializeComponent();
			LoginActionBTN.Click += LoginActionBTNClick;
			LoginEmailTB.GotFocus += hideWrongCredentials;
			LoginPasswordPB.GotFocus += hideWrongCredentials;
		}

		public event EventHandler<EventArgs> DoLogin;

		private void LoginActionBTNClick(object sender, RoutedEventArgs e) {
			var hash = SHA256_hash(LoginPasswordPB.Password);
			// using (var em = new EmailModel()) {
			// 	var user = em.UserAddress.FirstOrDefault(u => u.emailAddress == LoginEmailTB.Text);
			// 	if (user != null) {
			// 		if(App.Current.users.Exists(u => u.aID == user.aID)) {
			// 			showMultipleLoginWarning();
			// 		} else {
			// 			if (user.passwordHash == hash) {
			// 				if (DoLogin != null) {
			// 					App.Current.addUser(user);
			// 					DoLogin(this, new EventArgs());
			// 				}
			// 			}
			// 			else {
			// 				showWrongCredentials();
			// 			}
			// 		}
			// 	} else {
			// 		showWrongCredentials();
			// 	}
			//}
		}

		public static String SHA256_hash(string value) {
			StringBuilder Sb = new StringBuilder();

			using (var hash = SHA256.Create()) {
				Encoding enc = Encoding.UTF8;
				byte[] result = hash.ComputeHash(enc.GetBytes(value));

				foreach (byte b in result)
					Sb.Append(b.ToString("x2"));
			}

			return Sb.ToString();
		}

		private void showMultipleLoginWarning() {
			LoginCredentialsWarningTB.Text = "Cannot log in to an account more than once!";
		}
		private void showWrongCredentials() {
			LoginCredentialsWarningTB.Text = "Wrong email or password!";
		}
		private void hideWrongCredentials(object sender, RoutedEventArgs e) {
			LoginCredentialsWarningTB.Text = "";
		}
	}
}
