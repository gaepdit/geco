<%@ Page Title="Georgia EPD Event Details" Language="VB" MasterPageFile="~/MainMaster.master"
    AutoEventWireup="false" Inherits="GECO.EventRegistration_EventDetails" CodeBehind="Details.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <h1>
        <asp:Label ID="lblTitle" runat="server"></asp:Label>
    </h1>

    <asp:Label ID="lblLoginWarning" runat="server">
        <p class="message-update"><strong>A GECO account is required to register for any event.</strong></p>
    </asp:Label>

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

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="progressMessage">
                Please Wait...
                <br />
                <img alt="Loading" src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
