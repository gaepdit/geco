<%@ Page Title="Edit Release Point Apportionment" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_rpapportionment_edit" Codebehind="rpapportionment_edit.aspx.vb" %>
<%@ Register src="../Controls/PreventRePost.ascx" tagname="PreventRePost" tagprefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Edit Release Point Apportionment
    </div>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" CssClass="styledseparator" runat="server" Text="Release Point Apportionment"></asp:Label>
        <div class="sepbuttons">
            &nbsp;<asp:Button runat="server" ID="btnProcessSummary" Text="Return to Process Details"
                ToolTip="" Font-Size="Small" CausesValidation="False" />
        </div>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionsUnitID" CssClass="styled" runat="server" Text="Emission Unit ID:"></asp:Label>
        <asp:TextBox ID="txtEmissionsUnitID" runat="server" CssClass="readonly" Text=""
            MaxLength="6"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblEmissionsUnitDesc" CssClass="styled" runat="server" Text="Emission Unit Description:"></asp:Label>
        <asp:TextBox ID="txtEmissionsUnitDesc" runat="server" CssClass="readonly"
            MaxLength="100"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessID" CssClass="styled" runat="server" Text="Process ID:"></asp:Label>
        <asp:TextBox ID="txtProcessID" runat="server" CssClass="readonly" Text=""
            MaxLength="6"></asp:TextBox>
    </div>
    <div class="fieldwrapper">
        <asp:Label ID="lblProcessDesc" CssClass="styled" runat="server" Text="Process Description:"></asp:Label>
        <asp:TextBox ID="txtProcessDesc" runat="server" CssClass="readonly" Text=""
            MaxLength="200"></asp:TextBox>
    </div>
    <br />
    <asp:Label ID="lblRPApportionmentDeleteWarning" runat="server" Font-Bold="False"
        ForeColor="#FFFFFF" BackColor="Red" Font-Size="Medium"></asp:Label><br />
    <br />
    <div class="gridview">
        <asp:GridView ID="gvwRPApportionment" runat="server"
            DataKeyNames="FacilitySiteID,EmissionsUnitID,ProcessID,ReleasePointID"
            CellPadding="4" Font-Names="Verdana" Font-Size="Small" ForeColor="#333333"
            GridLines="None" AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="FacilitySiteID" Visible="false" />
                <asp:BoundField DataField="EmissionsUnitID" Visible="false" />
                <asp:BoundField DataField="ProcessID" Visible="False" />
                <asp:BoundField HeaderText="Release Point ID" DataField="ReleasePointID"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Release Point Description"
                    DataField="strRPDescription" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="RPType" HeaderText="Release Point Type" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Average % Emissions">
                    <ItemTemplate>
                        <asp:Label ID="lblAvgPctEmissions" runat="server" Text='<%# Eval("AvgPctEmissions")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Apportionment Comment">
                    <ItemTemplate>
                        <asp:Label ID="lblRPApportionmentComment" runat="server" Text='<%# Eval("RPApportionmentComment")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:BoundField DataField="LastEISSubmitDate" HeaderText="Last EPA Submittal"
                    ReadOnly="True" DataFormatString="{0:d}">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#BCD2EE" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <br />
        <div id="RPApportionmentCheck">
            <asp:Label ID="lblRPApportionmentTotal" runat="server" Font-Bold="True" Font-Size="Medium"
                Text="Apportionment Total:  "></asp:Label>
            <asp:TextBox ID="txtRPApportionmentTotal" runat="server" ReadOnly="True" MaxLength="3"
                Width="55px" Font-Bold="True" BorderStyle="None"></asp:TextBox><br />
            <asp:Label ID="lblRPApportionmentWarning" runat="server" BackColor="#ffffff" Font-Names="Verdana"
                Font-Size="Medium" Font-Bold="True" ForeColor="#FF0000" Height="70px" Width="500px"></asp:Label>
        </div>
        <act:RoundedCornersExtender ID="lblRPApportionmentWarning_RoundedCornersExtender"
            runat="server" Enabled="True" TargetControlID="lblRPApportionmentWarning">
        </act:RoundedCornersExtender>
        <br />
    </div>
    <br />
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnCancel2" CssClass="buttondiv"
            Text="Return to Process Details" CausesValidation="False"
            ToolTip="Takes you back to the Emission Unit Details page"
            UseSubmitBehavior="False" />
        <br />
    </div>
    <uc1:PreventRePost ID="PreventRePost1" runat="server" />
</asp:Content>
