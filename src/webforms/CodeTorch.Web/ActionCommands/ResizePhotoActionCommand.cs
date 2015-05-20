using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.ActionCommands
{
    public class ResizePhotoActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        ResizePhotoCommand Me = null;
        string DocumentID = null;

        string MaxHeightValue = null;
        string MaxWidthValue = null;

        int MaxHeight = 0;
        int MaxWidth = 0;


        public void ExecuteCommand()
        {
            

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (Command != null)
                {
                    Me = (ResizePhotoCommand)Command;
                }

                MaxWidthValue = Page.GetEntityIDValue(Page.Screen, Me.MaxWidth, Me.MaxWidthInputType);
                MaxHeightValue = Page.GetEntityIDValue(Page.Screen, Me.MaxHeight, Me.MaxHeightInputType);

                int.TryParse(MaxWidthValue, out MaxWidth);
                int.TryParse(MaxHeightValue, out MaxHeight);
                
               
                DownloadResizedDocument();

            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
            
        }

        private void DownloadResizedDocument()
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {

                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                if (!String.IsNullOrEmpty(Me.RetrieveCommand))
                {
                    List<ScreenDataCommandParameter> parameters = null;
                    parameters = pageDB.GetPopulatedCommandParameters(Me.RetrieveCommand, Page);
                    DataTable dt = dataCommandDB.GetDataForDataCommand(Me.RetrieveCommand, parameters);

                    if (dt.Rows.Count != 0)
                    {
                        byte[] data = null;
                        string DocumentUrl = dt.Rows[0]["DocumentUrl"].ToString();

                        Page.Response.Clear();
                        Page.Response.ContentType = dt.Rows[0]["ContentType"].ToString();
                        Page.Response.AddHeader("Content-Disposition", "attachment;filename=" + dt.Rows[0]["DocumentName"].ToString());
                       
                        if (String.IsNullOrEmpty(DocumentUrl))
                        {
                            data = (byte[])dt.Rows[0]["File"];

                            data = ScaleImageByteArray(data, MaxWidth, MaxHeight, GetImageFormatFromContentType(Page.Response.ContentType));

                            Page.Response.OutputStream.Write(data, 0, data.Length);

                            // Flush the data
                            Page.Response.Flush();
                        }
                        else
                        { 
                            //redirect to the document url location
                            UriBuilder remoteDocument = new UriBuilder(DocumentUrl);

                            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(remoteDocument.Uri.AbsoluteUri);
                            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

                            if (fileReq.ContentLength > 0)
                                fileResp.ContentLength = fileReq.ContentLength;


                            Stream stream = fileResp.GetResponseStream();
                            
                            int length = stream.Read(data, 0, Convert.ToInt32(fileResp.ContentLength));
                            data = ScaleImageByteArray(data, MaxWidth, MaxHeight, GetImageFormatFromContentType(Page.Response.ContentType));
                            Page.Response.OutputStream.Write(data, 0, data.Length);

                            // Flush the data
                            Page.Response.Flush();

                        }



                        HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }
                    else
                    {
                        throw new Exception("File Not Found");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }

            
        }


        public System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new System.Drawing.Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public System.Drawing.Image ConvertByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        public byte[] ConvertImageToByteArray(System.Drawing.Image imageIn, System.Drawing.Imaging.ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, format);
            return ms.ToArray();
        }

        public byte[] ConvertImageToByteArray(System.Drawing.Image imageIn)
        {
            return ConvertImageToByteArray(imageIn, System.Drawing.Imaging.ImageFormat.Png);
        }

        public byte[] ScaleImageByteArray(byte[] origImageData, int maxWidth, int maxHeight, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] newImageData = null;

            try
            {
                if ((maxWidth > 0) && (maxHeight > 0))
                {
                    using (System.Drawing.Image origImage = ConvertByteArrayToImage(origImageData))
                    {
                        //take orig image and convert to image object

                        //resize image object
                        System.Drawing.Image newImage = ScaleImage(origImage, maxWidth, maxHeight);

                        //convert resized image back to byte array
                        newImageData = ConvertImageToByteArray(newImage, format);
                    }
                }
            }
            catch (Exception ex)
            {
                newImageData = origImageData;
            }

            if (newImageData == null)
                newImageData = origImageData;

            return newImageData;
        }

        public byte[] ScaleImageByteArray(byte[] origImageData, int maxWidth, int maxHeight)
        {
            return ScaleImageByteArray(origImageData, maxWidth, maxHeight, System.Drawing.Imaging.ImageFormat.Png);
        }

        public System.Drawing.Imaging.ImageFormat GetImageFormatFromContentType(string contentType)
        {
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;

            if (contentType.ToLower().Contains("jpg") || contentType.ToLower().Contains("jpeg"))
            {
                format = System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            else if (contentType.ToLower().Contains("png") )
            {
                format = System.Drawing.Imaging.ImageFormat.Png;
            }
            else if (contentType.ToLower().Contains("bmp"))
            {
                format = System.Drawing.Imaging.ImageFormat.Bmp;
            }
            else if (contentType.ToLower().Contains("gif"))
            {
                format = System.Drawing.Imaging.ImageFormat.Gif;
            }
            else if (contentType.ToLower().Contains("ico"))
            {
                format = System.Drawing.Imaging.ImageFormat.Icon;
            }
            else if (contentType.ToLower().Contains("tiff"))
            {
                format = System.Drawing.Imaging.ImageFormat.Tiff;
            }

            return format;

        }
        
    }
}
