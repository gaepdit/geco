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
        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" autocomplete="username" />
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
        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" autocomplete="new-password" aria-describedby="password-constraints" />
        <asp:CustomValidator runat="server" ID="passwordRequirements" ControlToValidate="txtPwd" ClientValidationFunction="validatePassword"
            ValidateEmptyText="true" Display="Dynamic" ErrorMessage="Password is too short (minimum of 12 characters), cannot include your login, and is not in a list of passwords commonly used on other websites."> </asp:CustomValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" Display="Dynamic"
            ControlToValidate="txtPwd" ErrorMessage="Password is required." />
        <br />
        <i id="password-constraints">Password must contain at least 8 characters with at least 1 uppercase letter, 1 lowercase letter and 1 number.</i>
    </p>

    <p>
        <asp:Label ID="lblPwdConfirm" AssociatedControlID="txtPwdConfirm" runat="server" Text="Confirm Password:" />
        <br />
        <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" autocomplete="new-password" />
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
                <asp:TextBox ID="txtCaptcha" runat="server" />
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

    <asp:UpdateProgress ID="CaptchaUpdateProgress" runat="server" AssociatedUpdatePanelID="CaptchaUpdatePanel" DisplayAfter="200" class="progressIndicator">
        <ProgressTemplate>
            <div class="progressIndicator-inner">
                Please Wait...<br />
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">
        /**
         * Client side code to check user's password. Used in CustomValidator ID="passwordRequirements"
         * @param sender
         * @param args
         */
        function validatePassword(sender, args) {
            var currEmail = document.getElementById('<%=txtEmail.ClientID%>').value;
            var currPassword = document.getElementById('<%=txtPwd.ClientID%>').value;
            // check if password length is long enough
            if (currPassword.length < 12) {
                // change the content of the error message
                sender.textContent = "Password is too short (minimum of 12 characters)";
                // set the validator isValid to false to make it appear
                args.IsValid = false;
            }
            // if the password is not valid
            else if (!checkPasswordValid(currEmail.toLowerCase(), currPassword.toLowerCase())) {
                // change the content of the error message
                sender.textContent = "The password cannot contain segments of the URL, app name, or email.";
                // set the validator isValid to false to make it appear
                args.IsValid = false;
            }
        }

        /**
         * Helper function for the validatePassword()
         * @param email The user email
         * @param password The user password
         * @returns True if the password is valid, false otherwise.
         */
        function checkPasswordValid(email, password) {
            // check if these passwords matches the email or website
            var validPassEmail = FindIntersection(email, password);
            var validPassWebsite = FindIntersection(getWebsiteName(), password);

            if (validPassEmail == null || validPassWebsite == null) {
                return false;
            }

            // declare an arbitrary length
            var maxSequenceLength = 3;
            return validPassEmail.length <= maxSequenceLength && validPassWebsite.length <= maxSequenceLength;
        }

        /**
         * Get the current website name
         */
        function getWebsiteName() {
            return window.location.hostname.toLowerCase();
        }

        /**
         * Find where the sequence starts and its length between 2 strings
         * @param a First string
         * @param b Second string
         * @returns Null if there are no sequence, else return an object with position and length attributes
         */
        function FindIntersection(a, b) {
            var bestResult = null;
            for (var i = 0; i < a.length - 1; i++) {
                var result = FindIntersectionFromStart(a.substring(i), b);
                if (result) {
                    if (!bestResult) {
                        bestResult = result;
                    } else {
                        if (result.length > bestResult.length) {
                            bestResult = result;
                        }
                    }
                }
                if (bestResult && bestResult.length >= a.length - i)
                    break;
            }
            return bestResult;
        }

        /**
         * Helper method for FindIntersection()
         * @param a First string
         * @param b Second string
         * @returns Null if there are no sequence, else return an object with position and length attributes
         */
        function FindIntersectionFromStart(a, b) {
            for (var i = a.length; i > 0; i--) {
                d = a.substring(0, i);
                j = b.indexOf(d);
                if (j >= 0) {
                    return ({ position: j, length: i });
                }
            }
            return null;
        }

    </script>
</asp:Content>
