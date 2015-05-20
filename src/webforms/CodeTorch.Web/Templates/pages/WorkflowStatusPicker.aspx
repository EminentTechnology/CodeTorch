<%@ Page Language="C#" AutoEventWireup="true"  Inherits="CodeTorch.Web.Special.WorkflowStatusPicker"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Change Status</title>
    <script src="https://code.jquery.com/jquery.js"></script>
    <link  type="text/css" href="http://d2ml0zxrbmwn5n.cloudfront.net/assets/standard/css/style.css" rel="stylesheet" /> 
</head>
<body> 
    <form id="form1" runat="server">
    <asp:Literal ID="Javascript" runat="server"></asp:Literal>
    <script type="text/javascript">
       function querySt(ji) {
            hu = window.location.search.substring(1);
            gy = hu.split("&");
            for (i = 0; i < gy.length; i++) {
                ft = gy[i].split("=");
                if (ft[0] == ji)
                { return ft[1]; }
            }
        }
        function CloseWindowWithStatusUpdate(status, reload) {
            var ctrl = window.parent.document.getElementById(<asp:Literal ID="FieldName" runat="server"></asp:Literal>);
            ctrl.innerText = status;

            if(reload=='True')
            {
                 self.parent.location.reload();
            }

            parent.$.fancybox.close();
        }

        function CloseWindowCancel() {
            parent.$.fancybox.close();
        }
       
        
        
    </script>
     <div class="panel panel-primary">
      <div class="panel-heading">
        <h3 class="panel-title">Change Status</h3>
      </div>
      <div class="panel-body">

      


    <table cellspacing="1" cellpadding="1" width="95%" align="center" border="0">
     <tr>
		<td colspan=2><input id="returnvalue" type="hidden" name="returnvalue" runat="server"/>
		    <input id="returnname" type="hidden" name="returnname" runat="server"/>
            <asp:Label ID="Message" runat="server" Text=""  ></asp:Label>
		</td>
	</tr>
        <tr>
            <td width="100">
                Current Status:
            </td>
            <td>
                <asp:Label ID="CurrentStatusLabel" runat="server" Text="{Label}"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Next Status:
            </td>
            <td>
                <asp:DropDownList ID="StatusList" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">Comments:</td>
        </tr>
        <tr>
            <td colspan="2">
            <asp:TextBox ID="Comments" TextMode="MultiLine" runat="server" Width="100%" Height="70px" /></td>
        </tr>
    </table>
    <br />
    <br />
    <table cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
        <tr>
            <td align="center">
                &nbsp;
                <asp:Button ID="Save" Visible="True" runat="server" Text="Change Status"  CssClass="FormButton" />
                &nbsp;&nbsp; &nbsp;
                <input type=button value="Cancel" class="FormButton" onclick="CloseWindowCancel()" />
            </td>
        </tr>
    </table>
    </div>
    </div>
        <script type="text/javascript" language="javascript">
            $("#StatusList").change(function() {
               $("#Save").val($("#StatusList option:selected").text());
            });
        </script>
    </form>
</body>
</html>
