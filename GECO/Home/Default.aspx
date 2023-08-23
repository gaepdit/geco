<%@ Page Title="" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" Inherits="GECO.Home" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
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

            <asp:GridView ID="grdAccess" runat="server" AutoGenerateColumns="False" CssClass="table-simple table-bordered table-menu table-checkbox-menu"
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
                    <asp:CheckBoxField DataField="AdminAccess" HeaderText="User Admin">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField DataField="FeeAccess" HeaderText="Permit Fees">
                        <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                    </asp:CheckBoxField>
                    <asp:CheckBoxField DataField="EIAccess" HeaderText="Emissions Inventory">
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
    <br />

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

    <h2>Emissions Inventory</h2>
    <p>
        Participation in the emissions inventory is based on a facility's Potential to Emit (PTE) equaling or exceeding the defined thresholds.
        Look out for correspondence in the mail advising if EPD believes your facility needs to participate. 
        Visit the EPD <a href="https://epd.georgia.gov/submit-emissions-inventory" rel="noopener" target="_blank">Emissions Inventory Information</a> page 
        and the EPA <a href="https://www.epa.gov/air-emissions-inventories/air-emissions-reporting-requirements-aerr"
            rel="noopener" target="_blank">Air Emissions Reporting Requirements</a> page for more information.           
    </p>
    <h3>
        <asp:Label ID="lblEIYear2" runat="server" Text=""></asp:Label>
        Emissions Inventory Year:
    </h3>
    <ul>
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

    <h2>Annual Permit Fees</h2>
    <p>
        Annual permit fees are required for Synthetic Minor and Part 70 sources and certain NSPS sources. 
        Procedures for calculating and paying fees are on the Annual/Emissions Fees page for your facility.
        If you need to make an amendment to any past fee submittal, a Fee Amendment form (along with Fee 
        Calculation worksheets) is available for download as a Microsoft Excel file on the same page.
    </p>
    <p>
        The emission fee process begins July 1 each year, and the deadline for fee submittal is on or about 
        September 1. Visit the <a href="https://epd.georgia.gov/air-permit-fees" rel="noopener" target="_blank">Air Permit Fees</a>
        page for more information.       
    </p>
    <h3>
        <asp:Label ID="lblFeeYear1" runat="server" Text=""></asp:Label>
        Fee Year:
    </h3>
    <p>
        The
        <asp:Label ID="lblFeeYear2" runat="server" Text=""></asp:Label>
        Annual Permit Fee process begins 
        July&nbsp;1,&nbsp;<asp:Label ID="lblFeeYear3" runat="server" Text=""></asp:Label>. 
        The deadline for fee submittal is 
        <strong>September&nbsp;1,&nbsp;<asp:Label ID="lblFeeYear4" runat="server" Text=""></asp:Label></strong>. 
    </p>
</asp:Content>
