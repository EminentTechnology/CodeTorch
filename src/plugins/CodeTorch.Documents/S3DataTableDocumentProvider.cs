using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeTorch.Documents
{
    public class S3DataTableDocumentProvider: IDocumentProvider
    {
        const FileStorageMode StorageMode = FileStorageMode.AmazonS3;

        

        StorageProviderCredentialSource storageProviderUserNameSource = StorageProviderCredentialSource.AppSetting;
        StorageProviderCredentialSource storageProviderPasswordSource = StorageProviderCredentialSource.AppSetting;
        StorageProviderCredentialSource storageProviderRegionSource = StorageProviderCredentialSource.AppSetting;
        StorageProviderCredentialSource storageProviderContainerSource = StorageProviderCredentialSource.AppSetting;
        string storageProviderUserNameKey = String.Empty;
        string storageProviderPasswordKey = String.Empty;
        string storageProviderRegionKey = String.Empty;
        string storageProviderContainerKey = String.Empty;

        public void Initialize(List<CodeTorch.Core.Setting> settings)
        {
            foreach (Setting setting in settings)
            {
                switch (setting.Name.ToLower())
                { 
                    case "accesskeyidsource":
                        storageProviderUserNameSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "accesskeyid":
                        storageProviderUserNameKey = setting.Value;
                        break;
                    case "secretkeysource":
                        storageProviderPasswordSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "secretkey":
                        storageProviderPasswordKey = setting.Value;
                        break;
                    case "regionsource":
                        storageProviderRegionSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "region":
                        storageProviderRegionKey = setting.Value;
                        break;
                    case "bucketsource":
                        storageProviderContainerSource = (StorageProviderCredentialSource)Enum.Parse(typeof(StorageProviderCredentialSource), setting.Value);
                        break;
                    case "bucket":
                        storageProviderContainerKey = setting.Value;
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


            string accessKeyID = GetUserName(storageProviderUserNameSource, storageProviderUserNameKey);
            string secretAccessKey = GetPassword(storageProviderPasswordSource, storageProviderPasswordKey);
            string region = GetRegion(storageProviderRegionSource, storageProviderRegionKey);

            if (String.IsNullOrWhiteSpace(accessKeyID))
                throw new Exception("AWS access key ID for S3 has not been configured");

            if (String.IsNullOrWhiteSpace(secretAccessKey))
                throw new Exception("AWS access key for S3 has not been configured");

            if (String.IsNullOrWhiteSpace(region))
                throw new Exception("AWS region for S3 has not been configured");


            TransferUtility fileTransferUtility = new TransferUtility(accessKeyID, secretAccessKey, RegionEndpoint.GetBySystemName(region));

            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            request.BucketName = GetBucket(storageProviderContainerSource, storageProviderContainerKey);

            //TODO - need to secure and provide options for ACL and storage class
            request.CannedACL = S3CannedACL.PublicRead;
            request.StorageClass = S3StorageClass.ReducedRedundancy;

            doc.ID = Guid.NewGuid().ToString();

            string key = null;
            if (String.IsNullOrEmpty(storageProviderFolder))
            {
                key = String.Format("{0}{1}", doc.ID, fileExtension);
            }
            else
            {
                key = String.Format("{0}/{1}{2}", storageProviderFolder, doc.ID, fileExtension);
            }

            //substitute folder path with entity id if desired
            var regex = new Regex("{EntityId}", RegexOptions.IgnoreCase);
            key = regex.Replace(key, doc.EntityID);
            
            request.Key = key;

            request.AutoCloseStream = false;
            request.ContentType = doc.ContentType;
            request.InputStream = doc.Stream;
            request.InputStream.Position = 0;



            fileTransferUtility.Upload(request);

            doc.Url = String.Format("{0}.s3.amazonaws.com/{1}", request.BucketName, request.Key);

            DocumentFunctions utility = new DocumentFunctions();
            utility.InsertDocument(
                doc.ID, "AmazonS3", doc.EntityID, doc.EntityType, doc.FileName, doc.DocumentType, doc.ContentType,
                doc.Size, 1, doc.Url, fileContents, false, modifiedBy);
        }


        private  string GetUserName(StorageProviderCredentialSource StorageProviderUserNameSource, string StorageProviderUserNameKey)
        {
            string retVal = null;

            switch (StorageProviderUserNameSource)
            {
                case StorageProviderCredentialSource.AppSetting:
                    retVal = ConfigurationManager.AppSettings[StorageProviderUserNameKey];
                    break;
                default:
                    retVal = StorageProviderUserNameKey;
                    break;
            }

            return retVal;
        }

        private  string GetPassword(StorageProviderCredentialSource StorageProviderPasswordSource, string StorageProviderPasswordKey)
        {
            string retVal = null;

            switch (StorageProviderPasswordSource)
            {
                case StorageProviderCredentialSource.AppSetting:
                    retVal = ConfigurationManager.AppSettings[StorageProviderPasswordKey];
                    break;
                default:
                    retVal = StorageProviderPasswordKey;
                    break;
            }

            return retVal;
        }

        private  string GetBucket(StorageProviderCredentialSource StorageProviderContainerSource, string StorageProviderContainerKey)
        {
            string retVal = null;

            switch (StorageProviderContainerSource)
            {
                case StorageProviderCredentialSource.AppSetting:
                    retVal = ConfigurationManager.AppSettings[StorageProviderContainerKey];
                    break;
                default:
                    retVal = StorageProviderContainerKey;
                    break;
            }

            return retVal;
        }

        private  string GetRegion(StorageProviderCredentialSource StorageProviderRegionSource, string StorageProviderRegionKey)
        {
            string retVal = null;

            switch (StorageProviderRegionSource)
            {
                case StorageProviderCredentialSource.AppSetting:
                    retVal = ConfigurationManager.AppSettings[StorageProviderRegionKey];
                    break;
                default:
                    retVal = StorageProviderRegionKey;
                    break;
            }

            return retVal;
        }
    }
}
