<%@ Page Title="Report - Facility Processes" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_report_processes" CodeBehind="report_processes.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        Processes
        <asp:Button ID="btnReportsHome_Processes" runat="server" Text="Reports Home" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteID_Processes" class="styled" runat="server"
            Text="Facility ID"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteID_Processes" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilityName_Processes" class="styled" runat="server"
            Text="Facility Name"></asp:Label>
        <asp:TextBox ID="txtFacilityName_Processes" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <br />
    <div style="text-align: center;">
        <p>
            <asp:Button ID="btnExport_Processes" runat="server" Text="Download as Excel" CausesValidation="False"
                CssClass="summarybutton" UseSubmitBehavior="False"
                Visible="False" />
        </p>
        <asp:Label ID="lblEmptygvwProcesses" runat="server" Visible="False"
            ForeColor="#CC0000" Font-Bold="True" Font-Size="Medium"></asp:Label>
    </div>
    <asp:GridView ID="gvwProcesses"
        runat="server"
        AutoGenerateColumns="False"
        HorizontalAlign="Center"
        ForeColor="#333333"
        Width="95%"
        Caption="Processes" CssClass="reportview">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:BoundField DataField="EmissionsUnitID"
                HeaderText="Emission Unit ID">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strUnitDesc" HeaderText="Unit Status">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="30px" />
            </asp:BoundField>
            <asp:BoundField DataField="ProcessID" HeaderText="Process ID">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="30px" />
            </asp:BoundField>
            <asp:BoundField DataField="strProcessDescription" HeaderText="Process Description">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="SourceClassCode" HeaderText="SCC">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strSCCDesc" HeaderText="SCC Description">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="300px" />
            </asp:BoundField>
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}"
                HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="50px" />
            </asp:BoundField>
            <asp:BoundField DataField="strProcessComment" HeaderText="Comment">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="150px" VerticalAlign="Top" />
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
