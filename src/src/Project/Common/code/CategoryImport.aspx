<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryImport.aspx.cs" Inherits="Neamb.Project.Common.CategoryImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="formUpload" method="post" enctype="multipart/form-data" runat="server">
        <INPUT type=file id="fileUpload" name="fileUpload" runat="server" /><br />
        <br><asp:button runat="server" text="Upload" ID="buttonUpload" OnClick="buttonUpload_Click" />
        <br />
        <br />
        <asp:Label ID="lblResultProcess" runat="server"></asp:Label>
        <br />
        <br />
        <br />
    </form>
</body>
</html>
