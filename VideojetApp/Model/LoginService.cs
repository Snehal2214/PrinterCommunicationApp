using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using VideojetApp.View;
using System.Windows;

namespace VideojetApp.Model
{
    public class LoginService
    {
        SqlConnection ObjSqlConnection;
        SqlCommand ObjSqlCommand;

        public LoginService()
        {
            ObjSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            ObjSqlCommand = new SqlCommand();
            ObjSqlCommand.Connection = ObjSqlConnection;
            ObjSqlCommand.CommandType = CommandType.StoredProcedure;

        }

        public bool Checklogin(LoginModel loginModel)
        {
            bool IsChecked = false;
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "Checklogins";

                ObjSqlCommand.Parameters.AddWithValue("@Username", loginModel.Username);
                ObjSqlCommand.Parameters.AddWithValue("@Password", loginModel.Password);

                ObjSqlConnection.Open();

                
                var role = ObjSqlCommand.ExecuteScalar()?.ToString();
                if (role == "Admin")
                {
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    Application.Current.MainWindow.Close();
                }
                //else (role == Operator) 
                //{
                //    OperatorDashboard operatordashboar = new OperatorDashboard();
                //    operatordashboar.Show();
                //    ObjSqlConnection.Close();
                //}

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return IsChecked;
        }


    }
}
