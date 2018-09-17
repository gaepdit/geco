<%@ Page Language="VB" AutoEventWireup="false" Inherits="GECO.AirsRequestAccess" Codebehind="AirsRequestAccess.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en-us" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GA Air - Request Facility Access</title>
    <link rel="apple-touch-icon" sizes="180x180" href="~/assets/epd-favicons/apple-touch-icon.png?v=69kRrvbXdL" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-32x32.png?v=69kRrvbXdL" sizes="32x32" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-16x16.png?v=69kRrvbXdL" sizes="16x16" />
    <link rel="manifest" href="~/assets/epd-favicons/manifest.json?v=69kRrvbXdL" />
    <link rel="mask-icon" href="~/assets/epd-favicons/safari-pinned-tab.svg?v=69kRrvbXdL" color="#5bbad5" />
    <link rel="shortcut icon" href="~/favicon.ico?v=69kRrvbXdL" />
    <meta name="theme-color" content="#e5f6fa" />
    <link href="~/assets/css/fees.css?v=20180913" rel="stylesheet" type="text/css" />
    <%-- <script language="javascript" type="text/javascript">

    function AirsNoSelected (source, eventArgs) {
    document.getElementById("txtFacName").value = eventArgs.get_value();
    document.getElementById("lbtReqAccess").style.display = "block";
    document.getElementById('SendRequest').style.visibility='hidden';
    }
    </script>--%>
    <%--<style type="text/css">body{margin-top:5px;margin-left:11px;}</style>--%>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="Server" EnablePartialRendering="true">
        </asp:ScriptManager>

        <asp:Panel ID="pnlRequestAccess" runat="server" CssClass="pnlmodalpopup">
            <asp:UpdatePanel ID="upRequestAccess" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--<asp:Button ID="btnPopupDisplay" runat="server" Style="display: none;" />
                    <act:ModalPopupExtender ID="mdePopup" runat="server" TargetControlID="btnPopupDisplay"
                        PopupControlID="pnlRequestAccess" BackgroundCssClass="mdemodalbackground" RepositionMode="RepositionOnWindowResizeAndScroll"
                        Y="20">
                    </act:ModalPopupExtender>--%>
                    <asp:Panel ID="pnlRequestAccessheader" runat="server" CssClass="pnlmodalheader">
                        <div class="pnlmodalheaderleft">
                            <h2>GA Air - Facility Access Request Form</h2>
                        </div>
                        <div class="pnlmodalheaderright">
                            <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close this Window" CausesValidation="False" />
                        </div>
                        <div style="clear: both;">
                        </div>
                        <p>
                            To request access for a facility, <b>please find the facility</b> and then click
                            on <b>Send Request</b> button at the bottom of the page. Your request will be either
                            forwarded to the Administrator account of the facility or to the GA Air Protection
                            Branch if there is no GECO Facility Administrator.
                        </p>
                        <table id="table4" class="popuptable">
                            <tr>
                                <td>By AIRS Number:
                                </td>
                                <td>By Facility Name:
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-bottom:20px">
                                    <asp:TextBox ID="txtAirsNo" runat="server" AutoPostBack="true" />
                                    <act:AutoCompleteExtender ID="aceAIRS" runat="server" CompletionSetCount="15" MinimumPrefixLength="2"
                                        EnableCaching="true" CompletionInterval="10" TargetControlID="txtAirsNo"
                                        ServiceMethod="AutoCompleteAirs">
                                    </act:AutoCompleteExtender>
                                </td>
                                <td style="padding-bottom:20px">
                                    <asp:TextBox ID="txtFacility" CssClass="unwatermarked" runat="server" AutoPostBack="true" />
                                    <act:AutoCompleteExtender ID="aceFacility" runat="server" CompletionSetCount="15"
                                        MinimumPrefixLength="2" EnableCaching="true" CompletionInterval="10" TargetControlID="txtFacility"
                                        ServiceMethod="AutoCompleteFacility">
                                    </act:AutoCompleteExtender>                                    
                                </td>
                            </tr>
                            <tr>
                                <td>Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="unwatermarked"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required"
                                        Font-Size="Small"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Email:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="unwatermarked"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required"
                                        Font-Size="Small"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                        ControlToValidate="txtEmail" runat="server"
                                        ErrorMessage="Valid Email is required" Font-Size="Small"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Application Access:</td>
                                <td>
                                    <asp:CheckBoxList ID="lstbAccess" runat="server">
                                        <asp:ListItem Text="Facility Access" Value="Facility" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Emission Fees" Value="Fees"></asp:ListItem>
                                        <asp:ListItem Text="Emission Inventory" Value="Inventory"></asp:ListItem>
                                        <asp:ListItem Text="Emission Statement" Value="Statement"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>Additional Comments:</td>
                                <td>
                                    <asp:TextBox ID="txtComments" runat="server" Height="70px" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
                            </tr>
                        </table>

                        <blockquote>
                            <b>Message:</b>
                            <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
                        </blockquote>

                        <asp:Button ID="btnSend" runat="server" Text="Send Request" OnClick="btnSend_Click" />
                        <asp:Label ID="lblSuccess" runat="server" Font-Bold="True" ForeColor="#007700" Text="Your message has been sent."
                            Visible="False"></asp:Label>
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="#770000" Text="Error."
                            Visible="False"></asp:Label>
                    </asp:Panel>
                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="0">
                        <ProgressTemplate>
                            <div id="progressBackgroundFilter">
                            </div>
                            <div id="progressMessage">
                                Please Wait...
                                <br />
                                <img alt="Loading" src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </form>
</body>
</html>