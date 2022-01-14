using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EmailWPF.UC {
	/// <summary>
	/// Interaction logic for LoginControl.xaml
	/// </summary>
	public partial class LoginControl {
		public LoginControl() {
			InitializeComponent();
			LoginActionBTN.Click += LoginActionBTNClick;
			LoginEmailTB.GotFocus += hideWrongCredentials;
			LoginPasswordPB.GotFocus += hideWrongCredentials;
		}

		public event EventHandler<EventArgs> DoLogin;

		private void LoginActionBTNClick(object sender, RoutedEventArgs e) {
			var hash = SHA256_hash(LoginPasswordPB.Password);
			var dsAddress = App.Current.getDataSetForQuery($"select top 1 * from UserAddress as u where u.emailAddress = '{LoginEmailTB.Text}'");
			var dsAddressTable = dsAddress.Tables["UserAddress"];
			if (dsAddressTable.Rows.Count > 0) {
				if (App.Current.users.Exists(i => i["addressID"] == dsAddressTable.Rows[0]["addressID"])) {
					showMultipleLoginWarning();
				} else {
					if ((string)dsAddressTable.Rows[0]["passwordHash"] == hash) {
						if (DoLogin != null) {
							App.Current.addUser(dsAddressTable.Rows[0]);
							DoLogin(this, EventArgs.Empty);
						}
					} else {
						showWrongCredentials();
					}
				}
			} else {
				showWrongCredentials();
			}
		}

		private static string SHA256_hash(string value) {
			var Sb = new StringBuilder();

			using (var hash = SHA256.Create()) {
				var enc = Encoding.UTF8;
				var result = hash.ComputeHash(enc.GetBytes(value));

				foreach (var b in result)
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

		public void doInitializationSequence() {
			LoginEmailTB.Text = "johndoe@emailprovider.com";
			LoginPasswordPB.Password = "testpassword";
		}
	}
}
