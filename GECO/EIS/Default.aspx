<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO Emissions Inventory"
    Inherits="GECO.EIS_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <p id="pFacUpdateMessage" runat="server" visible="false" class="message-highlight">
        <a href="#facility-info" class="inner-anchor-link no-visited">Facility Information</a> was updated.
    </p>

    <p>
        Facilities whose potential emissions equal or exceed the thresholds indicated in the 
        Air Emission Reporting Requirements (AERR) rule (40 CFR Part 51 Subpart A) must report their actual 
        emissions annually or triennially. For assistance with calculating PTE, please use 
        the <a href="https://epd.georgia.gov/documents/potential-emit-guidelines" rel="noopener" target="_blank">Potential 
            to Emit Guidelines</a>.
        Since the 2019 Emissions Inventory, Georgia has used the <i>Combined Air Emissions 
            Reporting System</i> (<abbr>CAERS</abbr>) developed by U.S. EPA to comply with the AERR.
    </p>
    <p>
        Starting in 2023, the new Emissions Inventory process will be as follows. Based on previously 
        available information, the Georgia Air Protection Branch will enroll facilities that may need 
        to participate in the Emissions Inventory. <strong>(If your facility has not been enrolled, but you 
        believe it should be participating in the EI, please email 
        <a href="mailto:emissions.inventory@dnr.ga.gov">emissions.inventory@dnr.ga.gov</a>.)</strong>
    </p>
    <p>
        The next steps apply to all enrolled facilities:
    </p>
    <ol>
        <li>
            <p>
                Review the basic <a href="#facility-info" class="inner-anchor-link no-visited">Facility Information</a>
                below and if there is any mistake, please let EPD know and we will verify the updates.
            </p>
        </li>
        <li>
            <p>
                Update/verify the 
                <a href="#caers-users" class="inner-anchor-link no-visited">CAERS Users</a> below. Please ensure 
                <asp:HyperLink ID="lnkOtherContactInfo" runat="server" NavigateUrl="~/Facility/Contacts.aspx">all other contact information</asp:HyperLink>
                is correct.
            </p>
            <p><strong>For users new to CDX and CAERS,</strong> please be aware of the following roles:</p>
            <ol>
                <li>
                    <p>
                        A <strong>preparer</strong> is authorized to prepare an emissions report for Emissions 
                        Inventory (EI) and/or Toxics Release Inventory (TRI) data for a given facility. You 
                        may be a consultant or a staff person for the company owning the facility.
                    </p>
                </li>
                <li>
                    <p>
                        A <strong>certifier</strong> is authorized to sign the emissions report on behalf of the 
                        facility to meet your legal obligation for reporting to your State, Local, or Tribal 
                        authority (SLT). There can only be one certifier per facility.
                    </p>
                </li>
            </ol>
            <p>
                Note: A certifier is only needed to complete the opt-out process. However, a designated 
                preparer may fill out the opt-out form but must notify the designated certifier to 
                finalize the opt-out status.
            </p>
        </li>
        <li>
            <p>
                The opt-in/opt-out process will continue at EPA's Central Data Exchange (CDX). 
                Click the green button below to start. Note that CAERS is not opening until February 6, 2023.
            </p>
            <p>
                <asp:HyperLink ID="HyperLink1" runat="server" Text="Link to EPA CDX" Target="_blank" CssClass="button button-large button-proceed" Visible="false" />
                <asp:Label ID="Label1" runat="server" Visible="false" CssClass="label-highlight" Font-Bold="True" />
            </p>
            <p>
                CDX is used to access CAERS. Once in CAERS, select your facility, then click on the 
                “Create New Report” button for the <%= Now.Year - 1 %> EI Report. Prompts will help determine whether 
                your facility will opt in or opt out.
            </p>
            <p>
                If you are new to CAERS, you can click on 
                <a href="https://docs.google.com/gview?url=https://epd.georgia.gov/document/document/geco-eicaers-troubleshooting-matrix-0/download"
                    rel="noopener" target="_blank">GECO EI/CAERS Troubleshooting Matrix</a>
                to help you determine how to get started in CAERS. 
            </p>
        </li>
        <li>
            <p>
                If your facility qualifies to opt out, please download the 
                <a href="2022-opt-out-form_annual.xlsx" download="2022-opt-out-form_annual.xlsx">Opt-out form</a>
                to start your <%= Now.Year - 1 %> EI reporting. Upload the completed form to CAERS.
            </p>
            <p>
                If new to CDX/CAERS, the preparer(s) and certifier you have specified should follow this 
                procedure:
            </p>
            <ol>
                <li>
                    <p>Register in CDX using the link to EPA's CDX above and set up CAERS in CDX.</p>
                </li>
                <li>
                    <p>Await email approval from CDX that their CAERS account is linked to the correct facilities.</p>
                </li>
                <li>
                    <p>
                        Once approved, select facility, then click on the “Create New Report” button for the 
                        <%= Now.Year - 1 %> Report.
                    </p>
                </li>
            </ol>
        </li>
    </ol>

    <p class="centered">
        <asp:HyperLink ID="CdxLink" runat="server" Text="Link to EPA CDX" Target="_blank" CssClass="button button-large button-proceed" Visible="false" />
        <asp:Label ID="lblCdxAlt" runat="server" Visible="false" CssClass="label-highlight" Font-Bold="True" />
    </p>

    <h2 id="facility-info">Facility Information</h2>

    <p>If any facility information is incorrect, please email <a href="mailto:emissions.inventory@dnr.ga.gov">emissions.inventory@dnr.ga.gov</a>.</p>

    <table class="table-simple table-list">
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
        Use of CAERS requires one certifier and one or more preparers.
        If a single person serves both roles, they must be added as both.
    </p>

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
</asp:Content>
