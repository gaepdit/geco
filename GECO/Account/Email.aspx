<%@ Page Title="GECO Account" Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Account_Email" CodeBehind="Email.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Account</h1>

    <p>
        Logged in as
        <asp:Label runat="server" ID="lblDisplayName"></asp:Label>.
    </p>

    <p>
        <asp:Button ID="hlSignOut" runat="server" Text="Sign Out" PostBackUrl="~/Default.aspx?do=SignOut"></asp:Button>
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkEditProfile" runat="server" NavigateUrl="~/Account/">Profile</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkEditEmail" runat="server" NavigateUrl="~/Account/Email.aspx" Enabled="false" CssClass="selected-menu-item disabled">Email</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkEditPassword" runat="server" NavigateUrl="~/Account/Password.aspx">Password</asp:HyperLink>
        </li>
    </ul>

    <h2>Change Email</h2>

    <asp:UpdatePanel ID="UpdatePanel_email" runat="server">
        <ContentTemplate>
            <asp:Panel ID="subForm" runat="server" DefaultButton="btnSaveEmail">
                <p>
                    <asp:Label ID="lblEmailMessage" runat="server" CssClass="message-update"></asp:Label>
                </p>

                <table class="table-simple table-list">
                    <tr>
                        <th>Email address</th>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email address is required." Font-Size="Small" ValidationGroup="Email"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="A valid email address is required." Font-Size="Small"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Display="Dynamic" ValidationGroup="Email"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>

                <p>
                    <asp:Button ID="btnSaveEmail" runat="server" Text="Update Email" CausesValidation="true" ValidationGroup="Email" CssClass="button-large" />
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <p>NOTE: A change in email will not take effect until after the new email address has been confirmed.</p>

    <asp:UpdateProgress ID="EmailUpdateProgress" runat="server" DisplayAfter="0">
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
</asp:Content>
