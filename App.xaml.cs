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
		public int? threadReply = null;
		public List<int> activeThreads = new List<int>();
		public List<long> activeThreadEmails = new List<long>();

		private void App_Startup(object sender, StartupEventArgs e) {
			MainWindow w = new MainWindow();
			w.Show();
		}

		public List<DataRow> users = new List<DataRow>();
		public void addUser(DataRow user) {
			users.Add(user);
		}
		
		public SqlConnection getSqlConnection(string source = "MAVERICK", string database = "EmailDB") {
			string connStr = $"data source={source};initial catalog={database};persist security info=True;user id=virtualLogin;password=virtualPassword;MultipleActiveResultSets=True";

			return new SqlConnection(connStr);
		}

		public DataSet getDataSetForQuery(string Query) {
			string[] sep = Query.Split(' ');
			//Extract table from query
			string table = sep.GetValue(Array.FindIndex(sep, t => t.Equals("from", StringComparison.InvariantCultureIgnoreCase)) + 1).ToString();
			
			var conn = getSqlConnection();
			var ds = new DataSet(table);
			try {
				var da = new SqlDataAdapter(Query, conn);
				da.Fill(ds, table);
			}
			finally
			{
				conn.Close();
			}

			return ds;
		}
	}
}
