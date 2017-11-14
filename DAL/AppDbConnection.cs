using System.Data.Common;
using System.Web.Configuration;
using Common.EntityModel;

namespace DAL
{
    public class AppDbConnection
    {
        protected AppDbConnection(bool isAdministrator)
        {
            if (isAdministrator)
            {
                ConnectionString = WebConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
            }
            else
            {
                ConnectionString = WebConfigurationManager.ConnectionStrings["ClientConnection"].ConnectionString;
            }
            Entity = new DataContextContainer(ConnectionString);
        }

        private AppDbConnection()
        {
            //disallow the use of the default constructor
        }

        private string ConnectionString { get; set; }

        public DataContextContainer Entity { get; set; }
        public DbTransaction Transaction { get; set; }
    }
}