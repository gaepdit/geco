﻿<%@ Page Title="GECO Emissions Inventory Historical Data: Reporting Period Pollutant Details" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" CodeBehind="ReportingPeriodPollutants.aspx.vb" Inherits="GECO.EIS_History_ReportingPeriodPollutants" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <style>
        table caption {
            text-align: left;
        }
    </style>
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

    <h3>Reporting Period Pollutant Details</h3>

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
