<%@ Page Title="Report - Pollutant Details" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_report_pollutantdetails" CodeBehind="report_pollutantdetails.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        Reporting Period Pollutant Details
        <asp:Button ID="btnReportsHome_Pollutant" runat="server" Text="Reports Home" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteID_Pollutant" class="styled" runat="server" Text="Facility ID"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteID_Pollutant" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilityName_Pollutant" class="styled" runat="server" Text="Facility Name"></asp:Label>
        <asp:TextBox ID="txtFacilityName_Pollutant" class="readonly" runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div>
        <div class="fieldwrapper">
            <asp:Label ID="lblInventoryYear_Pollutant" CssClass="styled" runat="server" Text="Inventory Year"></asp:Label>
            <asp:DropDownList ID="ddlInventoryYear_Pollutant" runat="server" class="" AutoPostBack="True">
            </asp:DropDownList>
            &nbsp;
            <asp:Button ID="btnGO_Pollutant" runat="server" Text="GO" />
            <asp:RequiredFieldValidator ID="reqvYear_Pollutant" runat="server" ControlToValidate="ddlInventoryYear_Pollutant"
                Display="Dynamic" ErrorMessage="* Select a year" InitialValue="-Select Year-"></asp:RequiredFieldValidator>
        </div>
        <br />
        <div style="text-align: center;">
            <p>
                <asp:Button ID="btnExport_Pollutant" runat="server" Text="Download as Excel" CausesValidation="False"
                    CssClass="summarybutton" UseSubmitBehavior="False" Visible="False" />
            </p>
            <asp:Label ID="lblEmptygvwPollutantDetails" runat="server" Visible="False" ForeColor="#CC0000"
                Font-Bold="True" Font-Size="Medium"></asp:Label>
        </div>
        <div class="gridview">
            <asp:GridView ID="gvwPollutantDetails" runat="server" AutoGenerateColumns="False"
                DataKeyNames="FacilitySiteID" HorizontalAlign="Center"
                ForeColor="#333333" Caption="Reporting Period Pollutant Details"
                CssClass="reportview" EnableModelValidation="True">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="EMISSIONSUNITID" HeaderText="Emissions Unit ID" SortExpression="EMISSIONSUNITID" />
                    <asp:BoundField DataField="STRUNITDESCRIPTION" HeaderText="Emissisons Unit Desc"
                        SortExpression="STRUNITDESCRIPTION" />
                    <asp:BoundField DataField="PROCESSID" HeaderText="Process ID" SortExpression="PROCESSID" />
                    <asp:BoundField DataField="STRPROCESSDESCRIPTION" HeaderText="Process Desc" SortExpression="STRPROCESSDESCRIPTION" />
                    <asp:BoundField DataField="STRPOLLUTANT" HeaderText="Pollutant" SortExpression="STRPOLLUTANT" />
                    <asp:BoundField DataField="RPTPeriodType" HeaderText="Pollutant Period">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FLTTOTALEMISSIONS" HeaderText="Total Emissions" SortExpression="FLTTOTALEMISSIONS">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PollutantUnit">
                        <ItemStyle HorizontalAlign="Left" Width="30px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FLTEMISSIONFACTOR" HeaderText="Emission Factor" SortExpression="FLTEMISSIONFACTOR" />
                    <asp:BoundField DataField="EFUNITS" HeaderText="Emission Facotr Units"
                        SortExpression="EFUNITS" />
                    <asp:BoundField DataField="STREMCALCMETHOD" HeaderText="Emission Calculation Method"
                        SortExpression="STREMCALCMETHOD" />
                    <asp:BoundField DataField="EFNUMDESC" HeaderText="Emission Factor Numerator" SortExpression="EFNUMDESC" />
                    <asp:BoundField DataField="EFDENDESC" HeaderText="Emission Factor Denominator" SortExpression="EFDENDESC" />
                    <asp:BoundField DataField="STREMISSIONFACTORTEXT" HeaderText="Emission Factor Text"
                        SortExpression="STREMISSIONFACTORTEXT" />
                    <asp:BoundField DataField="STREMISSIONSCOMMENT" HeaderText="Emissions Comment" SortExpression="STREMISSIONSCOMMENT">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UPDATEUSER" HeaderText="Update User" SortExpression="UPDATEUSER" />
                    <asp:BoundField DataField="UPDATEDATETIME" HeaderText="Update Date & Time" SortExpression="UPDATEDATETIME" />
                    <asp:BoundField DataField="LASTEISSUBMITDATE" HeaderText="Last Submit Date" SortExpression="LASTEISSUBMITDATE" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:Label ID="lblPollutantDetails" runat="server" Font-Size="Small"></asp:Label>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
