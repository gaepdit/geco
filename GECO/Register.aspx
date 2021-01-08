<%@ Page Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.Register" Title="GECO - User Registration" CodeBehind="Register.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="captcha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h2>GECO Registration</h2>
    <p>
        Registration provides access to various facility-specific applications hosted by the Air Protection Branch. 
                A valid email address is required for registration.
    </p>

    <p>
        <asp:Label ID="lblEmail" AssociatedControlID="txtEmail" runat="server" Text="Email:" />
        <br />
        <asp:TextBox ID="txtEmail" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic"
            ControlToValidate="txtEmail" ErrorMessage="Email is required." />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
            ControlToValidate="txtEmail" ErrorMessage="Email address is invalid."
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        <asp:CustomValidator ID="cvEmailExists" runat="server" Display="Dynamic"
            ControlToValidate="txtEmail" ErrorMessage="The email address is already registered." />
    </p>

    <p>
        <asp:Label ID="lblPwd" AssociatedControlID="txtPwd" runat="server" Text="Password:" />
        <br />
        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" Display="Dynamic"
            ControlToValidate="txtPwd" ErrorMessage="Password is required." />
        <asp:RegularExpressionValidator ID="Regex3" runat="server"
            ControlToValidate="txtPwd" ErrorMessage="Password does not meet complexity requirements."
            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" />
        <br />
        <i>Password must contain at least 8 characters with at least 1 uppercase letter, 1 lowercase letter and 1 number.</i>
    </p>

    <p>
        <asp:Label ID="lblPwdConfirm" AssociatedControlID="txtPwdConfirm" runat="server" Text="Confirm Password:" />
        <br />
        <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" Display="Dynamic"
            ControlToValidate="txtPwdConfirm" ErrorMessage="Password confirmation is required." />
        <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic"
            ControlToCompare="txtPwd" ControlToValidate="txtPwdConfirm" ErrorMessage="Passwords do not match" />
    </p>

    <asp:UpdatePanel ID="CaptchaUpdatePanel" runat="server">
        <ContentTemplate>
            <p>
                <asp:Label ID="lblCaptcha" AssociatedControlID="txtCaptcha" runat="server" Text="Enter code as displayed in the image:" />
                <br />
                <asp:TextBox ID="txtCaptcha" runat="server" AutoCompleteType="None" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" Display="Dynamic"
                    ControlToValidate="txtCaptcha" ErrorMessage="Enter code as displayed in the image." />
                <asp:CustomValidator ID="cvCaptcha" runat="server" Display="Dynamic"
                    ControlToValidate="txtCaptcha" ErrorMessage="Code was incorrect or expired." />
                <captcha:CaptchaControl ID="captchaControl" runat="server" CaptchaBackgroundNoise="high"
                    CaptchaLength="5" CaptchaHeight="50" CaptchaWidth="180" CaptchaLineNoise="None"
                    CaptchaMinTimeout="1" CaptchaMaxTimeout="240"
                    ErrorInputTooFast="Whoa!" ErrorInputTooSlow="Image has expired." />
                <asp:LinkButton ID="lbtnRefreshCaptcha" runat="server" CausesValidation="false" Text="Refresh image" />
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <p>
        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="button-large" />
    </p>

    <p class="message-highlight">
        Once you have registered, a confirmation email with an activation link will be sent to your email 
                address. You must confirm your email before you will be able to sign into GECO. 
                <br />
        <br />
        If you do not receive a confirmation email, check your junk mail or spam folder and ensure that you can receive 
                emails from "GeorgiaAirProtectionBranch@dnr.ga.gov".
    </p>

    <asp:UpdateProgress ID="CaptchaUpdateProgress" runat="server" AssociatedUpdatePanelID="CaptchaUpdatePanel">
        <ProgressTemplate>
            <div class="ProgressIndicator">
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="Loading" />
            Please Wait...
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
