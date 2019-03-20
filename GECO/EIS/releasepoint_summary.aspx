<%@ Page Title="Release Point Summary - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_releasepoint_summary" Codebehind="releasepoint_summary.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label4" class="styledseparator" runat="server" Text="Active Fugitive Release Points"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAddFugitiveRP" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAddFugitiveRP_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancelNewFugitiveRP" DynamicServicePath="" Enabled="True"
                PopupControlID="pnlPopUpAddFugitiveRP" TargetControlID="btnAddFugitiveRP">
            </act:ModalPopupExtender>
        </div>
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
        <div class="sepbuttons">
            <asp:Button ID="btnAddStack" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAddStack_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancelNewStack" DynamicServicePath="" Enabled="True" TargetControlID="btnAddStack"
                PopupControlID="pnlPopUpAddStack">
            </act:ModalPopupExtender>
        </div>
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
    </div>
    <br />
    <div class="gridview">
        <asp:SqlDataSource ID="SqlDataSourceID2" runat="server"></asp:SqlDataSource>
    </div>
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
                    <asp:ButtonField CommandName="Undelete" Text="Undelete" />
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
    <asp:Panel ID="pnlPopUpAddFugitiveRP" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Fugitive
                        Release Point </span></strong>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Fugitive ID:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewFugitiveRP" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" ValidationGroup="vgFugitiveSave"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtNewFugitiveRP" runat="server" Enabled="True"
                        FilterType="Numbers, UppercaseLetters, LowercaseLetters, Custom" TargetControlID="txtNewFugitiveRP"
                        ValidChars="-">
                    </act:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqvNewFugitiveRP" ControlToValidate="txtNewFugitiveRP"
                        runat="server" ErrorMessage="*Required" ValidationGroup="vgFugitiveSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvFugitiveID" runat="server" ControlToValidate="txtNewFugitiveRP"
                        OnServerValidate="FugitiveRPIDCheck" ErrorMessage="* Release Point ID already used. Please enter another"
                        Font-Names="Arial" Font-Size="Small" Display="Dynamic" ValidationGroup="vgFugitiveSave"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Description:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewFugitiveRPDesc" runat="server" Font-Names="Arial" Font-Size="Small" Rows="4" 
                        MaxLength="100" TextMode="MultiLine" Width="250px" ValidationGroup="vgFugitiveSave"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvNewFugitiveRPDesc" ControlToValidate="txtNewFugitiveRPDesc"
                        runat="server" ErrorMessage="*Required" ValidationGroup="vgFugitiveSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="style3">After saving you will enter more details on the form that appears.
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnInsertFugitiveID" runat="server" Text="Save" Width="90px" ValidationGroup="vgFugitiveSave" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelNEWFugitiveRP" runat="server" Text="Cancel" Width="90px"
                        CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlPopUpAddStack" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Stack</span></strong>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Stack ID:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewStackID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6" ValidationGroup="vgStackSave"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="filtxtNewStackID" runat="server" Enabled="True"
                        FilterType="Numbers, UppercaseLetters, LowercaseLetters, Custom" TargetControlID="txtNewStackID"
                        ValidChars="-">
                    </act:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvStackID" runat="server" ControlToValidate="txtNewStackID"
                        OnServerValidate="StackIDCheck" ErrorMessage="*Stack ID already in use. Enter another."
                        Font-Names="Arial" Font-Size="Small" Display="Dynamic" ValidationGroup="vgStackSave"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqvNewStackID" runat="server" ControlToValidate="txtNewStackID"
                        ErrorMessage="*Stack ID required" ValidationGroup="vgStackSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">Stack Type:
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlRPtypeCode" runat="server" class="">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:RequiredFieldValidator ID="reqvRPtypeCode" runat="server" ControlToValidate="ddlRPtypeCode"
                        Display="Dynamic" ErrorMessage="*Choose stack type" InitialValue="--Select Stack Type--"
                        ValidationGroup="vgStackSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">Description:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewStackDesc" runat="server" Font-Names="Arial" Font-Size="Small" Rows="4" 
                        MaxLength="100" TextMode="MultiLine" Width="250px" ValidationGroup="vgStackSave"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvNewStackDesc" ControlToValidate="txtNewStackDesc"
                        runat="server" ErrorMessage="*Required" ValidationGroup="vgStackSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="style3">After saving you will enter more details on the form that appears.
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnInsertStack" runat="server" Text="Save" Width="90px" ValidationGroup="vgStackSave" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancelNewStack" runat="server" Text="Cancel" Width="90px" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>