﻿<%@ Application Language="C#" %>
<%@ Import Namespace="DBUtilities" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }

</script>