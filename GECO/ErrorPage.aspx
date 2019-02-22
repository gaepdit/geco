<%@ Page Language="VB" AutoEventWireup="false" Inherits="GECO.ErrorPage" Codebehind="ErrorPage.aspx.vb" %>
<!DOCTYPE html>
<html lang="en-us">
<head runat="server">
    <title>GECO Error Page</title>
    <link rel="apple-touch-icon" sizes="180x180" href="~/assets/epd-favicons/apple-touch-icon.png?v=69kRrvbXdL" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-32x32.png?v=69kRrvbXdL" sizes="32x32" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-16x16.png?v=69kRrvbXdL" sizes="16x16" />
    <link rel="mask-icon" href="~/assets/epd-favicons/safari-pinned-tab.svg?v=69kRrvbXdL" color="#5bbad5" />
    <link rel="shortcut icon" href="~/favicon.ico?v=69kRrvbXdL" />
    <meta name="theme-color" content="#e5f6fa" />
    <link href="~/assets/css/site.css?v=20180920" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="header" style="height: auto;">
        <a href="https://epd.georgia.gov/" target="_blank">
            <img src='<%= Page.ResolveUrl("~/assets/images/epd_logo.jpg") %>' alt="GA EPD" style="float: left; margin: 10px 20px;" />
        </a>
        <img src='<%= Page.ResolveUrl("~/assets/images/airbranch_header_bg.jpg") %>' alt="Air Protection Branch" style="margin: 15px 20px;" />
    </div>

    <div class="content" style="margin: 0; padding: 100px 25px; width: auto; max-width: 600px;float:none;">
        <h1>An Error Has Occurred</h1>
        <p>
            An unexpected error has occurred on our website. If the problem persists, please contact the Air Protection Branch 
            at 404-363-7000 or email <a href="mailto:epd_it@dnr.ga.gov">epd_it@dnr.ga.gov</a>.
        </p>
        <ul>
            <li><a href="<%= Page.ResolveUrl("~/Home/") %>">Return to the homepage</a></li>
        </ul>
    </div>

    <div class="footer">
        ©2018 • Georgia Environmental Protection Division, Air Protection Branch • All rights reserved
    </div>
</body>
</html>
