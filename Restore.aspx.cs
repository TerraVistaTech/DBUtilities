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

public partial class Restore : Page
{
    private String _ConnectionString;
    private ListItem disabledItem;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!bool.Parse(ConfigurationManager.AppSettings["EnableRestore"])) {
            Response.RedirectPermanent("~");
        }

        if (!Page.IsPostBack)
        {
            FillConnections();
            ReadBackupFiles();
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

    private void ReadBackupFiles()
    {
        try
        {
            try
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["DownloadDirectory"]);
            }
            catch (Exception _)
            {
                // Fine.
            }

            string[] files = Directory.GetFiles(ConfigurationManager.AppSettings["DownloadDirectory"], "*.bak");
            lstBackupfiles.DataSource = files;
            lstBackupfiles.DataBind();
            lstBackupfiles.SelectedIndex = 0;
        }
        catch (Exception exception)
        {
            lblMessage.Text = exception.Message.ToString();
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
            lblMessage.Text = "SQL error: " + sqlException.Message.ToString();
        }
        catch (Exception exception)
        {
            lblMessage.Text = "General error: " + exception.Message.ToString();
        }
    }

    protected void btnRestore_Click(object sender, EventArgs e)
    {
        _ConnectionString = ddlConnections.SelectedValue;

        if (_ConnectionString == "")
        {
            return;
        }

        try
        {
            string _DatabaseName = ddlDatabases.SelectedItem.Text;
            string _BackupName = lstBackupfiles.SelectedItem.Text;

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _ConnectionString;
            sqlConnection.Open();

            string sqlQuery = "USE master; "
                + "ALTER DATABASE " + _DatabaseName + " SET Single_User WITH ROLLBACK IMMEDIATE; "
                + "RESTORE DATABASE " + _DatabaseName + " FROM DISK ='" + _BackupName + "' WITH REPLACE; "
                + "ALTER DATABASE " + _DatabaseName + " SET Multi_User";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;

            Utils.LogGlobal(Request.UserHostAddress + " restored database " + _DatabaseName + " from " + _BackupName);

            int iRows = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            lblMessage.Text = _DatabaseName + " database restored from '" + _BackupName + "' successfully...";

        }
        catch (SqlException sqlException)
        {
            lblMessage.Text = "SQL error: " + sqlException.Message.ToString();
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