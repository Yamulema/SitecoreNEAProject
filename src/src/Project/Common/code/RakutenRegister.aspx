<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RakutenRegister.aspx.cs" Inherits="Neamb.Project.Common.RakutenRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="formUpload" method="post" enctype="multipart/form-data" runat="server">
        <br><asp:button runat="server" text="Register" ID="buttonRegister" OnClick="buttonRegister_Click" />
        <br />
        <br />
        Is Rakuten Member:<asp:Label ID="IsRakutenMember" runat="server"></asp:Label>
        <br />
        Id:<asp:Label ID="Id" runat="server"></asp:Label>
        <br />
        EbToken:<asp:Label ID="EBToken" runat="server"></asp:Label>
        <br />
        Creation date:<asp:Label ID="CreationDate" runat="server"></asp:Label>
        <br />
        Email address:<asp:Label ID="Emailaddress" runat="server"></asp:Label>
        <br />
        <br />
        <br />
    </form>
</body>
</html>
