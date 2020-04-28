<%@ Page Title="Emissions Statement" Language="VB" MasterPageFile="es.master" AutoEventWireup="false" Inherits="GECO.ES_espast" CodeBehind="espast.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 100%">
        <tr>
            <td align="center"></td>
        </tr>
        <tr>
            <td align="center">
                <span style="font-size: 14pt; font-weight: bold; color: #4169e1;">Georgia EPD - Air
                    Protection Branch<br />
                    Emissions Statement</span>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <span style="font-size: large; color: #4169e1;">Data For Calendar Year
               
                    <asp:Label ID="lblPastESYear" runat="server"></asp:Label>
                </span>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <table style="width: 500px" align="center">
                    <tr>
                        <td align="right">
                            <b>Facility Name:</b></td>
                        <td align="left">
                            <asp:Label ID="lblFacilityName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>AIRS No:</b></td>
                        <td align="left">
                            <asp:Label ID="lblAIRSNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Confirmation No:</b></td>
                        <td align="left">
                            <asp:Label ID="lblConfNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">&nbsp;&nbsp;</td>
                    </tr>
                </table>
                <asp:Panel ID="pnlOptedIn" runat="server">
                    <table align="center" style="width: 400px">
                        <tr>
                            <td align="right">
                                <b>VOC Emissions:</b></td>
                            <td align="left">
                                <asp:Label ID="lblVOC" runat="server"></asp:Label>
                                &nbsp;tpy</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <b>NO<sub>x</sub> Emissions:</b></td>
                            <td align="left">
                                <asp:Label ID="lblNOx" runat="server"></asp:Label>
                                &nbsp;tpy</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlOptedOut" runat="server">
                    <table align="center" style="width: 500px">
                        <tr>
                            <td>The facility opted out of the Emissions Statement for
                               
                                <asp:Label ID="lblESPastYear2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <table align="center" style="width: 500px">
                    <tr>
                        <td align="center">
                            <input id="Button1" onclick="window.print()" style="width: 88px" type="button"
                                value="Print" />&nbsp;&nbsp;
                           
                            <asp:Button ID="btnEiHome" runat="server" Text="Back to ES Home" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp; &nbsp;</td>
                    </tr>
                </table>
                <br />
                &nbsp;<br />
            </td>
        </tr>
    </table>
</asp:Content>
