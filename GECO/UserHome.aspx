<%@ Page Title="" Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.UserHome" CodeBehind="UserHome.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel_top" runat="server">
        <ContentTemplate>
            <act:TabContainer runat="server" ID="UserTabs">
                <act:TabPanel runat="Server" ID="FacilityAccess" HeaderText="Facility Access">
                    <ContentTemplate>
                        <asp:Label ID="lblNone" runat="server" ForeColor="#C04000" Visible="False">
                            No facilities assigned. If this is incorrect, please sign out and then sign back in. If still incorrect, please contact us.
                        </asp:Label>
                        <asp:Label ID="lblAccess" runat="server" Visible="false">
                            You have access to work on the following facilities:<br /><br />
                        </asp:Label>

                        <asp:GridView ID="grdAccess" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-bordered table-menu"
                            BackColor="White" BorderStyle="None" CellPadding="3" Visible="False">
                            <Columns>
                                <asp:TemplateField HeaderText="Facility Name" ItemStyle-CssClass="table-cell-link">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlFacility" runat="server"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AIRS Number" ItemStyle-CssClass="table-cell-link">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlAirs" runat="server"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CheckBoxField DataField="AdminAccess" HeaderText="Admin Access">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField DataField="FeeAccess" HeaderText="Emission Fees">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField DataField="EIAccess" HeaderText="Emission Inventory">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField DataField="ESAccess" HeaderText="Emission Statement">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                            </Columns>
                            <AlternatingRowStyle BackColor="#f3f3f7" />
                            <HeaderStyle CssClass="table-head" />
                            <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                        </asp:GridView>
                        <br />
                        <asp:Button runat="server" UseSubmitBehavior="false" CausesValidation="false"
                            OnClientClick="var w=window.open('AirsRequestAccess.aspx','', 'width=700,height=820,scrollbars=yes,resizeable=yes');"
                            Text="Request access to a facility" />
                    </ContentTemplate>
                </act:TabPanel>

                <act:TabPanel runat="Server" ID="tpAccount" HeaderText="My Account">
                    <ContentTemplate>
                        <asp:Label ID="lblPwdMsg" runat="server" ForeColor="White" BackColor="red" Visible="False"></asp:Label>
                        <table>
                            <tr>
                                <td align="right">Email:</td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="unwatermarked" Enabled="False"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Email address can't be blank." Font-Size="Small" ValidationGroup="Email"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="A valid email address is required." Font-Size="Small"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic" ValidationGroup="Email"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnEditEmail" runat="server" Text="Change My Email" CausesValidation="False" />
                        <asp:Button ID="btnEditPwd" runat="server" Text="Change My Password" CausesValidation="False" />&nbsp;
                        <asp:Button ID="btnSaveEmail" runat="server" Text="Save Email" CausesValidation="true" Enabled="False" Visible="false" ValidationGroup="Email" />
                        <asp:LinkButton ID="btnCancelEmail" runat="server" CausesValidation="False" Text="Cancel" Enabled="False" Visible="False" />&nbsp;
                        <br />
                        <br />
                        <asp:Label ID="lblEmailWarning" runat="server" Visible="false">
                            NOTE: A change in email will not take effect until after the new email address has been confirmed.
                        </asp:Label>
                        <br />
                        <asp:Panel ID="tblChangePwd" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td align="right">Old Password:</td>
                                    <td>
                                        <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" Enabled="False" CssClass="unwatermarked"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassword" Display="Dynamic"
                                            ErrorMessage="Old Password is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <p>New password must contain at least 8 characters with at least 1 uppercase letter, 1 lowercase letter and 1 number.</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">New Password:</td>
                                    <td>
                                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Enabled="False" CssClass="unwatermarked"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtNewPassword" Display="Dynamic"
                                            ErrorMessage="New Password is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="Regex3" runat="server" ControlToValidate="txtNewPassword"
                                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" ValidationGroup="Password"
                                            ErrorMessage="Password does not meet complexity requirements."
                                            ForeColor="Red" />

                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">Confirm New Password:</td>
                                    <td>
                                        <asp:TextBox ID="txtPwdConfirm" runat="server" TextMode="Password" Enabled="False" CssClass="unwatermarked"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPwdConfirm" Display="Dynamic"
                                            ErrorMessage="Password Confirmation is required." Font-Size="Small" ValidationGroup="Password"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtPwdConfirm"
                                            ErrorMessage="Password fields must match." Font-Size="Small" Display="Dynamic" ValidationGroup="Password"></asp:CompareValidator>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnPwdUpdate" runat="server" Text="Save Password" CausesValidation="true" Visible="false"
                                Enabled="False" ValidationGroup="Password" />
                            <asp:LinkButton ID="btnPwdCancel" runat="server" CausesValidation="False" Text="Cancel" Visible="false"
                                Enabled="False" />&nbsp;
                        </asp:Panel>
                    </ContentTemplate>
                </act:TabPanel>

                <act:TabPanel runat="Server" ID="MyProfile" HeaderText="My Profile">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td align="right">Salutation:</td>
                                        <td>
                                            <asp:TextBox ID="txtSalutation" runat="server" CssClass="unwatermarked" Enabled="False" MaxLength="5">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">First Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtFName" runat="server" CssClass="unwatermarked" Enabled="False">
                                            </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="txtFName" ErrorMessage="Type First Name" Font-Size="Small"
                                                ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtLName" runat="server" CssClass="unwatermarked" Enabled="False"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName" ErrorMessage="Type Last Name"
                                                Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Title:</td>
                                        <td>
                                            <asp:TextBox ID="txtTitle" runat="server" CssClass="unwatermarked" Enabled="False"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTitle" ErrorMessage="Type Title"
                                                Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Company Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtCoName" runat="server" CssClass="unwatermarked" Enabled="False"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCoName" ErrorMessage="Type Company Name"
                                                Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Street Address:</td>
                                        <td>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="unwatermarked" Enabled="False"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress" ErrorMessage="Type Street Address"
                                                Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">City:</td>
                                        <td>
                                            <asp:TextBox ID="txtCity" runat="server" CssClass="unwatermarked" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity"
                                                ErrorMessage="Type City Name" Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">State:</td>
                                        <td>
                                            <asp:TextBox ID="txtState" runat="server" CssClass="unwatermarked" Enabled="False" MaxLength="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtState"
                                                ErrorMessage="State Abbreviation" Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Zip Code:</td>
                                        <td>
                                            <asp:TextBox ID="txtZip" runat="server" CssClass="unwatermarked" MaxLength="5" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtZip"
                                                ErrorMessage="Type 5-digit Zip Code" Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>&nbsp;
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtZip"
                                                FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Telephone Number:</td>
                                        <td>
                                            <asp:TextBox ID="txtPhone" runat="server" CssClass="unwatermarked" MaxLength="10"
                                                Enabled="False"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                                                    runat="server" ControlToValidate="txtPhone" ErrorMessage="10-digit Phone Number"
                                                    Font-Size="Small" ValidationGroup="Profile"></asp:RequiredFieldValidator>
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPhone"
                                                FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Telephone Ext:</td>
                                        <td>
                                            <asp:TextBox ID="txtPhoneExt" runat="server" CssClass="unwatermarked" MaxLength="5"
                                                Width="64px" Enabled="False"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPhoneExt"
                                                FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Fax Number:</td>
                                        <td>
                                            <asp:TextBox ID="txtFax" runat="server" CssClass="unwatermarked" MaxLength="10" Enabled="False"></asp:TextBox>
                                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFax"
                                                FilterType="Numbers">
                                            </act:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">User Type:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlUserType" runat="server" Enabled="False">
                                                <asp:ListItem>-- Select One --</asp:ListItem>
                                                <asp:ListItem>Environmental Consultant</asp:ListItem>
                                                <asp:ListItem>Government Agency</asp:ListItem>
                                                <asp:ListItem>Public</asp:ListItem>
                                                <asp:ListItem>Work for a facility</asp:ListItem>
                                                <asp:ListItem>Work for Environmental Group</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlUserType"
                                                ErrorMessage="Select One" Font-Size="Small" ValidationGroup="Profile" InitialValue="-- Select One --"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Button ID="btnEditProfile" runat="server" Text="Edit My Profile" CausesValidation="False" />
                                <asp:Button ID="btnUpdateProfile" runat="server" Text="Save Profile" ValidationGroup="Profile"
                                    Enabled="False" CausesValidation="True" Visible="False" />
                                <asp:LinkButton ID="btnCancelProfile" runat="server" CausesValidation="False" Text="Cancel"
                                    Enabled="False" Visible="False" />
                                <asp:Label ID="lblProfileMsg" runat="server" ForeColor="#C000C0" Visible="False"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </act:TabPanel>
            </act:TabContainer>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />

    <h1>GECO News and Events</h1>
    <p>
        If you need any assistance, please use the "Contact Us" link above and indicate 
        which GECO application you need help with by selecting the correct item in
        the "Regarding" field on the form. This is the most efficient way of contacting 
        the correct staff at the Air Protection Branch.
    </p>

    <h2>
        <asp:Label ID="lblEIyear1" runat="server" Text=""></asp:Label>
        Emissions Inventory</h2>
    <p>
        The Emissions Inventory System (EIS) will soon be available for use to submit data for the
        <asp:Label ID="lblEIYear2" runat="server" Text=""></asp:Label>
        calendar year. Look out for correspondence in the mail advising if EPD believes your facility needs to participate. 
        Visit the <a href="https://epd.georgia.gov/air/emissions-inventory-system-eis" target="_blank">EIS Information</a> page 
        and EPA's <a href="https://www.epa.gov/air-emissions-inventories/air-emissions-reporting-requirements-aerr"
            target="_blank">Air Emissions Reporting Requirements</a> page for more information.
    </p>
    <ul>
        <li>
            <p>
                Participation in the emissions inventory is based on a facility's Potential
                to Emit (PTE) equaling or exceeding the defined thresholds.
                For assistance with calculating PTE, please use the 
                <a href="https://epd.georgia.gov/air/documents/potential-emit-guidelines" target="_blank">PTE Guidelines</a>.
            </p>
        </li>
        <li>
            <asp:Label ID="lblAnnualEIText" runat="server">
                <p>
                    The
                    <asp:Label ID="lblEIYear5" runat="server" Text=""></asp:Label>
                    EI is an <b>annual</b> data collection year; therefore the number of facilities
                    required to submit EI data is greatly reduced as the PTE thresholds are higher.
                </p>
            </asp:Label>
            <asp:Label ID="lblTriennialEIText" runat="server" Visible="false">
                <p>
                    The
                    <asp:Label ID="lblEIYear6" runat="server" Text=""></asp:Label>
                    EI is a <b>triennial</b> data collection year; therefore most facilities in the 
                    EI system are required to submit EI data as the PTE thresholds are lower.
                </p>
            </asp:Label>
        </li>
        <li>
            <p>
                The deadline to submit the EI for calendar year
                <asp:Label ID="lblEIYear3" runat="server" Text=""></asp:Label>
                is <b>June&nbsp;30,&nbsp;<asp:Label ID="lblEIYear4" runat="server" Text=""></asp:Label></b>.
            </p>
        </li>
    </ul>
    <h2>
        <asp:Label ID="lblFeeYear1" runat="server" Text=""></asp:Label>
        Emissions Fees
    </h2>
    <p>
        The
        <asp:Label ID="lblFeeYear2" runat="server" Text=""></asp:Label>
        Emissions Fee process begins 
        July&nbsp;1,&nbsp;<asp:Label ID="lblFeeYear3" runat="server" Text=""></asp:Label>. 
        The deadline for fee submittal is 
        <strong>September&nbsp;1,&nbsp;<asp:Label ID="lblFeeYear4" runat="server" Text=""></asp:Label></strong>. 
    </p>
    <p>
        If you need to make an amendment to any past fee submittal, please select the appropriate
            facility above, navigate to the Emissions Fees application, and select the
            Supporting Documents tab. The Fee Admentment form is available for download as a
            Microsoft Excel file. The Fee Calculation worksheets are there also.
    </p>

    <h2>
        <asp:Label ID="lblESYear1" runat="server" Text=""></asp:Label>
        Emissions Statement
    </h2>
    <p>
        The
        <asp:Label ID="lblESYear2" runat="server" Text="Label"></asp:Label>
        Emissions Statement is due 
        <strong>June&nbsp;15,&nbsp;<asp:Label ID="lblESYear3" runat="server" Text=""></asp:Label></strong>. 
    </p>
</asp:Content>
