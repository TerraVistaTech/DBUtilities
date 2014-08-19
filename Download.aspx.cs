using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ReadBackupFiles();
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

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        string _BackupName = lstBackupfiles.SelectedItem.Text;
        FileInfo file = new FileInfo(_BackupName);

        Response.Clear();

        Response.ClearHeaders();

        Response.ClearContent();

        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

        Response.AddHeader("Content-Length", file.Length.ToString());

        Response.ContentType = "application/octet-stream";

        Response.Flush();

        Utils.LogGlobal(Request.UserHostAddress + " downloading " + file.Name);

        Response.TransmitFile(file.FullName);

        Response.End();
    }
}