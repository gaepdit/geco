<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Reporting Period Emissions" 
    Inherits="GECO.EIS_History_ReportingPeriodEmissions" CodeBehind="ReportingPeriodEmissions.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>Historical Data</h2>
    <p><a href="Default.aspx" class="">← All Reports</a></p>

    <h3>Reporting Period Emissions</h3>

    <asp:UpdatePanel ID="YearUpdate" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ReportingPeriodExport" />
        </Triggers>
        <ContentTemplate>
            <p>
                <asp:Label ID="YearLabel" runat="server" Text="Select Inventory Year" AssociatedControlID="Years"></asp:Label>
                <asp:DropDownList ID="Years" runat="server" CssClass="input-small"></asp:DropDownList>
                <asp:Button ID="YearButton" runat="server" Text="Go" />
                <asp:Button ID="ReportingPeriodExport" runat="server" Text="Download as Excel" CausesValidation="False" />
            </p>

            <p id="ReportingPeriodEmptyNotice" runat="server" visible="False">
                No data exists for the selected year.
            </p>
            <asp:GridView ID="ReportingPeriod" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
                <Columns>
                    <asp:BoundField DataField="Pollutant" HeaderText="Pollutant" />
                    <asp:BoundField DataField="Emissions (tons)" HeaderText="Emissions (tons)" ItemStyle-HorizontalAlign="Right" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
