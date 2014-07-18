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
            Directory.CreateDirectory(ConfigurationManager.AppSettings["BackupDirectory"]);

            string[] files = Directory.GetFiles(ConfigurationManager.AppSettings["BackupDirectory"], "*.bak");
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
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _ConnectionString;
            sqlConnection.Open();

            string sqlQuery = "SELECT * FROM sys.databases";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            var tables = dataSet.Tables[0];

            foreach (DataRow row in tables.Rows)
            {
                if (!"master,msdb,model,tempdb".Split(',').Contains(row[0]))
                {
                    ddlDatabases.Items.Add(new ListItem(row["name"].ToString(), row["database_id"].ToString()));
                }
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
            int iRows = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            lblMessage.Text = _DatabaseName + " database restored from '" + _BackupName + "' successfully...";

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

    protected void ddlConnections_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlConnections.SelectedValue != "") { 
            _ConnectionString = ddlConnections.SelectedValue;

            FillConnections();
            FillDatabases();
        }
    }
}