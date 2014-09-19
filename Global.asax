<%@ Application Language="C#" %>
<%@ Import Namespace="DBUtilities" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        string[] files = System.IO.Directory.GetFiles(ConfigurationManager.AppSettings["DownloadDirectory"], "*.progress");
        
        foreach (String file in files)
        {
            System.IO.File.Delete(file);
        }
    }

</script>
