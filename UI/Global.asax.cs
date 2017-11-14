using Common.Views;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace UI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                List<VwRole> roleList = new BLL.Accounts.BlRoles().GetRolesOfUser(Context.User.Identity.Name);

                var roles = new string[roleList.Count];
                int counter = 0;
                foreach (var r in roleList)
                {
                    roles[counter] = r.Name;
                    counter++;
                }

                var gp = new GenericPrincipal(Context.User.Identity, roles);
                Context.User = gp;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}