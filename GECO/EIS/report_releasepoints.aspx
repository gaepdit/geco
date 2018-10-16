<%@ Page Title="Report - Facility Release Points" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_report_releasepoints" CodeBehind="report_releasepoints.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        Release Points
        <asp:Button ID="btnReportsHome_ReleasePoints" runat="server" Text="Reports Home" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteID_ReleasePoints" class="styled" runat="server"
            Text="Facility ID"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteID_ReleasePoints" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilityName_ReleasePoints" class="styled" runat="server"
            Text="Facility Name"></asp:Label>
        <asp:TextBox ID="txtFacilityName_ReleasePoints" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <br />
    <div style="text-align: center;">
        <p>
            <asp:Button ID="btExport_Fugitives" runat="server" Text="Download as Excel" CausesValidation="False"
                CssClass="summarybutton" UseSubmitBehavior="False"
                Visible="False" ToolTip="Export fugitive release points" />
        </p>
        <asp:Label ID="lblEmptygvwFugitives" runat="server" Visible="False"
            ForeColor="#CC0000" Font-Bold="True" Font-Size="Medium"></asp:Label>
    </div>
    <asp:GridView ID="gvwFugitives"
        runat="server"
        AutoGenerateColumns="False"
        HorizontalAlign="Center"
        ForeColor="#333333"
        Width="95%"
        Caption="Fugitive Release Points" CssClass="reportview">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:BoundField DataField="ReleasePointID"
                HeaderText="Release Point ID"
                SortExpression="ReleasePointID">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strRPDescription"
                HeaderText="Description">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPFugitiveHeightMeasure" HeaderText="Fugitive Height (ft)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPFugitiveWidthMeasure" HeaderText="Fugitive Width (ft)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPFugitiveLengthMeasure"
                HeaderText="Fugitive Length (ft)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPFugitiveAngleMeasure"
                HeaderText="Fugitive Angle (0&deg; to 179&deg;)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPFencelineDistMeasure"
                HeaderText="Fenceline Distance (ft)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strRPStatusCode"
                HeaderText="Operating Status">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numLatitudeMeasure"
                HeaderText="Latitude">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numLongitudeMeasure"
                HeaderText="Longitude">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="intHorAccuracyMeasure"
                HeaderText="Horiz Accuracy Measure (m)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="HorCollMetDesc" HeaderText="Horiz Collection Method">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="150px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="HorRefDatumDesc" HeaderText="Horiz Reference Datum">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}"
                HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strRPComment" HeaderText="Fugitive Comment">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="200px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strGeographicComment" HeaderText="Geo Coord Comment">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="200px" VerticalAlign="Top" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <br />
    <div style="text-align: center;">
        <p>
            <asp:Button ID="btnExport_Stacks" runat="server" Text="Download as Excel" CausesValidation="False"
                CssClass="summarybutton" UseSubmitBehavior="False"
                Visible="False" ToolTip="Export stack release points" />
        </p>
    </div>
    <div style="text-align: center;">
        <asp:Label ID="lblEmptygvwStacks" runat="server" Visible="False"
            ForeColor="#CC0000" Font-Bold="True" Font-Size="Medium"></asp:Label>
    </div>
    <asp:GridView ID="gvwStacks"
        runat="server"
        AutoGenerateColumns="False"
        HorizontalAlign="Center"
        ForeColor="#333333"
        Width="95%"
        Caption="Stack Release Points" CssClass="reportview">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:BoundField DataField="ReleasePointID"
                HeaderText="Release Point ID"
                SortExpression="ReleasePointID">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strRPDescription"
                HeaderText="Description">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
            </asp:BoundField>
            <asp:BoundField DataField="RPTypeDesc" HeaderText="Stack Type"
                SortExpression="RPTypecode">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="120px" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPStackHeightMeasure" HeaderText="Stack Height (ft)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPStackDiameterMeasure" HeaderText="Stack Diameter (ft)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPExitGasVelocityMeasure"
                HeaderText="Exit Gas Velocity (fps)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPExitGasFlowRateMeasure"
                HeaderText="Exit Gas Flow Rate (acfs)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" Width="40px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPExitGasTempMeasure"
                HeaderText="Exit Gas Temp (&deg;F)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" Width="40px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numRPFencelineDistMeasure"
                HeaderText="Fenceline Distance (ft)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="RPStatusDesc"
                HeaderText="Operating Status">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numLatitudeMeasure"
                HeaderText="Latitude">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="numLongitudeMeasure"
                HeaderText="Longitude">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="intHorAccuracyMeasure"
                HeaderText="Horiz Accuracy Measure (m)">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="HorCollMetDesc" HeaderText="Horiz Collection Method">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="150px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="HorRefDatumDesc" HeaderText="Horiz Reference Datum">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="200px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}"
                HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strRPComment" HeaderText="Stack Comment">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="200px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strGeographicComment" HeaderText="Geo Coord Comment">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="200px" VerticalAlign="Top" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <br />
    <br />
</asp:Content>
