<%--Empty Indices Software Engineering 2/3
    Chandler Mitchell, Jackson Lane, Matthew Phlegar, Chandler Hiatt, Chris Gibson
    Created 11/26/18--%>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginSuccess.aspx.vb" Inherits="WebApplication1.LoginSuccess" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <br />
            <br />
            <br />
            <br />
        </div>
        <div style="margin-left: 50%">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        </div>
        <asp:Button ID="Logout" runat="server" Text="Logout" OnClick="Logout_Click"/>
    </form>
</body>
</html>
