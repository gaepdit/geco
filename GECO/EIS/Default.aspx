<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO Emissions Inventory"
    Inherits="GECO.EIS_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">

    <p>
        For more information on how to submit your Emissions Inventory, visit 
        <a href="https://epd.georgia.gov/forms-permits/air-protection-branch-forms-permits/submit-emissions-inventory"
            rel="noopener" target="_blank">https://epd.georgia.gov/forms-permits/air-protection-branch-forms-permits/submit-emissions-inventory</a>.
    </p>

    <p>
        <asp:Label ID="lblEnrollmentStatus" runat="server" Visible="false" CssClass="message-highlight" Font-Bold="True" />
    </p>

    <h2 id="facility-info">Facility Information</h2>

    <p>
        Review the facility information below and if there is any mistake, please email: 
        <a href="mailto:emissions.inventory@dnr.ga.gov">emissions.inventory@dnr.ga.gov</a>.
    </p>

    <a href="#caers-users" class="button">Next</a>

    <table class="table-simple table-list" aria-labelledby="facility-info">
        <tbody>
            <tr>
                <th>Description</th>
                <td>
                    <asp:Label ID="lblDescription" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Operating Status</th>
                <td>
                    <asp:Label ID="lblOperatingStatus" runat="server" /><br />
                    <small>* Operating status pertains only to the last Emissions Inventory reported</small>
                </td>
            </tr>
            <tr>
                <th>NAICS Code</th>
                <td>
                    <asp:Label ID="lblNAICS" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Site Address</th>
                <td>
                    <asp:Label ID="lblSiteAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Geocoordinates</th>
                <td>
                    <asp:Label ID="lblLatitude" runat="server" />,
                        <asp:Label ID="lblLongitude" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>

    <div>
        <asp:HyperLink ID="lnkGoogleMap" runat="server" rel="noopener" Target="_blank" CssClass="no-visited">
            <asp:Image ID="imgGoogleStaticMap" runat="server" BorderStyle="Solid" BorderWidth="2px" /><br />
            Open map in new window
        </asp:HyperLink>
    </div>

    <h2 id="caers-users">CAERS Users</h2>

    <p>
        <asp:HyperLink ID="lnkOtherContactInfo" runat="server" NavigateUrl="~/Facility/Contacts.aspx" Target="_blank">
        Please ensure all contact information is correct in communication preferences before proceeding 
            updating CAERS contact information.</asp:HyperLink>
        Next add/update any of the CAERS users below. Use of CAERS requires one certifier and one or more 
        preparers. If a single person serves both roles, they must be added as both.
    </p>

    <a href="#submit-ei" class="button">Next</a>

    <h3>Current CAERS Users</h3>

    <p id="pNoUsersNotice" runat="server"><em>None.</em></p>

    <asp:GridView ID="grdCaersUsers" runat="server" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table-simple">
        <Columns>
            <asp:TemplateField HeaderText="Role">
                <ItemTemplate><%# Eval("CaerRole") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="User">
                <ItemTemplate>
                    <%# Eval("Honorific") %> <%# Eval("FirstName") %> <%# Eval("LastName") %><br />
                    <%# If(String.IsNullOrWhiteSpace(Eval("Title").ToString()), "", $"{Eval("Title")}<br />") %>
                    <%# Eval("Company") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <%# Eval("Street") %><br />
                    <%# If(String.IsNullOrWhiteSpace(Eval("Street2").ToString()), "", $"{Eval("Street2")}<br />") %>
                    <%# Eval("City")%>, <%# Eval("State") %> <%# Eval("PostalCode") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <ItemTemplate><%# Eval("Email") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate><%# Eval("PhoneNumber") %></ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <p>
        <asp:HyperLink ID="btnEditCaersUsers" runat="server" NavigateUrl="~/EIS/Users/Default.aspx" CssClass="button">
            Edit CAERS Users
        </asp:HyperLink>
    </p>

    <div id="submitEiSection" runat="server">
        <h2 id="submit-ei">Submit EI</h2>

        <p>
            If your facility qualifies to opt out, please download the 
            <a href="2022-opt-out-form_annual.xlsx" download="2022-opt-out-form_annual.xlsx">Opt-out form</a>
            to start your EI reporting. Upload the completed form to CAERS.
        </p>
        <p>
            If new to CDX/CAERS, the preparer(s) and certifier you have specified should follow this procedure:
        </p>
        <ol>
            <li>
                <p>Register in CDX using the link to EPA's CDX above and set up CAERS in CDX.</p>
            </li>
            <li>
                <p>Await email approval from CDX that their CAERS account is linked to the correct facilities.</p>
            </li>
            <li>
                <p>Once approved, select facility, then click on the “Create New Report” button for the Report.</p>
            </li>
        </ol>

        <p class="centered">
            <asp:HyperLink ID="CdxLink" runat="server" Visible="false" Text="Link to EPA CDX" Target="_blank" CssClass="button button-large button-proceed" />
            <asp:Label ID="lblCdxAlt" runat="server" Visible="false" CssClass="message-highlight" Font-Bold="True" Text="EI not applicable." />
        </p>
    </div>
</asp:Content>
