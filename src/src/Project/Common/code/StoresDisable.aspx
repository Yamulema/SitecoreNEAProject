<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoresDisable.aspx.cs" Inherits="Neamb.Project.Common.StoresDisable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stores Processor</title>
</head>
<body>
    <h1>Stores Processor</h1>
    <form id="form1" runat="server">
        <div>
            <asp:button runat="server" text="Search Stores" OnClick="SearchStores" ID="Process" />
            <br/>
        </div>
        <div>
            <asp:button runat="server" text="Disable Stores" OnClick="DisableStores" ID="Button1" />
        </div>
    </form>
</body>
</html>
