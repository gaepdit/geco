<%@ Page Language="VB" AutoEventWireup="false" Inherits="GECO.Http404Page" Codebehind="404.aspx.vb" %>
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
    <link href="~/assets/css/site.css?v=20180920" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="header" style="height: auto;">
        <a href="https://epd.georgia.gov/" target="_blank">
            <img src='<%= Page.ResolveUrl("~/assets/images/epd_logo.jpg") %>' alt="GA EPD" style="float: left; margin: 10px 20px;" />
        </a>
        <img src='<%= Page.ResolveUrl("~/assets/images/airbranch_header_bg.jpg") %>' alt="Air Protection Branch" style="margin: 15px 20px;" />
    </div>

    <div class="content" style="margin: 0; padding: 100px 25px; width: auto; max-width: 600px; float: none;">
        <h1>File or Page Not Found</h1>

        <p>
            The page you have requested could not be found. If you need assistance
            please contact the
            <asp:HyperLink id="lnkContact" runat="server">Air Protection Branch</asp:HyperLink>.
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
