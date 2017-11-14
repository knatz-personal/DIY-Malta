using BLL.Accounts;
using BLL.CustomExceptions;
using Common.Utilities;
using Common.Views;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;

namespace UI.accounts
{
    public partial class register : Page
    {
        #region Private Fields

        private static bool isAlreadyLoggedIn;

        #endregion Private Fields

        #region Public Methods

        [WebMethod]
        public static bool DoesEmailExist(string email)
        {
            bool result = true;
            try
            {
                if (new BlUsers().DoesEmailExist(email))
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                throw new UserAlreadyExistsException();
            }
            return result;
        }

        [WebMethod]
        public static bool IsUsernameTaken(string username)
        {
            bool result = true;
            try
            {
                if (new BlUsers().IsUsernameTaken(username))
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                throw new UserAlreadyExistsException();
            }
            return result;
        }

        [WebMethod]
        public static IQueryable<VwAddressType> LoadAddressTypeList()
        {
            IQueryable<VwAddressType> list = new BlAddresses().ListTypes();
            new JavaScriptSerializer().Serialize(list);
            return list;
        }

        [WebMethod]
        public static IQueryable<VwContactType> LoadContactTypeList()
        {
            IQueryable<VwContactType> list = new BlContacts().ListTypes();
            new JavaScriptSerializer().Serialize(list);
            return list;
        }

        [WebMethod]
        public static IQueryable<VwGender> LoadGenderList()
        {
            IQueryable<VwGender> list = new BlGenders().ListAll();
            new JavaScriptSerializer().Serialize(list);
            return list;
        }

        [WebMethod]
        public static IQueryable<VwTown> LoadTownList()
        {
            IQueryable<VwTown> list = new BlTowns().ListAll();
            new JavaScriptSerializer().Serialize(list);
            return list;
        }

        [WebMethod]
        public static object Register(string name, string middle, string surname, string email, int genderId, string dob,
            int phone, int mobile, int contactType, string address, string street, int addressType, string postCode,
            int townId, string username, string password)
        {
            var ans = new { result = false, message = "" };

            bool isNotEmpty = ValidateStringFields(name, surname, address, street);

            string messages = "";

            bool isNumeric = ValidateNumericFields(genderId, contactType, addressType, townId);

            bool isValidUsername = ValidateField(username, "^[a-zA-Z0-9._-]{6,}") &&
                                   new BlUsers().IsUsernameTaken(username) == false;
            bool isValidPassword = ValidateField(password, "^[a-zA-Z0-9._-]{6,}");

            DateTime dateOfBirth = ValidDate(dob);
            bool isComplex = ValidateComplexFields(phone, mobile, postCode, email);

            bool isValidated = isNotEmpty && isNumeric && isValidUsername && isValidPassword && isComplex;

            if (!isNotEmpty)
            {
                messages = "<p>Name, Surname, Address and Street cannot be empty</p>";
            }

            if (!isNumeric)
            {
                messages += "<p>Select Gender, Contact Type, Address Type and Town</p>";
            }

            if (!isValidUsername)
            {
                messages += "<p>The username you provided is either less than 6 characters or is already taken</p>";
            }

            if (!isValidPassword)
            {
                messages += "<p>The password you provided is empty or less than 6 characters</p>";
            }

            if (!isComplex)
            {
                messages += "<p>One or more complex fields is  invalid. Check telephone, mobile, post code and email</p>";
            }

            if (isValidated)
            {
                messages = "";
                string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                string encPassword = Encryption.EncryptTripleDES(hashedPassword);

                var user = new VwUser
                {
                    FirstName = name,
                    MiddleInitial = middle,
                    LastName = surname,
                    GenderID = genderId,
                    DateOfBirth = dateOfBirth,
                    Address = address,
                    AddressType = addressType,
                    Street = street,
                    ContactType = contactType,
                    Phone = phone,
                    Mobile = mobile,
                    Email = email,
                    Username = username,
                    Password = encPassword,
                    PostCode = postCode,
                    TownID = townId
                };
                bool isRegistered = new BlUsers().Register(user);

                if (isRegistered)
                {
                    ans = new { result = true, message = messages };
                    SendConfirmation(email, username, password);
                    FormsAuthentication.SetAuthCookie(username, false);
                }
            }

            return ans;
        }

