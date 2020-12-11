<%@ Page Title="GECO Emissions Inventory" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.eis_Default" CodeBehind="Default.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <h1>Emissions Inventory</h1>
    <p>
        Facilities whose potential emissions exceed the thresholds must report their 
        actual emissions. For assistance with calculating PTE, please use the        
        <a href="https://epd.georgia.gov/documents/potential-emit-guidelines" target="_blank">Potential 
            to Emit Guidelines</a>.
        Beginning with the 2019 Emissions Inventory, Georgia will be using the Combined Air Emissions 
        Reporting System (CAERS) developed by U.S. EPA.
    </p>

    <div class="announcement">
        GECO will be ready for the 2020 NEI on January 25, 2021. Thanks for your patience.
    </div>

    <div id="divCaersInstructions" runat="server">
        <p>
            The new Emissions Inventory process will be as follows:
        </p>
        <ol>
            <li>
                <p>
                    Based on previously available information, the Georgia Air Protection Branch will enroll 
                    facilities that may need to participate in the 2019 Emissions Inventory. (If your facility 
                    has not been enrolled, but you believe it should be participating in the EI, please 
                    contact the APB.)
                </p>
            </li>
            <li>
                <p>
                    Begin the EI process below. You will be asked to review basic facility and contact 
                    information. You will then be asked about facility emissions to determine if participation 
                    in the Emissions Inventory process is necessary. 
                    <em>All facilities must complete this step before proceeding to the CAERS.</em>
                </p>
            </li>
            <li>
                <p>
                    If it is determined that the facility will participate in the Emissions Inventory process, 
                    you will be directed to EPA's CAERS to complete the Emissions Inventory.
                </p>
            </li>
        </ol>
    </div>

    <asp:Panel ID="pnlStatus" runat="server" CssClass="rounded-panel rounded-panel-filled">
        <h2>
            <asp:Label ID="lblHeading" runat="server"></asp:Label>
        </h2>

        <div class="button-container-actions">
            <asp:Button ID="btnBegin" runat="server" CssClass="button button-large" Visible="false" />
            <asp:HyperLink ID="LinkToEpaCaers" runat="server" CssClass="button button-large" Visible="false"
                Text="Proceed to the EPA Combined Air Emissions Reporting System" Target="_blank" />
        </div>

        <p>
            <asp:Label ID="lblMainMessage" runat="server"></asp:Label>
        </p>

        <table>
            <tr>
                <th>Facility Name:</th>
                <td>
                    <asp:Label ID="lblFacilityNameText" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>AIRS No:</th>
                <td>
                    <asp:Label ID="lblFacilityIDText" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trStatus" runat="server">
                <th>Status:</th>
                <td>
                    <asp:Label ID="lblStatusText" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trOptOutReason" runat="server">
                <th>Reason Not Participating:</th>
                <td>
                    <asp:Label ID="lblOptOutReasonText" runat="server"></asp:Label></td>
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

        <div class="button-container-labeled">
            <asp:Button ID="btnReset" runat="server" CssClass="button" Visible="false" />
            <div>
                <asp:Label ID="lblOther" runat="server"></asp:Label>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlResetStatus" runat="server" CssClass="rounded-panel" Visible="False">
        <h2>
            <asp:Label ID="lblChangeHeading" runat="server">Reset Status</asp:Label>
        </h2>

        <table>
            <tr>
                <th>Current Status:</th>
                <td>
                    <asp:Label ID="lblResetStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>Submitted:</th>
                <td>
                    <asp:Label ID="lblResetDate" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

        <p>
            This will reset your facility status for the           
            <asp:Label ID="lblResetYear" runat="server" />
            EI.
            Are you sure you want to continue?
       
        </p>
        <div class="button-container-actions">
            <asp:Button ID="btnConfirmResetStatus" runat="server" CausesValidation="False" Text="Continue" CssClass="button button-large" />
            &nbsp;&nbsp;
           
            <asp:Button ID="btnCancelResetStatus" runat="server" CausesValidation="False" Text="Cancel" CssClass="button button-large button-cancel" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEisNotAvailable" runat="server" CssClass="rounded-panel"
        Visible="False" Style="display: inherit;">
        <h2>Emissions Inventory Not Available</h2>
        <p>
            The Emissions Inventory is currently unavailable to your facility.
            If you have questions, please contact us using the &quot;Contact Us&quot; menu item above.
       
        </p>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" CssClass="rounded-panel rounded-panel-filled"
        Visible="False">
        <h2>Contact the Air Protection Branch</h2>
        <p>
            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
        </p>
    </asp:Panel>

</asp:Content>
