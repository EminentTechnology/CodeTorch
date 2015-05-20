using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public enum MultipleFileSelection
    { 
        Disabled = 0,
        Automatic = 1
    }

    [Serializable]
    public enum FileStorageMode
    { 
        Database = 0,
        AmazonS3
    }

    [Serializable]
    public enum StorageProviderCredentialSource
    {
        AppSetting = 0,
        Constant = 1
    }

    [Serializable]
    public class FileUploadControl : BaseControl
    {
        ScreenInputType _EntityInputType = ScreenInputType.QueryString;
        bool _EnableInlineProgress = true;
        bool _AutoAddFileInputs = true;
        List<FileFilter> _FileFilters = new List<FileFilter>();
        int _InitialFileInputsCount = 1;
        int _InputSize = 23;
        int _MaxFileInputsCount = 1;
        int _MaxFileSize = 0;
        int _TemporaryFileExpirationMinutes = 240;

        Document _Document = new Document();

        public override string Type
        {
            get
            {
                return "FileUpload";
            }
            set
            {
                base.Type = value;
            }
        }





        [Category("Storage")]
        public ScreenInputType EntityInputType
        {
            get { return _EntityInputType; }
            set { _EntityInputType = value; }
        }


        [Category("Storage")]
        [TypeConverter("CodeTorch.Core.Design.DocumentRepositoryTypeConverter,CodeTorch.Core.Design")]
        public string DocumentRepository { get; set; }

        [Category("Storage")]
        public Document Document
        {
            get { return _Document; }
            set { _Document = value; }
        }

       



        [Category("Behavior")]
        [Description("Comma delimited list of allowed extensions - eg zip,doc,xls")]
        public string AllowedFileExtensions { get; set; }

        [Category("Behavior")]
        [Description("Comma delimited list of allowed mime types - video/mpeg,application/msword")]
        public string AllowedMimeTypes { get; set; }

        [Category("Behavior")]
        [Description("Displays a new File input automatically after selecting a file to upload")]
        public bool AutoAddFileInputs
        {
            get { return _AutoAddFileInputs; }
            set { _AutoAddFileInputs = value; }
        }

        

        [Category("Behavior")]
        [Description("If disabled does not send files in chunks - sending files in chunks bypasses web server limits")]
        public bool DisableChunkUpload { get; set; }

        [Category("Behavior")]
        [Description("If disabled will not use Silverlight or flash to perform upload but regular means")]
        public bool DisablePlugins { get; set; }

        [Category("File Handling")]
        [Description("Comma delimited list of CSS selectors that will allow drag and drop of files using HTML5 FileApi")]
        public string DropZones { get; set; }

        [Category("Behavior")]
        [Description("Displays an inline progress next to each item uploaded")]
        public bool EnableInlineProgress 
        {
            get { return _EnableInlineProgress; }
            set { _EnableInlineProgress = value; }
        }

        

        [XmlArray("FileFilters")]
        [XmlArrayItem("FileFilter")]
        [Description("List of file filters to use in open dialog window")]
        [Category("Behavior")]
        public virtual List<FileFilter> FileFilters
        {
            get
            {
                return _FileFilters;
            }

        }

        [Category("Behavior")]
        [Description("Hides the file input")]
        public bool HideFileInput { get; set; }

        [Category("Behavior")]
        [Description("The initial count of input fields to display")]
        public int InitialFileInputsCount
        {
            get { return _InitialFileInputsCount; }
            set { _InitialFileInputsCount = value; }
        }

        [Category("Behavior")]
        [Description("The size of the file input field")]
        public int InputSize
        {
            get { return _InputSize; }
            set { _InputSize = value; }
        }

        [Category("Behavior")]
        [Description("Maximum number of files that can be uploaded - 0 means unlimited")]
        public int MaxFileInputsCount
        {
            get { return _MaxFileInputsCount; }
            set { _MaxFileInputsCount = value; }
        }

        [Category("Behavior")]
        [Description("Maximum file size in bytes - 0 means unlimited")]
        public int MaxFileSize
        {
            get { return _MaxFileSize; }
            set { _MaxFileSize = value; }
        }

        [Category("Behavior")]
        [Description("Used in open file dialog to determine if one can select multiple files or not")]
        public MultipleFileSelection MultipleFileSelection { get; set; }

        [Category("Client Side Events")]
        public string OnClientAdded { get; set; }

        [Category("Client Side Events")]
        public string OnClientFileDropped { get; set; }

        [Category("Client Side Events")]
        public string OnClientFileSelected { get; set; }

        [Category("Client Side Events")]
        public string OnClientFilesSelected { get; set; }

        [Category("Client Side Events")]
        public string OnClientFilesUploaded { get; set; }

        [Category("Client Side Events")]
        public string OnClientFileUploaded { get; set; }

        [Category("Client Side Events")]
        public string OnClientFileUploadFailed { get; set; }

        [Category("Client Side Events")]
        public string OnClientFileUploading { get; set; }

        [Category("Client Side Events")]
        public string OnClientFileUploadRemoved { get; set; }

        [Category("Client Side Events")]
        public string OnClientFileUploadRemoving { get; set; }

        [Category("Client Side Events")]
        public string OnClientProgressUpdating { get; set; }

        [Category("Client Side Events")]
        public string OnClientValidationFailed { get; set; }

        [Category("File Handling")]
        [Description("Comma delimited list of Control IDs - client state is updated when these controls trigger a postback")]
        public string PostbackTriggers { get; set; }

        

        [Category("File Handling")]
        [Description("The number of minutes before temporary files are deleted")]
        public int TemporaryFileExpirationMinutes
        {
            get { return _TemporaryFileExpirationMinutes; }
            set { _TemporaryFileExpirationMinutes = value; }
        }

        [Category("File Handling")]
        [Description("Path to where files should be stored temporarirly on server")]
        public string TemporaryFolder { get; set; }

        [Category("File Handling")]
        [Description("URL to custom http handler for upload")]
        public string HttpHandlerUrl { get; set; }


        [Category("Appearance")]
        public string Skin { get; set; }
    }
}
