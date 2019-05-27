Imports System.Data.SqlClient
Imports System.Linq

Public Class LandingPage
    Inherits System.Web.UI.Page
    Dim connString As String = "Server=" + System.Net.Dns.GetHostName + "\SQLEXPRESS; database=Saws; Trusted_Connection=True"
    Dim myConn As New SqlConnection(connString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("username") = vbNullString Then
            Response.Redirect("Login2.aspx")
        End If
        CurrentUser.Text = Session("username")
        If Session("Accesslvl") < 3 Then
            TabContainer2.Visible = False
        End If
    End Sub

    Protected Sub Page_Refresh(Sender As Object, e As EventArgs)
        ManufacturersList2.SelectedIndex = 0
        BladeDiameter2.Text = ""
        BladeGrit2.Text = ""
        BladeThickness2.Text = ""
        NewBladeContentDiv.Style.Add("display", "none")
        BladeSearchYes.Style.Add("display", "none")
        BladeSearchNo.Style.Add("display", "none")
        BladeSearchMsg.Text = ""
        BladesSerialNumber.Text = ""
        BladesSerialText.Text = ""
        BladeContentDiv.Style.Add("display", "none")
        BladeDiameter.Text = ""
        BladeGrit.Text = ""
        BladeThickness.Text = ""
        BladesEdit.Visible = True
        BladesEdit.Enabled = True
        BladesOutForRework.Visible = True
        BladesOutForRework.Enabled = True
        BladesScrapped.Visible = True
        BladesScrapped.Enabled = True
        BladesSubmit.Visible = False
        BladesCancel.Visible = False
    End Sub

    Protected Sub New_Blade(Sender As Object, e As EventArgs)
        BladeSearchMsg.Text = ""
        ManufacturersList2.SelectedIndex = 0
        BladeDiameter2.Text = ""
        BladeGrit2.Text = ""
        BladeThickness2.Text = ""
        NewBladeContentDiv.Style.Add("display", "inline-block")
        BladeSearchYes.Style.Add("display", "none")
        BladeSearchNo.Style.Add("display", "none")
        BladesSerialNumber.Text = BladesSerialText.Text
    End Sub

    Protected Sub BtnLogout_Click(Sender As Object, e As EventArgs)
        Session.Clear()
        Response.Redirect("Login2.aspx")
    End Sub

    Protected Sub Blades_Search(Sender As Object, e As EventArgs)
        NewBladeContentDiv.Style.Add("display", "none")
        hardRule.Style.Add("visibility", "visible")
        BladeSearchMsg.Text = ""

        Dim dt As New DataTable
        Dim strSql As String = "Select dbo.Manufacturers.ManufactureName From dbo.Manufacturers Order By Manufacturers.ManufactureName ASC"
        Using cnn As New SqlConnection(connString)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using
        ManufacturersList.Items.Clear()
        ManufacturersList.DataSourceID = Nothing
        ManufacturersList.DataSource = dt
        ManufacturersList.DataBind()


        Dim regex As Regex = New Regex("^[0-9]*$")
        Dim match As Match = regex.Match(BladesSerialText.Text)
        If match.Success And BladesSerialText.Text <> "" Then
            Dim SerialNumInput = BladesSerialText.Text
            Dim WhoScrappedBlade = ""
            Dim DateofBladeScrap = ""
            Dim ReasonBladeIsOFR = ""

            myConn.Open()
            Dim cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_GetCurrentBladesOut"
            cmd.Parameters.AddWithValue("SerialNumber", SerialNumInput)
            Using readerObj As SqlClient.SqlDataReader = cmd.ExecuteReader
                If readerObj.HasRows Then
                    While readerObj.Read
                        ReasonBladeIsOFR = readerObj("DefectDescription").ToString
                    End While
                End If
                readerObj.Close()
            End Using

            cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_GetBladeInfo"
            cmd.Parameters.AddWithValue("SerialNumber", SerialNumInput)

            Using readerObj As SqlClient.SqlDataReader = cmd.ExecuteReader

                If readerObj.HasRows Then
                    BladeSearchYes.Style.Add("display", "none")
                    BladeSearchNo.Style.Add("display", "none")
                    DefectList.Style.Add("display", "none")
                    DefLabel.Style.Add("display", "none")
                    SubmitNewDefect.Style.Add("display", "none")
                    SubmitNewDefect.Style.Add("float", "none")
                    ScrappedBy.Style.Add("display", "none")
                    DateScrapped.Style.Add("display", "none")
                    BladeScrappedBy.Style.Add("display", "none")
                    BladeScrapDate.Style.Add("display", "none")
                    DefectList.Enabled = False
                    ManufacturersList.Enabled = False
                    StatusList.Enabled = False
                    LocationList.Enabled = False
                    BladeDiameter.Enabled = False
                    BladeGrit.Enabled = False
                    BladeThickness.Enabled = False
                    SubmitNewMan.Visible = False
                    SubmitNewStat.Visible = False
                    BladesEdit.Enabled = True
                    BladesEdit.Visible = True
                    BladesSubmit.Visible = False
                    BladesCancel.Visible = False
                    BladesOFRSubmit.Visible = False
                    BladesOutForRework.Visible = True
                    BladesOutForRework.Enabled = True
                    BladesScrapped.Visible = True
                    BladesScrapped.Enabled = True
                    While readerObj.Read
                        BladeResultSerial.Text = SerialNumInput
                        BladeDiameter.Text = readerObj("BladeDiameter").ToString
                        BladeGrit.Text = readerObj("BladeGrit").ToString
                        BladeThickness.Text = readerObj("BladeThickness").ToString
                        BladeResultInsert.Text = readerObj("InsertBy").ToString
                        BladeResultDate.Text = readerObj("InsertTimeStamp").ToString
                        StatusList.Text = readerObj("StatusDesc").ToString
                        ManufacturersList.Text = readerObj("ManufactureName").ToString
                        LocationList.Text = readerObj("LocationDesc").ToString
                        WhoScrappedBlade = readerObj("ScrappedBy").ToString
                        DateofBladeScrap = readerObj("ScrappedTimeStamp").ToString
                    End While

                    If StatusList.SelectedValue = "Out for rework" Or StatusList.SelectedValue = "Scrapped" Then
                        For Each item As ListItem In StatusList.Items
                            item.Enabled = True
                        Next
                    Else
                        For Each item As ListItem In StatusList.Items
                            If item.ToString = "Out for rework" Or item.ToString = "Scrapped" Then
                                item.Enabled = False
                            End If
                        Next
                    End If

                    If LocationList.SelectedValue = "Out For Rework" Or LocationList.SelectedValue = "Scrapped" Then
                        For Each item As ListItem In LocationList.Items
                            item.Enabled = True
                        Next
                    Else
                        For Each item As ListItem In LocationList.Items
                            If item.ToString = "Out For Rework" Or item.ToString = "Scrapped" Then
                                item.Enabled = False
                            End If
                        Next
                    End If

                    BladeContentDiv.Style.Add("display", "inline-block")
                    If StatusList.SelectedValue = "Out for rework" Then
                        BladesOutForRework.Enabled = False
                        DefLabel.Style.Add("display", "inline-block")
                        DefectList.Style.Add("display", "inline-block")
                        DefectList.Enabled = False
                        DefectList.SelectedValue = ReasonBladeIsOFR
                    ElseIf StatusList.SelectedValue = "Scrapped" Then
                        BladesScrapped.Enabled = False
                        BladesOutForRework.Enabled = False
                        BladesEdit.Enabled = False
                        ScrappedBy.Style.Add("display", "inline-block")
                        DateScrapped.Style.Add("display", "inline-block")
                        BladeScrappedBy.Style.Add("display", "inline-block")
                        BladeScrapDate.Style.Add("display", "inline-block")
                        BladeScrappedBy.Text = WhoScrappedBlade
                        BladeScrapDate.Text = DateofBladeScrap
                    End If
                Else
                    BladeSearchMsg.Text = "This Blade does not exist. Would you like to add it?"
                    BladeSearchYes.Style.Add("display", "inline-block")
                    BladeSearchNo.Style.Add("display", "inline-block")
                    BladeContentDiv.Style.Add("display", "none")
                End If
                readerObj.Close()
            End Using
            myConn.Close()
        Else
            BladesSerialText.Text = ""
            MsgBox("You must enter a whole number!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
        End If

    End Sub

    Private Sub Blades_Edit(sender As System.Object, e As System.EventArgs) Handles BladesEdit.Click
        ManufacturersList.Enabled = True
        StatusList.Enabled = True
        LocationList.Enabled = True
        BladeDiameter.Enabled = True
        BladeGrit.Enabled = True
        BladeThickness.Enabled = True
        BladesSubmit.Visible = True
        BladesCancel.Visible = True
        SubmitNewMan.Visible = True
        SubmitNewStat.Visible = True
        BladesEdit.Visible = False
        BladesOutForRework.Visible = False
        BladesScrapped.Visible = False
        BladeContentDiv.Style.Add("Padding", "20px 300px 20px 50px")
        If LocationList.SelectedValue = "Out For Rework" And StatusList.SelectedValue = "Out for rework" Then
            StatusList.Enabled = False
            LocationList.Enabled = False
            DefectList.Style.Add("display", "inline-block")
            DefectList.Enabled = False
            SubmitNewDefect.Style.Add("display", "inline-block")
        End If
    End Sub

    Private Sub Blades_Modify(sender As System.Object, e As System.EventArgs) Handles BladesSubmit.Click
        Dim regex As Regex = New Regex("^([0-9]+(\.[0-9]+)?|\.[0-9]+)$")
        Dim match1 As Match = regex.Match(BladeDiameter.Text)
        Dim match2 As Match = regex.Match(BladeGrit.Text)
        Dim match3 As Match = regex.Match(BladeThickness.Text)
        Dim SerialNumInput = BladesSerialText.Text
        If match1.Success And match2.Success And match3.Success Then
            myConn.Open()
            Dim cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_UpdateBlade"
            cmd.Parameters.AddWithValue("sn", SerialNumInput)
            cmd.Parameters.AddWithValue("manD", ManufacturersList.SelectedValue)
            cmd.Parameters.AddWithValue("statD", StatusList.SelectedValue)
            cmd.Parameters.AddWithValue("locationD", LocationList.SelectedValue)
            cmd.Parameters.AddWithValue("grit", BladeGrit.Text)
            cmd.Parameters.AddWithValue("diameter", BladeDiameter.Text)
            cmd.Parameters.AddWithValue("thickness", BladeThickness.Text)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As SqlException
                MsgBox("There was an error updating the blade", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
                myConn.Close()
                Page_Refresh(sender, e)
            End Try

            MsgBox("Blade Updated", MsgBoxStyle.SystemModal + MsgBoxStyle.Information, "Corning Web App")
            myConn.Close()

            BladesSerialText.Text = SerialNumInput
            Blades_Search(sender, e)

            If LocationList.SelectedValue = "Out For Rework" And StatusList.SelectedValue = "Out for rework" Then
                BladesOutForRework.Enabled = False
            ElseIf LocationList.SelectedValue = "Scrapped" And StatusList.SelectedValue = "Scrapped" Then
                BladesOutForRework.Enabled = False
                BladesScrapped.Enabled = False
                BladesEdit.Enabled = False
            End If
        Else
            If match1.Success = False Then
                BladeDiameter.Text = ""
                MsgBox("Blade Diameter must be a number or decimal!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
            If match2.Success = False Then
                BladeGrit.Text = ""
                MsgBox("Blade Grit must be a number or decimal!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
            If match3.Success = False Then
                BladeThickness.Text = ""
                MsgBox("Blade Thickness must be a number or decimal!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
        End If
    End Sub

    Private Sub Blades_OutForRework(sender As System.Object, e As System.EventArgs) Handles BladesOutForRework.Click
        MsgBox("You are about to send this blade out for rework. Please select a defect reason.", MsgBoxStyle.SystemModal + MsgBoxStyle.Information, "Corning Web App")
        BladeContentDiv.Style.Add("Padding", "20px 290px 20px 50px")

        DefLabel.Style.Add("display", "inline-block")
        DefectList.Style.Add("display", "inline-block")
        SubmitNewDefect.Style.Add("display", "inline-block")
        DefectList.SelectedIndex = 0
        DefectList.Enabled = True

        BladesOFRSubmit.Visible = True
        BladesCancel.Visible = True
        BladesEdit.Visible = False
        BladesOutForRework.Visible = False
        BladesScrapped.Visible = False
    End Sub

    Private Sub Blades_SubmitOFR(sender As System.Object, e As System.EventArgs) Handles BladesOFRSubmit.Click
        If MsgBox("You are about to send this blade out for rework. Are you sure?", MsgBoxStyle.SystemModal + MsgBoxStyle.YesNo, "Corning Web App") = MsgBoxResult.Yes Then
            Dim SerialNumInput = BladeResultSerial.Text

            myConn.Open()
            Dim cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_UpdateBlade"
            cmd.Parameters.AddWithValue("sn", SerialNumInput)
            cmd.Parameters.AddWithValue("manD", ManufacturersList.SelectedValue)
            cmd.Parameters.AddWithValue("statD", "Out for rework")
            cmd.Parameters.AddWithValue("locationD", "Out For Rework")
            cmd.Parameters.AddWithValue("grit", BladeGrit.Text)
            cmd.Parameters.AddWithValue("diameter", BladeDiameter.Text)
            cmd.Parameters.AddWithValue("thickness", BladeThickness.Text)

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox("Unable to send blade out for rework!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End Try

            cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_InsertReworkDetails"
            cmd.Parameters.AddWithValue("SerialNumber", SerialNumInput)
            cmd.Parameters.AddWithValue("ReasonOutDesc", DefectList.SelectedValue)

            Try
                cmd.ExecuteNonQuery()
                MsgBox("Blade was sent out for rework.", MsgBoxStyle.SystemModal + MsgBoxStyle.Information, "Corning Web App")
            Catch ex As Exception
                MsgBox("Unable to insert rework details!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End Try
            ReworkClear(sender, e)
            myConn.Close()

            BladesSerialText.Text = SerialNumInput
            Blades_Search(sender, e)
        Else
            DefLabel.Style.Add("display", "none")
            DefectList.Style.Add("display", "none")
            SubmitNewDefect.Style.Add("display", "none")

            BladesOFRSubmit.Visible = False
            BladesCancel.Visible = False
            BladesSubmit.Visible = False
            BladesEdit.Visible = True
            BladesOutForRework.Visible = True
            BladesScrapped.Visible = True
        End If
    End Sub

    Private Sub Blades_Scrapped(sender As System.Object, e As System.EventArgs) Handles BladesScrapped.Click
        If MsgBox("Are you sure you want to scrap this blade?", MsgBoxStyle.SystemModal + MsgBoxStyle.YesNo, "Corning Web App") = MsgBoxResult.Yes Then
            Dim SerialNumInput = BladeResultSerial.Text
            myConn.Open()
            Dim cmd = myConn.CreateCommand()
            If StatusList.SelectedValue = "Out for rework" And LocationList.SelectedValue = "Out For Rework" Then
                cmd = myConn.CreateCommand()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "dbo.sp_BringBackBlade"
                cmd.Parameters.AddWithValue("SerialNumber", SerialNumInput)
                cmd.Parameters.AddWithValue("DateIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                Try
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox("Unable to bring blade back in before scrapping blade!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
                End Try
            End If

            cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_ScrapBlade"
            cmd.Parameters.AddWithValue("SerialNum", SerialNumInput)
            cmd.Parameters.AddWithValue("ScrapUser", Session("username"))

            Try
                cmd.ExecuteNonQuery()
                MsgBox("Blade was scrapped!", MsgBoxStyle.SystemModal + MsgBoxStyle.Information, "Corning Web App")
            Catch ex As Exception
                MsgBox("Unable to scrap blade!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End Try

            myConn.Close()

            BladeResultSerial.Text = SerialNumInput
            Blades_Search(sender, e)
        End If
    End Sub

    Private Sub Blades_Add(sender As System.Object, e As System.EventArgs) Handles BladesSubmit2.Click
        Dim regex As Regex = New Regex("^([0-9]+(\.[0-9]+)?|\.[0-9]+)$")
        Dim match1 As Match = regex.Match(BladeDiameter2.Text)
        Dim match2 As Match = regex.Match(BladeGrit2.Text)
        Dim match3 As Match = regex.Match(BladeThickness2.Text)

        If match1.Success And match2.Success And match3.Success Then
            myConn.Open()
            Dim cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_InsertNewBlade"
            cmd.Parameters.AddWithValue("SerialNumber", BladesSerialNumber.Text)
            cmd.Parameters.AddWithValue("ManufacterDesc", ManufacturersList2.SelectedValue)
            cmd.Parameters.AddWithValue("BladeDiameter", BladeDiameter2.Text)
            cmd.Parameters.AddWithValue("BladeGrit", BladeGrit2.Text)
            cmd.Parameters.AddWithValue("BladeThickness", BladeThickness2.Text)
            cmd.Parameters.AddWithValue("InsertBy", Session("username"))
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox("Unable to add blade!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End Try

            cmd = myConn.CreateCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.sp_InsertBladeRevision"
            cmd.Parameters.AddWithValue("SerialNumber", BladesSerialNumber.Text)
            Try
                cmd.ExecuteNonQuery()
                MsgBox("Blade was added to the database!", MsgBoxStyle.SystemModal + MsgBoxStyle.Information, "Corning Web App")
            Catch ex As Exception
                MsgBox("Unable to update blade revision", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End Try

            NewBladeContentDiv.Style.Add("display", "none")
            BladeSearchMsg.Text = ""
            BladeSearchYes.Style.Add("display", "none")
            BladeSearchNo.Style.Add("display", "none")
            BladeContentDiv.Style.Add("display", "none")
            BladesSerialText.Text = ""
            ManufacturersList2.SelectedIndex = 0
            BladeDiameter2.Text = ""
            BladeGrit2.Text = ""
            BladeThickness2.Text = ""

            myConn.Close()

            BladesSerialText.Text = BladesSerialNumber.Text
            Blades_Search(sender, e)
        Else
            If match1.Success = False Then
                BladeDiameter2.Text = ""
                MsgBox("Blade Diameter must be a number or decimal!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
            If match2.Success = False Then
                BladeGrit2.Text = ""
                MsgBox("Blade Grit must be a number or decimal!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
            If match3.Success = False Then
                BladeThickness2.Text = ""
                MsgBox("Blade Thickness must be a number or decimal!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
        End If
    End Sub

    Protected Sub BladesModify_Cancel(sender As System.Object, e As System.EventArgs)
        DefLabel.Style.Add("display", "none")
        DefectList.Style.Add("display", "none")
        SubmitNewDefect.Style.Add("display", "none")
        BladesEdit.Visible = True
        BladesOutForRework.Visible = True
        BladesScrapped.Visible = True
        BladesOFRSubmit.Visible = False
        BladesSubmit.Visible = False
        BladesCancel.Visible = False
        ScrappedBy.Style.Add("display", "none")
        DateScrapped.Style.Add("display", "none")
        BladeScrappedBy.Style.Add("display", "none")
        BladeScrapDate.Style.Add("display", "none")

        BladesSerialText.Text = BladeResultSerial.Text
        Blades_Search(sender, e)
    End Sub

    Protected Sub BladesAdd_Cancel(sender As System.Object, e As System.EventArgs)
        If MsgBox("Are you sure you do not want to add this blade?", MsgBoxStyle.SystemModal + MsgBoxStyle.YesNo, "Corning Web App") = MsgBoxResult.Yes Then
            NewBladeContentDiv.Style.Add("display", "none")
            BladesSerialNumber.Text = ""
            BladeSearchMsg.Text = ""
            ManufacturersList2.SelectedIndex = 0
            BladeGrit2.Text = ""
            BladeDiameter2.Text = ""
            BladeThickness2.Text = ""
        End If
    End Sub

    Private Sub SubmitNewDefect_Click(sender As System.Object, e As System.EventArgs) Handles SubmitNewDefect.Click
        Dim DefDescInput = InputBox("Enter New defect:  ", "Corning Web App")
        If (DefDescInput <> "") Then

            myConn.Open()
            If DefectList.Items.Contains(New ListItem(DefDescInput)) Then
                MsgBox("Defect not added since it already exists in the database", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
            Else
                Dim cmd = myConn.CreateCommand()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "dbo.sp_InsertDefectReasons"
                cmd.Parameters.AddWithValue("DefectDesc", DefDescInput)
                Dim k = cmd.ExecuteNonQuery()
                If k <> 0 Then
                    MsgBox("New defect reason was succesfully entered into the database!", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
                End If

                Dim dt As New DataTable
                Dim strSql As String = "Select dbo.DefectReasons.DefectDescription From dbo.DefectReasons Order By dbo.DefectReasons.DefectDescription ASC"
                Using cnn As New SqlConnection(connString)
                    cnn.Open()
                    Using dad As New SqlDataAdapter(strSql, cnn)
                        dad.Fill(dt)
                    End Using
                    cnn.Close()
                End Using
                DefectList.Items.Clear()
                DefectList.DataSourceID = Nothing
                DefectList.DataSource = dt
                DefectList.DataBind()
            End If
            myConn.Close()
        End If
    End Sub

    'After a new status is entered this will put it in the database
    Private Sub SubmitNewStat_Click(sender As System.Object, e As System.EventArgs) Handles SubmitNewStat.Click
        Dim StatDescInput = InputBox("Enter New status: ", "Corning Web App")
        If (StatDescInput <> "") Then

            myConn.Open()
            If StatusList.Items.Contains(New ListItem(StatDescInput)) Then
                MsgBox("Duplicate data!!!", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
            Else
                Dim cmd = myConn.CreateCommand()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "dbo.sp_InsertStatuses"
                cmd.Parameters.AddWithValue("StatusDesc", StatDescInput)
                Dim k = cmd.ExecuteNonQuery()
                If k <> 0 Then
                    MsgBox("New status was succesfully entered into the database!", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
                End If

                Dim dt As New DataTable
                Dim strSql As String = "Select dbo.Statuses.StatusDesc From dbo.Statuses Where dbo.Statuses.StatusDesc != 'Out for rework' AND dbo.Statuses.StatusDesc != 'Scrapped' Order By dbo.Statuses.StatusDesc ASC"
                Using cnn As New SqlConnection(connString)
                    cnn.Open()
                    Using dad As New SqlDataAdapter(strSql, cnn)
                        dad.Fill(dt)
                    End Using
                    cnn.Close()
                End Using
                StatusList.Items.Clear()
                StatusList.DataSourceID = Nothing
                StatusList.DataSource = dt
                StatusList.DataBind()

                For Each item As ListItem In StatusList.Items
                    If "Out for rework" = item.ToString Or "Scrapped" = item.ToString Then
                        item.Enabled = False
                    End If
                Next

            End If
            myConn.Close()
        End If
    End Sub

    'After a new manufacturer is entered this will put it in the database
    Private Sub SubmitNewMan_Click(sender As System.Object, e As System.EventArgs) Handles SubmitNewMan.Click
        Dim ManInput = InputBox("Enter new manufacturer name: ", "Corning Web App")
        If (ManInput <> "") Then

            myConn.Open()
            If ManufacturersList.Items.Contains(New ListItem(ManInput)) Then
                MsgBox("Manufacturer not added because the manufacturer already exists", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
            Else
                Dim cmd = myConn.CreateCommand()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "dbo.sp_InsertManufacturers"
                cmd.Parameters.AddWithValue("Name", ManInput)
                Dim k = cmd.ExecuteNonQuery()
                If k <> 0 Then
                    MsgBox("New manufacturer was succesfully entered into the database!", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
                End If

                Dim dt As New DataTable
                Dim strSql As String = "Select dbo.Manufacturers.ManufactureName From dbo.Manufacturers Order By Manufacturers.ManufactureName ASC"
                Using cnn As New SqlConnection(connString)
                    cnn.Open()
                    Using dad As New SqlDataAdapter(strSql, cnn)
                        dad.Fill(dt)
                    End Using
                    cnn.Close()
                End Using
                ManufacturersList.Items.Clear()
                ManufacturersList.DataSourceID = Nothing
                ManufacturersList.DataSource = dt
                ManufacturersList.DataBind()
            End If
            myConn.Close()
        End If
    End Sub

    'After a new manufacturer is entered this will put it in the database
    Private Sub SubmitNewMan2_Click(sender As System.Object, e As System.EventArgs) Handles SubmitNewMan2.Click
        Dim ManInput = InputBox("Enter new manufacturer name: ", "Corning Web App")
        If (ManInput <> "") Then

            myConn.Open()
            If ManufacturersList.Items.Contains(New ListItem(ManInput)) Then
                MsgBox("Manufacturer not added because the manufacturer already exists", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
            Else
                Dim cmd = myConn.CreateCommand()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "dbo.sp_InsertManufacturers"
                cmd.Parameters.AddWithValue("Name", ManInput)
                Dim k = cmd.ExecuteNonQuery()
                If k <> 0 Then
                    MsgBox("New manufacturer was succesfully entered into the database!", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
                End If

                Dim dt As New DataTable
                Dim strSql As String = "Select dbo.Manufacturers.ManufactureName From dbo.Manufacturers Order By dbo.Manufacturers.ManufactureName ASC"
                Using cnn As New SqlConnection(connString)
                    cnn.Open()
                    Using dad As New SqlDataAdapter(strSql, cnn)
                        dad.Fill(dt)
                    End Using
                    cnn.Close()
                End Using
                ManufacturersList2.Items.Clear()
                ManufacturersList2.DataSourceID = Nothing
                ManufacturersList2.DataSource = dt
                ManufacturersList2.DataBind()
            End If
            myConn.Close()
        End If
    End Sub

    Protected Sub DisplayRework(Sender As Object, e As EventArgs)
        If ReworkBladeSerial.Text = "" Then

        Else
            ReworkBladeDetails.Style.Add("display", "inline-block")
            Dim SerialNumInput = ReworkBladeSerial.Text
            Try
                myConn.Open()

                Dim cmd = myConn.CreateCommand()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "dbo.sp_GetCurrentBladesOut"
                cmd.Parameters.AddWithValue("SerialNumber", SerialNumInput)

                Using readerObj As SqlClient.SqlDataReader = cmd.ExecuteReader
                    While readerObj.Read
                        ReworkDateOut.Text = readerObj("DateOut").ToString
                        ReworkReasonOut.Text = readerObj("DefectDescription").ToString
                    End While
                    readerObj.Close()
                End Using
                myConn.Close()
            Catch
                MsgBox("Error getting rework info", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
            End Try
        End If
        TabContainer2.ActiveTab = RD
    End Sub

    'clear rework tab and hide details
    Protected Sub ReworkClear(Sender As Object, e As EventArgs)
        ReworkBladeDetails.Style.Add("display", "none")
        ReworkReasonOut.Text = ""
        ReworkDateOut.Text = ""
        ReworkDateIn.Text = ""
        ReworkThickness.Text = ""
        ReworkRunOut.Text = ""

        Dim dt As New DataTable
        Dim strSql As String = "Select * FROM [v_CurrentBladesOut] Order By SerialNumber ASC"
        Using cnn As New SqlConnection(connString)
            cnn.Open()
            Using dad As New SqlDataAdapter(strSql, cnn)
                dad.Fill(dt)
            End Using
            cnn.Close()
        End Using
        ReworkBladeSerial.Items.Clear()
        ReworkBladeSerial.DataSourceID = Nothing
        ReworkBladeSerial.DataSource = dt
        ReworkBladeSerial.DataBind()
    End Sub

    'sends message cancelling update then clears
    Protected Sub ReworkClearMssg(Sender As Object, e As EventArgs)
        If MsgBox("Are you sure you wish to cancel?", MsgBoxStyle.SystemModal + MsgBoxStyle.OkCancel, "Corning Web App") = MsgBoxResult.Ok Then
            ReworkClear(Sender, e)
        End If
    End Sub

    'Bring a saw back from rework
    Protected Sub BringSawBack(Sender As Object, e As EventArgs)
        Dim numregex As Regex = New Regex("^\d*\.?\d*$")
        Dim match1 As Match = numregex.Match(ReworkRunOut.Text)
        Dim match2 As Match = numregex.Match(ReworkThickness.Text)
        Dim dateregex As Regex = New Regex("^\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d:\d\d\d$")
        Dim dateMatch As Match = dateregex.Match(ReworkDateIn.Text)

        If match1.Success And match2.Success And ReworkRunOut.Text IsNot "" And ReworkThickness.Text IsNot "" And dateMatch.Success Then
            Try
                myConn.Open()
                Using cmd = myConn.CreateCommand()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "dbo.sp_BringBackBlade"
                    cmd.Parameters.AddWithValue("SerialNumber", ReworkBladeSerial.Text)
                    cmd.Parameters.AddWithValue("DateIn", ReworkDateIn.Text)
                    cmd.Parameters.AddWithValue("CoreThickness", ReworkThickness.Text)
                    cmd.Parameters.AddWithValue("CoreRunOut", ReworkRunOut.Text)
                    cmd.ExecuteNonQuery()
                End Using
                Using cmd = myConn.CreateCommand()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "dbo.sp_InsertBladeRevision"
                    cmd.Parameters.AddWithValue("SerialNumber", ReworkBladeSerial.Text)
                    cmd.ExecuteNonQuery()
                End Using
                myConn.Close()
                MsgBox("Blade was successfully brought back from rework", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
                ReworkClear(Sender, e)
            Catch
                MsgBox("Error bringing blade back", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
            End Try
        Else
            If match1.Success = False Or ReworkRunOut.Text Is "" Then
                ReworkRunOut.Text = ""
                MsgBox("Core Run Out must be a number!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
            If match2.Success = False Or ReworkThickness.Text Is "" Then
                ReworkThickness.Text = ""
                MsgBox("Core Thickness must be a number!", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
            If dateMatch.Success = False Then
                ReworkDateIn.Text = ""
                MsgBox("Please use Calender dropdown for Date In", MsgBoxStyle.SystemModal + MsgBoxStyle.Exclamation, "Corning Web App")
            End If
        End If
    End Sub

    'Scrap saw from rework
    Protected Sub ReworkScrap(Sender As Object, e As EventArgs)
        Dim scrapCheck = MsgBox("Are you sure you want to scrap this blade?", MsgBoxStyle.SystemModal + vbQuestion + vbYesNo, "Corning Web App")
        If scrapCheck = vbYes Then
            Try
                myConn.Open()
                Using cmd = myConn.CreateCommand()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "dbo.sp_BringBackBlade"
                    cmd.Parameters.AddWithValue("SerialNumber", ReworkBladeSerial.Text)
                    cmd.Parameters.AddWithValue("DateIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    cmd.ExecuteNonQuery()
                End Using
                Using cmd = myConn.CreateCommand()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "dbo.sp_ScrapBlade"
                    cmd.Parameters.AddWithValue("SerialNum", ReworkBladeSerial.Text)
                    cmd.Parameters.AddWithValue("ScrapUser", Session("username"))
                    cmd.ExecuteNonQuery()
                End Using
                myConn.Close()
                MsgBox("Blade was successfully scrapped", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
                ReworkClear(Sender, e)
            Catch
                MsgBox("Error scrapping blade", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
            End Try
        Else
            MsgBox("Scrap Cancelled", MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, "Corning Web App")
        End If
    End Sub

    Private Sub Generate_report(sender As System.Object, e As System.EventArgs) Handles RR.Click
        Dim reportName = ReportSelector1.SelectedValue
        ReportViewer.ServerReport.ReportPath = "/Saws/" + reportName.ToString
        ReportViewer.Visible = "true"
    End Sub

    Protected Sub SetBladesAsActiveTab(sender As System.Object, e As System.EventArgs) Handles TabContainer2.ActiveTabChanged
        TabContainer2.ActiveTab = Blades
    End Sub

End Class