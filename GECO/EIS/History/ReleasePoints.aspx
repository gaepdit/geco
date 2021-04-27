<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO EIS Historical Data: Release Points"
    Inherits="GECO.EIS_History_ReleasePoints" CodeBehind="ReleasePoints.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <h2>
        <a href="<%= Page.ResolveUrl("~/EIS/History/") %>" class="no-visited">Historical Data</a>
        / Release Points
    </h2>

    <ul>
        <li><a href="#fugitive" class="no-visited">Fugitive Release Points</a></li>
        <li><a href="#stack" class="no-visited">Stack Release Points</a></li>
    </ul>

    <h3 id="fugitive">Fugitive Release Points</h3>
    <p id="FugitivesEmptyNotice" runat="server" visible="False">
        No fugitive release points exist for this facility in the EIS.
    </p>
    <asp:Button ID="FugitivesExport" runat="server" Text="Download as Excel" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="Fugitives" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="Release Point ID" HeaderText="Release Point ID" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Fugitive Height (ft)" HeaderText="Fugitive Height (ft)" />
            <asp:BoundField DataField="Fugitive Width (ft)" HeaderText="Fugitive Width (ft)" />
            <asp:BoundField DataField="Fugitive Length (ft)" HeaderText="Fugitive Length (ft)" />
            <asp:BoundField DataField="Fugitive Angle (0&deg; to 89&deg;)" HeaderText="Fugitive Angle (0&deg; to 89&deg;)" />
            <asp:BoundField DataField="Fenceline Distance (ft)" HeaderText="Fenceline Distance (ft)" />
            <asp:BoundField DataField="Operating Status" HeaderText="Operating Status" />
            <asp:BoundField DataField="Latitude" HeaderText="Latitude" />
            <asp:BoundField DataField="Longitude" HeaderText="Longitude" />
            <asp:BoundField DataField="Horiz Accuracy Measure (m)" HeaderText="Horiz Accuracy Measure (m)" />
            <asp:BoundField DataField="Horiz Collection Method" HeaderText="Horiz Collection Method" />
            <asp:BoundField DataField="Horiz Reference Datum" HeaderText="Horiz Reference Datum" />
            <asp:BoundField DataField="Last EPA Submittal" HeaderText="Last EPA Submittal"
                DataFormatString="{0:d}" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="Fugitive Comment" HeaderText="Fugitive Comment" />
            <asp:BoundField DataField="Geo Coord Comment" HeaderText="Geo Coord Comment" />
        </Columns>
    </asp:GridView>

    <h3 id="stack">Stack Release Points</h3>
    <p id="StacksEmptyNotice" runat="server" visible="False">
        No stacks exist for this facility in the EIS.
    </p>
    <asp:Button ID="StacksExport" runat="server" Text="Download as Excel"
        CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="Stacks" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="Release Point ID" HeaderText="Release Point ID" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Stack Type" HeaderText="Stack Type" />
            <asp:BoundField DataField="Stack Height (ft)" HeaderText="Stack Height (ft)" />
            <asp:BoundField DataField="Stack Diameter (ft)" HeaderText="Stack Diameter (ft)" />
            <asp:BoundField DataField="Exit Gas Velocity (fps)" HeaderText="Exit Gas Velocity (fps)" />
            <asp:BoundField DataField="Exit Gas Flow Rate (acfs)" HeaderText="Exit Gas Flow Rate (acfs)" />
            <asp:BoundField DataField="Exit Gas Temp (&deg;F)" HeaderText="Exit Gas Temp (&deg;F)" />
            <asp:BoundField DataField="Fenceline Distance (ft)" HeaderText="Fenceline Distance (ft)" />
            <asp:BoundField DataField="Operating Status" HeaderText="Operating Status" />
            <asp:BoundField DataField="Latitude" HeaderText="Latitude" />
            <asp:BoundField DataField="Longitude" HeaderText="Longitude" />
            <asp:BoundField DataField="Horiz Accuracy Measure (m)" HeaderText="Horiz Accuracy Measure (m)" />
            <asp:BoundField DataField="Horiz Collection Method" HeaderText="Horiz Collection Method" />
            <asp:BoundField DataField="Horiz Reference Datum" HeaderText="Horiz Reference Datum" />
            <asp:BoundField DataField="Last EPA Submittal" HeaderText="Last EPA Submittal"
                DataFormatString="{0:d}" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="Stack Comment" HeaderText="Stack Comment" />
            <asp:BoundField DataField="Geo Coord Comment" HeaderText="Geo Coord Comment" />
        </Columns>
    </asp:GridView>
</asp:Content>
