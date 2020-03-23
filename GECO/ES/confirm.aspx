<%@ Page Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="es.master" AutoEventWireup="false" Inherits="GECO.es_confirm" Title="GECO - Confirmation" Codebehind="confirm.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="text-align: center">
        <asp:Panel ID="pnlTop" runat="server" Width="100%" HorizontalAlign="Center">
            <table align="center">
                <tr>
                    <td>
                        <span style="font-size: 16pt; color: #4170e1; font-family: Arial"><strong>Emissions Statement
                                Confirmation<br />
                        </strong>Georgia EPD - Air Protection Branch</span></td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="pnlOptedOutYes" runat="server" Width="100%">
        <div style="text-align: center">
            <table width="75%">
                <tr>
                    <td align="center">
                        <strong><span style="font-size: 16pt; color: #4170e1; font-family: Arial">Opted Out</span></strong></td>
                </tr>
                <tr>
                    <td align="left">
                        <strong><span style="font-size: 16pt; color: #4170e1; font-family: Arial">&nbsp; </span>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td align="left">Your facility has chosen (or has previously chosen) to opt out of participating in the Emissions Statement
                        process for calendar year
                        <asp:Label ID="lblESYear1" runat="server" Font-Names="Verdana" Font-Size="Small"></asp:Label>
                        by indicating that actual emissions of VOCs and NOx were less than or equal to 25
                        tons for that year.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>The confirmation number issued for opting out was:<br />
                        <asp:Label ID="lblConfNum1" runat="server" Font-Bold="True" Font-Names="Verdana"
                            Font-Size="Medium"></asp:Label><br />
                        Today's Date:
                                        <asp:Label ID="lblDate1" runat="server"></asp:Label><br />
                        AIRS No.:
                                        <asp:Label ID="lblAirsNo1" runat="server"></asp:Label><br />
                        Facility:
                                        <asp:Label ID="lblFacility1" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnOptOutChange1" runat="server" Text="Make Changes"
                            Width="170px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="Button1" onclick="window.print()" style="width: 88px" type="button"
                        value="Print" /><br />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span style="color: #FF0000">NOTE: You will get a new confirmation number after
                    making changes. However, your original submission date is unchanged for
                    compliance purposes.</span></td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td>No changes can be made at this time. For further assistance,<br />
                        contact the Georgia Air Protection Branch.<br />
                        Telephone: 404-363-7000</td>
                </tr>
                <tr>
                    <td>&nbsp; &nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlConfFinal" runat="server" Width="100%">
        <div style="text-align: center">
            <table width="75%">
                <tr>
                    <td align="center">
                        <strong><span style="font-size: 16pt; color: #4170e1; font-family: Arial">Opted In</span></strong></td>
                </tr>
                <tr>
                    <td align="left">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left">Your facility has opted in (or previously opted in) and submitted the Emissions Statement data for
                        calendar year
                        <asp:Label ID="lblESYear3" runat="server" Font-Names="Verdana" Font-Size="Small"></asp:Label>
                        by indicating that emissions of VOCs and/or NOx were greater than 25 tons for that
                        year. The emission quantities you entered appear below.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" style="height: 148px">The confirmation number for submitting the data is:<br />
                        <asp:Label ID="lblConfNumFinalize" runat="server" Font-Bold="True" Font-Names="Verdana"
                            Font-Size="Medium"></asp:Label><br />
                        Today's
                Date:
                <asp:Label ID="lblDate2" runat="server"></asp:Label><br />
                        AIRS No.:
                <asp:Label ID="lblAirsNo2" runat="server"></asp:Label><br />
                        Facility:
                <asp:Label ID="lblFacility2" runat="server"></asp:Label><br />
                        <br />
                        VOC Emissions:
                <asp:Label ID="lblVOCAmt2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"></asp:Label>
                        tons/year<br />
                        NOx Emissions:
                <asp:Label ID="lblNOXAmt2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"></asp:Label>
                        tons/year<br />
                        &nbsp;</td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>&nbsp; &nbsp;</td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>
                        <asp:Button ID="btnConfFinal" runat="server" Text="Make Changes"
                            Width="170px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="btnPrint" onclick="window.print()" style="width: 88px" type="button"
                        value="Print" /><br />
                        <span style="color: #FF0000">NOTE: You will get a new confirmation number.
                    Making changes will not<br />
                            change your original submission date on record for compliance purposes.</span></td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>&nbsp;&nbsp;</td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>For further assistance,<br />
                        contact the Georgia Air Protection Branch.<br />
                        Telephone: 404-363-7000</td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>&nbsp;</td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>&nbsp;&nbsp;</td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>&nbsp;</td>
                </tr>
                <tr style="font-family: Verdana">
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>