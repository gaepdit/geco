<%@ Page Title="Georgia EPD Event Details" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" Inherits="GECO.EventRegistration_EventDetails" CodeBehind="Details.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>
        <asp:Label ID="lblTitle" runat="server"></asp:Label>
    </h1>

    <p id="pLoginWarning" runat="server" class="message-highlight">
        <strong>A GECO account is required to register for any event.</strong>
        <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">Sign in</asp:HyperLink>
        or
        <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx" CssClass="no-visited">create an account</asp:HyperLink>.
    </p>

    <p id="pUpdateRequired" runat="server" visible="false" class="message-highlight">
        Your profile is missing required information. 
        <asp:HyperLink ID="lnkUpdateProfile" runat="server" NavigateUrl="~/Account/" CssClass="no-visited">Please update</asp:HyperLink>
        before registering.
    </p>

    <p id="pUpdateRequiredRegistered" runat="server" visible="false" class="message-highlight">
        Your profile is missing required information. 
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Account/" CssClass="no-visited">Please update</asp:HyperLink>.
    </p>

    <div>
        <asp:Literal ID="litEventDetails" runat="server"></asp:Literal>
        <asp:Literal ID="litCapacity" runat="server"></asp:Literal>
    </div>

    <asp:Panel ID="pnlLoggedIn" runat="server" Visible="false">
        <h2>Registration</h2>

        <p class="message-update">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </p>

        <asp:Panel ID="pnlPasscode" runat="server" Visible="false" DefaultButton="btnPasscode">
            To register for this event, you must have a passcode. Enter the passcode that was provided to you.<br />
            <br />
            <asp:TextBox ID="txtPasscode" runat="server" ValidationGroup="passcode"></asp:TextBox>
            <asp:Button ID="btnPasscode" runat="server" Text="Submit" ValidationGroup="passcode" />
            <asp:Label ID="lblPasscodeWrong" runat="server" Visible="false" ForeColor="DarkRed"><br />Incorrect passcode</asp:Label>
        </asp:Panel>

        <asp:Panel ID="pnlRegister" runat="server" Visible="false" DefaultButton="btnRegister">
            <p>
                <asp:Literal ID="litConfirmation" runat="server"></asp:Literal>
            </p>

            <table>
                <tr>
                    <td valign="bottom" align="right">Email:</td>
                    <td valign="bottom">
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">Comments:</td>
                    <td valign="top">
                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" MaxLength="1990" ValidationGroup="register" Columns="30" Rows="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnRegister" runat="server" Text="Register" ValidationGroup="register" />
                        <asp:Button ID="btnCancelRegistration" runat="server" Text="Cancel Registration" ValidationGroup="register" Visible="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
