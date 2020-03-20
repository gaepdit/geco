<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO._Default" Title="Georgia Environmental Connections Online" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/MainMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Welcome to GECO</h1>

    <p>
        Georgia Environmental Connections Online ("GECO")
        is an online service allowing public access to various Georgia Air Protection Branch applications.
        You must be a registered user to access the online applications.
    </p>

    <p>
        <asp:HyperLink ID="SignIn" runat="server" Text="Sign In" NavigateUrl="~/Login.aspx" CssClass="button button-large" />
        <asp:HyperLink ID="Register" runat="server" Text="Register" NavigateUrl="~/Register.aspx" CssClass="button button-large" />
    </p>

    <%--<p><a href="#">Instructions for creating a GECO account</a></p>--%>

    <h2>Event Registration</h2>
    <p>
        The GECO system is used to register for classes, workshops, and other events hosted by Georgia EPD. You may view the 
        <asp:HyperLink ID="linkEvents" runat="server" NavigateUrl="~/EventRegistration/">upcoming events</asp:HyperLink>, 
        but a GECO account is required to register for any event.
    </p>

    <h2>Emissions Inventory</h2>
    <p>
        The <a href="https://www.epa.gov/air-emissions-inventories" target="_blank">National Emissions Inventory</a>
        (NEI) is a detailed estimate compiled by the US EPA of air emissions that include criteria pollutants and 
        hazardous air pollutants from air emissions sources. Emissions inventory data for all Part 70 major source
        facilities are collected on a three year cycle, while a subset of facilities must report on an annual basis.
    </p>
    <p class="message-highlight">
        Starting this year, Georgia will be using the Combined Air Emissions Reporting (CAER) "Common Emissions Form" 
        (CEF) for the 2019 Emissions Inventory data collection. The Emissions Inventory submittal is due on or about 
        June&nbsp;30 each year.
    </p>

    <h2>Emissions Statement</h2>
    <p>
        Facilities in the Atlanta metro maintenance area whose NO<sub>x</sub> and/or VOC actual emissions are greater 
        than 25 tons per year are required to submit an annual 
        <a href="https://epd.georgia.gov/submit-emissions-statement" target="_blank">Emissions Statement</a>
        (ES).
    </p>
    <p>
        The Emissions Statement is due on or about June&nbsp;15 each year.
    </p>

    <h2>Permit Application Fees</h2>
    <p>
        Permit application fees are required when applying for certain permit actions. Refer to the
        <a href="https://epd.georgia.gov/air-permit-fees" target="_blank">Air Permit Fee Manual</a>
        to determine what type of fee is due when submitting a permit application.
    </p>
    <p>
        Permit application fees are owed in addition to any annual fees described below.
    </p>

    <h2>Annual (Emissions) Fees</h2>
    <p>
        Annual permit fees are required for Synthetic Minor and Part 70 sources, and certain NSPS sources. The 
        <a href="https://epd.georgia.gov/air-permit-fees" target="_blank">Air Permit Fee Manual</a> provides
        instructions and should be used to calculate fee amounts.
    </p>
    <p>
        The emission fee process begins July&nbsp;1 each year, and the deadline for fee submittal is on or about September&nbsp;1.
    </p>

    <h2>Test Notifications</h2>
    <p>
        This tool displays a list of test notifications submitted by your facility to the Air Protection Branch.
    </p>
</asp:Content>
