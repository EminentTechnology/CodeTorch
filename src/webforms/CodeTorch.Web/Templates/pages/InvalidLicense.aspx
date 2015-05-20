<%@ Page Language="C#" AutoEventWireup="true" Inherits="CodeTorch.Web.Templates.InvalidLicense"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <p>You are seeing this page because your license is either invalid or has expired.</p>
    <p>Please contact your system administrator to rectify this.</p>
    <p>License Request Details:<br />
    ID: <asp:Label ID="LicenseID" runat="server" /><br />
    Machine: <asp:Label ID="ComputerName" runat="server" /><br />
    
    </p>
</asp:Content>
