<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EI Historical Data: Reporting Period Emissions"
    Inherits="GECO.EIS_History_ReportingPeriodEmissions" CodeBehind="ReportingPeriodEmissions.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>
        <a href="<%= Page.ResolveUrl("~/EIS/History/") %>" class="no-visited">Historical Data</a>
        / Reporting Period Emissions
    </h2>

    <asp:UpdatePanel ID="YearUpdate" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ReportingPeriodExport" />
        </Triggers>
        <ContentTemplate>
            <div id="dNoDataExists" runat="server" visible="false">
                <p>No historical data exists.</p>
            </div>
            <div id="dDataExists" runat="server">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
