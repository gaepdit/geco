<%@ Page Title="GECO Emissions Inventory Historical Data: Reporting Period Emissions" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" CodeBehind="ReportingPeriodEmissions.aspx.vb" Inherits="GECO.EIS_History_ReportingPeriodEmissions" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <a href="Default.aspx" class="button">← Back to EIS Reports</a>

    <h1>Emissions Inventory Historical Data: Reporting Period Emissions</h1>

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
