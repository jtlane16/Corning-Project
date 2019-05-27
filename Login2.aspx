<%--Empty Indices Software Engineering 2/3
    Chandler Mitchell, Jackson Lane, Matthew Phlegar, Chandler Hiatt, Chris Gibson
    Created 11/09/18--%>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login2.aspx.vb" Inherits="WebApplication1.Login2" %>

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
    <form id="form1" runat="server" method="post" autocomplete="off">
        <%--Login form content--%>
              <div class="content">
                  <h2>Employee Login</h2>
                  <asp:TextBox ID="Username" placeholder="Username" type="text" runat="server" AutoCompleteType="Disabled"></asp:TextBox><br /> 
                  <asp:TextBox ID="Password" placeholder="Password" type="password" runat="server" AutoCompleteType="Disabled"></asp:TextBox> 
                  <asp:Button ID="Submit" runat="server" Text="Login" buttontype="submit" OnClick="Submit_Click" Class="btn"/>
                  <asp:Button ID="Badge" runat="server" Text="Use Badge ID" buttontype="button" OnClick="Badge_Click" Class="btn"/>
                  <%--<asp:CheckBox ID="RememberMe" Text="Remember Me" runat="server" CssClass="checkbox" />--%>
                  <asp:Label ID="ErrorMes" runat="server" CssClass="ErrorMes"></asp:Label>
              </div>
    </form>
</body>
    
</html>
