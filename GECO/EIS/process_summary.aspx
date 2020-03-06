<%@ Page Title="Process Summary - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_process_summary" CodeBehind="process_summary.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblAdd" class="styledseparator" runat="server" Text="Process Summary"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwProcessSummary" runat="server" DataSourceID="SqlDataSourceID1"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EMISSIONSUNITID" HeaderText="Emission Unit ID">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="strEmissionsUnitStatus" HeaderText="Emission Unit Status">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="strEmissionsUnitDesc" HeaderText="Unit Description">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:HyperLinkField DataNavigateUrlFields="processID,EMISSIONSUNITID" DataNavigateUrlFormatString="process_details.aspx?ep={0}&amp;eu={1}"
                    DataTextField="processID" HeaderText="Process ID" NavigateUrl="~/eis/process_details.aspx">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataNavigateUrlFields="processID,EMISSIONSUNITID" DataNavigateUrlFormatString="process_details.aspx?ep={0}&amp;eu={1}"
                    DataTextField="STRPROCESSDESCRIPTION" HeaderText="Process Description" NavigateUrl="~/eis/process_details.aspx">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="SOURCECLASSCODE" HeaderText="SCC Code">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal"
                    DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="ControlApproach" HeaderText="Process Control Approach">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100" />
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
</asp:Content>
