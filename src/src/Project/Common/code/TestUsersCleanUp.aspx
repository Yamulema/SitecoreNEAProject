<%@ page language="C#" autoeventwireup="true" codebehind="TestUsersCleanUp.aspx.cs" inherits="Neamb.Project.Common.RemoveTestUsers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remove Test Users</title>
</head>
<body>
    <form id="form1" action="/api/Misc/DeleteAllTestUsers" method="post" enctype='application/json'>
        Click on the button below to remove the test users.<br />
        <br />
        <input type="submit" value="Remove All Test Users" />
    </form>
</body>
</html>
