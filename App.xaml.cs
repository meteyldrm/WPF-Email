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
		public static new App Current => Application.Current as App;

		private void App_Startup(object sender, StartupEventArgs e) {
			MainWindow w = new MainWindow();
			w.Show();
			// using (var em = new EmailModel()) {
			// 	em.TblUser.Where(i => i.userID < 0).ToList();
			// }
			
			//string connStr = "server=localhost;user=root;database="+ SelectedItemTextDatabase+";port=3306;password=test32";
			string connStr = "data source=MAVERICK;initial catalog=EmailDB;persist security info=True;user id=virtualLogin;password=virtualPassword;MultipleActiveResultSets=True";


			SqlConnection conn = new SqlConnection(connStr);
			
			string sql = "SELECT * FROM "+"TblUser";

			SqlCommand myCommand = new SqlCommand(sql, conn);

			conn.Open();
			SqlDataReader myReader;
			myReader = myCommand.ExecuteReader();
			try
			{
				while (myReader.Read())
				{
					Console.WriteLine(myReader.GetValue(0).ToString() + " " + myReader.GetValue(1).ToString());
				}
			}
			finally
			{
				myReader.Close();
				conn.Close();
			}
		}

		public List<UserAddress> users = new List<UserAddress>();
		public void addUser(UserAddress addr) {
			users.Add(addr);
		}
		
		
	}
}
