using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.Common;



namespace Closer_Sql
{
    public partial class MainPage : ContentPage
    {
        SqlConnection sqlConnection;

        public class MyTableList
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
      //접속 DB관련
        public MainPage()
        {
            InitializeComponent();

            //SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            //sqlConnectionStringBuilder.DataSource = "202.68.236.38";
            //sqlConnectionStringBuilder.InitialCatalog = "TEST_SYSTEM";
            //sqlConnectionStringBuilder.UserID = "test_user";
            //sqlConnectionStringBuilder.Password = "user1234!";
            //sqlConnectionStringBuilder.IntegratedSecurity = false;
            //SqlConnection conn = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);

            string serverdbname = "TEST_SYSTEM";
            string servername = "202.68.236.38";
            string serverUsername = "test_user";
            string serverPassword = "user1234!";

            string sqlconn = $"Data Source = {servername}; Initial Catalog={serverdbname}; User ID = {serverUsername}; Password={serverPassword}";
            sqlConnection = new SqlConnection(sqlconn);           
        }
        //조회 버튼
        private async void getbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<MyTableList> myTableLists = new List<MyTableList>();
                sqlConnection.Open();
                string queryString = "SELECT* FROM DBO.Xamarin_table";
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    myTableLists.Add(new MyTableList
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Title = reader["name"].ToString(),
                        Body = reader["body"].ToString()
                    }
                    );                 
                }
                reader.Close();
                sqlConnection.Close();

                MyListView.ItemsSource = myTableLists;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("에러", ex.Message, "OK");             
                 throw;
            }
        }
        //삽입 버튼 이벤트
        private async void postbutton_Clicked(object sender, EventArgs e)
        {           
            try
            {
                LoginAsync();
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Xamarin_table VALUES(@id , @name ,@body)", sqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("ID", UserId.Text));
                    command.Parameters.Add(new SqlParameter("name", UserTitle.Text));
                    command.Parameters.Add(new SqlParameter("body", UserBody.Text));
                    command.ExecuteNonQuery();
                }
                await App.Current.MainPage.DisplayAlert("알람", "데이터 삽입 성공", "OK");
                sqlConnection.Close();
                Make_Empty();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("에러", ex.Message, "OK");
                throw;
            }
        }
        //업데이트 버튼 이벤트
        private async void update_button_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                sqlConnection.Open();

                int intUpate = Convert.ToInt32(UserId.Text);
                string TitleUpdate = UserTitle.Text;
                string BodyUpdate = UserBody.Text;

                string qerystr = $"UPDATE dbo.Xamarin_table SET id ='{intUpate}' , name='{TitleUpdate}' , body = '{BodyUpdate}' WHERE Id = '{intUpate}'";

                

                using (SqlCommand command = new SqlCommand(qerystr, sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
                await App.Current.MainPage.DisplayAlert("알람", "데이터 수정 성공", "OK");
                sqlConnection.Close();
                Make_Empty();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("에러", ex.Message, "OK");
                throw;
            }
        }
        //삭제 버튼 이벤트
        private async void delbutton_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                sqlConnection.Open();
                int idDelete = Convert.ToInt32(UserId.Text);
                using (SqlCommand command = new SqlCommand($"DELETE dbo.Xamarin_table WHERE Id = '{idDelete}'", sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
                sqlConnection.Close();
                await App.Current.MainPage.DisplayAlert("알람", "삭제 성공", "OK");
                Make_Empty();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("에러", ex.Message, "OK");
                throw;
            }
        }
        //연결 상태 확인 이벤트
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                await App.Current.MainPage.DisplayAlert("알람", "연결상태 양호.", "OK");
                sqlConnection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }           
        }

        public void Make_Empty()
        {
            UserId.Text = string.Empty;
            UserTitle.Text = string.Empty;
            UserBody.Text = string.Empty;
        }
        
        public async Task Checkdata()
        {
            if(string.IsNullOrWhiteSpace(UserId.Text))
            {
                 await App.Current.MainPage.DisplayAlert("알람", "(ID) 공백입니다 입력 해주세요.", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("알람", "(ID) 공백입니다 입력 해주세요.", "OK");
            }
        }

        public async Task<bool> LoginAsync()
        {

            return await  Task.Run(() =>
            {
                return false;
            });
        }
    }
}
