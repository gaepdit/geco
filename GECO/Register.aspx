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
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ValidationGroup="confirmEmail"
            ControlToValidate="txtEmail" ErrorMessage="Email is required." />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ValidationGroup="confirmEmail"
            ControlToValidate="txtEmail" ErrorMessage="Email address is invalid."
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        <asp:CustomValidator ID="cvEmailExists" runat="server" Display="Dynamic" ValidationGroup="confirmEmail"
            ControlToValidate="txtEmail" ErrorMessage="The email address is already registered." />
    </p>

    <p>
        <asp:Label ID="lblPwd" AssociatedControlID="txtPwd" runat="server" Text="Password:" />
        <br />
        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" autocomplete="new-password" aria-describedby="password-constraints" />
        <asp:CustomValidator runat="server" ID="passwordRequirements" ControlToValidate="txtPwd" ClientValidationFunction="validatePassword"
            ValidateEmptyText="true" ValidationGroup="passwordRequirements" Display="Dynamic" ErrorMessage="This text will be changed later."> </asp:CustomValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" Display="Dynamic"
            ControlToValidate="txtPwd" ValidationGroup="passwordRequirements" ErrorMessage="Password is required." />
        <br />
        <em id="password-constraints">Password needs to have at least 12 characters, cannot include your login, and is not in a list of passwords commonly used on other websites.</em>
    </p>

    <p>
        <asp:Label ID="lblPwdConfirm" AssociatedControlID="txtPwdConfirm" runat="server" Text="Confirm Password:" />
        <br />
        <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" autocomplete="new-password" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" Display="Dynamic" ValidationGroup="confirmPassword"
            ControlToValidate="txtPwdConfirm" ErrorMessage="Password confirmation is required." />
        <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ValidationGroup="confirmPassword"
            ControlToCompare="txtPwd" ControlToValidate="txtPwdConfirm" ErrorMessage="Passwords do not match" />
    </p>

    <asp:UpdatePanel ID="CaptchaUpdatePanel" runat="server">
        <ContentTemplate>
            <p>
                <asp:Label ID="lblCaptcha" AssociatedControlID="txtCaptcha" runat="server" Text="Enter code as displayed in the image:" />
                <br />
                <asp:TextBox ID="txtCaptcha" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" Display="Dynamic" ValidationGroup="confirmCaptcha"
                    ControlToValidate="txtCaptcha" ErrorMessage="Enter code as displayed in the image." />
                <asp:CustomValidator ID="cvCaptcha" runat="server" Display="Dynamic" ValidationGroup="confirmCaptcha"
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
        // global variable to track whether the error message is visible
        var isValidatorValid = true;
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
            } else {
                // change the content of the error message (not visible yet)
                sender.textContent = "The password is in a list of passwords commonly used on other websites.";
                hibpCheck(currPassword);
                // set the error message to be visible through the global variable value
                args.IsValid = isValidatorValid;
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
            var validPassWebsite = FindIntersection("geco", password);
            var validPassDepartment = FindIntersection("gaepd", password);

            if (validPassEmail == null || validPassWebsite == null || validPassDepartment == null) {
                return true;
            }

            // declare an arbitrary length
            var maxSequenceLength = 3;
            return validPassEmail.length <= maxSequenceLength &&
                validPassWebsite.length <= maxSequenceLength &&
                validPassDepartment.length <= maxSequenceLength;
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

        /**
         * Code from https://github.com/mehdibo/hibp-js
         * Helper method to convert a string to sha1 hash
         * @param string Input String
         */
        function sha1(string) {
            var buffer = new TextEncoder("utf-8").encode(string);
            return crypto.subtle.digest("SHA-1", buffer).then(function (buffer) {
                var hexCodes = [];
                var view = new DataView(buffer);
                for (var i = 0; i < view.byteLength; i += 4) {
                    var value = view.getUint32(i);
                    var stringValue = value.toString(16);
                    var padding = '00000000';
                    var paddedValue = (padding + stringValue).slice(-padding.length);
                    hexCodes.push(paddedValue)
                }
                return hexCodes.join("")
            })
        }

        /**
         * Helper method to get the result from the URL
         * @param url the URL of the website
         */
        async function fetchAsync(url) {
            let response = await fetch(url);
            return await response.text();
        }

        /**
         * Check if the user password matches any pawned passwords
         * @param pwd The user password
         */
        function hibpCheck(pwd) {
            // convert the string password to sha1 hash
            sha1(pwd).then(function (sha1Pwd) {
                // remove the first 5 characters to follow the API
                const hashSub = sha1Pwd.slice(5).toUpperCase();
                // fetch the result
                let url = 'https://api.pwnedpasswords.com/range/' + sha1Pwd.substr(0, 5);
                //let results = await fetchAsync(url);
                fetchAsync(url).then(function (results) {
                    // iterate through the result
                    let found = false;
                    for (const result of results.split('\n')) {
                        if (hashSub.localeCompare(result.split(':')[0]) === 0) {
                            found = true;
                            break;
                        }
                    }
                    // a temporary var to store the value before the change
                    var stateBefore = isValidatorValid;
                    // display the error
                    if (found) {
                        console.log("isValidatorValid " + isValidatorValid);
                        isValidatorValid = false;
                    } else {
                        isValidatorValid = true;
                    }
                    // re-validate the password custom validator if the state has changed
                    if (stateBefore != isValidatorValid) {
                        Reload_Validator();
                    }
                });
            });
        }

        /**
         * A helper method to update all of the validators of a group when its TextBox
         * value is not empty.
         * This is done because calling Page_ClientValidate on a single group would reset
         * all of the other validators (does not matter if they are in a group or not).
         */
        function Reload_Validator() {
            var currEmail = document.getElementById('<%=txtEmail.ClientID%>').value;
            var currPwd = document.getElementById('<%=txtPwd.ClientID%>').value;
            var currConfirmPwd = document.getElementById('<%=txtPwdConfirm.ClientID%>').value;
            var currCaptcha = document.getElementById('<%=txtCaptcha.ClientID%>').value;
            var group = [];
            // if the TextBox is not empty, then update its group validators
            if (currEmail !== "") {
                group.push("confirmEmail");
            }
            if (currPwd !== "") {
                group.push("passwordRequirements");
            }
            if (currConfirmPwd !== "") {
                group.push("confirmPassword");
            }
            if (currCaptcha !== "") {
                group.push("confirmCaptcha");
            }
            // call the helper method to get all the groups validate again
            Page_ClientValidateMultiple(group);
        }

        /**
         * Page_ClientValidate only shows errors from the last validation group.
         * This method allows showing for multiple groups.
         * https://stackoverflow.com/questions/1560812/page-clientvalidate-with-multiple-validationgroups-how-to-show-multiple-summ
         * @param groups An array that contains the ValidationGroup that we want to re-validate
         */
        function Page_ClientValidateMultiple(groups) {
            var invalidIdxs = [];
            var result = true;

            // run validation from each group and remember failures
            for (var g = 0; g < groups.length; g++) {
                result = Page_ClientValidate(groups[g]) && result;
                for (var v = 0; v < Page_Validators.length; v++)
                    if (!Page_Validators[v].isvalid)
                        invalidIdxs.push(v);
            }

            // re-show any failures
            for (var i = 0; i < invalidIdxs.length; i++) {
                ValidatorValidate(Page_Validators[invalidIdxs[i]]);
            }

            // return false if any of the groups failed
            return result;
        };

    </script>
</asp:Content>
