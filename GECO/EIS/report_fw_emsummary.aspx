<%@ Page Title="Report - Facility Wide Emission Summary" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_report_fw_emsummary" Codebehind="report_fw_emsummary.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        Facility Wide Emissions Summary
        <asp:Button ID="btnReportsHome_fwemSummary" runat="server" Text="Reports Home" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteID_fwemSummary" class="styled" runat="server"
            Text="Facility ID"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteID_fwemSummary" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilityName_fwemSummary" class="styled" runat="server"
            Text="Facility Name"></asp:Label>
        <asp:TextBox ID="txtFacilityName_fwemSummary" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div>
        <div class="fieldwrapper">
            <asp:Label ID="lblInventoryYear_fwemSummary" CssClass="styled" runat="server"
                Text="Inventory Year"></asp:Label>
            <asp:DropDownList ID="ddlInventoryYear_fwemSummary" runat="server" class=""
                AutoPostBack="True">
            </asp:DropDownList>
            &nbsp;
        <asp:Button ID="btnGO_fwemSummary" runat="server" Text="GO" />
            <asp:RequiredFieldValidator ID="reqvYear_fwemSummary" runat="server"
                ControlToValidate="ddlInventoryYear_fwemSummary" Display="Dynamic"
                ErrorMessage="* Select a year" InitialValue="-Select Year-"></asp:RequiredFieldValidator>
        </div>
        <br />
        <div style="text-align: center;">
            <asp:Button ID="btnExport_fwemSummary" runat="server" Text="Download as Excel" CausesValidation="False"
                CssClass="summarybutton" UseSubmitBehavior="False"
                Visible="False" /><br />
            <div style="text-align: center;">
                <asp:Label ID="lblEmptygvwEmissionsSummary" runat="server" Visible="False"
                    ForeColor="#CC0000" Font-Bold="True" Font-Size="Medium"></asp:Label>
            </div>
            <asp:GridView ID="gvwEmissionsSummary"
                runat="server"
                AutoGenerateColumns="False"
                HorizontalAlign="Center"
                ForeColor="#333333"
                Caption="Facility Wide Emissions Summary"
                CssClass="reportview">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="STRPOLLUTANT"
                        HeaderText="Pollutant Description"
                        SortExpression="STRPOLLUTANT">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" Width="250px" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FLTTOTALEMISSIONS"
                        HeaderText="Total Emissions"
                        SortExpression="FLTTOTALEMISSIONS">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="100px" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle HorizontalAlign="Left" BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:Label ID="lblFWSummary" runat="server"></asp:Label>
            <br />
            <br />
</asp:Content>