<%@ Page MasterPageFile="~/EIS/EIS.master" Language="VB" AutoEventWireup="false"
    Title="GECO Emissions Inventory"
    Inherits="GECO.EIS_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/EIS/EIS.master" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="Server">
    <p id="pFacUpdateMessage" runat="server" visible="false" class="message-highlight">
        <a href="#facility-info" class="inner-anchor-link no-visited">Facility Information</a> was updated.
    </p>

    <p>
        Facilities whose potential emissions equal or exceed the thresholds must report their actual 
        emissions. For assistance with calculating PTE, please use 
        the <a href="https://epd.georgia.gov/documents/potential-emit-guidelines" target="_blank">Potential 
            to Emit Guidelines</a>.
        Since the 2019 Emissions Inventory, Georgia has used the <i>Combined Air Emissions 
            Reporting System</i> (<abbr>CAERS</abbr>) developed by U.S. EPA.
    </p>
    <p>
        Starting in 2022, the new Emissions Inventory process will be as follows:
    </p>
    <ol>
        <li>
            <p>
                Based on previously available information, the Georgia Air Protection Branch will enroll 
                facilities that may need to participate in the Emissions Inventory. (If your facility 
                has not been enrolled, but you believe it should be participating in the EI, please 
                email <a href="mailto:emissions.inventory@dnr.ga.gov">emissions.inventory@dnr.ga.gov</a>.)
            </p>
        </li>
        <li>
            <p>
                Review basic <a href="#facility-info" class="inner-anchor-link no-visited">Facility Information</a>
                below and update as needed. Update the 
                <a href="#caers-users" class="inner-anchor-link no-visited">CAERS Users</a> below. Please ensure 
                <asp:HyperLink ID="lnkOtherContactInfo" runat="server" NavigateUrl="~/Facility/Contacts.aspx">all other contact information</asp:HyperLink>
                is correct.
            </p>
            <p>For users new to CDX and CAERS, please be aware of the following roles:</p>
            <ol>
                <li>
                    <p>
                        A <b>preparer</b> is authorized to prepare an emissions report for National Emissions 
                        Inventory (NEI) and/or Toxics Release Inventory (TRI) data for a given facility. You 
                        may be a consultant, or a staff person for the company owning the facility.
                    </p>
                </li>
                <li>
                    <p>
                        A <b>certifier</b> is authorized to sign the emissions report on behalf of the 
                        facility to meet your legal obligation for reporting to your State, Local, or Tribal 
                        authority (SLT).
                    </p>
                </li>
            </ol>
            <p>
                Note: A certifier is only needed to complete the opt-out form. However, a designated 
                preparer may fill out the opt-out form but must notify the designated certifier to 
                formalize the opt-out status.
            </p>
        </li>
        <li>
            <p>
                The opt-in/opt-out process will continue at EPA's Central Data Exchange (CDX). See link 
                below. CDX is used to access CAERS. Once in CAERS, select facility, then click on the 
                “Create New Report” button for the 2021 EI Report. Prompts will help determine whether 
                your facility will opt in or opt out.
            </p>
            <p>
                If you are new to CAERS, you can click on 
                <a href="https://docs.google.com/gview?url=https://epd.georgia.gov/document/document/2021gecocaerstablefinalpdf"
                    target="_blank">GECO EI/CAERS Troubleshooting Matrix</a>
                to help you determine how to get started in CAERS. If your facility may qualify to opt out,
                please download the 
                <a href="2021-opt-out-form.xlsx" download="2021-opt-out-form.xlsx">Opt-out form</a> 
                to start your 2021 EI reporting and then upload this form to CAERS when you complete the 
                required opt-out form.
            </p>
            <p>
                Note: The preparers and certifiers you have specified should follow this procedure if new 
                to CDX/CAERS:
            </p>
            <ol>
                <li>
                    <p>Register in CDX using the link below and set up CAERS in CDX.</p>
                </li>
                <li>
                    <p>Await approval that your CAERS account is linked to the correct facilities.</p>
                </li>
                <li>
                    <p>
                        Once approved, select facility, then click on the “Create New Report” button for the 
                        2021 Report.
                    </p>
                </li>
            </ol>
        </li>
    </ol>

    <p>
        <asp:HyperLink ID="CdxLink" runat="server" Text="Link to EPA CDX" Target="_blank" CssClass="button button-large button-proceed" Visible="false" />
        <asp:Label ID="lblCdxAlt" runat="server" Visible="false" CssClass="label-highlight" Font-Bold="True" />
    </p>

    <h2 id="facility-info">Facility Information</h2>

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
                    <small>* Operating status pertains only to Emissions Inventory</small>
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
        <asp:HyperLink ID="lnkGoogleMap" runat="server" Target="_blank" CssClass="no-visited">
            <asp:Image ID="imgGoogleStaticMap" runat="server" BorderStyle="Solid" BorderWidth="2px" /><br />
            Open map in new window
        </asp:HyperLink>
    </div>

    <p>
        <asp:HyperLink ID="btnEditFacilityInfo" runat="server" NavigateUrl="~/EIS/Facility/Edit.aspx" CssClass="button">
            Edit Facility Information
        </asp:HyperLink>
    </p>

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
