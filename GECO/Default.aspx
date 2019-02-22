<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO._Default" Title="Georgia Environmental Connections Online" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>Georgia Environmental Connections Online</h1>
    <h2>Welcome to GECO!</h2>

    <p>
        GECO is an online service allowing public access to various Georgia Air Protection Branch applications.
        You must be a registered user to access the online applications.
    </p>

    <h2>Event Registration</h2>
    <p>
        The GECO system is used to register for classes, workshops, and other events hosted by Georgia EPD. You may view the 
        <asp:HyperLink ID="linkEvents" runat="server" NavigateUrl="~/EventRegistration/">upcoming events</asp:HyperLink>, 
        but a GECO account is required to register for any event.
    </p>

    <h2>Emissions Inventory System</h2>
    <p>
        The <a href="https://www.epa.gov/air-emissions-inventories" target="_blank">National Emissions Inventory</a>
        (NEI) is a detailed estimate compiled by the US EPA of air emissions that include criteria pollutants and 
        hazardous air pollutants from air emissions sources.
    </p>
    <p>
        The State of Georgia collects actual facility emissions data through the
        <a href="https://epd.georgia.gov/air/emissions-inventory-system-eis" target="_blank">Emissions Inventory System</a>
        (EIS) and submits them to EPA. Emissions inventory data for all Part 70 major 
        source facilities are collected on a three year cycle, while a subset of facilities must report on an annual basis.
    </p>
    <p>
        The Emissions Inventory submittal is due on or about June&nbsp;30 each year.
    </p>

    <h2>Emissions Statement</h2>
    <p>
        Facilities in the Atlanta metro maintenance area whose NO<sub>x</sub> and/or VOC actual emissions are greater 
        than 25 tons per year are required to submit an annual 
        <a href="https://epd.georgia.gov/air/emissions-statement" target="_blank">Emissions Statement</a>
        (ES).
    </p>
    <p>
        The Emissions Statement is due on or about June&nbsp;15 each year.
    </p>

    <h2>Permit Application Fees</h2>
    <p>
        Permit application fees are required when applying for certain permit actions. Refer to the
        <a href="https://epd.georgia.gov/air/air-permit-fees" target="_blank">Air Permit Fee Manual</a> 
        to determine what type of fee is due when submitting a permit application.
    </p>
    <p>
        Permit application fees are owed in addition to any annual fees described below.
    </p>

    <h2>Annual (Emissions) Fees</h2>
    <p>
        Annual permit fees are required for Synthetic Minor and Part 70 sources, and certain NSPS sources. The 
        <a href="https://epd.georgia.gov/air/air-permit-fees" target="_blank">Air Permit Fee Manual</a> provides
        instructions and should be used to calculate fee amounts.
    </p>
    <p>
        The emission fee process begins July&nbsp;1 each year, and the deadline for fee submittal is on or about September&nbsp;1.
    </p>

    <h2>Test Notifications</h2>
    <p>
        This tool displays a list of test notifcations submitted by your facility to the Air Protection Branch.
    </p>


