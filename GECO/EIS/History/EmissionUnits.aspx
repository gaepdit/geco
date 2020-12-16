<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Emission Units" 
    Inherits="GECO.EIS_History_EmissionUnits" CodeBehind="EmissionUnits.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
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
