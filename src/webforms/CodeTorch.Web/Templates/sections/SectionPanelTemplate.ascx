<%@ Control Language="C#" AutoEventWireup="true" Inherits="CodeTorch.Web.UserControls.SectionPanelTemplate" %>
<div class="section panel panel-default">
    <div class="panel-heading">
        <h3 class="panel-title">
            <asp:Literal ID="HeadingTitleLiteral" runat="server" />
        </h3>
    </div>
    <div class="panel-body">
        <h5 Visible="<%#!String.IsNullOrEmpty(IntroText) %>" runat="server"><asp:Literal ID="IntroTextLiteral" runat="server" /></h5>
        <asp:PlaceHolder ID="Body" runat="server" />
    </div>
</div>




