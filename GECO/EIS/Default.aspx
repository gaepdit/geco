<%@ Page Title="GECO Emissions Inventory System" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_Default" Codebehind="Default.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1>Emissions Inventory System</h1>
    <ul>
        <li>
            <p>
                Facilities whose potential emissions exceed the thresholds must report their actual emissions.
                For assistance with calculating PTE, please use the 
                <a href="https://epd.georgia.gov/air/documents/potential-emit-guidelines" target="_blank">PTE Guidelines</a>.
            </p>
        </li>
        <li>
            <p>
                The Facility Inventory (FI) will remain open all year and can be updated at any time. 
                The FI contains the following components: Facility Site/Contact Details, Release Points, 
                Emission Units, and Processes.
            </p>
        </li>
        <li>
            <p>
                The Emissions Inventory (EI) is only available for data submittal during the specified 
                submittal periods. The EI contains the following components: Process Reporting Period 
                and Reporting Period Emissions. Bulk entry is available for each.
            </p>
        </li>
    </ul>
    <asp:Panel ID="pnlStatus_Outer" runat="server" Width="100%">
        <table id="table7" align="center" style="width: 100%; text-align: center;">
            <tr>
                <td style="text-align: center; font-family: Verdana;">&nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: center; font-family: Verdana;" align="center">
                    <asp:Panel ID="pnlStatus_Inner" runat="server" Width="620px" BackColor="#009F50"
                        HorizontalAlign="Center">
                        <br />
                        <asp:Label ID="lblHeading" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                        <br />
                        <br />
                        <asp:Button ID="btnAction1" runat="server" Font-Size="Large" />
                        &nbsp;
                        <asp:Button ID="btnAction2" runat="server" Font-Size="Large" />
                        <br />
                        <br />
                        <div style="text-align: left; padding-left: 30px;">
                            <asp:Label ID="lblMainMessage" runat="server" Font-Size="Medium" ForeColor="White"></asp:Label>
                        </div>
                        <table align="center" cellspacing="1" style="width: 100%">
                            <tr>
                                <td align="right" style="width: 40%">&nbsp;&nbsp;
                                </td>
                                <td align="left">&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40%">
                                    <asp:Label ID="lblFacilityName" runat="server" Font-Bold="True" Font-Size="Medium"
                                        ForeColor="White"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFacilityNameText" runat="server" Font-Size="Medium" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40%">
                                    <asp:Label ID="lblFacilityID" runat="server" Font-Bold="True" Font-Size="Medium"
                                        ForeColor="White"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblFacilityIDText" runat="server" Font-Size="Medium" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40%">
                                    <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="White"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblStatusText" runat="server" Font-Size="Medium" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40%">
                                    <asp:Label ID="lblOptOutReason" runat="server" Font-Bold="True"
                                        Font-Size="Medium" ForeColor="White"></asp:Label></td>
                                <td align="left">
                                    <asp:Label ID="lblOptOutReasonText" runat="server" Font-Size="Medium"
                                        ForeColor="White"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40%">
                                    <asp:Label ID="lblConfNumber" runat="server" Font-Size="Medium" ForeColor="White"
                                        Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblConfNumberText" runat="server" Font-Size="Medium" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40%">
                                    <asp:Label ID="lblLastUpdate" runat="server" Font-Size="Medium" ForeColor="White"
                                        Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblLastUpdateText" runat="server" Font-Size="Medium" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 40%">&nbsp;&nbsp;</td>
                                <td align="left">&nbsp;&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblOther" runat="server" Font-Size="Small" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </asp:Panel>
                    <act:RoundedCornersExtender ID="RoundedCornersExtender4" TargetControlID="pnlStatus_Inner"
                        runat="server" Corners="All" Radius="9">
                    </act:RoundedCornersExtender>
                    <asp:Panel ID="pnlChange_Inner" runat="server" Width="620px" HorizontalAlign="Center"
                        BorderColor="Red" BorderStyle="Solid" BorderWidth="5px" Visible="False" Style="display: inherit;">
                        <br />
                        <asp:Label ID="lblChangeHeading" runat="server" Font-Bold="True" Font-Size="Large"
                            ForeColor="Black">Confirm Change</asp:Label>
                        <br />
                        <br />
                        <div style="font-size: medium;">
                            Click Continue to go ahead with the following change.<br />
                            Otherwise, click Cancel.
                        </div>
                        <br />
                        <asp:Label ID="lblChangeText" runat="server" Font-Size="Medium"></asp:Label>
                        <br />
                        <br />
                        <asp:Button ID="btnConfirmChange" runat="server" CausesValidation="False" Text="Continue" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Text="Cancel" />
                        <br />
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="pnlEISNotAvailable" runat="server" Width="620px" HorizontalAlign="Center"
                        BorderColor="Red" BorderStyle="Solid" BorderWidth="5px" Visible="False" Style="display: inherit;">
                        <br />
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Black">Emissions Inventory System Not Available</asp:Label>
                        <br />
                        <br />
                        <div style="font-size: medium;">
                            The EIS is currently unavailable to your facility.<br />
                            If this status continues for an extended period, please<br />
                            contact us using the &quot;Contact Us&quot; menu item above.
                        </div>
                        <br />
                        <br />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnFacilityHome" runat="server" CausesValidation="False" Text="Facility Home" />
                        <br />
                        <br />
                    </asp:Panel>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="text-align: center; font-family: Verdana;">&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
    </asp:Panel>
</asp:Content>
