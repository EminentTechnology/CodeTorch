using System;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using CodeTorch.Web.Data;
using CodeTorch.Web.Templates;
using CodeTorch.Core;
using Telerik.Web.UI;
using CodeTorch.Core.Services;
using CodeTorch.Core.Interfaces;

namespace CodeTorch.Web.FieldTemplates
{


    public class FileUpload : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadAsyncUpload ctrl;



        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadAsyncUpload();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }


        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();

        FileUploadControl _Me = null;
        public FileUploadControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (FileUploadControl)this.BaseControl;
                }
                return _Me;
            }
        }

        

        public override string Value
        {
            get
            {
                if (ViewState["Value"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    return ViewState["Value"].ToString();
                }
            }
            set
            {
                ViewState["Value"] = value;
            }
        }

        private  string NewValue
        {
            get
            {
                if (ViewState["NewValue"] == null)
                {
                    return String.Empty;
                }
                else
                {
                    return ViewState["NewValue"].ToString();
                }
            }
            set
            {
                ViewState["NewValue"] = value;
            }
        }

        private void AppendDocumentID(string documentID)
        {
            if ((Me.MultipleFileSelection == CodeTorch.Core.MultipleFileSelection.Automatic) || (Me.MaxFileInputsCount > 1))
            {
                if (String.IsNullOrEmpty(NewValue))
                {
                    NewValue = documentID;
                }
                else
                {
                    NewValue += "," + documentID;
                }

                Value = NewValue;
            }
            else
            {
                Value = documentID;
            }
        }


        



        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {
                if (!String.IsNullOrEmpty(Me.AllowedFileExtensions))
                {
                    ctrl.AllowedFileExtensions = Me.AllowedFileExtensions.Replace(" ", "").Split(',');
                }

                if (!String.IsNullOrEmpty(Me.AllowedMimeTypes))
                {
                    ctrl.AllowedMimeTypes = Me.AllowedMimeTypes.Replace(" ", "").Split(',');
                }

                ctrl.AutoAddFileInputs = Me.AutoAddFileInputs;

                
                ctrl.DisableChunkUpload = Me.DisableChunkUpload;

                

                ctrl.DisablePlugins = Me.DisablePlugins;

                if (!String.IsNullOrEmpty(Me.DropZones))
                {
                    ctrl.DropZones = Me.DropZones.Replace(" ", "").Split(',');
                }

                ctrl.EnableInlineProgress = Me.EnableInlineProgress;

                if (Me.FileFilters.Count > 0)
                {
                    foreach(CodeTorch.Core.FileFilter f in Me.FileFilters)
                    {
                        if (!String.IsNullOrEmpty(f.Extensions))
                        {
                            Telerik.Web.UI.FileFilter filter = new Telerik.Web.UI.FileFilter();
                            filter.Description = f.Description;
                            filter.Extensions = f.Extensions.Replace(" ", "").Split(',');

                            ctrl.FileFilters.Add(filter);
                        }
                    }
                }

                ctrl.HideFileInput = Me.HideFileInput;

                if (!String.IsNullOrEmpty(Me.HttpHandlerUrl))
                {
                    ctrl.HttpHandlerUrl = Me.HttpHandlerUrl;
                }

                ctrl.InitialFileInputsCount = Me.InitialFileInputsCount;
                ctrl.InputSize = Me.InputSize;
                ctrl.MaxFileInputsCount = Me.MaxFileInputsCount;
                ctrl.MaxFileSize = Me.MaxFileSize;
                ctrl.MultipleFileSelection = (Telerik.Web.UI.AsyncUpload.MultipleFileSelection)Enum.Parse(typeof(Telerik.Web.UI.AsyncUpload.MultipleFileSelection), Me.MultipleFileSelection.ToString());
                
                ctrl.OnClientAdded = Me.OnClientAdded;
                ctrl.OnClientFileDropped = Me.OnClientFileDropped;
                ctrl.OnClientFileSelected = Me.OnClientFileSelected;
                ctrl.OnClientFilesSelected = Me.OnClientFilesSelected;
                ctrl.OnClientFilesUploaded = Me.OnClientFilesUploaded;
                ctrl.OnClientFileUploaded = Me.OnClientFileUploaded;
                ctrl.OnClientFileUploadFailed = Me.OnClientFileUploadFailed;
                ctrl.OnClientFileUploading = Me.OnClientFileUploading;
                ctrl.OnClientFileUploadRemoved = Me.OnClientFileUploadRemoved;
                ctrl.OnClientFileUploadRemoving = Me.OnClientFileUploadRemoving;
                ctrl.OnClientProgressUpdating = Me.OnClientProgressUpdating;
                ctrl.OnClientValidationFailed = Me.OnClientValidationFailed;

                if (!String.IsNullOrEmpty(Me.PostbackTriggers))
                {
                    ctrl.PostbackTriggers = Me.PostbackTriggers.Replace(" ", "").Split(',');
                }
                

          

                if (!String.IsNullOrEmpty(Me.TemporaryFolder))
                {
                    ctrl.TemporaryFolder = Me.TemporaryFolder;
                }

                ctrl.TemporaryFileExpiration = TimeSpan.FromMinutes(Me.TemporaryFileExpirationMinutes);

        

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass = Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                if (!String.IsNullOrEmpty(Me.Skin))
                {
                    ctrl.Skin = Me.Skin;
                }

                ctrl.FileUploaded += new FileUploadedEventHandler(ctrl_FileUploaded);

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                System.Web.UI.WebControls.Label errorLabel = new System.Web.UI.WebControls.Label();
                errorLabel.Text = ErrorMessages;
                errorLabel.ForeColor = Color.Red;
                this.Controls.Add(errorLabel);

            }


        }

        void ctrl_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {

                //validate file
                e.IsValid = true;

                Document document = ObjectCopier.Clone<Document>(Me.Document);
                
                document.FileName = e.File.FileName;
                document.ContentType = e.File.ContentType;
                document.Size = Convert.ToInt32(e.File.ContentLength);
                document.Stream = e.File.InputStream;
                document.EntityID = ((BasePage)this.Page).GetEntityIDValue(this.Screen, document.EntityID, Me.EntityInputType);
                

                
                
                DocumentService documentService = DocumentService.GetInstance();
                DocumentRepository repo = DocumentRepository.GetByName(Me.DocumentRepository);

                IDocumentProvider documentProvider = documentService.GetProvider(repo);
                document.ID = documentProvider.Upload(document);




                AppendDocumentID(document.ID);

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                System.Web.UI.WebControls.Label errorLabel = new System.Web.UI.WebControls.Label();
                errorLabel.Text = ErrorMessages;
                errorLabel.ForeColor = Color.Red;
                this.Controls.Add(errorLabel);

                ((BasePage)this.Page).DisplayErrorAlert(ex);

            }
        }

        
        

       


        

    }
}
