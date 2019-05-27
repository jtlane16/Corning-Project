<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LandingPage.aspx.vb" Inherits="WebApplication1.LandingPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Reporting and Maintenance Tool</title> 
    <link href="LandingPageCSS.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        
        .auto-style1 {
            text-align: left;
        }
        html, body {
            padding:0px 0px 0px 0px;
            margin:0px 0px 0px 0px;
            width:100%;
            
            
        }
        body{
            position: relative;
            top:-22px;
            font-family:georgia,garamond,sans-serif;
        }
        .RDText{
            position:absolute;
            left:190px;
            width:173px;
        }
        .existingBDD{
            position:absolute;
            left:250px;
            width:211px;
        }
        .newBDD{
            position:absolute;
            left:250px;
            width:211px;
        }
        .newBTB{
            position:absolute;
            left:250px;
            width:207px;
        }
        #BladesSerialNumber{
            position:absolute;
            left:158px;
            width:207px;
        }
        .logo {
    position: absolute;
    left:20px;
    width:100px;
    height:100px;
    top: 10px;
    
    min-width: 50px;
    min-height: 50px;
    box-shadow: 0px 10px 10px;
}
       
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="background-color:#0066A4;width:100%; height: 100px">
        <h1 style="width: 500px; background-color:#0066A4; padding-left:10px; padding-top:55px; padding-left:30%; color:white">Reporting and Updating Tool</h1>
            <asp:HyperLink ID="HyperLink1" runat="server" style="position:absolute; right:30px;top:80px; color:white" NavigateUrl="UserManual.pdf" Target="_blank">User Manual</asp:HyperLink>
            <asp:Label ID="CurrentUser" runat="server" style="width: 200px; padding-left:10px; color:white; top:125px; right:-120px; position:absolute"></asp:Label>
             <img src="Stock/Corning.jpg" class="logo"/>
        <asp:Button ID="Logout" runat="server" Text="Logout" Style="position: absolute; height: 30px; right: 30px; top:20px; background-color:red; color:white" OnClick="BtnLogout_Click"/>
        </div>
            <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:SawsConnectionString %>" SelectCommand="SELECT * FROM [Blades]" ID="Saws_AllBladesDataSource"></asp:SqlDataSource>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:SawsConnectionString %>" SelectCommand="SELECT * FROM [Statuses] ORDER BY StatusDesc ASC" ID="Saws_AllStatusesDataSource"></asp:SqlDataSource>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:SawsConnectionString %>" SelectCommand="SELECT * FROM [DefectReasons] ORDER BY DefectDescription ASC" ID="Saws_AllDefectReasonsDataSource"></asp:SqlDataSource>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:SawsConnectionString %>" SelectCommand="SELECT * FROM [Manufacturers] ORDER BY ManufactureName ASC" ID="Saws_AllManufacturersDataSource"></asp:SqlDataSource>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:SawsConnectionString %>" SelectCommand="SELECT * FROM [Locations] ORDER BY LocationDesc ASC" ID="Saws_AllLocationsDataSource"></asp:SqlDataSource>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:SawsConnectionString %>" SelectCommand="SELECT * FROM [v_CurrentBladesOut]" ID="Saws_AllReworkNullDateIn"></asp:SqlDataSource>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:ReportServerConnectionString %>" SelectCommand="SELECT Name FROM Catalog WHERE (Path LIKE '%/Saws/%')" ID="GetReportsNames"></asp:SqlDataSource>
            <asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:SawsConnectionString %>" SelectCommand="SELECT DISTINCT SerialNumber FROM [Blades]" ID="BladesDataGrid"></asp:SqlDataSource>
                 <div style="height:40px; background-color:#0066A4"></div>
            <ajaxToolkit:TabContainer ID="Tabcontainer1" runat="server" ActiveTabIndex="0" UseVerticalStripPlacement="False" ScrollBars="Auto" Height="100%" Width="100%" backcolor="#0066A4">
                <ajaxToolkit:TabPanel runat="server" HeaderText="Reports" ID="Reports" Font-Overline="False">
                    <ContentTemplate>     
                        <div>
                            <div style="border-style: double; padding-left: 20px; padding-top: 10px">
                            STEPS TO VIEW REPORTS:
                            <br />
                            1.) Click on the below dropdown to select which report you would like to run.
                            <br />
                            2.) Click Generate Report.
                            <br />
                            3.) Provide search criteria if applicable.
                            <br />
                            4.) Click view report if search criteria was entered.
                            <br />
                            <br />
      
                            <asp:DropDownList ID="ReportSelector1" runat="server" DataSourceID="GetReportsNames" DataTextField="Name" Width="200px" height="30px" Padding="5px 10px">
                            </asp:DropDownList>
                            <asp:Button ID="RR" runat="server" Text="Generate Report" Height="30px" Width="120px" Class="btn" />
                            
                            </div>
                            <br /><br />
                            <rsweb:ReportViewer ID="ReportViewer" runat="server" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" style="text-align: center;" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="100%" DocumentMapWidth="50%" BackColor="" ClientIDMode="AutoID" ProcessingMode="Remote" HighlightBackgroundColor="" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemHoverBackColor="">
                                <serverreport ReportPath="/Saws/Current Saw Blades Report" reportserverurl="http://desktop-76emi5i:8080/ReportServer/" />
                               <localreport>
                                    <datasources>
                                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="Tables" />
                                    </datasources>
                                </localreport>
                             </rsweb:ReportViewer>
                           <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="WebApplication1.FullSawsDataSetTableAdapters.sp_GetAllTableNameTableAdapter"></asp:ObjectDataSource>
                        </div>
                    </ContentTemplate>
               </ajaxToolkit:TabPanel>
            

                <ajaxToolkit:TabPanel runat="server" HeaderText="Blade Maintenance" ID="B_M">
                        <ContentTemplate>
                            <ajaxToolkit:TabContainer ID="TabContainer2" runat="server" OnActiveTabChanged="SetBladesAsActiveTab" ScrollBars="Auto" Height="100%" Width="100%">                      
                                <ajaxToolkit:TabPanel ID="Blades" runat="server" HeaderText="Blades">
                                    <ContentTemplate>
                                        To add a blade enter the serial number and when prompted if you wish to add the blade select yes.
                                        <br />
                                        To modify a blade enter the serial number and click the search button, the information on the blade will then be displayed.
                                        <br />
                                        <br />
                                        Enter Blade Serial Number <asp:Textbox ID="BladesSerialText" runat="server"></asp:Textbox>
                                         
                                        <asp:Button ID="BladesSearch" runat="server" Text="Search" OnClick="Blades_Search" Height="25px" Width="55px"  CssClass="btn"/>
                                        <asp:Label ID="BladeSearchMsg" runat="server"></asp:Label>
                                        <asp:Button ID="BladeSearchYes" runat="server" Text="YES" OnClick="New_Blade" Height="25px" Width="55px" style="display:none" CssClass="btn"/>
                                        <asp:Button ID="BladeSearchNo" runat="server" Text="NO"  OnClick="Page_Refresh" Height="25px" Width="55px" style="display:none" CssClass="btn"/>
                                        <br />
                                        <br />
                                        <br />
                                        <div style="width:100%;text-align:center; visibility:hidden" runat="server" id="hardRule">
                                        </div>
                                        <br />
                                        <br />

                                       <div id="BladeContentDiv" runat="server" style="display:none; background-color:lightgrey;padding:20px 100px 20px 50px; border-radius:20px; box-shadow:0px 5px 5px 5px; margin:0px 0px 100px 30px  ">
                                            Serial Number  <asp:Label ID="BladeResultSerial" runat="server" ></asp:Label>
                                            <br />
                                            <br />
                                            Manufacturer Name <asp:DropDownList ID="ManufacturersList" runat="server"
                                                                    DataSourceID="Saws_AllManufacturersDataSource" DataTextField="ManufactureName" DataValueField="ManufactureName" 
                                                                    CssClass="existingBDD" AppendDataBoundItems="True" Enabled="false">
                                                               </asp:DropDownList>
                                                               <asp:Button id="SubmitNewMan" CssClass="btn" Height="25px" Width="55px" runat="server" Text="Add" type="submit" value="Submit" style="float: right; position:absolute; left:470px; top:417px" Visible="false"/>
                                            <br />
                                            <br />
                                            Blade Status <asp:DropDownList ID="StatusList" runat="server" 
                                                                    DataSourceID="Saws_AllStatusesDataSource" DataTextField="StatusDesc" DataValueField="StatusDesc" 
                                                                    CssClass="existingBDD" AppendDataBoundItems="True" Enabled="false">                                                
                                                           </asp:DropDownList> 
                                                           <asp:Button id="SubmitNewStat" CssClass="btn" Height="25px" Width="55px" runat="server" Text="Add" type="submit" value="Submit" style="float: right; position: absolute; left:470px; top:450px" Visible="false"/>
                                                            
                                            <br />
                                            <br />
                                            Location <asp:DropDownList ID="LocationList" runat="server"
                                                                    DataSourceID="Saws_AllLocationsDataSource" DataTextField="LocationDesc" DataValueField="LocationDesc"
                                                                    CssClass="existingBDD" AppendDataBoundItems="true" Enabled="false">
                                                     </asp:DropDownList>
                                           <br />
                                           <br />
                                           <asp:Label id="DefLabel" runat="server" style="display:none">Defect Reason </asp:Label><asp:DropDownList ID="DefectList" runat="server" Style="display:none"
                                                                    DataSourceID="Saws_AllDefectReasonsDataSource" DataTextField="DefectDescription" DataValueField="DefectDescription" 
                                                                    CssClass="existingBDD" AppendDataBoundItems="True">                                                
                                                           </asp:DropDownList> 
                                                           <asp:Button id="SubmitNewDefect" style="display:none; left: 470px; float: right; position: absolute; top: 512px;" Height="25px" Width="55px" CssClass="btn" runat="server" Text="Add" type="submit" value="Submit"/>
                                            <br />
                                            <br />
                                            Blade Diameter <asp:TextBox ID="BladeDiameter" runat="server" CssClass="newBTB" Enabled="false" ></asp:TextBox>
                                            <br />
                                            <br />
                                            Blade Grit <asp:TextBox ID="BladeGrit" runat="server" CssClass="newBTB" Enabled="false"></asp:TextBox>
                                            <br />
                                            <br />
                                            Blade Thickness <asp:TextBox ID="BladeThickness" runat="server"  CssClass="newBTB" Enabled="false"></asp:TextBox>
                                            <br />
                                            <br />
                                            Inserted By <asp:TextBox ID="BladeResultInsert" runat="server" CssClass="newBTB" Enabled="false"></asp:TextBox>
                                            <br />
                                            <br />
                                            Date of Insert <asp:TextBox ID="BladeResultDate" runat="server" CssClass="newBTB" Enabled="false"></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Label id="ScrappedBy" runat="server" style="display:none">Blade Scrapped By </asp:Label><asp:TextBox ID="BladeScrappedBy" runat="server" style="display:none" CssClass="newBTB" Enabled="false"></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Label id="DateScrapped" runat="server" style="display:none">Date Scrapped </asp:Label><asp:TextBox ID="BladeScrapDate" runat="server" style="display:none" CssClass="newBTB" Enabled="false"></asp:TextBox>
                                            <br />
                                            <br />
                                           <asp:Button ID="BladesEdit" runat="server" type="button" CssClass="btn" Height="25px" Width="55px" Text="Edit" style="float: left; margin-right:15px; margin-left:10px" Visible="true"/>
                                           <asp:Button ID="BladesOutForRework" runat="server" CssClass="btn" Height="25px" Width="95px" type="button" Text="Out for rework" style="float: left; margin-right:15px" Visible="true" />
                                           <asp:Button ID="BladesScrapped" runat="server" CssClass="btn" Height="25px" Width="85px" type="button" Text="Scrap blade" style="float: left; margin-right:15px" Visible="true" />
                                           <asp:Button id="BladesSubmit" runat="server" CssClass="btn" Height="25px" Width="55px" type="button" Text="Save" style="float: left;margin-right:15px" Visible="false"/>
                                           <asp:Button ID="BladesOFRSubmit" runat="server" CssClass="btn" Height="25px" Width="55px" type="button" Text="Submit" style="float: left;margin-right:15px" Visible="false" /> 
                                           <asp:Button id="BladesCancel" runat="server" CssClass="btn" Height="25px" Width="55px" type="button" Text="Cancel" style="float: left" Visible="false" onclick="BladesModify_Cancel"/>                                           
                                       </div>

                                        <div id="NewBladeContentDiv" runat="server" style="display:none; background-color:lightgrey; padding:10px 100px 10px 20px; border-radius:20px;box-shadow:0px 5px 5px 5px; margin:0px 0px 100px 30px">
                                            Serial Number <asp:TextBox ID="BladesSerialNumber" CssClass="newBTB" runat="server" Enabled="False"></asp:TextBox>
                                            <br />
                                            <br />
                                            Manufacturer <asp:DropDownList ID="ManufacturersList2" runat="server" 
                                                                    DataSourceID="Saws_AllManufacturersDataSource" DataTextField="ManufactureName" DataValueField="ManufactureName" 
                                                                    CssClass="newBDD" AppendDataBoundItems="True">
                                                             </asp:DropDownList>
                                                             <asp:Button id="SubmitNewMan2" Height="25px" Width="55px" CssClass="btn" runat="server" Text="Add new" type="submit" value="Submit" style="float: right; position: absolute; left:470px;top:407px" />
                                            <br />
                                            <br />
                                            Blade Status <asp:DropDownList ID="StatusList2" runat="server"                                      
                                                                    CssClass="newBDD" AppendDataBoundItems="True" Enabled="false"> 
                                                                <asp:ListItem>In Inventory</asp:ListItem>
                                                           </asp:DropDownList> 
                                            <br />
                                            <br />
                                            Location <asp:DropDownList ID="LocationList2" runat="server"                                                                    
                                                                    CssClass="newBDD" AppendDataBoundItems="True" Enabled="false">
                                                                <asp:ListItem>In Inventory</asp:ListItem>
                                                           </asp:DropDownList>
                                            <br />
                                            <br />
                                            Blade Diameter <asp:TextBox ID="BladeDiameter2" class="newBTB" runat="server" ></asp:TextBox>
                                            <br />
                                            <br />
                                            Blade Grit <asp:TextBox ID="BladeGrit2" class="newBTB" runat="server" ></asp:TextBox>
                                            <br />
                                            <br />
                                            Blade Thickness <asp:TextBox ID="BladeThickness2" class="newBTB" runat="server" ></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Button id="BladesSubmit2" CssClass="btn" Height="25px" width="55px" runat="server" type="button" Text="Save" style="float: left;margin-right:10px;margin-left:280px" />
                                            <asp:Button id="BladesCancel2" CssClass="btn" Height="25px" width="55px" runat="server" type="button" Text="Cancel" style="float: left" onclick="BladesAdd_Cancel"/>
                                        </div>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>



                            <ajaxToolkit:TabPanel ID="RD" runat="server" HeaderText="Update Rework Blade Status">
                                 <ContentTemplate>
                                    <br />
                                    <div style="width:100%;" class="auto-style1">
                                        <strong>Select blade below to update its status
                                        <br />
                                        Click Save or Cancel when Complete.</strong>
                                    </div>
                                    <br />
                                    Blade Serial Number <asp:DropDownList ID="ReworkBladeSerial" runat="server" DataSourceID="Saws_AllReworkNullDateIn" DataTextField="SerialNumber" DataValueField="SerialNumber" CssClass="custom">
                                                        </asp:DropDownList>
                                    <asp:Button ID="DisplayReworkBtn" runat="server" Height="25px" Width="55px" CssClass="btn" type="button" Text="Display" OnClick="DisplayRework" autopostback="false"/>
                                    <br />
                                    <br />
                                     
                                    <div id="ReworkBladeDetails" runat="server" style="display:none; background-color:lightgrey; padding:20px 30px 20px 20px; margin:0px 0px 100px 30px;border-radius:20px;box-shadow: 0px 5px 5px 5px">
                                    Date Out <asp:TextBox ID="ReworkDateOut" CssClass="RDText" runat="server" Enabled="False"></asp:TextBox>
                                    <br />
                                    <br />
                                    Reason Out <asp:TextBox ID="ReworkReasonOut" CssClass="RDText" runat="server" Enabled="False"></asp:TextBox>
                                    <br />
                                    <br />
                                    Date In <asp:TextBox ID="ReworkDateIn" CssClass="RDText" runat="server" ></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender runat="server" ID="ReworkCalendar" TargetControlID="ReworkDateIn" Format ="yyyy-MM-dd HH:mm:ss:fff"></ajaxToolkit:CalendarExtender>
                                    <br />
                                    <br />
                                    Blade Status  <asp:DropDownList ID="ReworkStatusList" runat="server" 
                                                            CssClass="RDText" AppendDataBoundItems="True"
                                                            Enabled="false">
                                                            <asp:ListItem Enabled="true" Text="Inventory" Value="0"></asp:ListItem>
                                                    </asp:DropDownList> 
                                    <br />
                                    <br />
                                    Core Thickness <asp:TextBox ID="ReworkThickness" CssClass="RDText" runat="server"></asp:TextBox>
                                    <br />
                                    <br />
                                    Core Run Out <asp:TextBox ID="ReworkRunOut" CssClass="RDText" runat="server"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Button id="ReworkSubmit" CssClass="btn" Height="25px" width="55px" runat="server" Text="Save" style="float: left;margin-right:10px; margin-left:88px" OnClick="BringSawBack" />
                                    <asp:Button id="ReworkCancel" CssClass="btn" Height="25px" width="55px" runat="server" type="button" Text="Cancel" style="float: left" onclick="ReworkClearMssg"/>
                                    <asp:Button id="ReworkScrapBut" CssClass="btn" Height="25px" width="85px" runat="server" Text="Scrap Blade" style="float: left;margin-right:10px; margin-left:10px" OnClick="ReworkScrap" />
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>

                            </ajaxToolkit:TabContainer>
                        </ContentTemplate>                    
                </ajaxToolkit:TabPanel>

            
            </ajaxToolkit:TabContainer>
        </div>
    </form>
</body>
</html>