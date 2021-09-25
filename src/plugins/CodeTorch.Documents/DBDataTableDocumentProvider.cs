using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Documents
{
    public class DBDataTableDocumentProvider: IDocumentProvider
    {
      

        public void Initialize(List<CodeTorch.Core.Setting> settings)
        {
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

            doc.ID = Guid.NewGuid().ToString();
            string modifiedBy = null;
            string fileExtension = Path.GetExtension(doc.FileName);
            byte[] fileContents = null;

            modifiedBy = UserIdentityService.GetInstance().IdentityProvider.GetUserName();

            foreach (Setting setting in doc.Settings)
            {
                switch (setting.Name.ToLower())
                { 

                    case "modifiedby":
                        if (!string.IsNullOrEmpty(setting.Value))
                        {
                            modifiedBy = setting.Value;
                        }
                        break;
                }
            }

            fileContents = new byte[doc.Size];
            using (Stream str = doc.Stream)
            {
                str.Read(fileContents, 0, doc.Size);
            }

            DocumentFunctions utility = new DocumentFunctions();
            utility.InsertDocument(
                doc.ID, "DB", doc.EntityID, doc.EntityType, doc.FileName, doc.DocumentType, doc.ContentType,
                doc.Size, 1, doc.Url, fileContents, false, modifiedBy);
        }
    }
}
