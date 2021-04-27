<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Emission Units"
    Inherits="GECO.EIS_History_EmissionUnits" CodeBehind="EmissionUnits.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>
        <a href="<%= Page.ResolveUrl("~/EIS/History/") %>" class="no-visited">Historical Data</a>
        / Emission Units
    </h2>

    <p id="EmissionUnitsEmptyNotice" runat="server" visible="False">
        No emission units exist for this facility in the EIS.
    </p>
    <asp:Button ID="EmissionUnitsExport" runat="server" Text="Download as Excel" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="EmissionUnits" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="Emission Unit ID" HeaderText="Emission Unit ID" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Unit Type" HeaderText="Unit Type" />
            <asp:BoundField DataField="Design Capacity" HeaderText="Design Capacity" />
            <asp:BoundField DataField="Design Capacity Units" HeaderText="Design Capacity Units" />
            <asp:BoundField DataField="Max Nameplate Capacity (MW)" HeaderText="Max Nameplate Capacity (MW)"
                NullDisplayText="Non Elec Gen" />
            <asp:BoundField DataField="Placed in Operation" HeaderText="Placed in Operation"
                DataFormatString="{0:d}" />
            <asp:BoundField DataField="Operating Status" HeaderText="Operating Status"
                NullDisplayText="No Data" />
            <asp:BoundField DataField="Last EPA Submittal" HeaderText="Last EPA Submittal"
                DataFormatString="{0:d}" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="Comment" HeaderText="Comment" />
        </Columns>
    </asp:GridView>
</asp:Content>
