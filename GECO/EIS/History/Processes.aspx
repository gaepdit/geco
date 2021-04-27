<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Processes"
    Inherits="GECO.EIS_History_Processes" CodeBehind="Processes.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>
        <a href="<%= Page.ResolveUrl("~/EIS/History/") %>" class="no-visited">Historical Data</a>
        / Processes
    </h2>

    <p id="ProcessesEmptyNotice" runat="server" visible="False">
        No emission units exist for this facility in the EIS.
    </p>
    <asp:Button ID="ProcessesExport" runat="server" Text="Download as Excel" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="Processes" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="Emission Unit ID" HeaderText="Emission Unit ID" />
            <asp:BoundField DataField="Unit Status" HeaderText="Unit Status" />
            <asp:BoundField DataField="Process ID" HeaderText="Process ID" />
            <asp:BoundField DataField="Process Description" HeaderText="Process Description" />
            <asp:BoundField DataField="SCC" HeaderText="SCC" />
            <asp:BoundField DataField="SCC Description" HeaderText="SCC Description" />
            <asp:BoundField DataField="Last EPA Submittal" HeaderText="Last EPA Submittal"
                DataFormatString="{0:d}" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="Comment" HeaderText="Comment" />
        </Columns>
    </asp:GridView>
</asp:Content>
