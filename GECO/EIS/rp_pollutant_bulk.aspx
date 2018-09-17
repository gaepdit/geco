<%@ Page Title="Pollutant Bulk Entry - GECO Emission Inventory" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_rp_pollutant_bulk" Codebehind="rp_pollutant_bulk.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
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
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Pollutant Bulk Entry"
            Font-Bold="True" Font-Size="Large"></asp:Label>
    </div>
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlPollutant" runat="server">
        <ContentTemplate>
            <div align="center">
                <table align="center" style="width: 100%">
                    <tr>
                        <td align="left">This page is used to enter
                            <%=EIYear%>
                            emissions quantities for the processes described by the indicated emission
                            unit and process IDs. The annual emission quantities are reported in tons per year (TPY).
                            Summer Day emissions are reported as tons per day (TPD).
                            <br />
                            <br />
                            <span style="color: #AA0000"><b>Note</b>: To save entered data, you must click one of
                                the <b><i>Update Changes</i></b> buttons above or below the table <b>before</b>
                                going to the next page or leaving this form.</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:ValidationSummary ID="vsumPollutantBulk" runat="server" Font-Bold="true"
                                HeaderText="* One or more Summer Day emission quantity below 0.001 tons per day or is missing."
                                ValidationGroup="vgPollutantBulk" ShowSummary="False" />
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <table class="bulk-table">
                                <thead>
                                    <tr>
                                        <td colspan="10">
                                            <asp:Button ID="btnUpdateTop" runat="server" Text="Update Changes" />&nbsp;&nbsp;
                                            <asp:Label ID="lblUpdateStatusTop" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label><br />
                                            <asp:Label ID="lblErrorForBlanks" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label><br />
                                            <asp:Label ID="lblErrorForSummerDay" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="bulk-table-controls">
                                        <td colspan="10">Records per page:
                                            <asp:DropDownList ID="ddlPager" runat="server" AutoPostBack="True">
                                                <asp:ListItem Selected="True">10</asp:ListItem>
                                                <asp:ListItem Value="25">25</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="bulk-table-controls">
                                        <td colspan="10">Records per page:
                                            <asp:DataPager runat="server" ID="TopPager" PagedControlID="lvEmissionsBulk">
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
                                        <th>Process</th>
                                        <th>Pollutant<br />
                                            <small style="font-weight: normal">(Link opens detail
                                                <br />
                                                view in new window)</small>
                                        </th>
                                        <th>Emissions<br />
                                            Period</th>
                                        <th>Most Recent<br />
                                            Previous Year</th>
                                        <th colspan="2">Most Recent<br />
                                            Previous<br />
                                            Emissions</th>
                                        <th colspan="2">
                                            <%=EIYear%> Total<br />
                                            Emissions</th>
                                        <th>&gt;20%<br />
                                            Change</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:ListView ID="lvEmissionsBulk" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEmissionsUnitID" runat="server" Text='<%# Eval("EmissionsUnitID") %>' />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblProcessID" runat="server" Text='<%# Eval("ProcessID") %>' />
                                                    -
                                                    <asp:Label ID="lblProcessDesc" runat="server" Text='<%# Eval("STRPROCESSDESCRIPTION") %>' />
                                                </td>
                                                <td>
                                                    <asp:HyperLink ID="lblPollutantLink" runat="server" Text='<%# Eval("STRPOLLUTANTDESCRIPTION") %>'
                                                        NavigateUrl='<%# "~/EIS/rp_emissions_edit.aspx?eu=" & Eval("EmissionsUnitID") & "&ep=" & Eval("ProcessID") & "&em=" & Eval("POLLUTANTCODE") %>'
                                                        Target="_blank"></asp:HyperLink>
                                                    <asp:Label ID="lblPollutantCode" runat="server" Text='<%# Eval("POLLUTANTCODE") %>' Visible="false" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:Label ID="lblRptPeriodType" runat="server" Text='<%#Eval("RptPeriodType") %>' />
                                                </td>
                                                <td style="text-align: center"><%# Eval("prev_intInventoryYear") %></td>
                                                <td style="text-align: right"><%# If(IsDBNull(Eval("prev_fltTotalEmissions")), "N/A", Eval("prev_fltTotalEmissions")) %></td>
                                                <td><%# If(IsDBNull(Eval("prev_fltTotalEmissions")), "", Eval("PollutantUnit")) %></td>
                                                <td style="text-align: right">
                                                    <asp:TextBox ID="txtNewTotalEmission" CssClass="grvTextBox" runat="server" Width="100px" MaxLength="6"
                                                        Text='<%# Bind("CURR_FLTTOTALEMISSIONS") %>'></asp:TextBox></td>
                                                <td><%# Eval("PollutantUnit") %>
                                                    <act:FilteredTextBoxExtender ID="txtNewTotalEmission_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtNewTotalEmission"
                                                        ValidChars=".">
                                                    </act:FilteredTextBoxExtender>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:Label ID="lbl20Percent" runat="server" Visible='<%# Eval("EmissionsChangeGreaterThan20Percent") %>'>
                                                        <span class='highlight-normal'>Yes**</span>
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </tbody>
                                <tfoot>
                                    <tr class="bulk-table-controls">
                                        <td colspan="10">
                                            <asp:DataPager runat="server" ID="BottomPager" PagedControlID="lvEmissionsBulk">
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
                                        <td colspan="10">
                                            <br />
                                            <asp:Button ID="btnUpdateBottom" runat="server" Text="Update Changes" />&nbsp;&nbsp;
                                            <asp:Label ID="lblUpdateStatusBottom" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSummerDayNote" runat="server" Font-Size="Small">
                                * Summer Day emissions = emissions on an average summer day May 1 through Sep 30. Units are in tons per day (TPD).
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl20PercentNote" runat="server" Font-Size="Small" CssClass="highlight-normal">
                                ** If total emissions have increased or decreased by more than 20 percent over previously reported emissions, 
                                then supporting documentation must be submitted to the Air Protection Branch by email.<br /><br />
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span style="color: #AA0000">After making changes, click the
                                    <b><i>Update Changes</i></b> button <b>before</b> going to another page or leaving this form.</span>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
