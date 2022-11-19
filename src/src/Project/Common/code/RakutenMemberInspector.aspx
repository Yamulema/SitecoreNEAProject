<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RakutenMemberInspector.aspx.cs" Inherits="Neamb.Project.Common.RakutenMemberInspector" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rakuten Members Report</title>
</head>
<body>
<form enctype="multipart/form-data" runat="server">
    <asp:button runat="server" text="Button" OnClick="Process_Click" ID="Button1" />
    <asp:button runat="server" text="One Page Report" OnClick="Process_Click_OnePage" ID="Process" />
</form>
</body>
</html>
