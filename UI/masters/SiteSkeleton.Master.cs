using BLL.Accounts;
using BLL.Navigation;
using Common.Views;
using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI.masters
{
    public partial class SiteSkeleton : MasterPage
    {
        #region Protected Methods

        protected void LogOutbttn_Click(object sender, EventArgs e)
        {
            if (Context.User.Identity.Name != String.Empty || Context.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    if (Context.User.IsInRole("Administrator"))
                    {
                        int roleId = new BlRoles().GetRoleId("Administrator");
                        BindMembersMenu(roleId, true);
                    }
                    else
                    {
                        int roleId = new BlRoles().GetRoleId("Customer");
                        BindMembersMenu(roleId, false);
                    }
                }
                else
                {
                    int roleId = new BlRoles().GetRoleId("Guest");
                    foreach (VwMenu m in new BlMenus().GetRootMenu(roleId, false))
                    {
                        var item = new MenuItem();
                        item.NavigateUrl = m.Url;
                        item.Text = m.Name;

                        foreach (VwMenu sub in new BlMenus().GetSubMenus(roleId, m.ID, false))
                        {
                            var item2 = new MenuItem { NavigateUrl = sub.Url, Text = sub.Name };
                            item.ChildItems.Add(item2);
                        }
                        mainMenu.Items.Add(item);
                    }
                }

                DisplayUsername();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void BindMembersMenu(int role, bool isAdmin)
        {
            foreach (VwMenu m in new BlMenus().GetRootMenu(role, isAdmin))
            {
                var item = new MenuItem { NavigateUrl = m.Url, Text = m.Name, ToolTip = m.Description };

                foreach (VwMenu sub in new BlMenus().GetSubMenus(role, m.ID, isAdmin))
                {
                    var item2 = new MenuItem { NavigateUrl = sub.Url, Text = sub.Name, ToolTip = sub.Description };
                    item.ChildItems.Add(item2);
                }
                mainMenu.Items.Add(item);
            }
        }

        private void DisplayUsername()
        {
            if (string.IsNullOrEmpty(Context.User.Identity.Name) == false || Context.User.Identity.IsAuthenticated)
            {
                lblGreeting.Text = Context.User.Identity.Name;
            }
            else
            {
                lblGreeting.Text = "Guest";
            }
        }

        #endregion Private Methods
    }
}