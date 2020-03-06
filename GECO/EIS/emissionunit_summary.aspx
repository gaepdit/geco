<%@ Page Title="Emissions Unit Summary - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    MaintainScrollPositionOnPostback="true" AutoEventWireup="false"
    Inherits="GECO.eis_emissionunit_summary" Codebehind="emissionunit_summary.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblAdd" class="styledseparator" runat="server" Text="Emission Unit Summary"></asp:Label>
    </div>
    <div style="font-size: medium; font-weight: bold;">
        Active Emission Units
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwEmissionUnitSummary" runat="server"
            DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
            CssClass="gridview" PageSize="20">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_details.aspx?eu={0}"
                    DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_details.aspx"
                    SortExpression="emissionsunitid">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_details.aspx?eu={0}"
                    DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_details.aspx"
                    SortExpression="strUnitDescription">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="unittypecode" HeaderText="Unit Type" NullDisplayText="No Data"
                    SortExpression="unittypecode">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="strUnitStatusCode" HeaderText="Operating Status" SortExpression="strUnitStatusCode" />
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal" DataFormatString="{0:d}"
                    HtmlEncode="false" SortExpression="LastEISSubmitDate" ConvertEmptyStringToNull="False"
                    NullDisplayText="Not Submitted">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Unit Control Approach" DataField="ControlApproach" SortExpression="ControlApproach">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>

    <asp:Panel ID="pnlDeletedEmissionUnits" runat="server">
        <asp:Button runat="server" ID="btnShowDeletedEU" CssClass="buttondiv" Text="Show Deleted Emission Units"
            CausesValidation="False" />
        <div class="gridview">
            <asp:GridView ID="gvwDeletedEU" runat="server" CellPadding="4"
                Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                CssClass="gridview" DataKeyNames="EmissionsUnitID">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="strUnitDescription" HeaderText="Description">
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
        </div>
        <br />
    </asp:Panel>
</asp:Content>
