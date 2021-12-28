<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EI Historical Data: Reporting Period Process Details"
    Inherits="GECO.EIS_History_ReportingPeriodProcesses" CodeBehind="ReportingPeriodProcesses.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>
        <a href="<%= Page.ResolveUrl("~/EIS/History/") %>" class="no-visited">Historical Data</a>
        / Reporting Period Process Details
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
                        <asp:BoundField DataField="Emissions Unit ID" HeaderText="Emissions Unit ID" />
                        <asp:BoundField DataField="Emissions Unit Desc" HeaderText="Emissions Unit Desc" />
                        <asp:BoundField DataField="Process ID" HeaderText="Process ID" />
                        <asp:BoundField DataField="Process Desc" HeaderText="Process Desc" />
                        <asp:BoundField DataField="Calculation Parameter Value" HeaderText="Calculation Parameter Value" />
                        <asp:BoundField DataField="Calculation Parameter Unit of Measure" HeaderText="Calculation Parameter Unit of Measure" />
                        <asp:BoundField DataField="Calculation Parameter Type Desc" HeaderText="Calculation Parameter Type Desc" />
                        <asp:BoundField DataField="Material Desc" HeaderText="Material Desc" />
                        <asp:BoundField DataField="Reporting Period Comment" HeaderText="Reporting Period Comment" />
                        <asp:BoundField DataField="Actual Hours Per Period" HeaderText="Actual Hours Per Period" />
                        <asp:BoundField DataField="Average Days Per Week" HeaderText="Average Days Per Week" />
                        <asp:BoundField DataField="Average Hours per Day" HeaderText="Average Hours per Day" />
                        <asp:BoundField DataField="Average Weeks per Period" HeaderText="Average Weeks per Period" />
                        <asp:BoundField DataField="Winter Activity %" HeaderText="Winter Activity %" />
                        <asp:BoundField DataField="Spring Activity %" HeaderText="Spring Activity %" />
                        <asp:BoundField DataField="Summer Activity %" HeaderText="Summer Activity %" />
                        <asp:BoundField DataField="Fall Activity %" HeaderText="Fall Activity %" />
                        <asp:BoundField DataField="Heat Content" HeaderText="Heat Content" />
                        <asp:BoundField DataField="Heat Content Numerator" HeaderText="Heat Content Numerator" />
                        <asp:BoundField DataField="Heat Content Denominator" HeaderText="Heat Content Denominator" />
                        <asp:BoundField DataField="Ash Content" HeaderText="Ash Content" />
                        <asp:BoundField DataField="Sulfur Content" HeaderText="Sulfur Content" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
