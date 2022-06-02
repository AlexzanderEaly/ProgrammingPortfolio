using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace webSystemGUI
{
    /// <summary>
    /// Interaction logic for ConfirmBox.xaml
    /// </summary>
    public partial class ConfirmBox : Window
    {
        public ConfirmBox(String item, int removeItem)
        {
            InitializeComponent();

            removeItemLbl.Content = item;
            itemNumLbl.Content = removeItem;
            
        }

        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                String item = (String)removeItemLbl.Content;

                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                SqlDataReader reader = null;

                con.Open();
                switch (itemNumLbl.Content)
                {
                    case 0:
                        SqlCommand deleteLink = new SqlCommand(
                    "DELETE URLS WHERE dbo.URLS.link = @link", con);

                        deleteLink.Parameters.AddWithValue("@link", item);

                        deleteLink.ExecuteNonQuery();

                        deleteLink = new SqlCommand("SELECT dbo.URLS.link FROM URLS WHERE dbo.URLS.link = @link ", con);

                        deleteLink.Parameters.AddWithValue("@link", item);

                        reader = deleteLink.ExecuteReader();

                        Message(reader, "Link");
                        break;

                    case 1:
                        SqlCommand deleteUser = new SqlCommand(
                    "DELETE users WHERE dbo.users.email = @user", con);

                        deleteUser.Parameters.AddWithValue("@user", item);

                        deleteUser.ExecuteNonQuery();

                        deleteUser = new SqlCommand("SELECT dbo.users.email FROM users WHERE dbo.users.email = @user ", con);

                        deleteUser.Parameters.AddWithValue("@user", item);

                        reader = deleteUser.ExecuteReader();

                        Message(reader, "User");
                        break;

                    case 2:
                        SqlCommand deleteCrawlLink = new SqlCommand(
                        "DELETE linksFound WHERE linksFound.linkURL = @linkUrl", con);

                        deleteCrawlLink.Parameters.AddWithValue("@linkUrl", item);

                        deleteCrawlLink.ExecuteNonQuery();

                        deleteCrawlLink = new SqlCommand("SELECT linkURL FROM linksFound WHERE linksFound.linkURL = @linkUrl ", con);

                        deleteCrawlLink.Parameters.AddWithValue("@linkUrl", item);

                        reader = deleteCrawlLink.ExecuteReader();

                        Message(reader, "Crawled Link");
                        break;

                    default:
                        MessageBox.Show("How did u get here");
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Database is down");
            }
            this.Close();

            static void Message(SqlDataReader reader,string item)
            {
                if (reader.HasRows)
                {
                    MessageBox.Show(item + " failed to be removed!");
                }
                else
                {
                    MessageBox.Show(item + " was removed!");
                }
                reader.Close();
            }
        }

        private void noBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
