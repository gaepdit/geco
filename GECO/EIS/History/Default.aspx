<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data"
    Inherits="GECO.EIS_History_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>Historical Data</h2>

    <p>
        Historical emissions inventory data is available on this site for inventory years 2008 through 2018.
        Reports can be printed and exported to Microsoft Excel.
    </p>

    <h3>Facility Inventory Reports</h3>
    <ul>
        <li>
            <p><a href="ReleasePoints.aspx">Release Points</a></p>
        </li>
        <li>
            <p><a href="EmissionUnits.aspx">Emission Units</a></p>
        </li>
        <li>
            <p><a href="Processes.aspx">Processes</a></p>
        </li>
    </ul>

    <h3>Reporting Period Reports</h3>
    <ul>
        <li>
            <p><a href="ReportingPeriodProcesses.aspx">Processes</a></p>
        </li>
        <li>
            <p><a href="ReportingPeriodPollutants.aspx">Pollutants</a></p>
        </li>
        <li>
            <p><a href="ReportingPeriodEmissions.aspx">Emissions</a></p>
        </li>
    </ul>
</asp:Content>
