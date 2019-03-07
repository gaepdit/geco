<%@ Page Title="GECO - Contact Us" Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false"
    Inherits="GECO.ContactUs" CodeBehind="ContactUs.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Georgia Air Protection Branch Contact Form</h1>

    <asp:UpdatePanel ID="MessageUpdatePanel" runat="server">
        <ContentTemplate>
            <p>
                <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName">Name</asp:Label>
                <br />
                <asp:TextBox ID="txtName" runat="server" />
            </p>
            <p>
                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail">Email</asp:Label>
                <br />
                <asp:TextBox ID="txtEmail" runat="server" />
            </p>
            <p>
                <asp:Label ID="lblSubject" runat="server" AssociatedControlID="ddlSubject">Regarding</asp:Label>
                <br />
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
                    ControlToValidate="ddlSubject" ErrorMessage="Select a category." InitialValue="-- Select One --" />
            </p>
            <p>
                <asp:Label ID="lblMessage" runat="server" AssociatedControlID="txtMessage">Message</asp:Label>
                <br />
                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                    ControlToValidate="txtMessage" ErrorMessage="Message is required." />
            </p>
            <p>
                <asp:Label ID="lblSuccess" runat="server" Text="Success! Your message has been sent." Visible="False" CssClass="message-highlight" />
                <asp:Label ID="lblError" runat="server" Text="There was an error sending the message; please try contacting us by phone instead." Visible="false" CssClass="message-highlight" />
            </p>
            <p>
                <asp:Button ID="btnSend" runat="server" Text="Send Message" OnClick="btnSend_Click" CssClass="button-large" />
            </p>

            <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="MessageUpdatePanel" runat="server" DisplayAfter="50">
                <ProgressTemplate>
                    <div class="ProgressIndicator">
                        <p>
                            Sending Message...<br />
                            <img src="<%= Page.ResolveUrl("~/assets/images/loading.gif") %>" />
                        </p>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