</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LeftMenuContent" runat="Server">
    <br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1">
        <ProgressTemplate>
            <div id="progressBackgroundFilter">
            </div>
            <div id="progressMessage">
                Please Wait...
                <br />
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="Loading" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:MultiView ID="mvLogin" runat="server" ActiveViewIndex="0">
        <asp:View ID="pnlLogin" runat="server">
            <asp:Panel ID="pnlDefault" runat="server" GroupingText="Sign in to use GECO">
                <p>
                    <asp:Label ID="lblEmail" AssociatedControlID="txtUserId" runat="server" Text="Email:"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtUserId" runat="server" ValidationGroup="Login"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="reqUserId" runat="server" Display="Dynamic" Font-Size="small"
                        ControlToValidate="txtUserId" ErrorMessage="Email is required." ValidationGroup="Login"></asp:RequiredFieldValidator>
                </p>
                <p>
                    <asp:Label ID="lblPassword" AssociatedControlID="txtPassword" runat="server" Text="Password:"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" ValidationGroup="Login"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="reqUserPwd" runat="server" Display="Dynamic" Font-Size="small"
                        ControlToValidate="txtPassword" ErrorMessage="Password is required." ValidationGroup="Login">
                    </asp:RequiredFieldValidator>
                </p>
                <p>
                    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember Me" Checked="false" />
                </p>
                <asp:Button ID="btnSignIn" runat="server" Text="Sign In" ValidationGroup="Login"></asp:Button>
                <p>
                    <asp:Label ForeColor="#AA0000" ID="lblUnconfirmed" runat="server" Visible="false">
                        This account has not yet been confirmed. Would you like us to 
                        <asp:HyperLink NavigateUrl="~/Account.aspx?action=resend" runat="server">resend the confirmation link</asp:HyperLink>?
                    </asp:Label>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#AA0000">
                        Either the email is not registered or the password is incorrect. Please try again.
                    </asp:Label>
                </p>
                <p>
                    <asp:LinkButton ID="lbtForgotPwd" runat="server" CausesValidation="False">Forgot password?</asp:LinkButton>
                </p>
                <p>
                    <asp:HyperLink ID="linkRegister" NavigateUrl="~/UserRegistration.aspx" runat="server">Create new account</asp:HyperLink>
                </p>
            </asp:Panel>
        </asp:View>

        <asp:View ID="pnlForgotPwd" runat="server">
            <asp:MultiView ID="mvResetPassword" runat="server" ActiveViewIndex="0">
                <asp:View ID="vResetPassword" runat="server">
                    <asp:Panel ID="pnlReset" runat="server" GroupingText="Password Reset">
                        <p>Enter your email. If you have an account, a link will be sent to reset your password.</p>
                        <p>
                            <asp:Label ID="lblEmailAddress" AssociatedControlID="txtEmailAddress" runat="server" Text="Email:"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtEmailAddress" runat="server" ValidationGroup="Password" Width="250"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmailAddress"
                                Display="Dynamic" ErrorMessage="Email is required." Font-Size="Small" ValidationGroup="Password">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvEmailExists" runat="server" ControlToValidate="txtEmailAddress"
                                Display="Dynamic" ErrorMessage="No account exists for that email." ValidationGroup="Password"
                                Font-Size="Small"></asp:CustomValidator>
                        </p>
                        <p>
                            <asp:Button ID="btnResetPassword" runat="server" Text="Submit" ValidationGroup="Password"></asp:Button>
                            &nbsp; 
                        <asp:LinkButton ID="lbCancel" runat="server" Text="Return to login" CausesValidation="false"></asp:LinkButton>
                        </p>
                    </asp:Panel>
                </asp:View>

                <asp:View ID="vResetResult" runat="server">
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Password Reset">
                        <p>If an account exists, then an email will be sent with instructions for resetting your password.</p>
                        <p>
                            <asp:LinkButton ID="lbReturn" runat="server" Text="Return to login" CausesValidation="false"></asp:LinkButton>.
                        </p>
                    </asp:Panel>
                </asp:View>

                <asp:View ID="vResetError" runat="server">
                    <asp:Panel ID="Panel2" runat="server" GroupingText="Password Reset">
                        <p>
                            There was an error resetting the password. Please try again
                            at a later time. If the problem persists, please contact us.
                        </p>
                        <p>
                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Return to login" CausesValidation="false"></asp:LinkButton>.
                        </p>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
        </asp:View>
    </asp:MultiView>

    <div class="announcement">
        <h2>Announcement</h2>
        <p>
            As of August 1, 2018, all GECO accounts must use a confirmed email address. If 
            you created your account prior to that date, you will need to request an email confirmation 
            link before you can sign in.
        </p>
        <p>
            <asp:HyperLink ID="lnkResend" runat="server" NavigateUrl="~/Account.aspx?action=resend">✉️ Request confirmation link</asp:HyperLink>
        </p>
        <p>
            If you no longer have access to the email address you 
            used when registering, please contact us to update your account.
        </p>
    </div>
</asp:Content>
