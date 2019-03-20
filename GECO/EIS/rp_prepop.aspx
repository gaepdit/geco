<%@ Page Title="Pre-populate data - GECO Emission Inventory" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_rp_prepop" Codebehind="rp_prepop.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="1500"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" /><br />
                Please Wait. Prepopulation may take a bit longer depending on facility size.
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>

    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="Emission Inventory Prepopulation"
            Font-Bold="True"></asp:Label>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="upnlPrepop" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <div style="text-align: center">
                            <table style="width: 75%">
                                <tr>
                                    <td align="left">The facility will be participating in the
                                        <asp:Label ID="lblEIYear" runat="server"></asp:Label>
                                        &nbsp;Emissions Inventory process.<br />
                                        <br />
                                        You may choose to have the&nbsp;Emissions Inventory prepopulated with data from a
                                        previous year. If you&nbsp; wish to skip this option and begin with blank forms,
                                        choose &quot;Do not prepopulate&quot; in the list and click &quot;Continue.&quot;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <span style="font-size: 12pt"><strong>What would you like to do?</strong></span>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="2" style="width: 60%">
                            <tr>
                                <td align="center">Choose the year:
                                    <asp:DropDownList ID="ddlEIYears" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:RequiredFieldValidator ID="reqvEIYears" runat="server" ControlToValidate="ddlEIYears"
                                        Display="Dynamic" ErrorMessage="Make a choice from the above dropdown box." Font-Size="Small"
                                        InitialValue="-Make Selection-"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnPrePopulate" runat="server" Text="Continue"
                                        Enabled="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblActionDone" runat="server" Font-Bold="True" Font-Names="Arial"
                                        Font-Size="Small" ForeColor="Red" Visible="False"></asp:Label>
                                    &nbsp;
                                    <asp:Button ID="btnContinue" runat="server" Text="Click to Continue"
                                        Visible="False" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="text-align: center">
                            &nbsp;
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>