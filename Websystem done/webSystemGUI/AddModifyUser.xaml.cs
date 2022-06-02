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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace webSystemGUI
{
    /// <summary>
    /// Interaction logic for AddModifyUser.xaml
    /// </summary>
    public partial class AddModifyUser : UserControl
    {
        public AddModifyUser()
        {
            InitializeComponent();
        }

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();

                SqlCommand userExists = new SqlCommand("Select email from users WHERE email = @email", con);
                userExists.Parameters.AddWithValue("@email", userEmailTxt.Text);
                SqlDataReader userExistsRow = userExists.ExecuteReader();

                if (!(userExistsRow.HasRows))
                {
                    userExistsRow.Close();

                    SqlCommand addUser = new SqlCommand("INSERT INTO users( firstName, lastName, email ) VALUES ( @fName, @lName, @email )", con);
                    addUser.Parameters.AddWithValue("@fName", firstNameTxt.Text.ToLower());
                    addUser.Parameters.AddWithValue("@lName", lastNameTxt.Text.ToLower());
                    addUser.Parameters.AddWithValue("@email", userEmailTxt.Text.ToLower());
                    addUser.ExecuteNonQuery();
                    MessageBox.Show("User was added");
                }
                else
                {
                    MessageBox.Show("User is already in the system");
                }
            }
            catch
            {
                MessageBox.Show("database is down");
            }
        }

        private void modfiyUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();

                SqlCommand userExists = new SqlCommand("Select email from users WHERE email = @email", con);
                userExists.Parameters.AddWithValue("@email", userEmailTxt.Text);
                SqlDataReader userExistsRow = userExists.ExecuteReader();

                if (userExistsRow.HasRows)
                {
                    SqlCommand modUser = new SqlCommand("UPDATE users SET firstName = @fName, lastName = @lName WHERE email = @email", con);
                    modUser.Parameters.AddWithValue("@fName", firstNameTxt.Text);
                    modUser.Parameters.AddWithValue("@lName", lastNameTxt.Text);
                    modUser.Parameters.AddWithValue("@email", userEmailTxt.Text);
                    modUser.ExecuteNonQuery();
                    MessageBox.Show("User Updated");
                }
                else
                {
                    MessageBox.Show("User not found");
                }
            }
            catch
            {
                MessageBox.Show("database is down");
            }
        }



        private void lookupUserBtn_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupUser = new FindResults(userEmailTxt.Text, 2);
            lookupUser.ResultIsReady += new EventHandler(GetSelectedUser);
            lookupUser.ShowDialog();
        }

        private void GetSelectedUser(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc != null)
            {
                userEmailTxt.Text = "";

                if (fc.resultsListBox.SelectedItem != null)
                {
                    userEmailTxt.Text = fc.resultsListBox.SelectedItem.ToString();
                    try
                    {
                        SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                        con.Open();

                        SqlCommand getUser = new SqlCommand("SELECT firstName, lastName, email FROM users " +
                                                            "WHERE email = @email", con);
                        getUser.Parameters.AddWithValue("@email", userEmailTxt.Text);

                        SqlDataReader userRecord = getUser.ExecuteReader();

                        if (userRecord.Read())
                        {
                            firstNameTxt.Text = userRecord[0].ToString();

                            lastNameTxt.Text = userRecord[1].ToString();
                        }
                        userRecord.Close();
                        
                        fc.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Database is down");
                    }
                }
            }
        }

    }
}
