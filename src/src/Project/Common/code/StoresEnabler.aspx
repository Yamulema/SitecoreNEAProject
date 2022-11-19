<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoresEnabler.aspx.cs" Inherits="Neamb.Project.Common.StoresEnabler" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stores Enabler</title>
</head>
<body>
    <h1>Stores Enabler</h1>
    <form id="form1" runat="server">
        <div>
            <asp:button runat="server" text="Get report with stores status" OnClick="GetStoresStatus" ID="Button1" />
            <asp:button runat="server" text="Enable Stores" OnClick="EnableStores" ID="Process" />
        </div>
    </form>
</body>
</html>
