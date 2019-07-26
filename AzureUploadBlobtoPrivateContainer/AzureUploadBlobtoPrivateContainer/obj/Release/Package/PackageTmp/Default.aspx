<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AzureUploadBlobtoPrivateContainer.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    上传图片并下载
     <br />
    <asp:FileUpload ID="FileUpload1" runat="server" />
    &nbsp;&nbsp;&nbsp;
        <asp:Button ID="BtnUpload" runat="server" Text="上传" OnClick="BtnUpload_Click" />
        <br><asp:Label ID="Lblstatus" runat="server"></asp:Label></br>
        <p>
            下载链接
            <asp:TextBox ID="txbUrl" runat="server" Width="558px" ReadOnly="True"></asp:TextBox>
        </p>
    </div>
        <p></p>
        <p></p>
        直接下载：请在下面的输入框，输入要下载的文件名(需要扩展名)
        <br />
        <asp:textbox ID="txbBlobName" runat="server"></asp:textbox>
        <asp:Button ID="btnDownload"  runat="server" OnClick="Button1_Click" Text="下载" />
    </form>
</body>

</html>
