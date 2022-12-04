<%@ Page Title="GECO Account" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.Account_Password" CodeBehind="Password.aspx.vb" %>

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

        <table class="table-simple table-list">
            <tr>
                <th>
                    <asp:Label AssociatedControlID="txtOldPassword" runat="server" Text="Old Password" />
                </th>
                <td>
                    <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" autocomplete="current-password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassword" Display="Dynamic"
                        ErrorMessage="Old Password is required." Font-Size="Small" ValidationGroup="OldPassword" ForeColor="Red"></asp:RequiredFieldValidator>&nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label AssociatedControlID="txtNewPassword" runat="server" Text="New Password" />
                </th>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" autocomplete="new-password" aria-describedby="password-constraints" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtNewPassword" Display="Dynamic"
                        ErrorMessage="New Password is required." Font-Size="Small" ValidationGroup="NewPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Regex3" runat="server" ControlToValidate="txtNewPassword"
                        ValidationExpression="^[a-zA-Z0-9]{12,}$" ValidationGroup="NewPassword"
                        ErrorMessage="Password is too short (minimum of 12 characters)." Display="Dynamic"
                        ForeColor="Red" />
                    <asp:CustomValidator runat="server" ID="passwordRequirements" ControlToValidate="txtNewPassword"
                        ClientValidationFunction="validatePassword" ForeColor="red" ValidateEmptyText="true" ValidationGroup="NewPassword"
                        Display="Dynamic" ErrorMessage="This text will be changed later."> </asp:CustomValidator>

                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label AssociatedControlID="txtPwdConfirm" runat="server" Text="Confirm New Password" />
                </th>
                <td>
                    <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" autocomplete="new-password" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPwdConfirm" Display="Dynamic"
                        ErrorMessage="Password Confirmation is required." Font-Size="Small" ValidationGroup="RepeatPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtPwdConfirm"
                        ErrorMessage="Password fields must match." Font-Size="Small" Display="Dynamic" ValidationGroup="RepeatPassword" ForeColor="Red"></asp:CompareValidator>&nbsp;
                </td>
            </tr>
        </table>

        <p>
            <asp:Button ID="btnPwdUpdate" runat="server" Text="Update Password" CausesValidation="true"
                OnClientClick="return client_btnRegister_Click();" CssClass="button-large" />
        </p>
    </asp:Panel>

    <p id="password-constraints">NOTE: New password must have at least 12 characters, cannot include your login, and is not in a list of passwords commonly used on other websites.</p>
    
    <script type="text/javascript">
        // global variable to track whether the error message is visible
        var isValidatorValid = true;
        /**
         * Client side code to check user's password. Used in CustomValidator ID="passwordRequirements"
         * @param sender
         * @param args
         */
        function validatePassword(sender, args) {
            var currEmail = document.getElementById('<%=lblDisplayName.ClientID%>').innerText;
            var currPassword = document.getElementById('<%=txtNewPassword.ClientID%>').value;
            if (!checkPasswordValid(currEmail.toLowerCase(), currPassword.toLowerCase())) {
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
            return true; // essential
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

            // check for falsy values
            if (!validPassEmail) {
                validPassEmail = "";
            }
            if (!validPassWebsite) {
                validPassWebsite = "";
            }
            if (!validPassDepartment) {
                validPassDepartment = "";
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
            var oldPwd = document.getElementById('<%=txtOldPassword.ClientID%>').value;
            var currPwd = document.getElementById('<%=txtNewPassword.ClientID%>').value;
            var currConfirmPwd = document.getElementById('<%=txtPwdConfirm.ClientID%>').value;
            var group = [];
            // if the TextBox is not empty, then update its group validators
            if (oldPwd !== "") {
                group.push("OldPassword");
            }
            if (currPwd !== "") {
                group.push("NewPassword");
            }
            if (currConfirmPwd !== "") {
                group.push("RepeatPassword");
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

        function client_btnRegister_Click() {
            // call all validators in the page
            Page_ClientValidate();
            // this return value determines whether the server-side function is called
            // true to calling the server-side function, false to prevent it
            return Page_IsValid;
        }

    </script>
</asp:Content>
