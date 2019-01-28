<%@ Page Language="VB" MasterPageFile="~/AnnualFees/Fees.master" AutoEventWireup="false" Inherits="GECO.AnnualFees_FeesReport" Title="GECO - Fees Reports" Codebehind="FeesReport.aspx.vb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <CR:CrystalReportViewer ID="myCrystalReportViewer" runat="server" AutoDataBind="true"
        EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
        Height="500px" Width="350px" EnableDrillDown="False"
        HasCrystalLogo="False" HasDrillUpButton="False"
        HasToggleGroupTreeButton="False"
        HasZoomFactorList="False" HasSearchButton="true" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftMenuContent" runat="Server">
    <h3>Report options</h3>

    To use the following options you may have to allow pop-ups.<br />

    - Click on
                        <img src="<%= Page.ResolveUrl("~/assets/images/export.gif") %>" />
    (Export) to export the report the report to one of the many options including PDF,
                    MS Excel, MS Word, etc.<br />
    - Click on &nbsp;<img src="<%= Page.ResolveUrl("~/assets/images/print.gif") %>" />
    (Print) to print the report. The report will open in PDF format before you can print
                    it.<br />
    - Click on
                    <img src="<%= Page.ResolveUrl("~/assets/images/first.gif") %>" />
    <img src="<%= Page.ResolveUrl("~/assets/images/prev.gif") %>" />
    <img src="<%= Page.ResolveUrl("~/assets/images/next.gif") %>" />
    <img src="<%= Page.ResolveUrl("~/assets/images/last.gif") %>" />
    buttons to navigate thorugh various pages of the report itself.<br />
    - You may be able to go to a specific page of the report by typing in the page number
                    and clicking on
                    <img src="<%= Page.ResolveUrl("~/assets/images/gotopage.gif") %>" />
    (Go To)<br />
    - You may be able to search for a particular text in the report by typing in the
                    text and clicking on
                    <img src="<%= Page.ResolveUrl("~/assets/images/search.gif") %>" />
    (Search)<br />
    <br />
    -&gt; The Past Report contains data submitted for all the years starting from the
                    year 2004; if the emission fee data was submitted.<br />
    <br />
    -&gt; The invoice report may show the total balance for the year on the top of the
                    report followed by the invoice details.<br />
    <br />
    -&gt; If you have selected "Four Quarterly Payments", you may have to navigate through
                    various pages to get the invoice for each quarter.<br />
</asp:Content>