<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.Login" Title="Georgia Environmental Connections Online" CodeBehind="Login.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="200" class="progressIndicator">
        <ProgressTemplate>
            <div class="progressIndicator-inner">
                Please Wait...<br />
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="login-page">
        <div class="login-form">
            <asp:UpdatePanel ID="LoginUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:MultiView ID="mvLogin" runat="server" ActiveViewIndex="0">
                        <asp:View ID="pnlLogin" runat="server">
                            <asp:Panel ID="pnlDefault" runat="server" GroupingText="Sign in to use GECO">
                                <p>
                                    <asp:Label ID="lblEmail" AssociatedControlID="txtUserId" runat="server" Text="Email:" />
                                    <br />
                                    <asp:TextBox ID="txtUserId" runat="server" ValidationGroup="Login" TextMode="Email" autocomplete="username" />
                                    <br />
                                    <asp:RequiredFieldValidator ID="reqUserId" runat="server" Display="Dynamic"
                                        ControlToValidate="txtUserId" ErrorMessage="Email is required." ValidationGroup="Login" />
                                </p>
                                <p>
                                    <asp:Label ID="lblPassword" AssociatedControlID="txtPassword" runat="server" Text="Password:" />
                                    <br />
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" ValidationGroup="Login" autocomplete="current-password" />
                                    <br />
                                    <asp:RequiredFieldValidator ID="reqUserPwd" runat="server" Display="Dynamic"
                                        ControlToValidate="txtPassword" ErrorMessage="Password is required." ValidationGroup="Login" />
                                </p>
                                <p>
                                    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember Me" Checked="false" />
                                </p>
                                <p>
                                    <asp:Button ID="btnSignIn" runat="server" Text="Sign In" ValidationGroup="Login" CssClass="button-large" />
                                    &nbsp;
                                    <asp:LinkButton ID="lbtForgotPwd" runat="server" CausesValidation="False">Forgot password?</asp:LinkButton>
                                </p>
                                <p>
                                    <asp:Label ForeColor="#AA0000" ID="lblUnconfirmed" runat="server" Visible="false">
                                        This account has not yet been confirmed. Would you like us to 
                                        <asp:HyperLink NavigateUrl="~/Account.aspx?action=resend" runat="server" CssClass="no-visited">resend the confirmation link</asp:HyperLink>?
                                    </asp:Label>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="#AA0000">
                                        Either the email is not registered or the password is incorrect. Please try again.
                                    </asp:Label>
                                </p>
                            </asp:Panel>
                            <div class="login-register">
                                <asp:HyperLink ID="linkRegister" NavigateUrl="~/Register.aspx" runat="server" CssClass="no-visited">Create new account</asp:HyperLink>
                            </div>
                        </asp:View>

                        <asp:View ID="pnlForgotPwd" runat="server">
                            <asp:MultiView ID="mvResetPassword" runat="server" ActiveViewIndex="0">
                                <asp:View ID="vResetPassword" runat="server">
                                    <asp:Panel ID="pnlReset" runat="server" GroupingText="Password Reset">
                                        <p>Enter your email. If you have an account, a link will be sent to reset your password.</p>
                                        <p>
                                            <asp:Label ID="lblEmailAddress" AssociatedControlID="txtEmailAddress" runat="server" Text="Email:" />
                                            <br />
                                            <asp:TextBox ID="txtEmailAddress" runat="server" ValidationGroup="Password" TextMode="Email" autocomplete="username" />
                                            <br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailAddress"
                                                Display="Dynamic" ErrorMessage="Email is required." ValidationGroup="Password" />
                                            <asp:CustomValidator ID="cvEmailExists" runat="server" ControlToValidate="txtEmailAddress"
                                                Display="Dynamic" ErrorMessage="No account exists for that email." ValidationGroup="Password" />
                                        </p>
                                        <p>
                                            <asp:Button ID="btnResetPassword" runat="server" Text="Submit" ValidationGroup="Password" CssClass="button-large" />
                                            &nbsp; 
                                            <asp:LinkButton ID="lbCancel" runat="server" Text="Return to login" CausesValidation="false" />
                                        </p>
                                    </asp:Panel>
                                </asp:View>

                                <asp:View ID="vResetResult" runat="server">
                                    <asp:Panel ID="Panel1" runat="server" GroupingText="Password Reset">
                                        <p>If an account exists, then an email will be sent with instructions for resetting your password.</p>
                                        <p>
                                            <asp:LinkButton ID="lbReturn" runat="server" Text="Return to login" CausesValidation="false" />
                                        </p>
                                    </asp:Panel>
                                </asp:View>

                                <asp:View ID="vResetError" runat="server">
                                    <asp:Panel ID="Panel2" runat="server" GroupingText="Password Reset">
                                        <p>
                                            There was an error resetting the password. Please try again at a later time. If the problem persists, please contact us.
                                        </p>
                                        <p>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Return to login" CausesValidation="false" />
                                        </p>
                                    </asp:Panel>
                                </asp:View>
                            </asp:MultiView>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
