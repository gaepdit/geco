<%@ Page Title="GECO Emissions Inventory Historical Data" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" Inherits="GECO.EIS_History_Default" CodeBehind="Default.aspx.vb" %>

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
            <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl="~/EIS/History/" Enabled="false" CssClass="selected-menu-item disabled">Historical Data</asp:HyperLink>
        </li>
    </ul>

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
