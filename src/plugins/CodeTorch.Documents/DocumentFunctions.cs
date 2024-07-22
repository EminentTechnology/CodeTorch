using CodeTorch.Core;
using CodeTorch.Core.Services;
using System.Collections.Generic;

namespace CodeTorch.Documents
{
    class DocumentFunctions
    {
        private const string DataCommandDocumentInsert = "Document_Insert";

        private const string ParameterDocumentID = "@DocumentID";
        private const string ParameterStorageType = "@StorageType";
        private const string ParameterEntityID = "@EntityID";
        private const string ParameterEntityType = "@EntityType";
        private const string ParameterDocumentName = "@DocumentName";
        private const string ParameterDocumentTypeCode = "@DocumentTypeCode";
        private const string ParameterDocumentUrl = "@DocumentUrl";
        private const string ParameterFile = "@File";

        private const string ParameterStatus = "@Status";
        private const string ParameterIsEntityTypeDefault = "@IsEntityTypeDefault";
        private const string ParameterContentType = "@ContentType";
        private const string ParameterSize = "@Size";
        private const string ParameterCreatedBy = "@CreatedBy";

        public void InsertDocument(
            string DocumentID, string StorageType, string EntityID, 
            string EntityType, string DocumentName, string DocumentTypeCode, 
            string ContentType, int Size, int? status, string DocumentUrl, 
            byte[] File, bool IsEntityTypeDefault, string CreatedBy)
        {
            DataCommandService sql = DataCommandService.GetInstance();
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterDocumentID, DocumentID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterStorageType, StorageType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityID, EntityID);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityType, EntityType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterDocumentName, DocumentName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterDocumentTypeCode, DocumentTypeCode);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterDocumentUrl, DocumentUrl);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterFile, File);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterStatus, status);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterIsEntityTypeDefault, IsEntityTypeDefault);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterContentType, ContentType);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterSize, Size);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterCreatedBy, CreatedBy);
            parameters.Add(p);

            sql.ExecuteDataCommand(DataCommandDocumentInsert, parameters);

            
        }
    }
}
