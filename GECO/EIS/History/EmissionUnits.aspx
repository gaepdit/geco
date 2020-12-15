<%@ Page Title="GECO Emissions Inventory Historical Data: Emission Units" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" CodeBehind="EmissionUnits.aspx.vb" Inherits="GECO.EIS_History_EmissionUnits" %>

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

    <h3>Emission Units</h3>

    <p id="EmissionUnitsEmptyNotice" runat="server" visible="False">
        No emission units exist for this facility in the EIS.
    </p>
    <asp:Button ID="EmissionUnitsExport" runat="server" Text="Download as Excel" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="EmissionUnits" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="EMISSIONSUNITID" HeaderText="Emission Unit ID" />
            <asp:BoundField DataField="STRUNITDESCRIPTION" HeaderText="Description" />
            <asp:BoundField DataField="strUnitType" HeaderText="Unit Type" />
            <asp:BoundField DataField="FLTUNITDESIGNCAPACITY" HeaderText="Design Capacity" />
            <asp:BoundField DataField="STRUNITDESIGNCAPACITYUOMCODE" HeaderText="Design Capacity Unit" />
            <asp:BoundField DataField="NUMMAXIMUMNAMEPLATECAPACITY" HeaderText="Max Nameplate Capacity (MW)" NullDisplayText="Non Elec Gen" />
            <asp:BoundField DataField="DATUNITOPERATIONDATE" HeaderText="Placed in Operation" SortExpression="DATUNITOPERATIONDATE" DataFormatString="{0:d}" />
            <asp:BoundField DataField="strUnitStatusCode" HeaderText="Operating Status" NullDisplayText="No Data" />
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}" HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="STRUNITCOMMENT" HeaderText="Comment" />
        </Columns>
    </asp:GridView>
</asp:Content>
