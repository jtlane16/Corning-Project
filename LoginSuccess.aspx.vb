'Empty Indices Software Engineering 2/3
'Chandler Mitchell, Jackson Lane, Matthew Phlegar, Chandler Hiatt, Chris Gibson
'Created: 11/26/18


'Class handles events for LoginSuccess.aspx
Public Class LoginSuccess
    Inherits System.Web.UI.Page

    'Event Happens when page loads
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim user As String = Session("Username") 'Username from session variable
        Dim access As Int32 = Session("AccessLvl") 'AccessLevel from session variable

        'Display variable values
        Label1.Text = "Username = " & user
        Label2.Text = "User access level = " & access

        'If there is no session value for username, then redirect to login page
        If user = vbNullString Then
            Response.Redirect("Login2.aspx")
        End If
    End Sub

    'Events occur when the Logout button is clicked
    Protected Sub Logout_Click(sender As Object, e As EventArgs)
        'Clear all session variables and redirect to login page
        Session.Clear()
        Response.Redirect("Login2.aspx")
    End Sub
End Class