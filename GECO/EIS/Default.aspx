<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO Emissions Inventory System"
    Inherits="GECO.EIS_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <p>
        Facilities whose potential emissions equal or exceed the thresholds must report their 
        actual emissions. For assistance with calculating PTE, please use the        
        <a href="https://epd.georgia.gov/documents/potential-emit-guidelines" target="_blank">Potential 
            to Emit Guidelines</a>.
        Since the 2019 Emissions Inventory, Georgia has used the <i>Combined Air Emissions 
        Reporting System</i> (<abbr>CAERS</abbr>) developed by U.S. EPA.
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
                    email <a href="mailto:emissions.inventory@dnr.ga.gov">emissions.inventory@dnr.ga.gov</a>.)
                </p>
            </li>
            <li>
                <p>
                    Begin the EI process below. You will be asked to review basic facility information. 
                    You will then be asked about facility PTE emissions to determine if participation in the 
                    Emissions Inventory process is necessary. 
                </p>
            </li>
            <li>
                <p>
                    If it is determined that the facility will participate in the Emissions Inventory process, 
                    you will be directed to provide preparer and certifier information. If it is determined 
                    that the facility will not participate in the Emissions Inventory process, the facility 
                    will be complete with the 2020 Emissions Inventory, and steps 4 and 5 can be ignored.
                </p>
            </li>
            <li>
                <p>
                    For facilities new to CDX/CAERS: Once you provide preparer and certifier information, you 
                    will be directed to EPA’s CDX to provide the same information. For facilities who 
                    participated in the 2019 EI, skip this step unless a new preparer/certifier needs to be added.
                </p>
            </li>
            <li>
                <p>
                    A notification will be sent in March indicating when facilities can begin their 2020 
                    Emissions Inventory.
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
                    <asp:Button ID="btnBeginEiProcess" runat="server" Text="Begin EI Process" CssClass="button-large button-proceed" />
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
                        <li>Await approval that your CAERS account is linked to the correct facilities.</li>
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
