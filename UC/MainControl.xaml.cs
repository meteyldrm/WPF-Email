using System;
using System.Collections.Generic;
using System.Data;
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
			ComposeDiscardBTN.Click += clickComposeDiscardBTN;
			InboxAccountCB.DropDownOpened += populateAccountCB;
			InboxAccountCB.DropDownClosed += populateInboxThreadScrollViewer;
			InboxThreadDG.SelectionMode = SelectionMode.Single;
			InboxThreadDG.SelectionChanged += populateInboxThreadEmailScrollViewer;
			InboxThreadEmailDG.SelectionMode = SelectionMode.Single;
			InboxThreadEmailDG.SelectionChanged += populateInboxEmailScrollViewer;
		}

		public event EventHandler<EventArgs> TempLogin;

		private void clickComposeFromNewBTN(object sender, EventArgs e) {
			if (TempLogin != null) TempLogin(this, EventArgs.Empty);
		}

		private void clickComposeDiscardBTN(object sender, EventArgs e) {
			ComposeToTB.Text = "";
			ComposeSubjectTB.Text = "";
			ComposeCategoryCB.SelectedItem = null;
			ComposeFromCB.SelectedItem = null;
			ComposeBodyTB.Text = "";
		}

		private void populateFromCB(object sender, EventArgs e) {
			resetFromCB();
			App.Current.users.ForEach(i => ComposeFromCB.Items.Add(i["emailAddress"]));
		}

		private void resetFromCB() {
			ComposeFromCB.Items.Clear();
		}

		private void populateCategoryCB(object sender, EventArgs e) {
			resetCategoryCB();
			foreach (DataRow row in App.Current.getDataSetForQuery("select * from Categories").Tables["Categories"].Rows) {
				ComposeCategoryCB.Items.Add(row["categoryName"]);
			}
		}

		private void resetCategoryCB() {
			ComposeCategoryCB.Items.Clear();
		}

		private void populateAccountCB(object sender, EventArgs e) {
			resetAccountCB();
			App.Current.users.ForEach(i => InboxAccountCB.Items.Add(i["emailAddress"]));
		}

		private void resetAccountCB() {
			InboxAccountCB.Items.Clear();
		}

		private void populateInboxThreadScrollViewer(object sender, EventArgs e) {
			resetInboxThreadScrollViewer();
			var ds = _retrieveThreads();
			foreach (DataRow row in ds.Tables["Recipients"].Rows) {
				var q = _retrieveTopThreadEmail((int)row["threadID"]);
				InboxThreadDG.Items.Add(q.Tables["InternalEmail"].Rows[0]["emailSubject"]);
				App.Current.activeThreads.Add((int)row["threadID"]);
			}
		}

		private void resetInboxThreadScrollViewer() {
			InboxThreadDG.Items.Clear();
			App.Current.activeThreads.Clear();
		}
		
		private void populateInboxThreadEmailScrollViewer(object sender, EventArgs e) {
			resetInboxThreadEmailScrollViewer();
			var ds = _retrieveThreadEmails(App.Current.activeThreads[InboxThreadDG.SelectedIndex]);
			foreach (DataRow row in ds.Tables["Thread"].Rows) {
				var q = _retrieveEmail((long)row["internalEmailID"]);
				InboxThreadEmailDG.Items.Add(q.Tables["InternalEmail"].Rows[0]["emailSubject"]);
				App.Current.activeThreadEmails.Add((long)row["internalEmailID"]);
			}
		}
		
		private void resetInboxThreadEmailScrollViewer() {
			InboxThreadEmailDG.Items.Clear();
			App.Current.activeThreadEmails.Clear();
		}
		
		private void populateInboxEmailScrollViewer(object sender, EventArgs e) {
			resetInboxEmailScrollViewer();
			var ds = _retrieveEmail(App.Current.activeThreadEmails[InboxThreadEmailDG.SelectedIndex]);
			InboxEmailDG.Items.Add(ds.Tables["InternalEmail"].Rows[0]["emailBody"]);
		}
		
		private void resetInboxEmailScrollViewer() {
			InboxEmailDG.Items.Clear();
		}

		private DataSet _retrieveThreads() {
			var user = App.Current.users.FirstOrDefault(u => (string)u["emailAddress"] == InboxAccountCB.Text);
			if (user != null) {
				var id = (int)user["userID"];
				var threadQuery = $"select threadID as threadID from Recipients inner join RecipientUser RU on Recipients.recipientID = RU.recipientID inner join UserAddress UA on UA.aID = RU.userID where UA.aID = {id}";
				return App.Current.getDataSetForQuery(threadQuery);
			} return null;
		}

		private DataSet _retrieveThreadEmails(int threadID) {
			var threadEmailQuery = $"select internalEmailID as internalEmailID from Thread inner join InternalEmail IE on Thread.internalEmailID = IE.intEmailID where thread.threadID = {threadID}";
			return App.Current.getDataSetForQuery(threadEmailQuery);
		}
		
		private DataSet _retrieveTopThreadEmail(int threadID) {
			var threadEmailQuery = $"select min(internalEmailID) as internalEmailID from Thread inner join InternalEmail IE on Thread.internalEmailID = IE.intEmailID where thread.threadID = {threadID}";
			var t = App.Current.getDataSetForQuery(threadEmailQuery);
			return _retrieveEmail((long)t.Tables["Thread"].Rows[0]["internalEmailID"]);
		}

		private DataSet _retrieveEmail(long intEmailID) {
			var emailQuery = $"select * from InternalEmail where intEmailID = {intEmailID}";
			return App.Current.getDataSetForQuery(emailQuery);
		}
	}
}
