﻿<%@ Page Title="GECO Account" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.Account_Password" CodeBehind="Password.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Account</h1>

    <p>
        Logged in as
        <asp:Label runat="server" ID="lblDisplayName"></asp:Label>.
    </p>

    <p>
        <asp:Button ID="hlSignOut" runat="server" Text="Sign Out" PostBackUrl="~/Default.aspx?do=SignOut" CssClass="button-large"></asp:Button>
    </p>

    <ul class="menu-list-horizontal">
        <li>
            <asp:HyperLink ID="lnkEditProfile" runat="server" NavigateUrl="~/Account/">Profile</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkEditEmail" runat="server" NavigateUrl="~/Account/Email.aspx">Email</asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkEditPassword" runat="server" NavigateUrl="~/Account/Password.aspx" Enabled="false" CssClass="selected-menu-item disabled">Password</asp:HyperLink>
        </li>
    </ul>

    <h2>Update Password</h2>

    <asp:Panel ID="subForm" runat="server" DefaultButton="btnPwdUpdate">
        <p>
            <asp:Label ID="lblPasswordMessage" runat="server" CssClass="message-update"></asp:Label>
        </p>

        <table class="table-simple table-list" aria-label="Password update form">
            <tr>
                <th>
                    <asp:Label AssociatedControlID="txtOldPassword" runat="server" Text="Old Password" />
                </th>
                <td>
                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" autocomplete="current-password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassword" Display="Dynamic"
                        ErrorMessage="Old Password is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>&nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label AssociatedControlID="txtNewPassword" runat="server" Text="New Password" />
                </th>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" autocomplete="new-password" aria-describedby="password-constraints" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtNewPassword" Display="Dynamic"
                        ErrorMessage="New Password is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Regex3" runat="server" ControlToValidate="txtNewPassword"
                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" ValidationGroup="Password"
                        ErrorMessage="Password does not meet complexity requirements."
                        ForeColor="Red" />

                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label AssociatedControlID="txtPwdConfirm" runat="server" Text="Confirm New Password" />
                </th>
                <td>
                    <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" autocomplete="new-password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPwdConfirm" Display="Dynamic"
                        ErrorMessage="Password Confirmation is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtPwdConfirm"
                        ErrorMessage="Password fields must match." Font-Size="Small" Display="Dynamic" ValidationGroup="Password"></asp:CompareValidator>&nbsp;
                </td>
            </tr>
        </table>

        <p>
            <asp:Button ID="btnPwdUpdate" runat="server" Text="Update Password" CausesValidation="true" ValidationGroup="Password" CssClass="button-large" />
        </p>
    </asp:Panel>

    <p id="password-constraints">NOTE: New password must contain at least 8 characters with at least 1 uppercase letter, 1 lowercase letter and 1 number.</p>
</asp:Content>
