<%@ Page Title="Process Bulk Entry - GECO Emission Inventory" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_rp_process_bulk" Codebehind="rp_process_bulk.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="1500"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" /><br />
                Working. Please wait ...
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Process Throughput Bulk Entry"
            Font-Bold="True" Font-Size="Large"></asp:Label>
    </div>
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <asp:UpdatePanel ID="upnlProcessThroughput" runat="server">
        <ContentTemplate>
            <div align="center">
                <table align="center" style="width: 100%">
                    <tr>
                        <td align="left">This page is used to enter <%=EIYear%>
                            throughput quantities for the processes described by the indicated emission
                            unit and process IDs.
                            <br />
                            <br />
                            NOTE: To change the throughput unit you must visit the <b><i>Edit Process Operating
                                    Details</i></b> page. This is accessible by first going to the Emissions Reporting
                                page.
                            <br />
                            <br />
                            <span style="color: #AA0000"><b>Note</b>: If any changes are made to the throughput numbers
                                on this page, you must click one of the <b><i>Update Changes</i></b> buttons above
                                or below the table <b>before</b> going to the next page or leaving this form.</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:ValidationSummary ID="vsumProcessBulk" runat="server" Font-Bold="true"
                                HeaderText="* Throughput values must be from 0 to 9,999,999,999. One or more is out of the range."
                                ValidationGroup="vgProcessBulk" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">

                            <table class="bulk-table">
                                <thead>
                                    <tr>
                                        <td colspan="8">
                                            <asp:Button ID="btnUpdateTop" runat="server" Text="Update Changes" CausesValidation="true" ValidationGroup="vgProcessBulk" />&nbsp;&nbsp;
                                            <br />
                                            <asp:Label ID="lblUpdateStatusTop" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr class="bulk-table-controls">
                                        <td colspan="8">Records per page:
                                            <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="True">
                                                <asp:ListItem Selected="True">10</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="bulk-table-controls">
                                        <td colspan="8">Records per page:
                                            <asp:DataPager runat="server" ID="TopPager" PagedControlID="lvProcessBulk">
                                                <Fields>
                                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowNextPageButton="False"
                                                        FirstPageText="«" LastPageText="»" PreviousPageText="‹" NextPageText="›" />
                                                    <asp:NumericPagerField ButtonCount="5" ButtonType="Button" />
                                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowPreviousPageButton="False"
                                                        FirstPageText="«" LastPageText="»" PreviousPageText="‹" NextPageText="›" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Emission<br />
                                            Unit ID</th>
                                        <th>Process<br />
                                            <small style="font-weight: normal">(Link opens details edit
                                                <br />
                                                page in new window)</small>
                                        </th>
                                        <th>Most Recent<br />
                                            Previous Year</th>
                                        <th colspan="2" style="text-align: left">Most Recent<br />
                                            Previous<br />
                                            Throughput</th>
                                        <th colspan="2" style="text-align: left">
                                            <%=EIYear%> Annual<br />
                                            Throughput</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:ListView ID="lvProcessBulk" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEmissionsUnitID" runat="server" Text='<%# Eval("EMISSIONSUNITID") %>' />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblProcessID" runat="server" Text='<%# Eval("PROCESSID") %>' />
                                                    -
                                                    <asp:HyperLink ID="lblPollutantLink" runat="server" Text='<%# Eval("STRPROCESSDESCRIPTION") %>'
                                                        NavigateUrl='<%# "~/EIS/rp_operscp_edit.aspx?eu=" & Eval("EmissionsUnitID") & "&ep=" & Eval("ProcessID") %>'
                                                        Target="_blank"></asp:HyperLink>
                                                </td>
                                                <td style="text-align: center"><%# Eval("PREV_INTINVENTORYYEAR") %></td>
                                                <td style="text-align: right"><%# If(IsDBNull(Eval("PREV_FLTCALCPARAMETERVALUE")), "N/A", Eval("PREV_FLTCALCPARAMETERVALUE")) %></td>
                                                <td><%# If(IsDBNull(Eval("PREV_FLTCALCPARAMETERVALUE")), "", Eval("PREV_UOM")) %></td>
                                                <td style="text-align: right">
                                                    <asp:TextBox ID="txtCalcParameterValue" CssClass="grvTextBox" runat="server" Width="120px" MaxLength="10"
                                                        Text='<%# Bind("CURR_FLTCALCPARAMETERVALUE") %>'></asp:TextBox>
                                                    <act:FilteredTextBoxExtender ID="txtCalcParameterValue_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtCalcParameterValue"
                                                        ValidChars=".">
                                                    </act:FilteredTextBoxExtender>
                                                </td>
                                                <td><%# Eval("CURR_UOM") %><asp:Label ID="lblInvalidUnits" runat="server" 
                                                    Visible='<%# Eval("InvalidUnitOfMeasure ") %>'><span class='highlight-normal'>**</span></asp:Label></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </tbody>
                                <tfoot>
                                    <tr class="bulk-table-controls">
                                        <td colspan="8">
                                            <asp:DataPager runat="server" ID="BottomPager" PagedControlID="lvProcessBulk">
                                                <Fields>
                                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="true" ShowNextPageButton="False"
                                                        FirstPageText="«" LastPageText="»" PreviousPageText="‹" NextPageText="›" />
                                                    <asp:NumericPagerField ButtonCount="5" ButtonType="Button" />
                                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="true" ShowPreviousPageButton="False"
                                                        FirstPageText="«" LastPageText="»" PreviousPageText="‹" NextPageText="›" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <br />
                                            <asp:Button ID="btnUpdateBottom" runat="server" Text="Update Changes" />
                                            <br />
                                            <asp:Label ID="lblUpdateStatusBottom" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInvalidUnitNote" runat="server" Font-Size="Small" CssClass="highlight-normal">
                                ** The highlighted processes have invalid units. Please open the details view to correct the units.<br /><br />
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span style="color: #AA0000">After making changes, click the
                                    <b><i>Update Changes</i></b> button <b>before</b> going to another page or leaving
                                    this form.</span>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
