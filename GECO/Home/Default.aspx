<%@ Page Title="" Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.Home" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel_top" runat="server">
        <ContentTemplate>
            <h1>GECO Home</h1>

            <p>Georgia Environmental Connections Online ("GECO") is an online service allowing public access to various Georgia Air Protection Branch applications.</p>

            <p id="pUpdateRequired" runat="server" visible="false" class="message-highlight">
                Your profile is missing required information. 
                <asp:HyperLink ID="lnkUpdateProfile" runat="server" NavigateUrl="~/Account/" CssClass="no-visited">Please update before continuing</asp:HyperLink>.
            </p>

            <asp:Label ID="lblNone" runat="server" ForeColor="#C04000" Visible="False">
                <p>No facilities assigned. If this is incorrect, please sign out and then sign back in. If still incorrect, please contact us.</p>
            </asp:Label>
            <asp:Label ID="lblAccess" runat="server" Visible="false">
                <p>You have access to work on the following facilities:</p>
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
                    <asp:CheckBoxField DataField="FeeAccess" HeaderText="Permit Fees">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField DataField="EIAccess" HeaderText="Emissions Inventory">
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

            <p>
                <asp:HyperLink ID="lnkRequestAccess" runat="server" NavigateUrl="~/Home/FacilityRequest.aspx"
                    CssClass="button button-large">Request access to a facility</asp:HyperLink>
            </p>

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

    <div class="announcement announcement-wide">
        <h2>Announcement</h2>
        <p>
            Starting this year, Georgia will be using the Combined Air Emissions Reporting (CAER) 
            "Common Emissions Form" (CEF) for the 2019 Emissions Inventory data collection. To help 
            transition facilities to this new form, Georgia APB and U.S. EPA have prepared a manual 
            and will conduct webinars and live virtual help sessions.
        </p>
        <p>
            If you would like to participate in the webinars, please visit the
            <asp:HyperLink ID="lnkEvents" runat="server"
                NavigateUrl="~/EventRegistration/">Event Registration</asp:HyperLink>
            page for dates and times. After the CEF is open for submissions, we will also be conducting
            virtual live help training sessions that will run from the week of April&nbsp;14,&nbsp;2020
            to the week of June&nbsp;26,&nbsp;2020.
        </p>
        <p>
            For additional information regarding the webinars and training sessions, please contact Jing 
            Wang at <a href="mailto:jing.wang@dnr.ga.gov">jing.wang@dnr.ga.gov</a>.
        </p>
    </div>

    <h1>GECO News and Events</h1>
    <p>
        If you need any assistance, please use the "Contact Us" link above and indicate 
        which GECO application you need help with by selecting the correct item in
        the "Regarding" field on the form. This is the most efficient way of contacting 
        the correct staff at the Air Protection Branch.
    </p>

    <h2>Event Registration</h2>
    <p>
        The GECO site is used to register for classes, workshops, and other events hosted by Georgia EPD. View all
        <asp:HyperLink ID="linkEvents" runat="server" NavigateUrl="~/EventRegistration/">upcoming events</asp:HyperLink>.
    </p>

    <h2>
        <asp:Label ID="lblEIyear1" runat="server" Text=""></asp:Label>
        Emissions Inventory</h2>
    <p>
        The Emissions Inventory (EI) will soon be available for use to submit data for the
        <asp:Label ID="lblEIYear2" runat="server" Text=""></asp:Label>
        calendar year. Look out for correspondence in the mail advising if EPD believes your facility needs to participate. 
        Visit the EPD <a href="https://epd.georgia.gov/submit-emissions-inventory" target="_blank">Emissions Inventory Information</a> page 
        and the EPA <a href="https://www.epa.gov/air-emissions-inventories/air-emissions-reporting-requirements-aerr"
            target="_blank">Air Emissions Reporting Requirements</a> page for more information.
    </p>
    <ul>
        <li>
            <p>
                Participation in the emissions inventory is based on a facility's Potential
                to Emit (PTE) equaling or exceeding the defined thresholds.
            </p>
        </li>
        <li>
            <p>
                <asp:Label ID="lblAnnualEIText" runat="server">The
                    <asp:Label ID="lblEIYear5" runat="server" Text=""></asp:Label>
                    EI is an <b>annual</b> data collection year; therefore the number of facilities
                    required to submit EI data is greatly reduced as the PTE thresholds are higher.
                </asp:Label>
                <asp:Label ID="lblTriennialEIText" runat="server" Visible="false">The
                    <asp:Label ID="lblEIYear6" runat="server" Text=""></asp:Label>
                    EI is a <b>triennial</b> data collection year; therefore most facilities in the 
                    EI universe are required to submit EI data as the PTE thresholds are lower.
                </asp:Label>
            </p>
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
        Annual Permit Fees
    </h2>
    <p>
        The
        <asp:Label ID="lblFeeYear2" runat="server" Text=""></asp:Label>
        Annual Permit Fee process begins 
        July&nbsp;1,&nbsp;<asp:Label ID="lblFeeYear3" runat="server" Text=""></asp:Label>. 
        The deadline for fee submittal is 
        <strong>September&nbsp;1,&nbsp;<asp:Label ID="lblFeeYear4" runat="server" Text=""></asp:Label></strong>. 
    </p>
    <p>
        If you need to make an amendment to any past fee submittal, please select the appropriate
        facility above, navigate to the Annual Permit Fees application, and select the
        Supporting Documents tab. The Fee Amendment form is available for download as a
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
