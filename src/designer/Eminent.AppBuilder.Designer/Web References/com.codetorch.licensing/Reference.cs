﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace CodeTorch.Designer.com.codetorch.licensing {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="LicenseServiceSoap", Namespace="http://licensing.codetorch.com/")]
    public partial class LicenseService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetAvailableLicensesOperationCompleted;
        
        private System.Threading.SendOrPostCallback ActivateLicenseOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public LicenseService() {
            this.Url = "http://appbuilder.eminenttechnology.com/service/licenseservice.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetAvailableLicensesCompletedEventHandler GetAvailableLicensesCompleted;
        
        /// <remarks/>
        public event ActivateLicenseCompletedEventHandler ActivateLicenseCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://licensing.codetorch.com/GetAvailableLicenses", RequestNamespace="http://licensing.codetorch.com/", ResponseNamespace="http://licensing.codetorch.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet GetAvailableLicenses(string UserName, string Password) {
            object[] results = this.Invoke("GetAvailableLicenses", new object[] {
                        UserName,
                        Password});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void GetAvailableLicensesAsync(string UserName, string Password) {
            this.GetAvailableLicensesAsync(UserName, Password, null);
        }
        
        /// <remarks/>
        public void GetAvailableLicensesAsync(string UserName, string Password, object userState) {
            if ((this.GetAvailableLicensesOperationCompleted == null)) {
                this.GetAvailableLicensesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAvailableLicensesOperationCompleted);
            }
            this.InvokeAsync("GetAvailableLicenses", new object[] {
                        UserName,
                        Password}, this.GetAvailableLicensesOperationCompleted, userState);
        }
        
        private void OnGetAvailableLicensesOperationCompleted(object arg) {
            if ((this.GetAvailableLicensesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAvailableLicensesCompleted(this, new GetAvailableLicensesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://licensing.codetorch.com/ActivateLicense", RequestNamespace="http://licensing.codetorch.com/", ResponseNamespace="http://licensing.codetorch.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode ActivateLicense(string UserName, string LicenseID, string MID, string MachineName) {
            object[] results = this.Invoke("ActivateLicense", new object[] {
                        UserName,
                        LicenseID,
                        MID,
                        MachineName});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public void ActivateLicenseAsync(string UserName, string LicenseID, string MID, string MachineName) {
            this.ActivateLicenseAsync(UserName, LicenseID, MID, MachineName, null);
        }
        
        /// <remarks/>
        public void ActivateLicenseAsync(string UserName, string LicenseID, string MID, string MachineName, object userState) {
            if ((this.ActivateLicenseOperationCompleted == null)) {
                this.ActivateLicenseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnActivateLicenseOperationCompleted);
            }
            this.InvokeAsync("ActivateLicense", new object[] {
                        UserName,
                        LicenseID,
                        MID,
                        MachineName}, this.ActivateLicenseOperationCompleted, userState);
        }
        
        private void OnActivateLicenseOperationCompleted(object arg) {
            if ((this.ActivateLicenseCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ActivateLicenseCompleted(this, new ActivateLicenseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void GetAvailableLicensesCompletedEventHandler(object sender, GetAvailableLicensesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAvailableLicensesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetAvailableLicensesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void ActivateLicenseCompletedEventHandler(object sender, ActivateLicenseCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ActivateLicenseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ActivateLicenseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlNode Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlNode)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591