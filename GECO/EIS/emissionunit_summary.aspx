<%@ Page Title="Emissions Unit Summary - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    MaintainScrollPositionOnPostback="true" AutoEventWireup="false"
    Inherits="GECO.eis_emissionunit_summary" Codebehind="emissionunit_summary.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblAdd" class="styledseparator" runat="server" Text="Emission Unit Summary"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAdd" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAdd_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancel" DynamicServicePath="" Enabled="True" OkControlID="btnCancel"
                PopupControlID="pnlPopUpAddUnit" TargetControlID="btnAdd">
            </act:ModalPopupExtender>
        </div>
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
                    <asp:ButtonField CommandName="Undelete" Text="Undelete" />
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
    <asp:Panel ID="pnlPopUpAddUnit" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Emission
                        Unit</span></strong>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblNewEmissionsUnitID" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewEmissionsUnitID" runat="server" CssClass="editable" Font-Names="Arial"
                        Font-Size="Small" MaxLength="6"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtNewEmissionsUnitID_FilteredTextBoxExtender" runat="server"
                        Enabled="True" FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="-"
                        TargetControlID="txtNewEmissionsUnitID">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvEmissionsUnitID" runat="server" ControlToValidate="txtNewEmissionsUnitID"
                        Display="Dynamic" ErrorMessage="* Emission unit ID is required." ValidationGroup="vgAddEmissionUnit"
                        Width="100%" CssClass="validator"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cusvEmissionUnitID" runat="server" ControlToValidate="txtNewEmissionsUnitID"
                        OnServerValidate="EmissionsUnitIDCheck" ErrorMessage="* Unit ID already used. Enter another."
                        Font-Names="Arial" Font-Size="Small" ValidationGroup="vgAddEmissionUnit" Width="100%"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblNewEmissionUnitDesc" runat="server" CssClass="label" Text="Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewEmissionUnitDesc" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="400" Rows="4" TextMode="MultiLine" Width="250px" CssClass="editable"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvEmissionsUnitDesc" runat="server" ControlToValidate="txtNewEmissionUnitDesc"
                        Display="Dynamic" ErrorMessage="* Description is required." ValidationGroup="vgAddEmissionUnit"
                        Width="100%"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;<asp:Label ID="lbl" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;
                    <asp:Button ID="btnInsertEmissionUnit" runat="server" Text="Save" ValidationGroup="vgAddEmissionUnit" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
