<%@ Page Title="GECO Emissions Inventory Historical Data: Processes" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" CodeBehind="Processes.aspx.vb" Inherits="GECO.EIS_History_Processes" %>

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

    <h3>Processes</h3>

    <p id="ProcessesEmptyNotice" runat="server" visible="False">
        No emission units exist for this facility in the EIS.
    </p>
    <asp:Button ID="ProcessesExport" runat="server" Text="Download as Excel" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="Processes" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID" />
            <asp:BoundField DataField="strUnitDesc" HeaderText="Unit Status" />
            <asp:BoundField DataField="ProcessID" HeaderText="Process ID" />
            <asp:BoundField DataField="strProcessDescription" HeaderText="Process Description" />
            <asp:BoundField DataField="SourceClassCode" HeaderText="SCC" />
            <asp:BoundField DataField="strSCCDesc" HeaderText="SCC Description" />
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}" HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="strProcessComment" HeaderText="Comment" />
        </Columns>
    </asp:GridView>
</asp:Content>
