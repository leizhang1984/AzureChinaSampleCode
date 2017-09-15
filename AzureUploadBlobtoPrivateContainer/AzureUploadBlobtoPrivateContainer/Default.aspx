<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AzureUploadBlobtoPrivateContainer.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:FileUpload ID="FileUpload1" runat="server" />
    &nbsp;&nbsp;&nbsp;
        <asp:Button ID="BtnUpload" runat="server" Text="上传" OnClick="BtnUpload_Click" />
        <br><asp:Label ID="Lblstatus" runat="server"></asp:Label></br>
        <p>
            下载链接
            <asp:TextBox ID="txbUrl" runat="server" Width="558px" ReadOnly="True"></asp:TextBox>
        </p>
    </div>
    </form>
</body>
</html>