        #endregion Public Methods

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                isAlreadyLoggedIn = Context.User.Identity.IsAuthenticated;
                if (isAlreadyLoggedIn)
                {
                    Response.Redirect("/default.aspx");
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private static void SendConfirmation(string email, string username, string password)
        {
            var html = new StringBuilder();
            html.Append("<html><head><style type='text/css'>");
            html.Append("body,td {font-family: arial, sans-serif; font-size: 13px");
            html.Append("a:link,a:active {color: #1155CC;text-decoration: none}");
            html.Append("a:hover {text-decoration: underline;cursor: pointer}");
            html.Append(" a:visited {color: #6611CC}");
            html.Append("img {border: 0px}");
            html.Append("pre {white-space: pre;white-space: -moz-pre-wrap;white-space: -o-pre-wrap;" +
                        "white-space: pre-wrap;word-wrap: break-word;max-width: 800px;" +
                        "overflow: auto;} </style></head>");
            html.Append("<body><table border='0' width='100%' cellpadding='0' cellspacing='0'>");
            html.Append("<tbody><tr> <td colspan='2'><table border='0' width='100%' cellpadding='12' cellspacing='0'>");
            html.Append("<tbody><tr><td><div style='overflow: hidden;'><font size='-1'>");
            html.Append("<div style='margin-top:0;margin-bottom:0;margin-left:0;margin-right:0;padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;min-width:100%;background-color:#f6f9fb'>");
            html.Append("<table style='border-collapse:collapse;border-spacing:0;display:table;table-layout:fixed;width:100%;min-width:620px;background-color:#f6f9fb'>");
            html.Append("<tbody><tr><td style='padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;vertical-align:top'><center>");
            html.Append("<table style='border-collapse:collapse;border-spacing:0;width:100%'><tbody><tr>");
            html.Append("<td style='padding-top:8px;padding-bottom:8px;padding-left:32px;padding-right:32px;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;width:50%;color:#b3b3b3;text-align:left;font-family:sans-serif'></td>");
            html.Append("<td style='padding-top:8px;padding-bottom:8px;padding-left:32px;padding-right:32px;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;width:50%;color:#b3b3b3;text-align:left;font-family:sans-serif'></td>");
            html.Append("<div>No Images? <a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3'>Click here</a></div></td></tr>");
            html.Append("</tbody></table><table style='border-collapse:collapse;border-spacing:0;Margin-left:auto;Margin-right:auto;width:600px'>");
            html.Append("<tbody><tr><td style='padding-top:16px;padding-bottom:32px;padding-left:0;padding-right:0;vertical-align:top;font-size:24px;line-height:32px;letter-spacing:-0.01em;color:#2e3b4e;font-family:Cabin,Avenir,sans-serif!important' align='center'>");
            html.Append("<center><div><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0;display:block;Margin-left:auto;Margin-right:auto;max-width:225px' src='cid:logo' alt='' height='134' width='150'></div></center></td>");
            html.Append("</tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;width:100%'><tbody><tr>");
            html.Append("<td style='padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;vertical-align:top' align='center'>");
            html.Append("<table style='border-collapse:collapse;border-spacing:0;Margin-left:auto;Margin-right:auto;width:600px'>");
            html.Append("<tbody><tr><td style='padding-top:0;padding-bottom:0;padding-left:0;padding-right:0;vertical-align:top;text-align:left'><div><div style='font-size:52px;line-height:52px'>&nbsp;</div></div>");
            html.Append("<table style='border-collapse:collapse;border-spacing:0;width:100%'><tbody><tr><td style='padding-top:0;padding-bottom:0;padding-left:90px;padding-right:90px;vertical-align:top'><h3 style='Margin-top:0;color:#2e3b4e;font-size:16px;Margin-bottom:16px;font-family:Cabin,Avenir,sans-serif!important;line-height:24px'>You have successfully created an account with DIY-Malta</h3>");
            html.Append("<ol style='Margin-top:0;padding-left:0;color:#4e5561;font-size:16px;Margin-left:30px;font-family:sans-serif;line-height:25px;Margin-bottom:25px;font-weight:300'>");
            html.Append("<li style='Margin-top:0;padding-left:0;Margin-bottom:13px'>Username :" +
                        username + "</li>");
            html.Append("<li style='Margin-top:0;padding-left:0;Margin-bottom:13px'>Password :" +
                        password + "</li>");
            html.Append("<li style='Margin-top:0;padding-left:0;Margin-bottom:13px'>Email :" +
                        email + "</li></ol>");
            html.Append("<p style='Margin-top:0;color:#4e5561;font-size:16px;font-family:sans-serif;line-height:25px;Margin-bottom:26px;font-weight:300'><strong style='font-weight:bold'>Thank you for joining us</strong>.Take a look at our <a href='#' style='text-decoration:underline;color:#2186b8'> <em>catalog</em>.</a></p>");
            html.Append(
                "<br/><div style='background:#eaeaea;text-align:center;padding:5px;'><p style='font-size:12px;margin:0; text-align:center;'>Thank you, <strong>DIY-Malta</strong></p></div><p>This is an automatic e-mail message generated by the DIY-Malta system. Please do not reply to this e-mail.</p>");
            html.Append("</td></tr></tbody></table><div style='font-size:26px;line-height:26px'>&nbsp;</div></td></tr></tbody></table>");
            html.Append("</td></tr></tbody></table><table style='border-collapse:collapse;border-spacing:0;width:100%;background-color:#f6f9fb'><tbody><tr>");
            html.Append("<td style='padding-top:60px;padding-bottom:55px;padding-left:0;padding-right:0;vertical-align:top' align='center'><table style='border-collapse:collapse;border-spacing:0;width:600px'><tbody><tr>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:22px;padding-left:0;padding-right:5px;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;text-align:left;width:55%;color:#b3b3b3;font-family:sans-serif'><table style='border-collapse:collapse;border-spacing:0'>");
            html.Append(
                "<tbody><tr><td style='padding-top:0;padding-bottom:20px;padding-left:0;padding-right:15px;vertical-align:top;text-transform:uppercase;line-height:21px;font-size:11px'>");
            html.Append(
                "<a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3;display:block;font-family:sans-serif'><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0' src='cid:twitter' align='top' height='20' width='25'>Tweet</a></td>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:20px;padding-left:0;padding-right:15px;vertical-align:top;text-transform:uppercase;line-height:21px;font-size:11px'><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3;display:block;font-family:sans-serif' rel='cs_facebox'><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0' src='cid:facebook' align='top' height='20' width='25'>Like</a></td>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:20px;padding-left:0;padding-right:15px;vertical-align:top;text-transform:uppercase;line-height:21px;font-size:11px'><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3;display:block;font-family:sans-serif'><img style='border-left-width:0;border-top-width:0;border-bottom-width:0;border-right-width:0' src='cid:arrow' align='top' height='20' width='25'>Forward</a></td>");
            html.Append("</tr></tbody></table></td>");
            html.Append(
                "<td style='padding-top:0;padding-bottom:22px;padding-left:5px;padding-right:0;vertical-align:top;font-size:11px;font-weight:400;letter-spacing:0.01em;line-height:17px;text-align:right;width:45%;color:#b3b3b3;font-family:sans-serif'><div style='font-size:1px;line-height:20px;width:100%'>&nbsp;</div>");
            html.Append(
                "<div><span><span><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3'>Preferences</a><span> &nbsp;|&nbsp; </span></span></span><span><a style='font-weight:700;letter-spacing:0.03em;text-decoration:none;color:#b3b3b3'>Unsubscribe</a></span></div></td></tr></tbody></table></td></tr></tbody></table></center></td></tr></tbody></table>");
            html.Append("</div></font></div></td></tr></tbody></table></td></tr></tbody></table></body></html>");

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (html.ToString(), null, MediaTypeNames.Text.Html);

            var logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/logo.png"), "image/png")
            {
                ContentId = "logo"
            };
            avHtml.LinkedResources.Add(logo);

            var arrow = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/arrow.png"), "image/png")
            {
                ContentId = "arrow"
            };
            avHtml.LinkedResources.Add(arrow);

            var facebook = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/facebook.png"), "image/png")
            {
                ContentId = "facebook"
            };
            avHtml.LinkedResources.Add(facebook);

            var twitter = new LinkedResource(HttpContext.Current.Server.MapPath("~/img/skeleton/twitter.png"), "image/png")
            {
                ContentId = "twitter"
            };
            avHtml.LinkedResources.Add(twitter);

            new Communication().SendEmail(email, "Registration Confirmation", avHtml);
        }

        private static bool ValidateComplexFields(int phone, int mobile, string postCode, string email)
        {
            return ValidateField(phone + "", "(00356)?(21|27|22|25)[0-9]{6}") &&
                   ValidateField(mobile + "", "(00356)?(99|79|77)[0-9]{6}") &&
                   ValidateField(postCode + "", "[A-Z]{3}\\d{4}") &&
                   ValidateField(email, "^[a-zA-Z0-9._-]+@[a-zA-Z0-9-]+\\.[a-zA-Z.]{2,5}$");
        }

        private static bool ValidateField(string field, string regex)
        {
            bool result = false;

            if (string.IsNullOrEmpty(field))
            {
            }
            else if (Regex.IsMatch(field, regex))
            {
                result = true;
            }

            return result;
        }

        private static bool ValidateNumericFields(int genderId, int contactType, int addressType, int townId)
        {
            return ValidateField(genderId + "", "^[0-9]") &&
                   ValidateField(contactType + "", "^[0-9]") &&
                   ValidateField(addressType + "", "^[0-9]") &&
                   ValidateField(genderId + "", "^[0-9]") &&
                   ValidateField(townId + "", "^[0-9]");
        }

        private static bool ValidateStringFields(string name, string surname, string address, string street)
        {
            return ValidateField(name, "^[a-zA-Z0-9._-]") &&
                   ValidateField(surname, "^[a-zA-Z0-9._-]") &&
                   ValidateField(address, "^[a-zA-Z0-9._-]") &&
                   ValidateField(street, "^[a-zA-Z0-9._-]");
        }

        private static DateTime ValidDate(string dob)
        {
            DateTime dateOfBirth;
            try
            {
                dateOfBirth = DateTime.Parse(dob);
            }
            catch (Exception)
            {
                string date = DateTime.Today.AddYears(-18).ToShortDateString();
                dateOfBirth = DateTime.Parse(date);
            }
            return dateOfBirth;
        }

        #endregion Private Methods
    }
}