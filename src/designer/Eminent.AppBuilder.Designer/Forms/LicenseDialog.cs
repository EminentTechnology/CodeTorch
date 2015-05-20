using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CodeTorch.Designer.com.codetorch.licensing;
using System.Management;
using System.Xml;

namespace CodeTorch.Designer.Forms
{
    public partial class LicenseDialog : Telerik.WinControls.UI.RadForm
    {
        LicenseService svc = new LicenseService();

        public string LicenseFilePath { get; set; }

        const int PAGE_SELECTION = 0;
        const int PAGE_SIGNUP = 1;
        const int PAGE_LOGIN = 2;
        const int PAGE_SELECT_LICENSE = 3;

        
        const int PAGE_CONFIRM_LICENSE = 4;

        bool fromSignup = false;

        public LicenseDialog()
        {
            InitializeComponent();

            //radWizard1.NextButton.Enabled = false;
        }

        private void radWizard1_Next(object sender, Telerik.WinControls.UI.WizardCancelEventArgs e)
        {


            bool retVal = Advance();
            e.Cancel = !retVal;
        }

        private void radWizard1_Previous(object sender, Telerik.WinControls.UI.WizardCancelEventArgs e)
        {

            bool retVal = Reverse();
            e.Cancel = !retVal;
        }

        private bool Reverse()
        {
            bool retVal = true;

            if (this.radWizard1.SelectedPage == this.radWizard1.Pages[PAGE_LOGIN])
            {
                if (fromSignup)
                {
                    retVal = false;
                    this.radWizard1.SelectedPage = this.radWizard1.Pages[PAGE_SELECTION];

                }
                
            }

            return retVal;
        }

        private bool Advance()
        {
            bool retVal = true;

            //next from license selection
            if (this.radWizard1.SelectedPage == this.radWizard1.Pages[PAGE_SELECT_LICENSE])
            {
                retVal = AdvanceToLicenseSummary();
                
            }

            //initial credentials entry page
            if (this.radWizard1.SelectedPage == this.radWizard1.Pages[PAGE_LOGIN])
            {
                retVal = AdvanceToLicenseList();
            }

            //initial credentials entry page
            if (this.radWizard1.SelectedPage == this.radWizard1.Pages[PAGE_SIGNUP])
            {
                retVal = false;
                AdvanceToLogin();

            }


            //initial activation login/signup screen
            if (this.radWizard1.SelectedPage == this.radWizard1.Pages[PAGE_SELECTION])
            {
                retVal = false;
                if(LoginOption.IsChecked)
                {
                    fromSignup = false;
                    AdvanceToLogin();
                }
                else
                {
                    fromSignup = true;
                    AdvanceToSignupPage();
                }
            }

            

            

            


            return retVal;
            
        }

        private bool AdvanceToSignupPage()
        {
            bool retVal = false;

            try
            {
                this.radWizard1.SelectedPage = this.radWizard1.Pages[PAGE_SIGNUP];

                string URL = "http://portal.codetorch.com/app/Signup/SignupDetails.aspx?MemberType=Enterprise";
                System.Diagnostics.Process.Start(URL);
                
            }
            catch (Exception ex)
            {
                
                string error = String.Format("The following error occurred:/r/n{0}", ex.Message);
                retVal = false;
                MessageBox.Show(error);

            }
            
            return retVal;
        }

        private bool AdvanceToLogin()
        {
            bool retVal = false;

            try
            {
                this.radWizard1.SelectedPage = this.radWizard1.Pages[PAGE_LOGIN];
                ValidateLoginCredentials();
                

            }
            catch (Exception ex)
            {

                string error = String.Format("The following error occurred:/r/n{0}", ex.Message);
                retVal = false;
                MessageBox.Show(error);

            }

            return retVal;
        }

        private bool AdvanceToLicenseSummary()
        {
            bool retVal = true;

            if (LicenseList.SelectedValue == null)
            {
                retVal = false;
                this.radWizard1.SelectedPage = this.radWizard1.Pages[PAGE_SELECT_LICENSE];
                MessageBox.Show("Please select a valid license");
            }
            else
            {
                DataRowView data = (DataRowView)LicenseList.SelectedItem.DataBoundItem;

                LicenseID.Text = data["LicenseID"].ToString();
                LicensedTo.Text = data["CustomerName"].ToString();
                ExpiresOn.Text = Convert.ToDateTime(data["ExpirationDate"]).ToString("dd MMM yyyy");

               
            }

            return retVal;
        }

        private bool AdvanceToLicenseList()
        {
            bool retVal = true;

            try
            {
                DataSet licenses = svc.GetAvailableLicenses(EmailAddress.Text, Password.Text);
                LicenseList.DataSource = licenses.Tables[0].DefaultView;
                
            }
            catch (Exception ex)
            {
                //if login fails
                string error = String.Format("The following error occurred:/r/n{0}", ex.Message);
                retVal = false;
                this.radWizard1.SelectedPage = this.radWizard1.Pages[PAGE_LOGIN];
                MessageBox.Show(error);

            }

            return retVal;
        }

        private void EmailAddress_TextChanged(object sender, EventArgs e)
        {
            ValidateLoginCredentials();
        }

        private void ValidateLoginCredentials()
        {
            if ((EmailAddress.Text.Trim().Length > 0) && (Password.Text.Trim().Length > 0))
            {
                radWizard1.NextButton.Enabled = true;
            }
            else
            {
                radWizard1.NextButton.Enabled = false;
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            ValidateLoginCredentials();
        }

        private void radWizard1_Finish(object sender, EventArgs e)
        {

            FinishActivation();
            //MessageBox.Show(doc.OuterXml);

        }

        private void FinishActivation()
        {
            XmlNode license = svc.ActivateLicense(EmailAddress.Text, LicenseList.SelectedItem.Value.ToString(), GetMachineID(), System.Environment.MachineName);
            string LicenseXML = license.OuterXml;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(LicenseXML);
            doc.Save(LicenseFilePath);

            this.Close();
        }

        string GetMachineID()
        {
            
            string retVal = null;

            //Get motherboard's serial number
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            mbsList = mbs.Get();
            foreach (ManagementObject mo in mbsList)
            {
                retVal = mo["SerialNumber"].ToString();
            }

           
           


            return retVal;
        }

        private void EmailAddress_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessEnter(e);
            
        }

        private void ProcessEnter(KeyEventArgs e)
        {
                    if ((e.KeyCode == Keys.Enter) && (radWizard1.NextButton.Enabled))
                    {
                        bool retVal = Advance();
                        if (retVal)
                        {
                            if (this.radWizard1.SelectedPage == this.radWizard1.Pages[PAGE_LOGIN])
                            {
                                this.radWizard1.SelectedPage = this.radWizard1.Pages[PAGE_SELECT_LICENSE];
                            }
                            else
                            {
                                if (this.radWizard1.SelectedPage == this.radWizard1.Pages[PAGE_SELECT_LICENSE])
                                {
                                    this.radWizard1.SelectedPage = this.radWizard1.Pages[PAGE_CONFIRM_LICENSE];
                                }
                                else
                                {
                                    FinishActivation();
                                }
                        
                            }

                    
                        }
                    }
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessEnter(e);
        }

        private void radWizard1_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radWizard1_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            string URL = "http://support.codetorch.com";
            System.Diagnostics.Process.Start(URL);
        }

        

        
    }
}
