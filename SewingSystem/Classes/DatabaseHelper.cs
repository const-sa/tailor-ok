using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SewingSystem.Classes
{
    public class DatabaseHelper
    {
        public void AttachDatabase(string databaseName, string mdfFilePath, string ldfFilePath, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string attachQuery = $@"
                CREATE DATABASE [{databaseName}] 
                ON (FILENAME = N'{mdfFilePath}'), 
                (FILENAME = N'{ldfFilePath}') 
                FOR ATTACH";

                SqlCommand command = new SqlCommand(attachQuery, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RestoreDatabase(string databaseName, string backupFilePath, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string restoreQuery = $@"
            RESTORE DATABASE [{databaseName}]
            FROM DISK = N'{backupFilePath}'
            WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5";

                SqlCommand command = new SqlCommand(restoreQuery, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        public static string GetConnectionString(string databaseName, string instanceName, string username, string password, bool useWindowsAuthentication)
        {
            var entityConnectionString = ConfigurationManager.ConnectionStrings[Program.DBName_static + "Entities"].ConnectionString;
            var entityBuilder = new EntityConnectionStringBuilder(entityConnectionString);
            var sqlBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(entityBuilder.ProviderConnectionString)
            {
                DataSource = $"{instanceName}",
                InitialCatalog = databaseName,
                IntegratedSecurity = useWindowsAuthentication,
                MultipleActiveResultSets = true
            };

            if (!useWindowsAuthentication)
            {
                sqlBuilder.UserID = username;
                sqlBuilder.Password = password;
            }

            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            return entityBuilder.ToString();
        }

        public static void UpdateConnectionString(string name, string connectionString)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlNode node = doc.SelectSingleNode($"//connectionStrings/add[@name='{name}']");
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes["connectionString"];
                if (attribute != null)
                {
                    attribute.Value = connectionString;
                    doc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                }
            }
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
