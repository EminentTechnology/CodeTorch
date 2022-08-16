using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeTorch.Core;
using System.Collections;
using RestSharp;
using CodeTorch.Designer.Code;
using CodeTorch.Core.Services;
using CodeTorch.Core.Interfaces;

namespace CodeTorch.Designer.Forms
{
    public partial class ResourceTranslationForm : Form
    {
        App app;
        List<ResourceItem> keys;

        private readonly DataConnection connection;
        public ResourceTranslationForm(DataConnection connection)
        {
            InitializeComponent();

            app = CodeTorch.Core.Configuration.GetInstance().App;
            this.connection = connection;
        }

        private void ResourceTranslationForm_Load(object sender, EventArgs e)
        {
            try
            {
                
                keys = Localization.GetResourceKeys(connection);

                SetupProgressBar(keys);


                DefaultCultureCode.Text = app.LocalizationDefaultCulture;

                GoogleAPIKey.Text = Properties.Settings.Default.GoogleTranslateAPIKey;
               
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void SetupProgressBar(List<ResourceItem> resourceKeys)
        {
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = resourceKeys.Count;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                IResourceProvider resource = ResourceService.GetInstance().ResourceProvider;
                resource.Connection = connection;

                StartButton.Enabled = false;

                

                int translationCount = 0;

                if (String.IsNullOrEmpty(CultureCode.Text))
                { 
                    throw new ApplicationException("Culture code is required");
                }

                //Save API Key for next time use
                if (GoogleAPIKey.Enabled)
                {
                    if (String.IsNullOrEmpty(GoogleAPIKey.Text))
                    {
                        throw new ApplicationException("API Key is required");
                    }

                    Properties.Settings.Default.GoogleTranslateAPIKey = GoogleAPIKey.Text;
                    Properties.Settings.Default.Save();
                }

                if (UpdateCultureFromConfig.Checked)
                {
                    foreach (ResourceItem key in keys)
                    {
                        key.CultureCode = CultureCode.Text;
                    }

                    resource.Save( keys, ForceUpdate.Checked);
                }
                else
                {
                    //loop through each resource key while length of text is less than 1000
                    int maxChars = 1000;
                    ArrayList List = new ArrayList();

                    int TranslationLength = 0;
                    

                    List<ResourceItem> batchKeys = new List<ResourceItem>();

                    if (UpdateCultureFromGoogleUsingDB.Checked)
                    {
                        keys = Localization.GetResourceKeysFromDB( DefaultCultureCode.Text);
                        SetupProgressBar(keys);
                    }

                    foreach (ResourceItem key in keys)
                    {
                        TranslationLength += key.Value.Length;

                        batchKeys.Add(key);

                        if (TranslationLength >= maxChars)
                        {
                            List.Add(batchKeys);

                            TranslationLength = 0;
                            batchKeys = new List<ResourceItem>();
                        }

                        
                    }

                    if (!List.Contains(batchKeys))
                    {
                        List.Add(batchKeys);
                    }

                    foreach (List<ResourceItem> batch in List)
                    {
                        var client = new RestClient("https://www.googleapis.com");

                        var request = new RestRequest("language/translate/v2", Method.Get);
                        request.AddParameter("key", GoogleAPIKey.Text);
                        request.AddParameter("source", DefaultCultureCode.Text.Substring(0, 2));
                        request.AddParameter("target", CultureCode.Text.Substring(0, 2));

                        foreach (ResourceItem key in batch)
                        {
                            request.AddParameter("q", key.Value);
                        }

                        RestResponse<GoogleTranslateResponse> response = (RestResponse<GoogleTranslateResponse>) client.Execute<GoogleTranslateResponse>(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            for (int i = 0; i < batch.Count; i++)
                            {
                                batch[i].CultureCode = CultureCode.Text;
                                batch[i].Value = response.Data.data.translations[i].translatedText;

                            }

                            resource.Save(batch, ForceUpdate.Checked);
                        }

                       
                    }
                }

                for (int i = 0; i <= progressBar.Maximum; i++)
                {
                    progressBar.Value = i;
                    translationCount = i;
                    Application.DoEvents();
                }


                MessageBox.Show(String.Format("{0} translations updated for culture {1}", translationCount, CultureCode.Text));

                this.Close();
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
            finally
            {
                StartButton.Enabled = true;
            }
        }

        private void UpdateCultureFromConfig_CheckedChanged(object sender, EventArgs e)
        {
            ProcessCheck();
        }

        private void UpdateCultureFromGoogle_CheckedChanged(object sender, EventArgs e)
        {
            ProcessCheck();
        }

        private void UpdateCultureFromGoogleUsingDB_CheckedChanged(object sender, EventArgs e)
        {
            ProcessCheck();
        }

        private void ProcessCheck()
        {
            if (UpdateCultureFromConfig.Checked)
            {
                GoogleAPIKey.Enabled = false;
            }
            else
            {
                GoogleAPIKey.Enabled = true;
            }
        }

        
    }
}
