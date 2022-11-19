<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManagement.aspx.cs" Inherits="Neambc.Seiumb.Project.Seiu.integ.AccountManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h1>Integration Test SEIUMB</h1>
    <div>
        Methods to test
    <ul>
        <li>IsUsernameAvailable </li>
        <li>ValidateUsernameAndPassword </li>

        <li>RegisterUser </li>
        <li>RetrieveUserData </li>
        <li>UpdateUser </li>
        <li>FindUserIdByUserData </li>
        <li>RequestResetToken </li>
        <li>ValidateResetToken </li>

        <li>ResetPassword </li>
        <li>CancelResetToken </li>

        <li>UpdateUsername </li>
        <li>UpdatePassword </li>
        <li>AuthenticatePassword </li>
        <li>DeleteRegistration </li>


    </ul>
    </div>
    <form id="isUsernameAvailableForm" runat="server">
        <div>
            <h2>IsUsernameAvailable</h2>
            <span>Username:
                <input type="text" runat="server" id="tbUsername" /></span>
            <span>
                <input type="button" runat="server" onserverclick="TestIsUsernameAvailable" value="Submit" /></span>
            <br />
            Response:
            <br />
            <textarea runat="server" id="IsUsernameAvailableResponse" cols="25" rows="5"></textarea>
        </div>

        <div>
            <h2>ValidateUsernameAndPassword</h2>
            <span>Username:
                <input type="text" runat="server" id="tbUsernameVUP" /></span>
            <span>Password:
                <input type="text" runat="server" id="tbPasswordVUP" /></span>
            <span>CellCode:
                <input type="text" runat="server" id="tbCellCodeVUP" /></span>
            <span>
                <input type="button" runat="server" onserverclick="TestValidateUsernameAndPassword" value="Submit" /></span>
            <br />
            Response:
            <br />
            <textarea runat="server" id="validateUsernameAndPasswordResponse" cols="25" rows="5"></textarea>
        </div>
        <div>

            <%--string firstName, string lastName, string streetAddress, string city, string stateCode,
            string zipCode, string dob, string phone, string username, string password, string permissionIndicator, string webUserSource,
            string unionId, string campcode, string cellCode--%>

            <h2>RegisterUser</h2>
            <span>firstName:
                <input type="text" runat="server" id="tbfirstNameRG" /></span>
            <span>lastName:
                <input type="text" runat="server" id="tblastNameRG" /></span>
            <span>streetAddress:
                <input type="text" runat="server" id="tbstreetAddressRG" /></span>
            <span>city:
            <input type="text" runat="server" id="tbcityRG" /></span>
            <span>stateCode:
                <input type="text" runat="server" id="tbstateCodeRG" /></span>
            <span>zipCode:
                <input type="text" runat="server" id="tbzipCodeRG" /></span>
            <span>dob:
                <input type="text" runat="server" id="tbdobRG" /></span>
            <span>phone:
                <input type="text" runat="server" id="tbphoneRG" /></span>
            

            <span>Username:
                <input type="text" runat="server" id="tbUsernameRG" /></span>
            <span>Password:
                <input type="text" runat="server" id="tbPasswordRG" /></span>
            <span>permissionIndicator:
                <input type="text" runat="server" id="tbpermissionIndicatorRG" /></span>
            <span>webUserSource:
                <input type="text" runat="server" id="tbwebUserSourceRG" /></span>
            <span>unionId:
                <input type="text" runat="server" id="tbunionIdRG" /></span>
            <span>campcode:
                <input type="text" runat="server" id="tbcampcodeRG" /></span>
            <span>CellCode:
                <input type="text" runat="server" id="tbcellCodeRG" /></span>
            <span>
                <input type="button" runat="server" onserverclick="TestRegisterUser" value="Submit" /></span>
            <br />
            Response:
            <br />
            <textarea runat="server" id="validateRegisterUserResponse" cols="25" rows="5"></textarea>
        </div>
    </form>
</body>
</html>
