<%@ Page Title="Reporting Period Summary - GECO Emissions Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" MaintainScrollPositionOnPostback="true" Inherits="GECO.EIS_rp_summary" CodeBehind="rp_summary.aspx.vb" %>

<%@ Register Src="../Controls/PreventRePost.ascx" TagName="PreventRePost" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        Process Reporting Period Summary
    </div>

    <div class="fieldwrapperseparator">
        <asp:Label ID="lblRPSummary" class="styledseparator" runat="server" Text="Processes in the Reporting Period"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwReportingPeriodSummary" runat="server"
            CellPadding="4" Font-Names="Arial" Font-Size="Small" DataKeyNames="EmissionsUnitID,ProcessID"
            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            class="gridview" PageSize="20">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID"
                    DataNavigateUrlFormatString="~/EIS/rp_details.aspx?ep={0}&amp;eu={1}"
                    DataTextField="ProcessID" HeaderText="Process ID"
                    NavigateUrl="~/EIS/rp_details.aspx">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:HyperLinkField>
                <asp:HyperLinkField DataNavigateUrlFields="ProcessID,EmissionsUnitID"
                    DataNavigateUrlFormatString="~/EIS/rp_details.aspx?ep={0}&amp;eu={1}"
                    DataTextField="strProcessDescription" HeaderText="Process Description"
                    NavigateUrl="~/EIS/rp_details.aspx">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="ControlApproach" HeaderText="Control Approach"
                    ConvertEmptyStringToNull="False" NullDisplayText="No Data">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Last EPA Submittal" DataField="LastEISSubmitDate"
                    DataFormatString="{0:d}" NullDisplayText="Not Submitted">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:ButtonField CommandName="Remove" Text="Remove" ButtonType="Button" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <br />
    </div>

    <div class="fieldwrapperseparator">
        <asp:Label ID="lblRPSummaryNoRP" class="styledseparator" runat="server" Text="Processes not in the Reporting Period"></asp:Label>
    </div>
    <div class="gridview">
        <asp:GridView ID="gvwRPSummary_NoRP" runat="server"
            CellPadding="4" Font-Names="Arial" Font-Size="Small"
            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
            class="gridview" AllowPaging="True" DataKeyNames="EmissionsUnitID,ProcessID"
            EnableModelValidation="True">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ProcessID" HeaderText="Process ID">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="strProcessDescription"
                    HeaderText="Process Description">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ControlApproach" HeaderText="Control Approach"
                    ConvertEmptyStringToNull="False" NullDisplayText="No Data">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Last EPA Submittal" DataField="LastEISSubmitDate"
                    DataFormatString="{0:d}" NullDisplayText="Not Submitted">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:ButtonField CommandName="Add" Text="Add" ButtonType="Button" />
                <asp:ButtonField CommandName="PrePop" Text="Prepopulate" ButtonType="Button" />
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    <uc1:PreventRePost ID="PreventRePost1" runat="server" />
</asp:Content>
