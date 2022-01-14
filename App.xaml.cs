using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Sql;
using System.Data.SqlClient;

namespace EmailWPF {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public new static App Current => Application.Current as App;
		public string currentUserEmailAddress = "";
		public List<int> activeThreads = new List<int>();
		public List<long> activeThreadEmails = new List<long>();
		public long? activeRecipients = null;
		public long? activeThread = null;

		private void App_Startup(object sender, StartupEventArgs e) {
			MainWindow w = new MainWindow();
			w.Show();
		}

		public List<DataRow> users = new List<DataRow>();
		public void addUser(DataRow user) {
			users.Add(user);
			currentUserEmailAddress = user["emailAddress"].ToString();
		}
		
		public SqlConnection getSqlConnection(string source = "MAVERICK", string database = "EmailDB_301") {
			string connStr = $"data source={source};initial catalog={database};persist security info=True;user id=virtualLogin;password=virtualPassword;MultipleActiveResultSets=True";

			try {
				return new SqlConnection(connStr);
			} catch (Exception e) {
				return null;
			}
		}

		public DataSet getDataSetForQuery(string Query, bool remainOpen = false) {
			if (Query == "") {
				var conn = getSqlConnection();
				if(!remainOpen) conn.Close();
			}
			string[] sep = Query.Split(' ');
			//Extract table from query
			string table = sep.GetValue(Array.FindIndex(sep, t => t.Equals("from", StringComparison.InvariantCultureIgnoreCase)) + 1).ToString();

			try {
				var conn = getSqlConnection();
				var ds = new DataSet(table);
				try {
					var da = new SqlDataAdapter(Query, conn);
					da.Fill(ds, table);
				}
				finally
				{
					if(!remainOpen) conn.Close();
				}

				return ds;
			} catch (Exception e) {
				return null;
			}
		}
		
		public void executeVoidQuery(string Query, bool remainOpen = false) {
			if (Query == "") {
				var conn = getSqlConnection();
				if(!remainOpen) conn.Close();
			}
			try {
				var conn = getSqlConnection();
				try {
					SqlCommand command = new SqlCommand(Query, conn);
					command.ExecuteNonQuery();
				}
				finally
				{
					if(!remainOpen) conn.Close();
				}
			} catch (Exception e) {
				// ignored
			}
		}
	}
}
