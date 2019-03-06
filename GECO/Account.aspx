<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Account" Title="GECO - Registration" CodeBehind="Account.aspx.vb" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="captcha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel ID="AccountUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="ErrorResult" runat="server">
                    <h1 style="color: #ff0033">Error</h1>
                    <p>
                        There was an error during the process. Please try again
                    at a later time. If the problem persists, please contact us.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="RegisterExists" runat="server">
                    <h1 style="color: #ff0033">Account Exists</h1>
                    <p>
                        There is already an account registered with the email address that you provided.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="RegisterSuccess" runat="server">
                    <h1 style="color: #339900">Registration Successful</h1>
                    <p>
                        An email has been sent to the email address you provided with an activation link
                        to confirm your account. The link will expire after 2 hours.
                        Your account will not be active until you confirm using the activation link. 
                        If you have any questions, please contact us.                
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="NewEmailSent" runat="server">
                    <h1 style="color: #339900">Email Address Change Confirmation Sent</h1>
                    <p>
                        An email with an activation link has been sent to the address you provided. 
                        The link will expire after 2 hours.
                        The new email address will not be active until confirmed using the activation link.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="ConfirmSuccess" runat="server">
                    <h1 style="color: #339900">Account Confirmed</h1>
                    <p>
                        Thank you for confirming your account; your account is now active.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="ConfirmFailed" runat="server">
                    <h1 style="color: #ff0033">Account Confirmation Failed</h1>
                    <p>
                        Either the account does not exist or the account confirmation link has expired.
                    </p>
                    <p>
                        You can
                        <asp:LinkButton ID="lbtResend" runat="server">resend the confirmation email</asp:LinkButton>
                        or return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="ResendConfirmation" runat="server">
                    <h1 style="color: #339900">Send Account Confirmation Email</h1>
                    <p>
                        Enter the email address you used when registering for your GECO account.
                    </p>
                    <p>
                        <asp:Label ID="lblEmailAddress" AssociatedControlID="txtEmailAddress" runat="server" Text="Email:" />
                        <br />
                        <asp:TextBox ID="txtEmailAddress" runat="server" Width="250" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailAddress"
                            Display="Dynamic" ErrorMessage="Email is required." ValidationGroup="Resend" />
                        <asp:CustomValidator ID="cvEmailExists" runat="server" ControlToValidate="txtEmailAddress"
                            Display="Dynamic" ErrorMessage="No account exists for that email." ValidationGroup="Resend" />
                    </p>
                    <p>
                        Enter code as displayed in the image:<br />
                        <asp:TextBox ID="txtCaptcha" runat="server" AutoCompleteType="None" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCaptcha"
                            ValidationGroup="Resend" ErrorMessage="Enter code as displayed in the image." />
                        <br />
                        <asp:CustomValidator ID="cvCaptcha" runat="server" ControlToValidate="txtCaptcha" ValidationGroup="Resend"
                            Display="Dynamic" ErrorMessage="Code was incorrect or expired." />
                        <captcha:CaptchaControl ID="captchaControl" runat="server" CaptchaBackgroundNoise="high"
                            CaptchaLength="5" CaptchaHeight="50" CaptchaWidth="180" CaptchaLineNoise="None"
                            CaptchaMinTimeout="1" CaptchaMaxTimeout="240"
                            ErrorInputTooFast="Whoa!" ErrorInputTooSlow="Image has expired." />
                        <asp:LinkButton ID="lbtnRefreshCaptcha" runat="server" CausesValidation="false" Text="Refresh image"
                            ForeColor="DarkRed" />
                    </p>
                    <p>
                        <asp:Button ID="btnResend" runat="server" Text="Submit" ValidationGroup="Resend" CssClass="button-large" />
                    </p>
                </asp:View>

                <asp:View ID="ConfirmEmailSuccess" runat="server">
                    <h1 style="color: #339900">New Email Address Confirmed</h1>
                    <p>
                        Thank you for confirming your email address. Your new email should now be used when logging into GECO.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="ConfirmEmailFailed" runat="server">
                    <h1 style="color: #ff0033">New Email Address Confirmation Failed</h1>
                    <p>
                        An error occurred or the account confirmation link has expired.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>

                <asp:View ID="ResetAllowed" runat="server">
                    <asp:HiddenField ID="hidEmail" runat="server" Visible="false" />
                    <h1>Reset Your Password</h1>
                    <p>
                        New Password:<br />
                        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtPwd" ValidationGroup="NewPassword"
                            Display="Dynamic" ErrorMessage="Password is required." />
                        <asp:RegularExpressionValidator ID="Regex3" runat="server" ControlToValidate="txtPwd"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" ValidationGroup="NewPassword"
                            ErrorMessage="Password does not meet complexity requirements."
                            ForeColor="Red" />
                    </p>
                    <p>New password must contain at least 8 characters with at least 1 uppercase letter, 1 lowercase letter and 1 number.</p>
                    <p>
                        Confirm Password:
                    <br />
                        <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPwdConfirm" ValidationGroup="NewPassword"
                            Display="Dynamic" ErrorMessage="Password confirmation is required." />
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPwd" Display="Dynamic"
                            ControlToValidate="txtPwdConfirm" ErrorMessage="Passwords do not match" ValidationGroup="NewPassword" />
                    </p>
                    <p>
                        <asp:Button ID="btnSetPassword" runat="server" Text="Set Password" ValidationGroup="NewPassword" />&nbsp;
                        <asp:HyperLink runat="server" NavigateUrl="~" CssClass="no-visited">Cancel</asp:HyperLink>
                    </p>
                </asp:View>

                <asp:View ID="ResetFailed" runat="server">
                    <h1 style="color: #ff0033">Password Reset Failed</h1>
                    <p>
                        Either the account does not exist or the password reset link has expired.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in or to attempt the password reset again.
                    </p>
                </asp:View>

                <asp:View ID="ResetSuccess" runat="server">
                    <h1 style="color: #339900">Password Reset Successful</h1>
                    <p>
                        Your password has been changed.
                    </p>
                    <p>
                        Return to the
                        <asp:HyperLink runat="server" NavigateUrl="~/Login.aspx" CssClass="no-visited">login page</asp:HyperLink>
                        to sign in.
                    </p>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
