using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Backup : Page
{
    private String _ConnectionString;
    private ListItem disabledItem;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillConnections();
        }
    }

    private void FillConnections()
    {
        ddlConnections.Items.Clear();
        ddlConnections.ClearSelection();

        disabledItem = new ListItem("Select a connection", "");
        disabledItem.Attributes["disabled"] = "disabled";
        ddlConnections.Items.Add(disabledItem);

        foreach(ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
        {
            if (item.Name != "LocalSqlServer") {
                ListItem newitem = new ListItem(item.Name, item.ConnectionString);

                if (_ConnectionString != null && _ConnectionString == item.ConnectionString)
                {
                    newitem.Selected = true;
                }

                ddlConnections.Items.Add(newitem);
            }
        }
    }

    private void FillDatabases()
    {
        _ConnectionString = ddlConnections.SelectedValue;

        if (_ConnectionString == "")
        {
            return;
        }

        ddlDatabases.Items.Clear();

        try
        {
            foreach (String db in Utils.GetDBsForUser(_ConnectionString))
            {
                ddlDatabases.Items.Add(new ListItem(db, db));
            }
        }
        catch (SqlException sqlException)
        {
            lblMessage.Text = sqlException.Message.ToString();
        }
        catch (Exception exception)
        {
            lblMessage.Text = exception.Message.ToString();
        }
    }

    private String processFile()
    {
        String connection = ddlConnections.SelectedItem.Text;
        String database = ddlDatabases.SelectedItem.Text;
        String dir = ConfigurationManager.AppSettings["DownloadDirectory"];

        return Path.Combine(dir, connection + "_" + database + ".progress");
    }

    protected void btnBackup_Click(object sender, EventArgs e)
    {
        _ConnectionString = ddlConnections.SelectedValue;

        if (_ConnectionString == "")
        {
            return;
        }

        try
        {
            string _DatabaseName = ddlDatabases.SelectedItem.Text;
            string _BackupName = Path.Combine(ConfigurationManager.AppSettings["BackupDirectory"], _DatabaseName + "_" + DateTime.Now.ToString("MMM.dd.yyyy.HHmm") + ".bak");

            try
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["BackupDirectory"]);
            }
            catch (Exception _)
            {
                // Fine.
            }

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _ConnectionString;
            sqlConnection.Open();

            using (new FileStream(processFile(), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, 1, FileOptions.DeleteOnClose))
            {
                string sqlQuery = "BACKUP DATABASE " + _DatabaseName + " TO DISK = '" + _BackupName + "' WITH FORMAT, NAME = '" + _BackupName + "';";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlCommand.CommandType = CommandType.Text;

                Utils.LogGlobal(Request.UserHostAddress + " backed up database " + _DatabaseName + " to " + _BackupName);

                int iRows = sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
            }

            lblMessage.Text = _DatabaseName + " was backed up to '" + _BackupName + "' successfully...";
        }
        catch (SqlException sqlException)
        {
            lblMessage.Text = "SQL error: " + sqlException.Message.ToString();
        }
        catch (IOException exception)
        {
            if (exception.Message.Contains("already exists"))
            {
                lblMessage.Text = "Backup error: A backup for this database is already in progress.";
            }
            else
            {
                lblMessage.Text = "General error: " + exception.Message;
            }
        }
        catch (Exception exception)
        {
            lblMessage.Text = "General error: " + exception.Message.ToString();
        }
    }

    protected void ddlConnections_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConnections.SelectedValue != "") { 
            _ConnectionString = ddlConnections.SelectedValue;

            FillConnections();
            FillDatabases();
        }
    }
}