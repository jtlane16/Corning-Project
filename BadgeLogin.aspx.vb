Imports System.Data.SqlClient
Imports System.Drawing

Public Class BadgeLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set error label message to empty
        ErrorMes.Text = ""
    End Sub

    Protected Sub Submit_Click(sender As Object, e As EventArgs)
        Dim FName As String 'String user's first name
        Dim LName As String 'String user's last name
        Dim Uname As String = "" 'String user's username
        Dim checkBadge As String = "" 'String to check if username exists
        Dim BadgeID As String = Badge_ID.Text.ToString 'String with content from username textbox
        Dim AccessLevel As Int32 'Int32 user's AccessLevel


        Dim connObj As New SqlClient.SqlConnection("Server=" + System.Net.Dns.GetHostName + "\SQLEXPRESS; database=Users; Trusted_Connection=True") 'Connection string to server with the Users database
        Dim cmdObj As New SqlClient.SqlCommand("dbo.sp_LoginBadge", connObj) 'LoginBadge Stored Procedure

        cmdObj.CommandType = CommandType.StoredProcedure
        cmdObj.Parameters.AddWithValue("badgeID", BadgeID) '@Username Parameter for LoginTest

        connObj.Open()

        'Assign values to variables given row result from stored procedure LoginTest
        Using readerObj As SqlClient.SqlDataReader = cmdObj.ExecuteReader
            While readerObj.Read
                FName = readerObj("Fname").ToString
                LName = readerObj("Lname").ToString
                Uname = readerObj("Username").ToString
                checkBadge = readerObj("Badge").ToString
                AccessLevel = readerObj("AccessLvl")
            End While
        End Using

        connObj.Close()

        'If the stored procedure returned a row, assign session values and redirect to landing page
        If Uname <> vbNullString And BadgeID = checkBadge Then
            Session("Username") = Uname
            Session("AccessLvl") = AccessLevel
            Response.Redirect("LandingPage.aspx")

        ElseIf Uname <> vbNullString And BadgeID <> checkBadge Then
            ErrorMes.ForeColor = Color.Red
            ErrorMes.Text = "Check Capitalization"
        Else
            ErrorMes.ForeColor = Color.Red
            ErrorMes.Text = "BadgeID is Incorrect"
        End If

        'If the username textbox was left unfilled
        If BadgeID = vbNullString Then
            ErrorMes.ForeColor = Color.Red
            ErrorMes.Text = "Please fill out BadgeID"
        End If
    End Sub

    Protected Sub Uname_Click(sender As Object, e As EventArgs)
        Response.Redirect("Login2.aspx")
    End Sub
End Class