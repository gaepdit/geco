<%@ Page Title="GECO Emissions Inventory Historical Data" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" Inherits="GECO.EIS_History_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <a href="<%= Page.ResolveUrl("~/EIS/") %>" class="button">← Back to Emissions Inventory Page</a>

    <h1>Emissions Inventory Historical Data</h1>

    <p>
        Historical emissions inventory data is available on this site for inventory years 2008 through 2018.
        Reports can be printed and exported to Microsoft Excel.
    </p>

    <h3>Facility Inventory Reports</h3>
    <ul>
        <li><p><a href="ReleasePoints.aspx">Release Points</a></p></li>
        <li><p><a href="EmissionUnits.aspx">Emission Units</a></p></li>
        <li><p><a href="Processes.aspx">Processes</a></p></li>
    </ul>

    <h3>Reporting Period Reports</h3>
    <ul>
        <li><p><a href="ReportingPeriodProcesses.aspx">Process Reporting Period</a></p></li>
        <li><p><a href="ReportingPeriodPollutants.aspx">Pollutant Report</a></p></li>
        <li><p><a href="ReportingPeriodEmissions.aspx">Facility-Wide Emissions Summary</a></p></li>
    </ul>

</asp:Content>
