using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Utils
{
    public static List<String> GetDBsForUser(string connection)
    {
        var builder = new SqlConnectionStringBuilder(connection);
        String user = builder.UserID;

        if (user == "")
        {
            user = "public";
        }

        // the following is originally from http://social.msdn.microsoft.com/Forums/sqlserver/en-US/2044d912-72b2-4859-af58-d089284a7120/
        string query = @"DECLARE @DBuser_sql VARCHAR(4000) 
            DECLARE @DBuser_table TABLE (DBName VARCHAR(200), UserName VARCHAR(250), LoginType VARCHAR(500), AssociatedRole VARCHAR(200)) 
            SET @DBuser_sql='SELECT ''?'' AS DBName,a.name AS Name,a.type_desc AS LoginType,USER_NAME(b.role_principal_id) AS AssociatedRole 
                FROM ?.sys.database_principals a 
                LEFT OUTER JOIN ?.sys.database_role_members b ON a.principal_id=b.member_principal_id 
                WHERE a.sid NOT IN (0x01,0x00) AND 
                    a.sid IS NOT NULL AND 
                    a.type NOT IN (''C'') AND 
                    a.is_fixed_role <> 1 AND 
                    a.name NOT LIKE ''##%'' AND 
                    ''?'' NOT IN (''master'',''msdb'',''model'',''tempdb'') ORDER BY Name'
            INSERT @DBuser_table 
            EXEC sp_MSforeachdb @command1=@dbuser_sql 
            SELECT * FROM @DBuser_table WHERE UserName = '" + user + "' ORDER BY DBName ";

        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.ConnectionString = connection;
        sqlConnection.Open();
        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
        sqlCommand.CommandType = CommandType.Text;
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
        DataSet dataSet = new DataSet();
        sqlDataAdapter.Fill(dataSet);

        var tables = dataSet.Tables[0];

        List<String> list = new List<string>();
        foreach (DataRow row in tables.Rows)
        {
            list.Add(row["DBname"].ToString());
        }

        return list;
    }

    public static String LogDirectory()
    {
        return ConfigurationManager.AppSettings["LogDirectory"];
    }

    public static void LogGlobal(string line) {
        LogToFile(line, "Global-" + DateTime.Now.ToLongDateString() + ".log");
    }

    public static void LogToFile(string line, string file)
    {
        Directory.CreateDirectory(LogDirectory());

        using (StreamWriter w = File.AppendText(LogDirectory() + file))
        {
            w.WriteLine("{0:dd/MM/yy\tHH:mm:ss}\t{1}", DateTime.Now, line);
        }
    }
}