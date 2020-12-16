<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Processes" 
    Inherits="GECO.EIS_History_Processes" CodeBehind="Processes.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
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
