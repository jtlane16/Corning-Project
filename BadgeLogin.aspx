<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BadgeLogin.aspx.vb" Inherits="WebApplication1.BadgeLogin" %>

<!DOCTYPE html>


<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title>Login</title> 
    <link href="LoginPageCSS.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
    </style>
</head>
<body>
    <p>
    <img src="Stock/Corning.jpg" class="logo"/></p>
    <form id="form1" runat="server" method="post">
        <%--Login form content--%>
              <div class="content">
                  <h2>Employee Login</h2>
                  <asp:TextBox ID="Badge_ID" placeholder="Badge ID" type="password" runat="server"></asp:TextBox> 
                  <asp:Button ID="Submit" runat="server" Text="Login" buttontype="submit" OnClick="Submit_Click" Class="btn"/>
                  <asp:Button ID="Username_Password" runat="server" Text="Use Username/Password" Class="Badgebutton" OnClick="Uname_Click"  />
                  <asp:Label ID="ErrorMes" runat="server" CssClass="ErrorMes"></asp:Label>
              </div>
    </form>
</body>
    
</html>