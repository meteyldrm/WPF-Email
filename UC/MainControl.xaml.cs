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

namespace EmailWPF.UC {
	/// <summary>
	/// Interaction logic for MainControl.xaml
	/// </summary>
	public partial class MainControl : UserControl {
		public MainControl() {
			InitializeComponent();
			ComposeFromCB.DropDownOpened += populateFromCB;
			ComposeCategoryCB.DropDownOpened += populateCategoryCB;
			ComposeFromNewBTN.Click += clickComposeFromNewBTN;
		}

		public event EventHandler<EventArgs> TempLogin;

		private void clickComposeFromNewBTN(object sender, EventArgs e) {
			TempLogin(this, new EventArgs());
		}

		private void populateFromCB(object sender, EventArgs e) {
			resetFromCB();
			App.Current.users.ForEach(i => ComposeFromCB.Items.Add(i.emailAddress));
		}

		private void resetFromCB() {
			ComposeFromCB.Items.Clear();
		}

		private void populateCategoryCB(object sender, EventArgs e) {
			resetCategoryCB();
			using (var em = new EmailModel()) {
				em.Categories.ToList().ForEach(i => ComposeCategoryCB.Items.Add(i.categoryName));
			}
		}

		private void resetCategoryCB() {
			ComposeCategoryCB.Items.Clear();
		}

		private void populateAccountCB(object sender, EventArgs e) {
			resetCategoryCB();
			App.Current.users.ForEach(i => InboxAccountCB.Items.Add(i.emailAddress));
		}

		private void resetAccountCB() {
			InboxAccountCB.Items.Clear();
		}

		private void populateInboxScrollViewer(object sender, EventArgs e) {
			var user = App.Current.users.FirstOrDefault(u => u.emailAddress == InboxAccountCB.Text);
			List<InternalEmail> ie = new List<InternalEmail>();
		}

		private void resetInboxScrollViewer() {

		}
	}
}
