using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EmailWPF.UC {
	/// <summary>
	/// Interaction logic for MainControl.xaml
	/// </summary>
	public partial class MainControl {
		public MainControl() {
			InitializeComponent();
			ComposeFromCB.DropDownOpened += populateFromCB;
			ComposeFromCB.DropDownClosed += setComposeActiveUser;
			ComposeCategoryCB.DropDownOpened += populateCategoryCB;
			ComposeFromNewBTN.Click += clickComposeFromNewBTN;
			ComposeDiscardBTN.Click += clickComposeDiscardBTN;
			ComposeSendBTN.Click += clickComposeSendBTN;
			InboxAccountCB.DropDownOpened += populateAccountCB;
			// InboxAccountCB.DropDownClosed += populateInboxThreadScrollViewer;
			// InboxThreadDG.SelectionMode = SelectionMode.Single;
			// InboxThreadDG.SelectionChanged += populateInboxThreadEmailScrollViewer;
			// InboxThreadEmailDG.SelectionMode = SelectionMode.Single;
			// InboxThreadEmailDG.SelectionChanged += populateInboxEmailScrollViewer;
			// InboxEmailReplyBTN.Click += doReply;
			
			List<string> _t = new List<string>{"Email Portal is Live", "Team Assignments", "Library Mobile Client Project", "Library Server Project", "Happy Hour on Friday"};
			foreach (var VARIABLE in _t) {
				InboxThreadDG.Items.Add(VARIABLE);
			}
			List<string> _te = new List<string>{"Email Portal v1 is available", "Multi-account login feature added", "Would you like extra categories?", "New categories are added"};
			foreach (var VARIABLE in _te) {
				InboxThreadEmailDG.Items.Add(VARIABLE);
			}

			InboxEmailDG.Text = "There's a new multi-account login functionality which allows you to use multiple accounts at the same time. If you would like a different email address for another team, contact the IT department.";
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

		private void clickComposeSendBTN(object sender, EventArgs e) {
			sendEmail(App.Current.currentUserEmailAddress, 
				ComposeToTB.Text, ComposeSubjectTB.Text,
				ComposeCategoryCB.SelectedItem.ToString(),ComposeBodyTB.Text,
				App.Current.activeThread.GetValueOrDefault(), ComposeThreadNameTB.Text);
			
			
		}

		private void populateFromCB(object sender, EventArgs e) {
			resetFromCB();
			App.Current.users.ForEach(i => ComposeFromCB.Items.Add(i["emailAddress"]));
		}

		private void setComposeActiveUser(object sender, EventArgs e) {
			if (ComposeFromCB.SelectedIndex < 0) return;
			App.Current.currentUserEmailAddress = ComposeFromCB.SelectedItem.ToString();
		}
		
		private void setInboxActiveUser(object sender, EventArgs e) {
			if (InboxAccountCB.SelectedIndex < 0) return;
			App.Current.currentUserEmailAddress = InboxAccountCB.SelectedItem.ToString();
		}

		private void resetFromCB() {
			ComposeFromCB.Items.Clear();
		}

		private void populateCategoryCB(object sender, EventArgs e) {
			resetCategoryCB();
			foreach (DataRow row in App.Current.getDataSetForQuery("select * from Category").Tables["Category"].Rows) {
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
			if (InboxAccountCB.SelectedIndex < 0) return;
			setInboxActiveUser(this, EventArgs.Empty);
			resetInboxThreadScrollViewer();
			var ds = _retrieveThreads();
			foreach (DataRow row in ds.Tables["Thread"].Rows) {
				InboxThreadDG.Items.Add(row["threadName"]);
				App.Current.activeThreads.Add((int)row["threadID"]);
			}
		}

		private void resetInboxThreadScrollViewer() {
			InboxThreadDG.Items.Clear();
			App.Current.activeThreads.Clear();
		}
		
		private void populateInboxThreadEmailScrollViewer(object sender, EventArgs e) {
			if (InboxThreadDG.SelectedIndex < 0) return;
			resetInboxThreadEmailScrollViewer();
			var ds = _retrieveThreadEmails(App.Current.activeThreads[InboxThreadDG.SelectedIndex]);
			foreach (DataRow row in ds.Tables["Thread"].Rows) {
				InboxThreadEmailDG.Items.Add(row["threadName"].ToString());
				App.Current.activeThreadEmails.Add((long)row["emailID"]);
				App.Current.activeThread = App.Current.activeThreads[InboxThreadDG.SelectedIndex];
			}

		}
		
		private void resetInboxThreadEmailScrollViewer() {
			InboxThreadEmailDG.Items.Clear();
			App.Current.activeThreadEmails.Clear();
			App.Current.activeThread = null;
		}
		
		private void populateInboxEmailScrollViewer(object sender, EventArgs e) {
			if (InboxThreadEmailDG.SelectedIndex < 0) return;
			resetInboxEmailScrollViewer();
			var ds = _retrieveEmail(App.Current.activeThreadEmails[InboxThreadEmailDG.SelectedIndex]);
			var email = ds.Tables["Email"].Rows[0];
			InboxEmailDG.Text = email["emailBody"].ToString();
			var s = _retrieveUserData((int)email["senderID"]);
			InboxEmailFromField.Text = $"From: {s.Tables["UserAddress"].Rows[0]["emailAddress"]}";
			var c = _retrieveCategoryData((int)email["categoryID"]);
			InboxEmailCategoryField.Text = $"Category: {c.Tables["Category"].Rows[0]["categoryName"]}";
			InboxEmailReplyBTN.Visibility = Visibility.Visible;
		}
		
		private void resetInboxEmailScrollViewer() {
			InboxEmailDG.Text = "";
			InboxEmailFromField.Text = "";
			InboxEmailCategoryField.Text = "";
			InboxEmailReplyBTN.Visibility = Visibility.Hidden;
		}

		private void doReply(object sender, EventArgs e) {
			populateAccountCB(this, EventArgs.Empty);
			populateCategoryCB(this, EventArgs.Empty);
			populateFromCB(this, EventArgs.Empty);

			ComposeFromCB.SelectedItem = App.Current.currentUserEmailAddress;
			ComposeCategoryCB.SelectedIndex = ComposeCategoryCB.Items.IndexOf(InboxEmailCategoryField.Text.Replace("Category: ", ""));
			ComposeSubjectTB.Text = InboxThreadEmailDG.SelectedItem.ToString();
			var ds = _retrieveEmail(App.Current.activeThreadEmails[InboxThreadEmailDG.SelectedIndex]);
			
			//Get selected email
			var email = ds.Tables["Email"].Rows[0];

			App.Current.activeThread = (int)email["threadID"];

			ComposeThreadNameTB.Text = App.Current.getDataSetForQuery($"select threadName as threadName from Thread where threadID = {(int)email["threadID"]}").Tables["Thread"].Rows[0]["threadName"].ToString();
			ComposeThreadNameTB.IsEnabled = false;
			
			//Get email addresses from recipient group
			var dsUserAccount = App.Current.getDataSetForQuery($"select emailAddress as emailAddress from UserAddress inner join UserEmails UE on UserAddress.addressID = UE.addressID where UE.emailID = {email.Table.Rows[0]["emailID"]}");
			
			List<string> addresses = new List<string>();
			foreach (DataRow row in dsUserAccount.Tables["UserAddress"].Rows) {
				var user = row["emailAddress"].ToString();
				if (user != InboxAccountCB.Text) {
					addresses.Add(user);
				}

				var senderAddress = App.Current.getDataSetForQuery($"select emailAddress as emailAddress from UserAddress inner join UserEmails UE on UserAddress.addressID = {email.Table.Rows[0]["senderID"]}").Tables["UserAddress"].Rows[0]["emailAddress"].ToString();
				addresses.Add(senderAddress);
			}
			
			//Join with "; "
			ComposeToTB.Text = string.Join("; ", addresses);
			MainTabControl.SelectedItem = MainTabControl.Items[0];
		}

		private DataSet _retrieveUserData(int senderID) {
			var senderQuery = $"select emailAddress as emailAddress from UserAddress inner join Email IE on UserAddress.addressID = {senderID}";
			return App.Current.getDataSetForQuery(senderQuery);
		}

		private DataSet _retrieveCategoryData(int categoryID) {
			var categoryQuery = $"select categoryName as categoryName from Category where categoryID = {categoryID}";
			return App.Current.getDataSetForQuery(categoryQuery);
		}

		private DataSet _retrieveThreads() {
			var user = App.Current.users.FirstOrDefault(u => (string)u["emailAddress"] == InboxAccountCB.Text);
			if (user != null) {
				var id = (int)user["userID"];
				var threadQuery = $"select * from Thread inner join Email E on Thread.threadID = E.threadID inner join UserEmails UE on E.emailID = UE.emailID where addressID = {id}";
				return App.Current.getDataSetForQuery(threadQuery);
			} return null;
		}

		private DataSet _retrieveThreadEmails(int threadID) {
			var threadEmailQuery = $"select emailID as emailID from Email where threadID = {threadID}";
			return App.Current.getDataSetForQuery(threadEmailQuery);
		}
		
		private DataSet _retrieveEmail(long emailID) {
			var emailQuery = $"select * from Email where emailID = {emailID}";
			return App.Current.getDataSetForQuery(emailQuery);
		}

		private void sendEmail(string from, string recipients, string subject, string category, string body, long threadID = -1, string threadName = "") {
			if (threadID == -1) {
				App.Current.executeVoidQuery($"insert into Thread (threadName, threadStartTime) values ('{threadName}','{DateTime.Now}')");
				threadID = (long)App.Current.getDataSetForQuery("select max(threadID) as threadID from Thread").Tables["Thread"].Rows[0]["threadID"];
			}
			
			//Create InternalEmail
			int cID = (int)App.Current.getDataSetForQuery($"select categoryID as categoryID from Category where categoryName = '{category}'").Tables["Category"].Rows[0]["categoryID"];
			int uID = (int)App.Current.getDataSetForQuery($"select addressID as addressID from UserAddress where emailAddress = '{from}'").Tables["UserAddress"].Rows[0]["addressID"];
			App.Current.executeVoidQuery($"insert into Email (priority, subject, body, composeTime, categoryID, senderID, threadID, isDraft) values (1, '{subject}', '{body}', '{DateTime.Now}', {cID}, {uID}, {threadID}, {false})");
			
			long emailID;
			try {
				emailID = (long)App.Current.getDataSetForQuery("select max(emailID) as emailID from Email").Tables["Email"].Rows[0]["emailID"]; //Null throws cast exception
			} catch (InvalidCastException e) {
				emailID = 1;
			}
			
			
			//Split array overload
			foreach (var recipient in recipients.Split(new []{"; "}, StringSplitOptions.None)) {
				int rUID = (int)App.Current.getDataSetForQuery($"select addressID as addressID from UserAddress where emailAddress = '{recipient}'").Tables["UserAddress"].Rows[0]["addressID"];
				App.Current.executeVoidQuery($"insert into UserEmails values ({rUID}, {emailID}, {false}, {false}, {false})", true);
			}
			App.Current.executeVoidQuery(""); //Close connection
		}
	}
}
