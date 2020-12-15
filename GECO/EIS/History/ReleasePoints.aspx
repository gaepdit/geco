<%@ Page Title="GECO Emissions Inventory Historical Data: Release Points" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" CodeBehind="ReleasePoints.aspx.vb" Inherits="GECO.EIS_History_ReleasePoints" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <a href="Default.aspx" class="button">← Back to EIS Reports</a>

    <h1>Emissions Inventory Historical Data: Release Points</h1>

    <ul>
        <li><a href="#fugitive">Fugitive Release Points</a></li>
        <li><a href="#stack">Stack Release Points</a></li>
    </ul>

    <h2 id="fugitive">Fugitive Release Points</h2>
    <p id="FugitivesEmptyNotice" runat="server" visible="False">
        No fugitive release points exist for this facility in the EIS.
    </p>
    <asp:Button ID="FugitivesExport" runat="server" Text="Download as Excel" CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="Fugitives" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="ReleasePointID" HeaderText="Release Point ID" />
            <asp:BoundField DataField="strRPDescription" HeaderText="Description" />
            <asp:BoundField DataField="numRPFugitiveHeightMeasure" HeaderText="Fugitive Height (ft)" />
            <asp:BoundField DataField="numRPFugitiveWidthMeasure" HeaderText="Fugitive Width (ft)" />
            <asp:BoundField DataField="numRPFugitiveLengthMeasure" HeaderText="Fugitive Length (ft)" />
            <asp:BoundField DataField="numRPFugitiveAngleMeasure" HeaderText="Fugitive Angle (0&deg; to 89&deg;)" />
            <asp:BoundField DataField="numRPFencelineDistMeasure" HeaderText="Fenceline Distance (ft)" />
            <asp:BoundField DataField="strRPStatusCode" HeaderText="Operating Status" />
            <asp:BoundField DataField="numLatitudeMeasure" HeaderText="Latitude" />
            <asp:BoundField DataField="numLongitudeMeasure" HeaderText="Longitude" />
            <asp:BoundField DataField="intHorAccuracyMeasure" HeaderText="Horiz Accuracy Measure (m)" />
            <asp:BoundField DataField="HorCollMetDesc" HeaderText="Horiz Collection Method" />
            <asp:BoundField DataField="HorRefDatumDesc" HeaderText="Horiz Reference Datum" />
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}" HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="strRPComment" HeaderText="Fugitive Comment" />
            <asp:BoundField DataField="strGeographicComment" HeaderText="Geo Coord Comment" />
        </Columns>
    </asp:GridView>

    <h2 id="stack">Stack Release Points</h2>
    <p id="StacksEmptyNotice" runat="server" visible="False">
        No stacks exist for this facility in the EIS.
    </p>
    <asp:Button ID="StacksExport" runat="server" Text="Download as Excel"
        CausesValidation="False" UseSubmitBehavior="False" />
    <asp:GridView ID="Stacks" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-striped">
        <Columns>
            <asp:BoundField DataField="ReleasePointID" HeaderText="Release Point ID" />
            <asp:BoundField DataField="strRPDescription" HeaderText="Description" />
            <asp:BoundField DataField="RPTypeDesc" HeaderText="Stack Type" SortExpression="RPTypecode" />
            <asp:BoundField DataField="numRPStackHeightMeasure" HeaderText="Stack Height (ft)" />
            <asp:BoundField DataField="numRPStackDiameterMeasure" HeaderText="Stack Diameter (ft)" />
            <asp:BoundField DataField="numRPExitGasVelocityMeasure" HeaderText="Exit Gas Velocity (fps)" />
            <asp:BoundField DataField="numRPExitGasFlowRateMeasure" HeaderText="Exit Gas Flow Rate (acfs)" />
            <asp:BoundField DataField="numRPExitGasTempMeasure" HeaderText="Exit Gas Temp (&deg;F)" />
            <asp:BoundField DataField="numRPFencelineDistMeasure" HeaderText="Fenceline Distance (ft)" />
            <asp:BoundField DataField="RPStatusDesc" HeaderText="Operating Status" />
            <asp:BoundField DataField="numLatitudeMeasure" HeaderText="Latitude" />
            <asp:BoundField DataField="numLongitudeMeasure" HeaderText="Longitude" />
            <asp:BoundField DataField="intHorAccuracyMeasure" HeaderText="Horiz Accuracy Measure (m)" />
            <asp:BoundField DataField="HorCollMetDesc" HeaderText="Horiz Collection Method" />
            <asp:BoundField DataField="HorRefDatumDesc" HeaderText="Horiz Reference Datum" />
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}" HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted" />
            <asp:BoundField DataField="strRPComment" HeaderText="Stack Comment" />
            <asp:BoundField DataField="strGeographicComment" HeaderText="Geo Coord Comment" />
        </Columns>
    </asp:GridView>
</asp:Content>
