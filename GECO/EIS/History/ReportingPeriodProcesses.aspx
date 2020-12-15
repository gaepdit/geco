﻿<%@ Page Title="GECO Emissions Inventory Historical Data: Reporting Period Process Details" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" CodeBehind="ReportingPeriodProcesses.aspx.vb" Inherits="GECO.EIS_History_ReportingPeriodProcesses" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h1>Emissions Inventory System</h1>

    <p>
        <b>
            <asp:Label ID="lblFacilityDisplay" runat="server"></asp:Label></b>
        <br />
        AIRS Number:
        <asp:Label ID="lblAIRS" runat="server"></asp:Label>
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkEisHome" runat="server" NavigateUrl="~/EIS/">EIS Home</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFacilityInfo" runat="server" NavigateUrl="~/EIS/Facility/">Edit Facility Info</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl="~/EIS/History/" CssClass="selected-menu-item">Historical Data</asp:HyperLink>
        </li>
    </ul>

    <h2>Historical Data</h2>
    <p><a href="Default.aspx" class="">← All Reports</a></p>

    <h3>Reporting Period Process Details</h3>

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
            <asp:GridView ID="ReportingPeriod" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
                <Columns>
                    <asp:BoundField DataField="EMISSIONSUNITID" HeaderText="Emissions Unit ID" />
                    <asp:BoundField DataField="STRUNITDESCRIPTION" HeaderText="Emissisons Unit Desc" />
                    <asp:BoundField DataField="PROCESSID" HeaderText="Process ID" />
                    <asp:BoundField DataField="STRPROCESSDESCRIPTION" HeaderText="Process Desc" />
                    <asp:BoundField DataField="FLTCALCPARAMETERVALUE" HeaderText="Calculation Parameter Value" />
                    <asp:BoundField DataField="STRCPUOMDESC" HeaderText="Calculation Parameter Unit of Measure" />
                    <asp:BoundField DataField="STRCPTYPEDESC" HeaderText="Calculation Parameter Type Desc" />
                    <asp:BoundField DataField="STRMATERIAL" HeaderText="Material Desc" />
                    <asp:BoundField DataField="STRREPORTINGPERIODCOMMENT" HeaderText="Reporting Period Comment" />
                    <asp:BoundField DataField="INTACTUALHOURSPERPERIOD" HeaderText="Actual Hours Per Period" />
                    <asp:BoundField DataField="NUMAVERAGEDAYSPERWEEK" HeaderText="Average Days Per Week" />
                    <asp:BoundField DataField="NUMAVERAGEHOURSPERDAY" HeaderText="Average Hours per Day" />
                    <asp:BoundField DataField="NUMAVERAGEWEEKSPERPERIOD" HeaderText="Average Weeks per Period" />
                    <asp:BoundField DataField="NUMPERCENTWINTERACTIVITY" HeaderText="Winter Activity %" />
                    <asp:BoundField DataField="NUMPERCENTSPRINGACTIVITY" HeaderText="Spring Activity %" />
                    <asp:BoundField DataField="NUMPERCENTSUMMERACTIVITY" HeaderText="Summer Activity %" />
                    <asp:BoundField DataField="NUMPERCENTFALLACTIVITY" HeaderText="Fall Activity %" />
                    <asp:BoundField DataField="HEATCONTENT" HeaderText="Heat Content" />
                    <asp:BoundField DataField="HCNUMER" HeaderText="Heat Content Numerator" />
                    <asp:BoundField DataField="HCDENOM" HeaderText="Heat Content Denominator" />
                    <asp:BoundField DataField="ASHCONTENT" HeaderText="Ash Content" />
                    <asp:BoundField DataField="SULFURCONTENT" HeaderText="Sullfur Content" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>