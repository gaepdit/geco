﻿<%@ Page Language="VB" AutoEventWireup="false" Inherits="GECO.Http404Page" CodeBehind="404.aspx.vb" %>

<!DOCTYPE html>
<html lang="en-us">
<head runat="server">
    <title>File or Page Not Found</title>
    <link rel="apple-touch-icon" sizes="180x180" href="~/assets/epd-favicons/apple-touch-icon.png?v=69kRrvbXdL" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-32x32.png?v=69kRrvbXdL" sizes="32x32" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-16x16.png?v=69kRrvbXdL" sizes="16x16" />
    <link rel="mask-icon" href="~/assets/epd-favicons/safari-pinned-tab.svg?v=69kRrvbXdL" color="#5bbad5" />
    <link rel="shortcut icon" href="~/favicon.ico?v=69kRrvbXdL" />
    <meta name="theme-color" content="#e5f6fa" />
    <link href="~/assets/css/site.css?v=20190419" rel="stylesheet" type="text/css" />
</head>
<body>
    <a class="skipnav" href="#main-content">Skip to main content</a>
    <div id="header" class="header-no-menu">
        <div id="epdlogo">
            <a href="https://epd.georgia.gov/" target="_blank">
                <img src='<%= Page.ResolveUrl("~/assets/images/epd-icon.png") %>' alt="Georgia EPD" /></a>
        </div>
        <div id="apptitle">
            <img src='<%= Page.ResolveUrl("~/assets/images/airbranch_header_bg.jpg") %>' alt="Georgia Air Protection Branch" />
        </div>
    </div>

    <div class="content" id="main-content">
        <h1>File or Page Not Found</h1>

        <p>
            The page you have requested could not be found. If you need assistance
            please contact the
           
            <asp:HyperLink ID="lnkContact" runat="server">Air Protection Branch</asp:HyperLink>.
       
        </p>

        <ul>
            <li><a href="<%= Page.ResolveUrl("~/Home/") %>">Return to the homepage</a></li>
        </ul>
    </div>

    <div class="footer">
            ©<%= Now.Year %> <a href="https://epd.georgia.gov/">Georgia Environmental Protection Division</a>, 
            <a href="https://epd.georgia.gov/air-protection-branch">Air Protection Branch</a> 
            • All rights reserved
    </div>
</body>
</html>
