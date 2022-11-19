<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemoveVisited.aspx.cs" Inherits="Neamb.Project.Common.RemoveVisited" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" action="/api/Misc/RemoveVisited" method="post" enctype='application/json'>
        <div>Select Page Type:</div>
        <div>
            <select name="PageType">
                <option value="1">Profile</option>
                <option value="2">Subscription</option>
                <option value="3">Complife</option>
            </select>
            <input type="submit" value="Delete All Keys"/>
        </div>
    </form>
</body>
</html>
