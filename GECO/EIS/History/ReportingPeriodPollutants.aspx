<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Reporting Period Pollutant Details"
    Inherits="GECO.EIS_History_ReportingPeriodPollutants" CodeBehind="ReportingPeriodPollutants.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>
        <a href="<%= Page.ResolveUrl("~/EIS/History/") %>" class="no-visited">Historical Data</a>
        / Reporting Period Pollutant Details
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
                <asp:GridView ID="ReportingPeriod" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped"
                    Caption="* Summer Day emissions = emissions on an average summer day May 1 through Sep 30. Units are in tons per day (TPD).">
                    <Columns>
                        <asp:BoundField DataField="Inventory Year" HeaderText="Inventory Year" />
                        <asp:BoundField DataField="Emissions Unit ID" HeaderText="Emissions Unit ID" />
                        <asp:BoundField DataField="Emissions Unit Desc" HeaderText="Emissions Unit Desc" />
                        <asp:BoundField DataField="Process ID" HeaderText="Process ID" />
                        <asp:BoundField DataField="Process Desc" HeaderText="Process Desc" />
                        <asp:BoundField DataField="Pollutant" HeaderText="Pollutant" />
                        <asp:BoundField DataField="Pollutant Period" HeaderText="Pollutant Period" />
                        <asp:BoundField DataField="Total Emissions Units" HeaderText="Total Emissions Units" />
                        <asp:BoundField DataField="Total Emissions" HeaderText="Total Emissions" />
                        <asp:BoundField DataField="Emission Factor" HeaderText="Emission Factor" />
                        <asp:BoundField DataField="Emissions Factor Units" HeaderText="Emissions Factor Units" />
                        <asp:BoundField DataField="Emission Calculation Method" HeaderText="Emission Calculation Method" />
                        <asp:BoundField DataField="Emission Factor Numerator" HeaderText="Emission Factor Numerator" />
                        <asp:BoundField DataField="Emission Factor Denominator" HeaderText="Emission Factor Denominator" />
                        <asp:BoundField DataField="Emission Factor Text" HeaderText="Emission Factor Text" />
                        <asp:BoundField DataField="Emissions Comment" HeaderText="Emissions Comment" />
                        <asp:BoundField DataField="Last EPA Submittal" HeaderText="Last EPA Submittal" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
