<%@ Page Title="GECO - Request Facility Access" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false"
    Inherits="GECO.Home_FacilityRequest" CodeBehind="FacilityRequest.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h2>GECO Facility Access Request</h2>

    <p id="pUpdateRequired" runat="server" visible="false" class="message-highlight">
        Your profile is missing required information.
        <asp:HyperLink ID="lnkUpdateProfile" runat="server" NavigateUrl="~/Account/" CssClass="no-visited">Please update before continuing</asp:HyperLink>.
    </p>

    <asp:Panel ID="pnlRequestAccess" runat="server">
        <asp:UpdatePanel ID="upRequestAccess" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <p class="announcement announcement-severe">All access requests must be reviewed and approved. Please allow at least 24 hours for requests to be processed.</p>
                <p>To begin, find the facility by AIRS number or name and select the type of access requested.</p>

                <table class="table-simple">
                    <tr>
                        <th scope="col">
                            <asp:Label ID="lblAirsNo" AssociatedControlID="txtAirsNo" runat="server">By AIRS Number</asp:Label><br />
                            <b class="table-cell-subhead">(without dashes)</b>
                        </th>
                        <th scope="col">
                            <asp:Label ID="lblFacility" AssociatedControlID="txtFacility" runat="server">By Facility Name</asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtAirsNo" runat="server" AutoPostBack="true" CssClass="input-small" />
                            <act:AutoCompleteExtender ID="aceAIRS" runat="server" CompletionSetCount="15" MinimumPrefixLength="3"
                                EnableCaching="true" CompletionInterval="10" TargetControlID="txtAirsNo"
                                ServiceMethod="AutoCompleteAirs" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFacility" runat="server" AutoPostBack="true" />
                            <act:AutoCompleteExtender ID="aceFacility" runat="server" CompletionSetCount="15" MinimumPrefixLength="3"
                                EnableCaching="true" CompletionInterval="10" TargetControlID="txtFacility"
                                ServiceMethod="AutoCompleteFacility" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <b>
                                <asp:Label ID="lblAccess" runat="server" AssociatedControlID="lstbAccess">GECO Access Type Requested</asp:Label></b>
                            <em>(Select all that apply.)</em><br />
                            <asp:CheckBoxList ID="lstbAccess" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="Facility Access" Selected="True" Enabled="false" />
                                <asp:ListItem Text="Permit Fees" />
                                <asp:ListItem Text="Emissions Inventory" />
                                <asp:ListItem Text="Emissions Statement" />
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>

                <asp:Panel ID="pnlNextSteps" runat="server" Visible="false" CssClass="announcement announcement-mild announcement-wide">
                    <asp:Panel ID="pnlHasAdmin" runat="server" Visible="false">
                        <p>
                            This facility has the following admin users. Please contact them directly to 
                            request access or use the form below to send an automated message.
                        </p>
                        <asp:BulletedList ID="lstAdminUsers" runat="server" />
                        <p class="inline-checkbox label-highlight label-highlight-mild">
                            <asp:CheckBox ID="chkAssistanceNeeded" runat="server" AutoPostBack="True"
                                Text="If these admin users are unavailable or you need assistance 
                                from the Air Protection Branch instead, check this box." />
                        </p>
                    </asp:Panel>

                    <p id="pNoAdmin" runat="server" visible="false">
                        Use this form to send your request to the Georgia Air Protection Branch to be processed.
                    </p>

                    <b>
                        <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments">Additional Comments</asp:Label></b>
                    <em>(Optional; these comments will be attached to the message.)</em><br />
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Columns="40" />
                    <p>
                        <asp:Button ID="btnSend" runat="server" Text="Send Request" Enabled="false" CssClass="button-large button-proceed" />
                        <asp:Label ID="lblAdminInstructions" runat="server" Visible="False">
                            The following message will be sent to the <b>Facility Administrators</b> listed above.
                        </asp:Label>
                        <asp:Label ID="lblApbInstructions" runat="server" Visible="False">
                            The following message will be sent to the <b>Air Protection Branch.</b>
                        </asp:Label>
                        <asp:Label ID="lblSuccess" runat="server" Font-Bold="True" CssClass="text-success" Visible="False">
                            Success! The message has been sent.
                        </asp:Label>
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" CssClass="text-error" Visible="False">
                            There was an error sending the message.
                        </asp:Label>
                    </p>

                <blockquote id="bqMessage" runat="server" visible="false" class="document">
                    <asp:Literal ID="ltlMessage" runat="server" />
                    <asp:Literal ID="ltlMessagePart2" runat="server" />
                    <asp:Literal ID="ltlMessagePart3" runat="server" />
                </blockquote>
                </asp:Panel>

                <asp:UpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="200" class="progressIndicator">
                    <ProgressTemplate>
                        <div class="progressIndicator-inner">
                            Please Wait...<br />
                            <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" alt="" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
