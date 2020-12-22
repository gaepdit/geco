<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO Emissions Inventory System"
    Inherits="GECO.EIS_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <p>
        Facilities whose potential emissions exceed the thresholds must report their 
        actual emissions. For assistance with calculating PTE, please use the        
        <a href="https://epd.georgia.gov/documents/potential-emit-guidelines" target="_blank">Potential 
            to Emit Guidelines</a>.
        Beginning with the 2019 Emissions Inventory, Georgia will be using the <i>Combined Air Emissions 
        Reporting System</i> (CAERS) developed by U.S. EPA.
    </p>

    <div id="dNewProcess" runat="server">
        <p>
            The new Emissions Inventory process will be as follows:
        </p>
        <ol>
            <li>
                <p>
                    Based on previously available information, the Georgia Air Protection Branch will enroll 
                    facilities that may need to participate in the Emissions Inventory. (If your facility 
                    has not been enrolled, but you believe it should be participating in the EI, please 
                    contact the APB.)
                </p>
            </li>
            <li>
                <p>
                    Begin the EI process below. You will be asked to review basic facility and contact 
                    information. You will then be asked about facility emissions to determine if 
                    participation in the Emissions Inventory process is necessary. 
                    <em>All facilities must complete this step before proceeding to the CAERS.</em>
                </p>
            </li>
            <li>
                <p>
                    If it is determined that the facility will participate in the Emissions Inventory 
                    process, you will be directed to EPA's CAERS to complete the Emissions Inventory.
                </p>
            </li>
        </ol>
    </div>

    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlStatus" runat="server" CssClass="panel">
                <h2><%= EiStatus.MaxYear.ToString %> Emissions Inventory Process</h2>

                <p>
                    Current Status:
                    <strong>
                        <asp:Label ID="lblMainStatus" runat="server"></asp:Label>
                    </strong>
                </p>

                <p>
                    <asp:Label ID="lblMainMessage" runat="server"></asp:Label>
                </p>

                <p id="pBeginProcess" runat="server" visible="false">
                    <a href="<%= Page.ResolveUrl("~/EIS/Process/")  %>" class="button button-large">Begin EI Process</a>
                </p>

                <p id="pReset" runat="server" visible="false">
                    <asp:Button ID="btnReset" runat="server" Text="Reset EIS Status" />
                </p>

                <div id="dCdxNext" runat="server" visible="false">
                    <p>
                        The Emissions Inventory process will continue at EPA's <i>Central Data Exchange</i> (CDX).
                        CDX is used to access CAERS. The preparers and certifiers you have specified should 
                        follow this procedure if new to CDX/CAERS:
                    </p>
                    <ol>
                        <li>Register in CDX using the link below.</li>
                        <li>Set up CAERS in CDX.</li>
                        <li>Await the approval email from CAERS that your account is linked to the correct facilities.</li>
                    </ol>
                    <p>
                        <asp:HyperLink ID="CdxLink" runat="server" Text="Link to EPA CDX" Target="_blank" CssClass="button" />
                    </p>
                </div>

                <table id="StatusTable" runat="server" class="table-simple table-list">
                    <tr id="trOptOutReason" runat="server" visible="false">
                        <th>Reason Not Participating:</th>
                        <td>
                            <asp:Label ID="lblOptOutReasonText" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trConfNumber" runat="server">
                        <th>Confirmation #:</th>
                        <td>
                            <asp:Label ID="lblConfNumberText" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trLastUpdate" runat="server">
                        <th>Submitted on:</th>
                        <td>
                            <asp:Label ID="lblLastUpdateText" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlResetStatus" runat="server" CssClass="panel panel-inprogress" Visible="False">
                <h2>Reset <%= EiStatus.MaxYear.ToString %> Emissions Inventory Status</h2>

                <table class="table-simple table-list">
                    <tr>
                        <th>Status:</th>
                        <td>
                            <asp:Label ID="lblResetStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>Submitted on:</th>
                        <td>
                            <asp:Label ID="lblResetDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>

                <p>This will reset your facility status for the <%= EiStatus.MaxYear.ToString %> Emissions Inventory. Are you sure you want to continue?</p>
                <p>
                    <asp:Button ID="btnConfirmResetStatus" runat="server" CausesValidation="False" Text="Yes" CssClass="button-large" />
                    <asp:Button ID="btnCancelResetStatus" runat="server" CausesValidation="False" Text="Cancel" CssClass="button-large button-cancel" />
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="pnlEisNotAvailable" runat="server" CssClass="panel panel-noaction" Visible="False">
        <h2><%= EiStatus.MaxYear.ToString %> Emissions Inventory Not Available</h2>
        <p>
            The Emissions Inventory is currently unavailable to your facility.
            If you have questions, please contact the Air Protection Branch.
        </p>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" CssClass="panel panel-error" Visible="False">
        <h2>Emissions Inventory Error</h2>
        <p>
            There is a data error. Please contact the Air Protection Branch for further assistance. 
            (<asp:Label ID="lblErrorId" runat="server"></asp:Label>)
        </p>
    </asp:Panel>
</asp:Content>
