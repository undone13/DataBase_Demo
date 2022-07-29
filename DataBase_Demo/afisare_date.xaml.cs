using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace DataBase_Demo
{
    /// <summary>
    /// Interaction logic for afisare_date.xaml
    /// </summary>
    public partial class afisare_date : Window
    {
        public afisare_date()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string data = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=AppUsers;Integrated Security=True";

            SqlConnection sql = new SqlConnection(data);

            SqlCommand com = new SqlCommand("joinAddress_User", sql);
            SqlParameter p1 = new SqlParameter("user_name", DataBase_Demo.MainWindow.user_name);
            com.Parameters.Add(p1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);

            DataSet d = new DataSet();
            da.Fill(d);

            label_userid.Content += d.Tables[0].Rows[0][0].ToString();
            label_username.Content += d.Tables[0].Rows[0][1].ToString();
            label_address.Content += d.Tables[0].Rows[0][2].ToString();
            label_phonenumber.Content += d.Tables[0].Rows[0][3].ToString();
            label_email.Content += d.Tables[0].Rows[0][4].ToString();

            this.Title = "User Data for " + DataBase_Demo.MainWindow.user_name;
        }
    }
}
