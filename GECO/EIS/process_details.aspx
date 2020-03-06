<%@ Page Title="Process Details - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_process_details" Codebehind="process_details.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Process Details
        <asp:TextBox ID="txtEmissionUnitID" runat="server" Text="" class="readonly" ReadOnly="True"
            Visible="false" Width="10px"></asp:TextBox>
        <asp:Button ID="btnReturnToSummary" runat="server" Text="Summary" CausesValidation="False"
            CssClass="summarybutton" PostBackUrl="~/eis/process_summary.aspx" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorNoText1" class="styledseparator" runat="server" Text="Process"></asp:Label>
        <asp:Label ID="lblEmissionUnitStatusWarning" runat="server" Font-Bold="True"
            Font-Size="Small" ForeColor="#CC0000"></asp:Label>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitID" class="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:HyperLink ID="hlinkEmissionUnitID" runat="server" ToolTip="Click to go to Emission Unit Details"></asp:HyperLink>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitStatus" class="styled" runat="server" Text="Emission Unit Status:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitStatus" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionUnitDesc" class="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtEmissionUnitDesc" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessID" class="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessDescription" class="styled" runat="server" Text="Process Description:"></asp:Label>
        <asp:TextBox ID="txtProcessDescription" runat="server" Text="" class="readonly" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSourceClassCode" class="styled" runat="server" Text="Source Classification Code:"></asp:Label>
        <asp:TextBox ID="txtSourceClassCode" runat="server" Text="" class="readonly" ReadOnly="True"
            Width="100px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblSccDescLabel" class="styled" runat="server" Text="SCC Description:"></asp:Label>
        <asp:Label ID="lblSccDesc" runat="server" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessComment" class="styled" runat="server" Text="Process Comment:"></asp:Label>
        <asp:TextBox ID="txtProcessComment" runat="server" Text="" class="readonly" TextMode="MultiLine" Rows="4" 
            runat="server" Width="400px" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastEISSubmit" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
        <asp:TextBox ID="txtLastEISSubmit" class="readonly" runat="server" Text="" ReadOnly="True"
            Width="200px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblLastUpdated" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
        <asp:TextBox ID="txtLastUpdate" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Release Point Apportionment Information"></asp:Label>
        <asp:Label ID="lblRPApportionInfoWarning" runat="server" Font-Size="Small" ForeColor="#CA0000"
            CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server" DataSourceID="sqldsRPApportionment"
            CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="RELEASEPOINTID" HeaderText="Release Point ID" SortExpression="RELEASEPOINTID">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="STRRPDESCRIPTION" HeaderText="Release Point Description">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="RPType" HeaderText="Release Point Type">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="Apportionment" HeaderText="Apportionment %">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="LASTEISSUBMITDATE" HeaderText="Last Submission to EPA">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="sqldsRPApportionment" runat="server"></asp:SqlDataSource>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblProcessControlApproach" class="styledseparator" runat="server"
            Text="Process Control Approach"></asp:Label>
        <asp:Label ID="lblProcessControlApproachWarning" runat="server" Font-Size="Small"
            ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
    </div>
    <asp:Panel ID="pnlProcessControlApproach" runat="server">
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachDescription" class="styled" runat="server" Text="Control Approach Description:"></asp:Label>
            <asp:TextBox ID="txtControlApproachDescription" class="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachCapEffic" class="styled" runat="server" Text="Percent Control Approach Capture Efficiency:"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachCapEffic" class="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblPctCtrlApproachEffect" class="styled" runat="server" Text="Percent Control Approach Effectiveness:"></asp:Label>
            <asp:TextBox ID="txtPctCtrlApproachEffect" class="readonly" runat="server" Text=""
                ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblFirstInventoryYear" class="styled" runat="server" Text="First Inventory Year:"></asp:Label>
            <asp:TextBox ID="txtFirstInventoryYear" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblLastInventoryYear" class="styled" runat="server" Text="Last Inventory Year:"></asp:Label>
            <asp:TextBox ID="txtLastInventoryYear" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lblControlApproachComment" class="styled" runat="server" Text="Control Approach Comment:"></asp:Label>
            <asp:TextBox ID="txtControlApproachComment" class="readonly" runat="server" Text="" Rows="4" 
                TextMode="MultiLine" Width="400px"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lbllastsubmitEPA_CP" class="styled" runat="server" Text="Last Submitted to EPA:"></asp:Label>
            <asp:TextBox ID="txtLastSubmitEPA_CP" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapper">
            <asp:Label ID="lbllastupdate_CP" class="styled" runat="server" Text="Last Updated On:"></asp:Label>
            <asp:TextBox ID="txtLastUpdate_CP" class="readonly" runat="server" Text="" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Process Control Measures"></asp:Label>
            <asp:Label ID="lblProcessControlMeasureWarning" runat="server" Font-Size="Small"
                ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
        </div>
        <div class="gridview">
            <asp:GridView ID="gvwProcessControlMeasure" runat="server" DataSourceID="SqlDataSourceID1"
                CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="CDType" HeaderText="Control Device" SortExpression="CDType">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last Submission to EPA">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceID1" runat="server"></asp:SqlDataSource>
        </div>
        <div class="fieldwrapperseparator">
            <asp:Label ID="Label1" class="styledseparator" runat="server" Text="Process Control Pollutants"></asp:Label>
            <asp:Label ID="lblProcessControlPollutantWarning" runat="server" Font-Size="Small"
                ForeColor="#CA0000" CssClass="labelwarningleft"></asp:Label>
        </div>
        <div class="gridview">
            <asp:GridView ID="gvwProcessControlPollutant" runat="server" DataSourceID="SqlDataSourceID2"
                CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="PollutantType" HeaderText="Pollutant">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CtrlEfficiency" HeaderText="Reduction Efficiency (%)">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LASTEISSUBMITDATE" HeaderText="Last Submission to EPA">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CalculatedReduction" HeaderText="Calculated Overall<br> Pollutant Reduction (%)" ReadOnly="True" SortExpression="CalculatedReduction" HtmlEncode="false" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceID2" runat="server"></asp:SqlDataSource>
        </div>
    </asp:Panel>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblRPEmissions" class="styledseparator" runat="server" Text="Annual Reporting Period Emissions"></asp:Label>
        <asp:Label ID="lblGVWReportingPeriodEmpty" runat="server" Font-Size="Small"
            ForeColor="#0033CC" CssClass="labelwarningleft"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwReportingPeriods" runat="server" DataSourceID="sqldsReportingPeriod"
            CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="strPollutantDescription" HeaderText="Pollutant"
                    SortExpression="strPollutantDescription">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="RptPeriodType" HeaderText="Reporting Period Type"
                    SortExpression="RptPeriodType">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="curr_flttotalemissions" HeaderText="Year1"
                    NullDisplayText="No Data">
                    <ItemStyle Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev1_flttotalemissions" HeaderText="Year2"
                    NullDisplayText="No Data">
                    <ItemStyle Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev2_flttotalemissions" HeaderText="Year3"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"
                        Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev3_flttotalemissions" HeaderText="Year4"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"
                        Font-Size="Small" />
                </asp:BoundField>
                <asp:BoundField DataField="prev4_flttotalemissions" HeaderText="Year5"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"
                        Font-Size="Small" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="sqldsReportingPeriod" runat="server"></asp:SqlDataSource>
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_"
            Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnSummary2" CssClass="buttondiv" Text="Summary" CausesValidation="False"
            PostBackUrl="process_summary.aspx" UseSubmitBehavior="False" />
    </div>
</asp:Content>