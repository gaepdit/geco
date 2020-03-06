<%@ Page Title="Facility Status - GECO Emission Inventory" Language="VB" MaintainScrollPositionOnPostback="true"
    MasterPageFile="eismaster.master" AutoEventWireup="false"
    Inherits="GECO.EIS_rp_facilitystatus" CodeBehind="rp_facilitystatus.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <h1>Facility Operational Status</h1>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlStatusQuery" runat="server" HorizontalAlign="Center" Width="550px">
                <br />
                <div style="text-align: center; font-size: medium;">
                    Did the facility operate at any time during<br />
                    calendar year
                    <asp:Label ID="lblEIYear1" runat="server" Text=""></asp:Label>?
                </div>
                <br />
                <asp:RadioButtonList ID="rblOperate" runat="server" RepeatDirection="Horizontal"
                    Font-Size="Medium" AutoPostBack="True" RepeatLayout="Flow">
                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                    <asp:ListItem Value="No">No</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:RequiredFieldValidator ID="reqvOperateYesNo" runat="server" ControlToValidate="rblOperate"
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
                <asp:Panel ID="pnlShutdownStatus" runat="server" Visible="false">
                    <div style="text-align: center;">
                        <p>
                            If the facility did not operate during
                            <asp:Label ID="lblEIYear2" runat="server" Text=""></asp:Label>,
                            <br />
                            it will not participate in the Emissions Inventory process.
                        </p>
                        <br />
                        <div style="text-align: center; font-size: medium;">
                            Is your facility colocated with another facility?
                        </div>
                        <br />
                        <asp:RadioButtonList ID="rblIsColocated" runat="server" RepeatDirection="Horizontal"
                            Font-Size="Medium" AutoPostBack="True" RepeatLayout="Flow">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                        <asp:RequiredFieldValidator ID="reqIsColocated" runat="server" ControlToValidate="rblIsColocated"
                            ErrorMessage="Select Yes or No to continue" Font-Bold="False" />
                        <asp:Panel ID="pnlColocation" runat="server" Visible="false">
                            <p>
                                What facility are you colocated with? Please provide name and AIRS&nbsp;# of colocated facility.
                                Your facility and/or the colocated facility will be contacted by the Air Protection 
                                Branch about possible EI submittal.
                            </p>
                            <asp:TextBox ID="txtColocatedWith" runat="server" BackColor="White"
                                TextMode="MultiLine" Width="350px" MaxLength="4000" Rows="4"></asp:TextBox>

                        </asp:Panel>
                    </div>
                </asp:Panel>
                <br />
                <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="button button-large" />
                <br />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
