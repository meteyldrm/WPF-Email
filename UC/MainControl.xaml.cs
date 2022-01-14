using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
			InboxAccountCB.DropDownClosed += populateInboxThreadScrollViewer;
			InboxThreadDG.SelectionMode = SelectionMode.Single;
			InboxThreadDG.SelectionChanged += populateInboxThreadEmailScrollViewer;
			InboxThreadEmailDG.SelectionMode = SelectionMode.Single;
			InboxThreadEmailDG.SelectionChanged += populateInboxEmailScrollViewer;
			InboxEmailReplyBTN.Click += doReply;

			ComposeFromCB.SelectedItem = App.Current.currentUserEmailAddress;
			InboxAccountCB.SelectedItem = App.Current.currentUserEmailAddress;
			populateInboxThreadScrollViewer(this, EventArgs.Empty);
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
			sendEmail(App.Current.currentUserEmailAddress, ComposeToTB.Text, ComposeSubjectTB.Text, ComposeCategoryCB.SelectedItem.ToString(),ComposeBodyTB.Text, App.Current.activeRecipients.GetValueOrDefault(), App.Current.activeThread.GetValueOrDefault());
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
			foreach (DataRow row in App.Current.getDataSetForQuery("select * from Categories").Tables["Category"].Rows) {
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
			if (InboxThreadDG.SelectedIndex < 0) return;
			resetInboxThreadEmailScrollViewer();
			var ds = _retrieveThreadEmails(App.Current.activeThreads[InboxThreadDG.SelectedIndex]);
			foreach (DataRow row in ds.Tables["Thread"].Rows) {
				var q = _retrieveEmail((long)row["internalEmailID"]);
				InboxThreadEmailDG.Items.Add(q.Tables["InternalEmail"].Rows[0]["emailSubject"]);
				App.Current.activeThreadEmails.Add((long)row["internalEmailID"]);
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
			var email = ds.Tables["InternalEmail"].Rows[0];
			InboxEmailDG.Items.Add(email["emailBody"]);
			var s = _retrieveUserData((int)email["senderID"]);
			InboxEmailFromField.Text = $"From: {s.Tables["UserAddress"].Rows[0]["emailAddress"]}";
			var c = _retrieveCategoryData((int)email["categoryID"]);
			InboxEmailCategoryField.Text = $"Category: {c.Tables["Categories"].Rows[0]["categoryName"]}";
			InboxEmailReplyBTN.Visibility = Visibility.Visible;
		}
		
		private void resetInboxEmailScrollViewer() {
			InboxEmailDG.Items.Clear();
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
			var email = ds.Tables["InternalEmail"].Rows[0];
			
			//Get thread from selected email
			var dsThread = App.Current.getDataSetForQuery($"select threadID as threadID from Thread inner join InternalEmail IE on Thread.internalEmailID = {email["intEmailID"]}", true);
			
			//Get thread recipients
			var dsRecipient = App.Current.getDataSetForQuery($"select top 1 recipientID as recipientID from Recipients where threadID = {dsThread.Tables["Thread"].Rows[0]["threadID"]}", true);
			App.Current.activeRecipients = (long)dsRecipient.Tables["Recipients"].Rows[0]["recipientID"];
			
			//Get email addresses from recipient group
			var dsUserAccount = App.Current.getDataSetForQuery($"select emailAddress as emailAddress from UserAddress inner join RecipientUser RU on UserAddress.aID = RU.userID where recipientID = {dsRecipient.Tables["Recipients"].Rows[0]["recipientID"]}");
			
			List<string> addresses = new List<string>();
			foreach (DataRow row in dsUserAccount.Tables["UserAddress"].Rows) {
				addresses.Add(row["emailAddress"].ToString());
			}
			
			//Join with "; "
			ComposeToTB.Text = string.Join("; ", addresses);
			MainTabControl.SelectedItem = MainTabControl.Items[0];
		}

		private DataSet _retrieveUserData(int senderID) {
			var senderQuery = $"select emailAddress as emailAddress from UserAddress inner join InternalEmail IE on UserAddress.aID = {senderID}";
			return App.Current.getDataSetForQuery(senderQuery);
		}

		private DataSet _retrieveCategoryData(int categoryID) {
			var categoryQuery = $"select categoryName as categoryName from Categories where categoryID = {categoryID}";
			return App.Current.getDataSetForQuery(categoryQuery);
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

		private void sendEmail(string from, string recipients, string subject, string category, string body, long recipientID = -1, long threadID = -1) {
			var _recipientID = recipientID;
			if (recipientID == -1) { //part 1, get undisclosed recipient ID
				//Create new recipients entries
				App.Current.executeVoidQuery("insert into Undisclosed default values");
				long rID = (long)App.Current.getDataSetForQuery("select max(undisclosedID) as undisclosedID from Undisclosed").Tables["Undisclosed"].Rows[0]["undisclosedID"];
				_recipientID = rID;
			}
			
			//Create InternalEmail
			int cID = (int)App.Current.getDataSetForQuery($"select categoryID as categoryID from Categories where categoryName = {category}").Tables["Categories"].Rows[0]["categoryID"];
			int uID = (int)App.Current.getDataSetForQuery($"select aID as aID from UserAddress where emailAddress = {from}").Tables["UserAddress"].Rows[0]["aID"];
			App.Current.executeVoidQuery($"insert into InternalEmail (emailPriority,emailSubject,emailBody,emailTime,categoryID,senderID) values (1, {subject}, {body}, {DateTime.Now}, {cID}, {uID})");
			
			if (threadID == -1) {//Create new thread
				//Retrieve max internalEmailID
				long eID = (long)App.Current.getDataSetForQuery($"select max(intEmailID) as intEmailID from InternalEmail").Tables["InternalEmail"].Rows[0]["intEmailID"];
				App.Current.executeVoidQuery($"insert into Thread (internalEmailID) values ({eID})"); //Increments thread by 1
			} else {
				long eID = (long)App.Current.getDataSetForQuery($"select max(intEmailID) as intEmailID from InternalEmail").Tables["InternalEmail"].Rows[0]["intEmailID"];

				try {
					App.Current.executeVoidQuery("set IDENTITY_INSERT Thread ON");
				} catch (Exception e) {
					// ignored
				} finally {
					var conn = App.Current.getSqlConnection();
					var transaction = conn.BeginTransaction();
					try {
						//Hopefully avoid threadID identity error
						new SqlCommand($"insert into Thread (threadID, internalEmailID) values ({threadID},{eID})", conn, transaction).ExecuteNonQuery();
						transaction.Commit();
					} catch (SqlException e) {
						transaction.Rollback();
					} finally {
						conn.Close();
					}
				}
			}

			if (recipientID == -1) { //part 2, map recipients to new undisclosed ID
				//Split array overload
				foreach (var recipient in recipients.Split(new []{"; "}, StringSplitOptions.None)) {
					long rUID = (long)App.Current.getDataSetForQuery($"select aID as aID from UserAddress where emailAddress = {recipient}").Tables["UserAddress"].Rows[0]["aID"];
					App.Current.executeVoidQuery($"insert into RecipientUser values ({rUID}, {_recipientID})", true);
				}
			}
		}
	}
}
