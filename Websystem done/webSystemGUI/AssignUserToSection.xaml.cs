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
    /// Interaction logic for AssignUserToSection.xaml
    /// </summary>
    public partial class AssignUserToSection : UserControl
    {
        public AssignUserToSection()
        {
            InitializeComponent();

        }

        private void lookupUserBtn_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupUser = new FindResults(userEmailTxt.Text, 2);
            lookupUser.ResultIsReady += new EventHandler(GetSelectedUser);
            lookupUser.ShowDialog();
        }

        private void lookupSectionBtn_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupCategory = new FindResults(categoryTxt.Text, 3);
            lookupCategory.ResultIsReady += new EventHandler(GetSelectedCategory);
            lookupCategory.ShowDialog();
        }

        private void assignUserToSectionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string cat = categoryTxt.Text.ToLower().Trim();

                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();

                if (cat != "")
                {
                    AddCat(cat, con);
                }

                SqlCommand findUser = new SqlCommand(
                            "SELECT dbo.users.email FROM users WHERE dbo.users.email = @email ", con);
                findUser.Parameters.AddWithValue("@email", userEmailTxt.Text);

                //creats a reader that will go through the query and read each row found
                SqlDataReader userExists = findUser.ExecuteReader();

                if (!(userExists.HasRows))
                {
                    MessageBox.Show("User does not exist");

                    //closes the reader to free it for the sql command
                    userExists.Close();
                }
                else if (cat == "")
                {
                    MessageBox.Show("please enter in a section");
                }
                else
                {
                    //closes the reader to free it for the sql command
                    userExists.Close();

                    UpdateCurrentAssignSection(con);

                    if (!(currentUserCategoriesScrollBox.Content.ToString().Contains(categoryTxt.Text.ToLower().Trim())))
                    {
                        SqlCommand assignUserTo = new SqlCommand("DECLARE @userID INT = 0 " +
                                                                 "DECLARE @secID INT = 0 " +
                                                                 "SELECT @userID = userID FROM users WHERE users.email = @email " +
                                                                 "SELECT @secID = linkSections.secID FROM linkSections WHERE linkSections.secName = @cat " +
                                                                 "INSERT assignUser (secID, userID) VALUES (@secID, @userID)", con);
                        assignUserTo.Parameters.AddWithValue("@email", userEmailTxt.Text);
                        assignUserTo.Parameters.AddWithValue("@cat", categoryTxt.Text.ToLower().Trim());

                        assignUserTo.ExecuteNonQuery();

                        UpdateCurrentAssignSection(con);


                    }
                    else
                    {
                        MessageBox.Show("User is already assigned to the section.");
                    }

                }
            }
            catch
            {
                MessageBox.Show("Database is down");
            }
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

                    fc.Close();
                    try
                    {
                        SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                        con.Open();

                        UpdateCurrentAssignSection(con);
                    }
                    catch
                    {
                        MessageBox.Show("Database is down");
                    }
                }
            }
        }


        //this will run a query to that will update the currently assigned sections to the user enetered.
        private void UpdateCurrentAssignSection(SqlConnection con)
        {

            currentUserCategoriesScrollBox.Content = "";
            try
            {
                SqlCommand getAssignedUserSectionList = new SqlCommand("SELECT linkSections.secName, users.firstName FROM linkSections JOIN assignUser ON assignUser.secID = linkSections.secID JOIN users ON assignUser.userID = users.userID WHERE users.email = @email", con);
                getAssignedUserSectionList.Parameters.AddWithValue("@email", userEmailTxt.Text);

                SqlDataReader assignedSectionList = getAssignedUserSectionList.ExecuteReader();

                string assignedSections = "";

                while (assignedSectionList.Read())
                {
                    assignedSections += assignedSectionList[0].ToString() + "\n";
                }

                assignedSectionList.Close();

                currentUserCategoriesScrollBox.Content = assignedSections;

                assignedSections = null;
            }
            catch
            {
                MessageBox.Show("Database is down");
            }
        }

        private void GetSelectedCategory(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc != null)
            {
                categoryTxt.Text = "";

                if (fc.resultsListBox.SelectedItem != null)
                {
                    categoryTxt.Text = fc.resultsListBox.SelectedItem.ToString();
                    fc.Close();

                }
            }
        }


        private static void AddCat(string cat, SqlConnection con)
        {

            cat = cat.ToLower();
            cat = cat.Trim();
            try
            {
                SqlCommand addCat = new SqlCommand("SELECT dbo.linkSections.secID FROM linkSections WHERE linkSections.secName = @cat", con);
                addCat.Parameters.AddWithValue("@cat", cat);
                addCat.ExecuteNonQuery();
                SqlDataReader catExists = addCat.ExecuteReader();

                if (!(catExists.HasRows))
                {
                    catExists.Close();

                    addCat = new SqlCommand("INSERT INTO linkSections(secName) VALUES (@cat)", con);
                    addCat.Parameters.AddWithValue("@cat", cat);
                    addCat.ExecuteNonQuery();
                }
                catExists.Close();
            }
            catch
            {
                MessageBox.Show("Database is down");
            }
        }

        private void unassignUserFromSectionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();


                AddCat(categoryTxt.Text, con);

                SqlCommand findUser = new SqlCommand(
                            "SELECT dbo.users.email FROM users WHERE dbo.users.email = @email ", con);
                findUser.Parameters.AddWithValue("@email", userEmailTxt.Text);

                //creats a reader that will go through the query and read each row found
                SqlDataReader userExists = findUser.ExecuteReader();

                if (!(userExists.HasRows))
                {
                    MessageBox.Show("User does not exist");

                    //closes the reader to free it for the sql command
                    userExists.Close();
                }
                else if (categoryTxt.Text == "")
                {
                    MessageBox.Show("please enter in a section");
                }
                else
                {
                    //closes the reader to free it for the sql command
                    userExists.Close();

                    UpdateCurrentAssignSection(con);

                    if (currentUserCategoriesScrollBox.Content.ToString().Contains(categoryTxt.Text.ToLower().Trim()))
                    {
                        SqlCommand assignUserTo = new SqlCommand("DECLARE @userID INT = 0 " +
                                                                 "DECLARE @secID INT = 0 " +
                                                                 "SELECT @userID = userID FROM users WHERE users.email = @email " +
                                                                 "SELECT @secID = linkSections.secID FROM linkSections WHERE linkSections.secName = @cat " +
                                                                 "DELETE FROM assignUser WHERE assignUser.secID = @secID AND assignUser.userID = @userID ", con);
                        assignUserTo.Parameters.AddWithValue("@email", userEmailTxt.Text);
                        assignUserTo.Parameters.AddWithValue("@cat", categoryTxt.Text.ToLower().Trim());

                        assignUserTo.ExecuteNonQuery();

                        UpdateCurrentAssignSection(con);


                    }
                    else
                    {
                        MessageBox.Show("User is not assigned to the section.");
                    }

                }
            }
            catch
            {
                MessageBox.Show("Database is down");
            }
        }

    }
}
