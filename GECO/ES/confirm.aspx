<%@ Page Title="GECO - Emissions Statement Confirmation" Language="VB" MasterPageFile="~/Main.master"
    AutoEventWireup="false" Inherits="GECO.es_confirm" CodeBehind="Confirm.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">

    <asp:Panel ID="pnlTop" runat="server" Width="100%">
                    <span style="font-size: 16pt; color: #4170e1;"><strong>Emissions Statement Confirmation</strong></span>
                    <br />
                    <br />
    </asp:Panel>

    <asp:Panel ID="pnlOptedOutYes" runat="server" Width="100%">
            <table>
                <tr>
                    <td align="center">
                        <strong><span style="font-size: 16pt; color: #4170e1;">Opted Out</span></strong>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Your facility has chosen to opt out of participating in the Emissions Statement
                        process for calendar year                       
                        <asp:Label ID="lblESYear1" runat="server"></asp:Label>
                        by indicating that actual emissions of VOCs and NO<sub>x</sub> were each less than or equal to 25
                        tons for that year.
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        Confirmation number:<br />
                        <asp:Label ID="lblConfNum1" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                        <br />
                        <br />
                        Facility:
                        <asp:Label ID="lblFacility1" runat="server"></asp:Label><br />
                        AIRS Number:
                        <asp:Label ID="lblAirsNo1" runat="server"></asp:Label><br />
                        Today's Date:
                        <asp:Label ID="lblDate1" runat="server"></asp:Label><br />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOptOutChange1" runat="server" Text="Make Changes" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span style="color: #FF0000">NOTE: You will get a new confirmation number if you make changes. 
                            However, your original submission date will be unchanged for compliance purposes.</span>
                        <br />
                    </td>
                </tr>
            </table>
    </asp:Panel>

    <asp:Panel ID="pnlConfFinal" runat="server" Width="100%">
            <table>
                <tr>
                    <td align="center">
                        <strong><span style="font-size: 16pt; color: #4170e1;">Opted In</span></strong>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Your facility has opted in and submitted the Emissions Statement data for
                        calendar year                       
                        <asp:Label ID="lblESYear3" runat="server"></asp:Label>
                        by indicating that emissions of VOCs and/or NO<sub>x</sub> were greater than 25 tons for that
                        year. The emission quantities entered appear below.<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        Confirmation number:<br />
                        <asp:Label ID="lblConfNumFinalize" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                        <br />
                        <br />
                        Facility:
                        <asp:Label ID="lblFacility2" runat="server"></asp:Label><br />
                        AIRS Number:
                        <asp:Label ID="lblAirsNo2" runat="server"></asp:Label><br />
                        Today's Date:
                        <asp:Label ID="lblDate2" runat="server"></asp:Label><br />
                        <br />
                        VOC Emissions:
                        <asp:Label ID="lblVOCAmt2" runat="server" Font-Bold="True"></asp:Label>
                        tons/year<br />
                        NO<sub>x</sub> Emissions:
                        <asp:Label ID="lblNOXAmt2" runat="server" Font-Bold="True"></asp:Label>
                        tons/year<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnConfFinal" runat="server" Text="Make Changes" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span style="color: #FF0000">NOTE: You will get a new confirmation number if you make changes. 
                            However, your original submission date will be unchanged for compliance purposes.</span>
                        <br />
                    </td>
                </tr>
            </table>
    </asp:Panel>

</asp:Content>
