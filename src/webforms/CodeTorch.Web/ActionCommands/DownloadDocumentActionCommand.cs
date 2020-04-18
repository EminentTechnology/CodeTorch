using CodeTorch.Abstractions;
using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class DownloadDocumentActionCommand : IActionCommandStrategy
    {
        public Templates.BasePage Page { get; set; }

  

        public ActionCommand Command { get; set; }

        DownloadDocumentCommand Me = null;
        string DocumentID = null;


        public bool ExecuteCommand()
        {
            bool success = true;
            ILog log = Resolver.Resolve<ILogManager>().GetLogger(this.GetType());

            try
            {
                if (Command != null)
                {
                    Me = (DownloadDocumentCommand)Command;
                }

               
                DownloadDocument();

            }
            catch (Exception ex)
            {
                success = false;

                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }

            return success;

        }

        private void DownloadDocument()
        {
            ILog log = Resolver.Resolve<ILogManager>().GetLogger(this.GetType());

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

                        //Force file download with content disposition
                        if (Me.ForceDownloadWithContentDisposition)
                        {
                            Page.Response.AddHeader("Content-Disposition", "attachment;filename=" + dt.Rows[0]["DocumentName"].ToString());
                        }
                       
                        if (String.IsNullOrEmpty(DocumentUrl))
                        {
                            data = (byte[])dt.Rows[0]["File"];

                            Page.Response.OutputStream.Write(data, 0, Convert.ToInt32(dt.Rows[0]["Size"]));

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
                            int bytesToRead = 10000;
                            int length;
                            data = new Byte[bytesToRead];
                            do
                            {
                                // Verify that the client is connected.
                                if (Page.Response.IsClientConnected)
                                {
                                    // Read data into the buffer.
                                    length = stream.Read(data, 0, bytesToRead);

                                    // and write it out to the response's output stream
                                    Page.Response.OutputStream.Write(data, 0, length);

                                    // Flush the data
                                    Page.Response.Flush();

                                    //Clear the buffer
                                    data = new Byte[bytesToRead];
                                }
                                else
                                {
                                    // cancel the download if client has disconnected
                                    length = -1;
                                }
                            } while (length > 0); //Repeat until no data is read

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

        private void DownloadDocumentFromUrl(string url)
        {
            ILog log = Resolver.Resolve<ILogManager>().GetLogger(this.GetType());

            try
            {
                Page.Response.Clear();
            }
            catch (Exception ex)
            {
                Page.DisplayErrorAlert(ex);

                log.Error(ex);
            }


        }



        
    }
}
