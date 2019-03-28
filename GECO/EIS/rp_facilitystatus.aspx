<%@ Page Title="Facility Status - GECO Emission Inventory" Language="VB" MaintainScrollPositionOnPostback="true"
    MasterPageFile="eismaster.master" AutoEventWireup="false"
    Inherits="GECO.EIS_rp_facilitystatus" CodeBehind="rp_facilitystatus.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Facility Operational Status
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlStatusQuery_Outer" runat="server" Width="100%">
                <table align="center" style="width: 100%; text-align: center;">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="text-align: center; font-family: Verdana;" align="center">
                            <asp:Panel ID="pnlStatusQuery_Inner" runat="server" HorizontalAlign="Center" Width="550px"
                                BackColor="#9bd7ff">
                                <br />
                                <div style="text-align: center; font-size: medium; font-weight: bold;">
                                    Did the facility operate at any time during<br />
                                    calendar year
                                    <asp:Label ID="lblEIYear1" runat="server" Text=""></asp:Label>?
                                </div>
                                <asp:RadioButtonList ID="rblOperate" runat="server" RepeatDirection="Horizontal"
                                    Font-Size="Medium" AutoPostBack="True" RepeatLayout="Flow">
                                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                    <asp:ListItem Value="No">No</asp:ListItem>
                                </asp:RadioButtonList><br />
                                <asp:RequiredFieldValidator ID="reqvOperateYesNo" runat="server"
                                    ControlToValidate="rblOperate"
                                    ErrorMessage="Select Yes or No to continue" Font-Bold="False" />
                                <br />
                                <div style="vertical-align: top">
                                    <asp:TextBox ID="txtComment" runat="server" BackColor="White"
                                        TextMode="MultiLine" Width="350px" MaxLength="400" Rows="4"></asp:TextBox>
                                    <act:TextBoxWatermarkExtender ID="txtComment_TextBoxWatermarkExtender"
                                        runat="server" Enabled="True" TargetControlID="txtComment"
                                        WatermarkCssClass="watermarked" WatermarkText="Type optional comments here">
                                    </act:TextBoxWatermarkExtender>
                                </div>
                                <br />
                                <asp:Panel ID="pnlShutdownStatus" runat="server" Visible="false">
                                    <div style="text-align: center;">
                                        <p>
                                            If the facility did not operate during
                                            <asp:Label ID="lblEIYear2" runat="server" Text=""></asp:Label>,
                                            <br />
                                            it will not participate in the Emissions Inventory process.
                                        </p>
                                        <div style="text-align: center; font-size: medium;">
                                            Is your facility colocated with another facility?
                                        </div>
                                        <asp:RadioButtonList ID="rblIsColocated" runat="server" RepeatDirection="Horizontal"
                                            Font-Size="Medium" AutoPostBack="True" RepeatLayout="Flow">
                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                        </asp:RadioButtonList><br />
                                        <asp:RequiredFieldValidator ID="reqIsColocated" runat="server"
                                            ControlToValidate="rblIsColocated"
                                            ErrorMessage="Select Yes or No to continue" Font-Bold="False" />
                                        <asp:Panel ID="pnlColocation" runat="server" Visible="false">
                                            <div style="text-align: center; font-size: medium;">
                                                What facility are you colocated with? Please provide name and AIRS # of colocated facility.
                                                Your facility and/or the colocated facility will be contacted by the Air Protection 
                                                Branch about possible EI submittal.
                                                <br />
                                                <asp:TextBox ID="txtColocatedWith" runat="server" BackColor="White"
                                                    TextMode="MultiLine" Width="350px" MaxLength="4000" Rows="4"></asp:TextBox><br />
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                                <br />
                                <asp:Button ID="btnContinue" runat="server" Text="Continue" />
                                <br />
                                <br />
                            </asp:Panel>
                            <act:RoundedCornersExtender ID="pnlStatusQuery_rce" runat="server" Enabled="True"
                                TargetControlID="pnlStatusQuery_Inner" Radius="9" Corners="All">
                            </act:RoundedCornersExtender>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
