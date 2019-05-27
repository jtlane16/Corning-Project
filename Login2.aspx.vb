'Empty Indices Software Engineering 2/3
'Chandler Mitchell, Jackson Lane, Matthew Phlegar, Chandler Hiatt, Chris Gibson
'Created: 11/26/18

Imports System.Data.SqlClient
Imports System.Drawing

'Class handles events for Login2.aspx
Public Class Login2
    Inherits System.Web.UI.Page

    'Event Happens when page loads
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set error label message to empty
        ErrorMes.Text = ""
    End Sub

    'Events occur when the Login button is clicked
    Protected Sub Submit_Click(sender As Object, e As EventArgs)
        Dim FName As String 'String user's first name
        Dim LName As String 'String user's last name
        Dim Uname As String = "" 'String user's username
        Dim checkUname As String = "" 'String to check if username exists
        Dim bUser As String = Username.Text.ToString 'String with content from username textbox
        Dim bPass As String = Password.Text.ToString 'String with content from password textbox
        Dim AccessLevel As Int32 'Int32 user's AccessLevel

        Dim connObj As New SqlClient.SqlConnection("Server=" + System.Net.Dns.GetHostName + "\SQLEXPRESS; database=Users; Trusted_Connection=True") 'Connection string to server with the Users database
        Dim cmdObj As New SqlClient.SqlCommand("dbo.sp_LoginTest", connObj) 'LoginTest Stored Procedure
        Dim checkObj As New SqlClient.SqlCommand("dbo.sp_CheckUser", connObj) 'Query to check for existing user based on the given username

        cmdObj.CommandType = CommandType.StoredProcedure
        cmdObj.Parameters.AddWithValue("Username", bUser) '@Username Parameter for LoginTest
        cmdObj.Parameters.AddWithValue("Password", bPass) '@Password Parameter for LoginTest

        checkObj.CommandType = CommandType.StoredProcedure
        checkObj.Parameters.AddWithValue("Username", bUser) '@Username Parameter for CheckUser
        connObj.Open()

        'Assign values to variables given row result from stored procedure LoginTest
        Using readerObj As SqlClient.SqlDataReader = cmdObj.ExecuteReader
            While readerObj.Read
                FName = readerObj("Fname").ToString
                LName = readerObj("Lname").ToString
                Uname = readerObj("Username").ToString
                AccessLevel = readerObj("AccessLvl")
            End While
        End Using

        'Assign values to variables given row result from checkObj query
        Using readerObj2 As SqlClient.SqlDataReader = checkObj.ExecuteReader
            While readerObj2.Read
                checkUname = readerObj2("Username").ToString
            End While
        End Using

        connObj.Close()

        'If the username given exists
        If checkUname <> vbNullString Then

            'If the stored procedure returned a row, assign session values and redirect to landing page
            If Uname <> vbNullString And bUser = Uname Then
                Session("Username") = bUser
                Session("AccessLvl") = AccessLevel
                Response.Redirect("LandingPage.aspx")

            ElseIf Uname <> vbNullString And bUser <> Uname Then
                ErrorMes.ForeColor = Color.Red
                ErrorMes.Text = "Check Capitalization"
            Else
                ErrorMes.ForeColor = Color.Red
                ErrorMes.Text = "Password is Incorrect"
            End If

        Else
            ErrorMes.ForeColor = Color.Red
            ErrorMes.Text = "User does not exist"
        End If

        'If the username textbox was left unfilled
        If bUser = vbNullString Then
            ErrorMes.ForeColor = Color.Red
            ErrorMes.Text = "Please fill out Username and Password"
        End If
    End Sub

    Protected Sub Badge_Click(sender As Object, e As EventArgs)
        Response.Redirect("BadgeLogin.aspx")
    End Sub
End Class