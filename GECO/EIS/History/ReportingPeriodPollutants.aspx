<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Reporting Period Pollutant Details" 
    Inherits="GECO.EIS_History_ReportingPeriodPollutants" CodeBehind="ReportingPeriodPollutants.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>
        <a href="<%= Page.ResolveUrl("~/EIS/History/") %>" class="no-visited">Historical Data</a>
        / Reporting Period Pollutant Details
    </h2>

    <asp:UpdatePanel ID="YearUpdate" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ReportingPeriodExport" />
        </Triggers>
        <ContentTemplate>
            <p>
                <asp:Label ID="YearLabel" runat="server" Text="Select Inventory Year" AssociatedControlID="Years"></asp:Label>
                <asp:DropDownList ID="Years" runat="server" CssClass="input-small"></asp:DropDownList>
                <asp:Button ID="YearButton" runat="server" Text="Go" />
                <asp:Button ID="ReportingPeriodExport" runat="server" Text="Download as Excel" CausesValidation="False" />
            </p>

            <p id="ReportingPeriodEmptyNotice" runat="server" visible="False">
                No data exists for the selected year.
            </p>
            <asp:GridView ID="ReportingPeriod" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped"
                Caption="* Summer Day emissions = emissions on an average summer day May 1 through Sep 30. Units are in tons per day (TPD).">
                <Columns>
                    <asp:BoundField DataField="EMISSIONSUNITID" HeaderText="Emissions Unit ID" />
                    <asp:BoundField DataField="STRUNITDESCRIPTION" HeaderText="Emissisons Unit Desc" />
                    <asp:BoundField DataField="PROCESSID" HeaderText="Process ID" />
                    <asp:BoundField DataField="STRPROCESSDESCRIPTION" HeaderText="Process Desc" />
                    <asp:BoundField DataField="STRPOLLUTANT" HeaderText="Pollutant" />
                    <asp:BoundField DataField="RPTPeriodType" HeaderText="Pollutant Period" />
                    <asp:BoundField DataField="FLTTOTALEMISSIONS" HeaderText="Total Emissions" />
                    <asp:BoundField DataField="PollutantUnit" />
                    <asp:BoundField DataField="FLTEMISSIONFACTOR" HeaderText="Emission Factor" />
                    <asp:BoundField DataField="EFUNITS" HeaderText="Emission Facotr Units" />
                    <asp:BoundField DataField="STREMCALCMETHOD" HeaderText="Emission Calculation Method" />
                    <asp:BoundField DataField="EFNUMDESC" HeaderText="Emission Factor Numerator" />
                    <asp:BoundField DataField="EFDENDESC" HeaderText="Emission Factor Denominator" />
                    <asp:BoundField DataField="STREMISSIONFACTORTEXT" HeaderText="Emission Factor Text" />
                    <asp:BoundField DataField="STREMISSIONSCOMMENT" HeaderText="Emissions Comment" />
                    <asp:BoundField DataField="UPDATEUSER" HeaderText="Update User" />
                    <asp:BoundField DataField="UPDATEDATETIME" HeaderText="Update Date & Time" />
                    <asp:BoundField DataField="LASTEISSUBMITDATE" HeaderText="Last Submit Date" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
