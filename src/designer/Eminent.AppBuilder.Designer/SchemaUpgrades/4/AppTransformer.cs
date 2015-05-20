using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._4
{
    class AppTransformer: ICodeTransformer
    {
        

        public bool Execute()
        {
            ProcessApp(Document);
            return true;
        }

        public string EntityType {get;set;}
        public XDocument Document { get; set; }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("App");

            return types;
        }
        
  

        public  void ProcessApp(XDocument Document)
        {
            bool UsesWindowsAuthentication = false;
            bool UsesUserIDForAuthorization = false;

            XElement UserIdentityProviderAssembly = Document.Root.Element("UserIdentityProviderAssembly");

            //should not exist
            if (UserIdentityProviderAssembly == null)
            {

                XElement FormsAuthenticationMode = Document.Root.Element("FormsAuthenticationMode");
                if (FormsAuthenticationMode != null)
                {
                    UsesWindowsAuthentication = false;
                }

                XElement WindowsAuthenticationMode = Document.Root.Element("WindowsAuthenticationMode");
                if (WindowsAuthenticationMode != null)
                {
                    UsesWindowsAuthentication = true;
                }

                XElement UserIDAuthorizationMode = Document.Root.Element("UserIDAuthorizationMode");
                if (UserIDAuthorizationMode != null)
                {
                    UserIDAuthorizationMode.Remove();
                    UsesUserIDForAuthorization = true;
                }

                XElement UserNameAuthorizationMode = Document.Root.Element("UserNameAuthorizationMode");
                if (UserNameAuthorizationMode != null)
                {
                    UserNameAuthorizationMode.Remove();
                    UsesUserIDForAuthorization = false;
                }

                Document.Root.Add(new XElement("DefaultConnection", "Default"));
                Document.Root.Add(new XElement("SaveSequenceDataCommand", "Sequence_Save"));
                Document.Root.Add(new XElement("SMSSendDataCommand", "SMS_Send"));
                Document.Root.Add(new XElement("LookupProviderAssembly", "CodeTorch.Lookups"));
                Document.Root.Add(new XElement("LookupProviderClass", "CodeTorch.Lookups.DataCommandLookupProvider"));
                Document.Root.Add(new XElement("ResourceProviderAssembly", "CodeTorch.Resources.Web"));
                Document.Root.Add(new XElement("ResourceProviderClass", "CodeTorch.Resources.Web.DataCommandResourceProvider"));

                if (UsesWindowsAuthentication)
                {
                    Document.Root.Add(new XElement("UserIdentityProviderAssembly", "CodeTorch.Security"));
                    Document.Root.Add(new XElement("UserIdentityProviderClass", "CodeTorch.Security.Identity.WindowsUserIdentityProvider"));
                }
                else
                {
                    Document.Root.Add(new XElement("UserIdentityProviderAssembly", "CodeTorch.Security"));
                    Document.Root.Add(new XElement("UserIdentityProviderClass", "CodeTorch.Security.Identity.FormsUserIdentityProvider"));
                }

                if (UsesUserIDForAuthorization)
                {
                    Document.Root.Add(new XElement("AuthorizationProviderAssembly", "CodeTorch.Security"));
                    Document.Root.Add(new XElement("AuthorizationProviderClass", "CodeTorch.Security.Authorization.DataCommandUserIDAuthorizationProvider"));
                }
                else
                {
                    Document.Root.Add(new XElement("AuthorizationProviderAssembly", "CodeTorch.Security"));
                    Document.Root.Add(new XElement("AuthorizationProviderClass", "CodeTorch.Security.Authorization.DataCommandUserNameAuthorizationProvider"));
                }

            }
  

        }

        


        
    }
}
