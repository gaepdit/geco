<%@ Page Title="Report - Facility Emission Units" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_report_emissionunits" CodeBehind="report_emissionunits.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="pageheader">
        Emission Units
        <asp:Button ID="btnReportsHome_EmissionsUnit" runat="server" Text="Reports Home" CausesValidation="False"
            CssClass="summarybutton" UseSubmitBehavior="False" />
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilitySiteID_EmissionsUnit" class="styled" runat="server"
            Text="Facility ID"></asp:Label>
        <asp:TextBox ID="txtFacilitySiteID_EmissionsUnit" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblFacilityName_EmissionsUnit" class="styled" runat="server"
            Text="Facility Name"></asp:Label>
        <asp:TextBox ID="txtFacilityName_EmissionsUnit" class="readonly"
            runat="server" Text=""
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>
    <br />
    <div style="text-align: center;">
        <p>
            <asp:Button ID="btnExport_EmissionsUnit" runat="server" Text="Download as Excel" CausesValidation="False"
                CssClass="summarybutton" UseSubmitBehavior="False"
                Visible="False" />
        </p>
        <asp:Label ID="lblEmptygvwEmissionsUnit" runat="server" Visible="False"
            ForeColor="#CC0000" Font-Bold="True" Font-Size="Medium"></asp:Label>
    </div>
    <asp:GridView ID="gvwEmissionsUnit"
        runat="server"
        AutoGenerateColumns="False"
        HorizontalAlign="Center"
        ForeColor="#333333" Width="95%"
        Caption="Emission Units" CssClass="reportview">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:BoundField DataField="EMISSIONSUNITID"
                HeaderText="Emission Unit ID"
                SortExpression="EMISSIONSUNITID">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="STRUNITDESCRIPTION"
                HeaderText="Description">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="200px" />
            </asp:BoundField>
            <asp:BoundField DataField="strUnitType" HeaderText="Unit Type"
                SortExpression="strUnitType">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="120px" />
            </asp:BoundField>
            <asp:BoundField DataField="FLTUNITDESIGNCAPACITY" HeaderText="Design Capacity">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" Width="30px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="STRUNITDESIGNCAPACITYUOMCODE" HeaderText="Design Capacity Unit">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="NUMMAXIMUMNAMEPLATECAPACITY"
                HeaderText="Max Nameplate Capacity (MW)" NullDisplayText="Non Elec Gen">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="DATUNITOPERATIONDATE"
                HeaderText="Placed in Operation" SortExpression="DATUNITOPERATIONDATE" DataFormatString="{0:d}">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" Width="40px" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="strUnitStatusCode"
                HeaderText="Operating Status" NullDisplayText="No Data">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="LastEISSubmitDate" DataFormatString="{0:d}"
                HeaderText="Last EPA Submittal" NullDisplayText="Not Submitted">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
            </asp:BoundField>
            <asp:BoundField DataField="STRUNITCOMMENT" HeaderText="Comment">
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
