<%@ Page Title="GECO Emissions Inventory Historical Data: Emission Units" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" CodeBehind="EmissionUnits.aspx.vb" Inherits="GECO.EIS_History_EmissionUnits" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <a href="Default.aspx" class="button">← Back to Reports List</a>

    <h1>Emissions Inventory Historical Data: Emission Units</h1>

    <p id="EmissionUnitsEmptyNotice" runat="server" visible="False">
        No emission units exist for this facility in the EIS.
    </p>
    <asp:Button ID="EmissionUnitsExport" runat="server" Text="Download as Excel" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="EmissionUnits" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="EMISSIONSUNITID" HeaderText="Emission Unit ID"></asp:BoundField>
            <asp:BoundField DataField="STRUNITDESCRIPTION" HeaderText="Description"></asp:BoundField>
            <asp:BoundField DataField="strUnitType" HeaderText="Unit Type"></asp:BoundField>
            <asp:BoundField DataField="FLTUNITDESIGNCAPACITY" HeaderText="Design Capacity"></asp:BoundField>
            <asp:BoundField DataField="STRUNITDESIGNCAPACITYUOMCODE" HeaderText="Design Capacity Unit"></asp:BoundField>
            <asp:BoundField DataField="NUMMAXIMUMNAMEPLATECAPACITY" HeaderText="Max Nameplate Capacity (MW)" NullDisplayText="Non Elec Gen"></asp:BoundField>
            <asp:BoundField DataField="DATUNITOPERATIONDATE" HeaderText="Placed in Operation" SortExpression="DATUNITOPERATIONDATE" DataFormatString="{0:d}"></asp:BoundField>
            <asp:BoundField DataField="strUnitStatusCode" HeaderText="Operating Status" NullDisplayText="No Data"></asp:BoundField>
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}" HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted"></asp:BoundField>
            <asp:BoundField DataField="STRUNITCOMMENT" HeaderText="Comment"></asp:BoundField>
        </Columns>
    </asp:GridView>
</asp:Content>
