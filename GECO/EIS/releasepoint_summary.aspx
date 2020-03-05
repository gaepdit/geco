<%@ Page Title="Release Point Summary - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_releasepoint_summary" Codebehind="releasepoint_summary.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label4" class="styledseparator" runat="server" Text="Active Fugitive Release Points"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwFugRPSummary" runat="server" DataSourceID="SqlDataSourceID1" DataKeyNames="releasepointid"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ReleasepointID" DataNavigateUrlFormatString="fugitive_details.aspx?fug={0}"
                    DataTextField="ReleasepointID" HeaderText="Fugitive ID" NavigateUrl="~/eis/fugitive_details.aspx"
                    SortExpression="ReleasepointID">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataNavigateUrlFields="releasepointid" DataNavigateUrlFormatString="fugitive_details.aspx?fug={0}"
                    DataTextField="strRPDescription" HeaderText="Fugitive Description" NavigateUrl="~/eis/fugitive_details.aspx"
                    SortExpression="strRPDescription">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="RPStatus" HeaderText="Operating Status" NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal" DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
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
        <asp:Label ID="Label1" class="styledseparator" runat="server" Text="Active Stack Release Points"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPSummary" runat="server" DataSourceID="SqlDataSourceID2" CellPadding="4" DataKeyNames="releasepointid"
            Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="releasepointid" DataNavigateUrlFormatString="stack_details.aspx?stk={0}"
                    DataTextField="releasepointid" HeaderText="Stack ID" NavigateUrl="~/eis/stack_details.aspx"
                    SortExpression="releasepointid">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataNavigateUrlFields="releasepointid" DataNavigateUrlFormatString="stack_details.aspx?stk={0}"
                    DataTextField="strrpdescription" HeaderText="Stack Description" NavigateUrl="~/eis/stack_details.aspx"
                    SortExpression="strrpdescription">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="RPTypeCode" HeaderText="Release Point Type" NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="numrpstackheightmeasure" HeaderText="Stack Height (feet)"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="right" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="numrpexitgasflowratemeasure" HeaderText="Gas Flow Rate (ACFS)"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="right" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="RPStatusCode" HeaderText="Operarting Status" NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal" DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceID2" runat="server"></asp:SqlDataSource>
    </div>
    <br />
    <asp:Panel ID="pnlDeletedRP" runat="server">
        <asp:Button runat="server" ID="btnShowDeletedRP" CssClass="buttondiv" Text="Show Deleted Release Points"
            CausesValidation="False" />
        <div class="gridview">
            <asp:GridView ID="gvwDeletedRP" runat="server" DataSourceID="sqldsDeletedRP" CellPadding="4"
                Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                CssClass="gridview" DataKeyNames="ReleasePointID">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="ReleasePointID" HeaderText="Release Point ID">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="strRPDescription" HeaderText="Description">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="strRPType" HeaderText="Release Point Type">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:SqlDataSource ID="sqldsDeletedRP" runat="server"></asp:SqlDataSource>
        </div>
        <br />
    </asp:Panel>
</asp:Content>