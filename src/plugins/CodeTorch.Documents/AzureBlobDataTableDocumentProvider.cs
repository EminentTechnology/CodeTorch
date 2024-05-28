using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace CodeTorch.Documents
{
    public class AzureBlobDataTableDocumentProvider : IDocumentProvider
    {
        const FileStorageMode StorageMode = FileStorageMode.AzureBlobStorage;
        StorageProviderCredentialSource storageProviderAccountNameSource = StorageProviderCredentialSource.AppSetting;
        StorageProviderCredentialSource storageProviderConnectionStringSource = StorageProviderCredentialSource.AppSetting;
        StorageProviderCredentialSource storageProviderAccountKeySource = StorageProviderCredentialSource.AppSetting;
        StorageProviderCredentialSource storageProviderContainerSource = StorageProviderCredentialSource.AppSetting;
        StorageProviderCredentialSource storageProviderAuthenticationMethodSource = StorageProviderCredentialSource.AppSetting;
        string storageProviderAccountNameKey = String.Empty;
        string storageProviderConnectionStringKey = String.Empty;
        string storageProviderAccountKeyKey = String.Empty;
        string storageProviderContainerKey = String.Empty;
        string storageProviderAuthenticationMethodKey = String.Empty;

        const string AuthenticationMethodConnectionString = "ConnectionString";
        const string AuthenticationMethodSharedKey = "SharedKey";
        const string AuthenticationMethodDefaultAzureCredentials = "DefaultAzureCredentials";

        public void Initialize(List<CodeTorch.Core.Setting> settings)
        {
            foreach (Setting setting in settings)
            {
                switch (setting.Name.ToLower())
                { 
                    case "accountnamesource":
                        storageProviderAccountNameSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "accountname":
                        storageProviderAccountNameKey = setting.Value;
                        break;
                    case "connectionstringsource":
                        storageProviderConnectionStringSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "connectionstring":
                        storageProviderConnectionStringKey = setting.Value;
                        break;
                    case "accountkeysource":
                        storageProviderAccountKeySource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "accountkey":
                        storageProviderAccountKeyKey = setting.Value;
                        break;
                    case "containersource":
                        storageProviderContainerSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "container":
                        storageProviderContainerKey = setting.Value;
                        break;
                    case "authenticationmethodsource":
                        storageProviderAuthenticationMethodSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "authenticationmethod":
                        storageProviderAuthenticationMethodKey = setting.Value;
                        break;
                }
            }
        }

        public string Upload(Document doc)
        {
            UploadDocument(doc);
            return doc.ID;
        }

        private void UploadDocument(
            Document doc
            )
        {

            string modifiedBy = null;
            string fileExtension = Path.GetExtension(doc.FileName);
            string storageProviderFolder = null;
            byte[] fileContents = null;

            modifiedBy = UserIdentityService.GetInstance().IdentityProvider.GetUserName();

            foreach (Setting setting in doc.Settings)
            {
                switch (setting.Name.ToLower())
                {
                    case "folder":
                        storageProviderFolder = setting.Value;
                        break;
                    case "modifiedby":
                        if (!string.IsNullOrEmpty(setting.Value))
                        {
                            modifiedBy = setting.Value;
                        }
                        break;
                }
            }

            doc.ID = Guid.NewGuid().ToString();

            string blobName = GetBlobName(doc, fileExtension, ref storageProviderFolder);

            //get blob container client
            string authentionMethod = GetAuthenticationMethod();
            string connectionString = GetConnectionString();
            string containerName = GetContainer();
            string accountName = GetAccountName();
            string accountKey = GetAccountKey();

            BlobContainerClient containerClient = GetBlobContainerClient(authentionMethod, connectionString, containerName, accountName, accountKey);

            var blobClient = containerClient.GetBlobClient(blobName);
            doc.Stream.Position = 0;
            var blobContentInfo = blobClient.Upload(doc.Stream);

            doc.Url = String.Format($"{accountName}.blob.core.windows.net/{containerName}/{blobName}", accountName, containerName, blobName);

            DocumentFunctions utility = new DocumentFunctions();
            utility.InsertDocument(
                doc.ID, "AzureBlob", doc.EntityID, doc.EntityType, doc.FileName, doc.DocumentType, doc.ContentType,
                doc.Size, 1, doc.Url, fileContents, false, modifiedBy);
        }

        private BlobContainerClient GetBlobContainerClient(string authentionMethod, string connectionString, string containerName, string accountName, string accountKey)
        {
            BlobContainerClient containerClient = null;

            if (String.IsNullOrWhiteSpace(accountName))
            {
                throw new Exception("Account Name for Azure Storage has not been configured");
            }

            if (String.IsNullOrWhiteSpace(containerName))
                throw new Exception("Container Name for Azure Storage has not been configured");


            switch (authentionMethod)
            {
                case AuthenticationMethodConnectionString:
                    if (String.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new Exception("ConnectionString for Azure Storage has not been configured");
                    }

                    containerClient = GetBlobContainerClientWithConnectionString(connectionString, containerName);
                    break;
                case AuthenticationMethodDefaultAzureCredentials:
                    containerClient = GetBlobContainerClientWithDefaultAzureCredentials(accountName, containerName);
                    break;
                case AuthenticationMethodSharedKey:
                    if (String.IsNullOrWhiteSpace(accountKey))
                    {
                        throw new Exception("Account Key for Azure Storage has not been configured");
                    }

                    containerClient = GetBlobContainerClientWithSharedKey(accountName, accountKey, containerName);
                    break;
                default:
                    throw new Exception($"Azure Document Upload Authentication Method is not configured correctly - valid values are {AuthenticationMethodConnectionString}, {AuthenticationMethodDefaultAzureCredentials}, {AuthenticationMethodSharedKey}");
            }

            if (containerClient == null)
            {
                throw new Exception($"Invalid Azure Blob Container name '{containerName}' - please check app configuration.");
            }

            return containerClient;
        }

        private static string GetBlobName(Document doc, string fileExtension, ref string storageProviderFolder)
        {
            string blobName = null;
            if (String.IsNullOrEmpty(storageProviderFolder))
            {
                blobName = String.Format("{0}{1}", doc.ID, fileExtension);
            }
            else
            {
                if (!storageProviderFolder.EndsWith("/"))
                {
                    //add separator if folder does not end with separator
                    storageProviderFolder += "/";
                }

                blobName = String.Format("{0}{1}{2}", storageProviderFolder, doc.ID, fileExtension);
            }

            //substitute folder path with entity id if desired
            var regex = new Regex("{EntityId}", RegexOptions.IgnoreCase);
            blobName = regex.Replace(blobName, doc.EntityID);
            return blobName;
        }

        private BlobContainerClient GetBlobContainerClientWithDefaultAzureCredentials(string accountName, string containerName)
        {
            if (String.IsNullOrWhiteSpace(accountName))
                throw new Exception("Account Name for Azure Blob Storage has not been configured");

            if (String.IsNullOrWhiteSpace(containerName))
                throw new Exception("Container Name for Azure Blob Storage has not been configured");

            string containerEndpoint = $"https://{accountName}.blob.core.windows.net/{containerName}";
            return new BlobContainerClient(new Uri(containerEndpoint), new DefaultAzureCredential());
        }

        private BlobContainerClient GetBlobContainerClientWithConnectionString(string connectionstring, string containerName)
        {
            if (String.IsNullOrWhiteSpace(connectionstring))
                throw new Exception("Connection String for Azure Blob Storage has not been configured");

            if (String.IsNullOrWhiteSpace(containerName))
                throw new Exception("Container Name for Azure Blob Storage has not been configured");

            return new BlobContainerClient(connectionstring, containerName);
        }

        private BlobContainerClient GetBlobContainerClientWithSharedKey(string accountName, string accountKey, string containerName)
        {
            if (String.IsNullOrWhiteSpace(accountName))
                throw new Exception("Account Name for Azure Blob Storage has not been configured");

            if (String.IsNullOrWhiteSpace(accountKey))
                throw new Exception("Account Key for Azure Blob Storage has not been configured");

            if (String.IsNullOrWhiteSpace(accountName))
                throw new Exception("Account Name for Azure Blob Storage has not been configured");


            string accountUri =  $"https://{accountName}.blob.core.windows.net/";
            // Create a SharedKeyCredential that we can use to authenticate
            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(accountName, accountKey);

            // Create a client that can authenticate with a connection string
            BlobServiceClient service = new BlobServiceClient(new Uri(accountUri), credential);

            return service.GetBlobContainerClient(containerName);
        }

        private  string GetValueFromStorageProviderCredentialSource(StorageProviderCredentialSource source, string key)
        {
            string retVal = null;

            switch (source)
            {
                case StorageProviderCredentialSource.AppSetting:
                    retVal = ConfigurationManager.AppSettings[key];
                    break;
                default:
                    retVal = key;
                    break;
            }

            return retVal;
        }

        private string GetAccountName()
        {
            return GetValueFromStorageProviderCredentialSource(storageProviderAccountNameSource, storageProviderAccountNameKey);
        }

        private string GetConnectionString()
        {
            return GetValueFromStorageProviderCredentialSource(storageProviderConnectionStringSource, storageProviderConnectionStringKey);
        }

        private string GetAccountKey()
        {
            return GetValueFromStorageProviderCredentialSource(storageProviderAccountKeySource, storageProviderAccountKeyKey);
        }

        private string GetContainer()
        {
            return GetValueFromStorageProviderCredentialSource(storageProviderContainerSource, storageProviderContainerKey);
        }

        private string GetAuthenticationMethod()
        {
            string retVal =  GetValueFromStorageProviderCredentialSource(storageProviderAuthenticationMethodSource, storageProviderAuthenticationMethodKey);
            if (String.IsNullOrEmpty(retVal))
            {
                retVal = AuthenticationMethodDefaultAzureCredentials;
            }

            return retVal;
        }
    }
}
