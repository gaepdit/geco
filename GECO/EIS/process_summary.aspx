<%@ Page Title="Process Summary - GECO Facility Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_process_summary" Codebehind="process_summary.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblAdd" class="styledseparator" runat="server" Text="Process Summary"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAdd" runat="server" Text="Add" ToolTip="" Font-Size="Small"
                CausesValidation="False" />
            <act:ModalPopupExtender ID="btnAdd_ModalPopupExtender" runat="server" BackgroundCssClass="modalProgressGreyBackground"
                CancelControlID="btnCancel" DynamicServicePath="" Enabled="True" OkControlID="btnCancel"
                PopupControlID="pnlPopUpAddProcess" TargetControlID="btnAdd">
            </act:ModalPopupExtender>
        </div>
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
    <br />
    <br />
    <asp:Panel ID="pnlPopUpAddProcess" BackColor="#ffffff" runat="server" BorderColor="#333399"
        BorderStyle="Ridge" ScrollBars="Auto" Width="450px" Style="display: none;">
        <table border="0" cellpadding="2" cellspacing="1" width="100%">
            <tr>
                <td align="center" colspan="2">
                    <strong><span style="color: #4169e1; font-size: 12pt; font-family: Verdana;">Add Process</span></strong>
                </td>
            </tr>
            <tr>
                <td align="center" width="30%" colspan="2">
                    <asp:ValidationSummary ID="sumvAddProcessModalpopup" runat="server" HeaderText="You have received the following errors:"
                        ValidationGroup="AddProcess" />
                </td>
            </tr>
            <tr>
                <td align="right" width="30%"></td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblEmissionUnitID" runat="server" CssClass="label" Text="Emission Unit ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlExistEmissionUnitID" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqvexistEmissionUnitID" ControlToValidate="ddlExistEmissionUnitID"
                        InitialValue="--Select Emission Unit ID--" runat="server" ErrorMessage="The Emission Unit ID is required."
                        Display="Dynamic" ValidationGroup="AddProcess">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblNewProcessID" runat="server" CssClass="label" Text="Process ID:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewProcessID" runat="server" Font-Names="Arial" Font-Size="Small"
                        MaxLength="6"></asp:TextBox>
                    <act:FilteredTextBoxExtender ID="txtNewProcessID_FilteredTextBoxExtender"
                        runat="server" Enabled="True" TargetControlID="txtNewProcessID"
                        FilterType="Custom, Numbers, LowercaseLetters, UppercaseLetters" ValidChars="-">
                    </act:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="reqvNewProcessID" runat="server" ControlToValidate="txtNewProcessID"
                        Display="Dynamic" ErrorMessage="Process ID is required." ValidationGroup="AddProcess">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
                <td valign="top">
                    <asp:CustomValidator ID="cusvProcessID" runat="server" ControlToValidate="txtNewProcessID"
                        OnServerValidate="ProcessIDCheck" ErrorMessage="* Process ID already used. Enter another."
                        Font-Names="Arial" Font-Size="Small" Display="Dynamic" ValidationGroup="AddProcess"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblexistReleasePointID" runat="server" CssClass="label" Text="Release Point ID:"
                    BorderStyle="None"></asp:Label>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlexistReleasePointID" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqvexistReleasePointID" ControlToValidate="ddlexistReleasePointID"
                        InitialValue="--Select Release Point ID--" runat="server" ErrorMessage="The Release Point ID is required."
                        Display="Dynamic" ValidationGroup="AddProcess">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" width="30%">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" width="30%" valign="top">&nbsp;<asp:Label ID="lblNewProcessDesc" runat="server" CssClass="label" Text="Process Description:"></asp:Label>
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtNewProcessDesc" runat="server" Font-Names="Arial" Font-Size="Small" Rows="4" 
                        MaxLength="100" TextMode="MultiLine" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqvNewProcessID0" runat="server"
                        ControlToValidate="txtNewProcessDesc" Display="Dynamic"
                        ErrorMessage="Process Description is required." ValidationGroup="AddProcess">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" class="style4">&nbsp;<asp:Label ID="lbl" runat="server" CssClass="label" Text="After saving you will enter more details on the form that appears."></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnInsertProcess" runat="server" Text="Save" Width="90px" ValidationGroup="AddProcess" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>