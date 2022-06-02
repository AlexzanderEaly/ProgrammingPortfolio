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
using System.Data.SqlTypes;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace webSystemGUI
{
    /// <summary>
    /// Interaction logic for AddModifyLink.xaml
    /// </summary>
    public partial class AddModifyLink : UserControl
    {
        public AddModifyLink()
        {
            InitializeComponent();

            String[] moinitorListInputs = { "Every 8 hours", "Every 24 Hours", "Once a Week", "Once a month" };
            foreach (String item in moinitorListInputs)
            {
                monitorRateList.Items.Add(item);
            }
        }

        /*excutes the query to add the link to the database, insterting the 
 LinkTxt into the URLS table and the rest into the Link Options table*/
        private void submitLinkBtn_Click(object sender, RoutedEventArgs e)
        {
            //holds the input from each of the textBoxes on the GUI page AddLink
            String filterSettings = filterSettingsTxt.Text,
                    link = linkTxt.Text.Trim(),
                    cat = categoryTxt.Text;

            //formats Categroy names to all be the same regardless of caps or spaceing.
            cat = cat.ToLower();
            cat = cat.Trim();

            //gets all the statues of the checkBoxes on the GUI page AddLink.
            Boolean pageIsDynamic = (bool)pageIsDynamicCheck.IsChecked,
                    crwalLinks = (bool)crwalLinksTrueCheck.IsChecked;

            //how often the link will be monitored 0 being the most frequent and 3 being the least frequent
            int monitorRate = monitorRateList.SelectedIndex;

            if (ValidateLink(filterSettings, link, cat, pageIsDynamic, monitorRate))
            {
                try
                {
                    SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                    con.Open();

                    AddCat(cat, con);

                    SqlCommand deleteLink = new SqlCommand(
                        "SELECT dbo.URLS.link FROM URLS WHERE dbo.URLS.link = @link ", con);

                    deleteLink.Parameters.AddWithValue("@link", link);

                    SqlDataReader reader = deleteLink.ExecuteReader();
                    if (!(reader.HasRows))
                    {
                        reader.Close();
                        SqlCommand addLinkToURL = new SqlCommand(
                            "Insert INTO linkOptions (filterParamerters, pageIsDynamic, monitorRate, crwalLinks) VALUES (@filter, @pageIsDynamic, @monitorRate, @crwalLinks) " +
                            "DECLARE @lastRowAddedToLinkOptions INT = 0 " +
                            "SELECT TOP 1 @lastRowAddedToLinkOptions = linkOptions.filterID FROM linkOptions ORDER BY linkOptions.filterID DESC " +
                            "INSERT INTO URLS ([link], filterID) VALUES (@link, @lastRowAddedToLinkOptions) " +
                            "DECLARE @linkID INT = 0 " +
                            "DECLARE @secID INT = 0 " +
                            "SELECT @linkID = URLS.linkID FROM URLS WHERE URLS.link = @link " +
                            "SELECT @secID = linkSections.secID FROM linkSections WHERE linkSections.secName = @cat " +
                            "INSERT INTO assignLink(linkID, secID) VALUES(@linkID, @secID) ", con);
                        addLinkToURL.Parameters.AddWithValue("@filter", filterSettings);
                        addLinkToURL.Parameters.AddWithValue("@pageIsDynamic", pageIsDynamic);
                        addLinkToURL.Parameters.AddWithValue("@monitorRate", monitorRate);
                        addLinkToURL.Parameters.AddWithValue("@crwalLinks", crwalLinks);
                        addLinkToURL.Parameters.AddWithValue("@link", link);
                        addLinkToURL.Parameters.AddWithValue("@cat", cat);
                        addLinkToURL.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("hey hey hey IT WORKED");
                    }
                    else
                    {
                        MessageBox.Show("Link is in Database");
                    }
                }
                catch
                {
                    MessageBox.Show("Database is down");
                }
            }
        }



        private void findLinkBtn_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupLinks = new FindResults(linkTxt.Text.Trim(), 0);

            /*This tells this form that when it recives the signal GetSelectedLink which is just a methods being called
             on some other form it should run the function listed inside the EventHandler()*/
            lookupLinks.ResultIsReady += new EventHandler(GetSelectedLink);
            lookupLinks.ShowDialog();


        }

        /*This pulls the currently selected item from the pop up form and closes*/
        private void GetSelectedLink(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc != null)
            {
                linkTxt.Text = "";

                if (fc.resultsListBox.SelectedItem != null)
                {
                    linkTxt.Text = fc.resultsListBox.SelectedItem.ToString();

                    try
                    {
                        SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                        con.Open();

                        SqlCommand getLink = new SqlCommand("SELECT link, linkOptions.pageIsDynamic, filterParamerters, crwalLinks, secName, monitorRate " +
                            " FROM URLS JOIN assignLink ON assignLink.linkID = URLS.linkID JOIN linkSections ON linkSections.secID = assignLink.secID JOIN linkOptions ON  URLS.filterID = linkOptions.filterID " +
                            " WHERE link = @link", con);
                        getLink.Parameters.AddWithValue("@link", linkTxt.Text);

                        SqlDataReader linkRecord = getLink.ExecuteReader();

                        if (linkRecord.Read())
                        {

                            pageIsDynamicCheck.IsChecked = (bool)linkRecord[1];

                            filterSettingsTxt.Text = linkRecord[2].ToString();

                            crwalLinksTrueCheck.IsChecked = (bool)linkRecord[3];

                            categoryTxt.Text = linkRecord[4].ToString();

                            monitorRateList.SelectedIndex = (int)linkRecord[5];
                        }
                        linkRecord.Close();

                        fc.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Database is down");
                    }
                }
            }
        }

        private void GetSelectedFilter(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc != null)
            {
                filterSettingsTxt.Text = "";

                if (fc.resultsListBox.SelectedItem != null)
                {
                    filterSettingsTxt.Text = fc.resultsListBox.SelectedItem.ToString();
                    fc.Close();

                }
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



        private void findFilterBtn_Click(object sender, RoutedEventArgs e)
        {

            FindResults lookupFilter = new FindResults(filterSettingsTxt.Text, 1);
            lookupFilter.ResultIsReady += new EventHandler(GetSelectedFilter);
            lookupFilter.ShowDialog();

        }

        private void findCat_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupCategory = new FindResults(categoryTxt.Text, 3);
            lookupCategory.ResultIsReady += new EventHandler(GetSelectedCategory);
            lookupCategory.ShowDialog();
        }

        private void modfiyLinkBtn_Click(object sender, RoutedEventArgs e)
        {
            //holds the input from each of the textBoxes on the GUI page AddLink
            String filterSettings = filterSettingsTxt.Text.Trim().ToLower(),
                    link = linkTxt.Text.Trim(),
                    cat = categoryTxt.Text;

            //formats Categroy names to all be the same regardless of caps or spaceing.
            cat = cat.ToLower();
            cat = cat.Trim();

            //gets all the statues of the checkBoxes on the GUI page AddLink.
            Boolean pageIsDynamic = (bool)pageIsDynamicCheck.IsChecked,
                    crwalLinks = (bool)crwalLinksTrueCheck.IsChecked;

            //how often the link will be monitored 1 being the most frequent and 4 being the least frequent

            int monitorRate = monitorRateList.SelectedIndex;



            if (ValidateLink(filterSettings, link, cat, pageIsDynamic, monitorRate))
            {
                try
                {

                    /*opends a connection to the database and then excutes a query that looks up to see if the link exists 
                 * then askes the user to confirm the deletion of the link*/
                    SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                    con.Open();

                    AddCat(cat, con);

                    SqlCommand deleteLink = new SqlCommand(
                        "SELECT dbo.URLS.link FROM URLS WHERE dbo.URLS.link = @link ", con);

                    deleteLink.Parameters.AddWithValue("@link", link);

                    SqlDataReader reader = deleteLink.ExecuteReader();
                    /*sees if link exists*/
                    if (reader.HasRows)
                    {
                        reader.Close();
                        SqlCommand addLinkToURL = new SqlCommand(
                        "Insert INTO linkOptions (filterParamerters, pageIsDynamic, monitorRate, crwalLinks) VALUES (@filter, @pageIsDynamic, @monitorRate, @crwalLinks) " +
                        "DECLARE @lastRowAddedToLinkOptions INT = 0 " +
                        "SELECT TOP 1 @lastRowAddedToLinkOptions = linkOptions.filterID FROM linkOptions ORDER BY linkOptions.filterID DESC " +
                        "UPDATE URLS SET URLS.filterID = @lastRowAddedToLinkOptions WHERE URLS.[link] = @link " +
                        "DECLARE @linkID INT = 0 " +
                        "DECLARE @secID INT = 0 " +
                        "SELECT @linkID = URLS.linkID FROM URLS WHERE URLS.link = @link " +
                        "SELECT @secID = linkSections.secID FROM linkSections WHERE linkSections.secName = @cat " +
                        "UPDATE assignLink SET assignLink.secID = @secID WHERE assignLink.linkID = @linkID ", con);
                        addLinkToURL.Parameters.AddWithValue("@filter", filterSettings);
                        addLinkToURL.Parameters.AddWithValue("@pageIsDynamic", pageIsDynamic);
                        addLinkToURL.Parameters.AddWithValue("@monitorRate", monitorRate);
                        addLinkToURL.Parameters.AddWithValue("@crwalLinks", crwalLinks);
                        addLinkToURL.Parameters.AddWithValue("@link", link);
                        addLinkToURL.Parameters.AddWithValue("@cat", cat);
                        addLinkToURL.ExecuteNonQuery();
                        MessageBox.Show("hey hey hey IT WORKED");
                    }
                    else
                    {
                        MessageBox.Show("Link was not found");
                    }
                }
                catch
                {
                    MessageBox.Show("Database is down");
                }
            }
        }

        private bool ValidateLink(string filterSettings, string link, string cat, bool pageIsDynamic, int monitorRate)
        {
            bool validLinkRecord = false;

            if (link == "")
            {
                MessageBox.Show("Please enter the URL");
                return validLinkRecord;
            }
            else if (filterSettings == "")
            {
                MessageBox.Show("Enter a Filter please");
                return validLinkRecord;
            }
            else if (monitorRate == -1)
            {
                MessageBox.Show("Please select how often you want to mointor the link");
                return validLinkRecord;
            }
            else if (cat == "")
            {
                MessageBox.Show("Please enter a category");
                return validLinkRecord;
            }
            else if (!(PreviewLinkSetting(link, filterSettings, pageIsDynamic)))
            {
                return validLinkRecord;
            }
            else
            {
                validLinkRecord = true;
                return validLinkRecord;
            }
        }

        private static void AddCat(string cat, SqlConnection con)
        {
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

        private void previewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (linkTxt.Text.Trim() == "")
                MessageBox.Show("Please enter the URL");

            if (filterSettingsTxt.Text.Trim() == "")
                MessageBox.Show("Enter a Filter please");
            else
            {
                //shows a preview of what the script would scrape if 
                PreviewLinkSetting(linkTxt.Text, filterSettingsTxt.Text, (bool)pageIsDynamicCheck.IsChecked, true);

            }
        }

        private bool PreviewLinkSetting(String url, string filter, bool pageIsDynamic, bool previewMode = false)
        {
            int pageIs = 0;

            if (pageIsDynamic)
            {
                pageIs = 1;
            }

            //put -i if you want to see error messages
            //path to the python script
            //Take the main.py script put it somewheres that is not in the project and then boom it should run, and have the path below point to it
            string arg = string.Format("C:\\Users\\lcs\\Desktop\\main.py {0} \"{1}\" {2}", url, filter, pageIs);
            try
            {
                Process p1 = new Process();
                //C:\Users\lcs\PycharmProjects\PreviewSoup\venv\Scripts\python.exe
                p1.StartInfo = new ProcessStartInfo(@"C:\Users\lcs\Desktop\FSU college\SPRING 2021\CAP STONE\webSystemGUI\Python code and stuff\PreviewSoup\venv\Scripts\python.exe ", arg);
                p1.StartInfo.UseShellExecute = false;
                p1.StartInfo.RedirectStandardOutput = true;
                p1.StartInfo.RedirectStandardError = true;
                //C:\Users\lcs\PycharmProjects\PreviewSoup\
                p1.StartInfo.WorkingDirectory = @"C:\Users\lcs\Desktop\FSU college\SPRING 2021\CAP STONE\webSystemGUI\Python code and stuff\PreviewSoup\";
                p1.Start();
                StreamReader output = p1.StandardOutput;
                string outputCode = output.ReadToEnd();
                p1.Kill();

                //holds a buffer of any errors thrown by the python script.
                StreamReader errorOutput = p1.StandardError;

                //moves the pointer forward by one line.
                errorOutput.ReadLine();

                //if pointer is at the end it means there is no error, otherwise there is an error.
                if (!(errorOutput.EndOfStream))
                {
                    String errorMsg = errorOutput.ReadToEnd();

                    //shows the last couple lines of the error.
                    MessageBox.Show(errorMsg);

                    //clears the buffer for the next time
                    errorOutput.DiscardBufferedData();
                    errorOutput.Close();

                    //returns false because there wasn error message.
                    return false;
                }

                if (outputCode.Length < 4)
                {
                    //tells user that their filter doesn't find any html on the webpage
                    MessageBox.Show("filter failed to find any HTML");

                    //returns false beacuse filter was empty aka over filtering or bad logical filter
                    return false;
                }

                if (previewMode && outputCode.Length > 4)
                {
                    //shows what was scraped off the website
                    PreviewLink linkFilter = new PreviewLink(outputCode);
                    linkFilter.Show();
                }

                //clears the buffer for the next time
                errorOutput.DiscardBufferedData();
                errorOutput.Close();

                return true;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
    }
}
