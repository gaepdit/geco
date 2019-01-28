<%@ Page Language="VB" AutoEventWireup="false" Inherits="GECO.ContactUs" Codebehind="ContactUs.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en-us" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GECO - Contact Us</title>
    <link rel="apple-touch-icon" sizes="180x180" href="~/assets/epd-favicons/apple-touch-icon.png?v=69kRrvbXdL" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-32x32.png?v=69kRrvbXdL" sizes="32x32" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-16x16.png?v=69kRrvbXdL" sizes="16x16" />
    <link rel="mask-icon" href="~/assets/epd-favicons/safari-pinned-tab.svg?v=69kRrvbXdL" color="#5bbad5" />
    <link rel="shortcut icon" href="~/favicon.ico?v=69kRrvbXdL" />
    <meta name="theme-color" content="#e5f6fa" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <h1>Georgia Air Protection Branch <br />Contact Form</h1>
                    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                        <ProgressTemplate>
                            <div class="ProgressIndicator">
                                <p>
                                    Sending Email...<br />
                                    <img src="<%= Page.ResolveUrl("~/assets/images/loading.gif") %>" />
                                </p>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <div><asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close this Window" CausesValidation="False" /></div>
                    <asp:Label ID="lblSuccess" runat="server" Font-Bold="True" ForeColor="#007700" Text="Success! Your Email has been sent."
                        Visible="False"></asp:Label>
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="#770000" Text="There was an error sending the email. Please try contacting us by phone instead."
                        Visible="False"></asp:Label>
                    <br />
                    <br />
                    Name:<br />
                    <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
                    <br />
                    Email:<br />
                    <asp:TextBox ID="txtEmail" runat="server" Width="300px"></asp:TextBox><br />
                    Regarding:<br />
                    <asp:DropDownList ID="ddlSubject" runat="server">
                        <asp:ListItem>-- Select One --</asp:ListItem>
                        <asp:ListItem>Emission Inventory</asp:ListItem>
                        <asp:ListItem>Emission Statement</asp:ListItem>
                        <asp:ListItem>Permit Fees</asp:ListItem>
                        <asp:ListItem>Event Registration</asp:ListItem>
                        <asp:ListItem>Permitting</asp:ListItem>
                        <asp:ListItem>Compliance</asp:ListItem>
                        <asp:ListItem>Monitoring</asp:ListItem>
                        <asp:ListItem>GECO Account</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server"
                        ControlToValidate="ddlSubject" ErrorMessage="Select One" Font-Size="Small" InitialValue="-- Select One --">
                    </asp:RequiredFieldValidator><br />
                    Message:<br />
                    <asp:TextBox ID="txtMessage" runat="server" Height="160px" TextMode="MultiLine" Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMessage" ErrorMessage="Message is required"
                        Font-Size="Small"></asp:RequiredFieldValidator><br />
                    <br />
                    <asp:Button ID="btnSend" runat="server" Text="Send Message" OnClick="btnSend_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>