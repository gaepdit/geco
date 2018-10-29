<%@ Page Language="VB" AutoEventWireup="false" Inherits="GECO.EIS_eis_changelog" Codebehind="eis_changelog.aspx.vb" %>
<!DOCTYPE html>
<html lang="en-us">

<head>
    <title>GECO Emission Inventory System Changelog</title>
    <link rel="apple-touch-icon" sizes="180x180" href="~/assets/epd-favicons/apple-touch-icon.png?v=69kRrvbXdL" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-32x32.png?v=69kRrvbXdL" sizes="32x32" />
    <link rel="icon" type="image/png" href="~/assets/epd-favicons/favicon-16x16.png?v=69kRrvbXdL" sizes="16x16" />
    <link rel="mask-icon" href="~/assets/epd-favicons/safari-pinned-tab.svg?v=69kRrvbXdL" color="#5bbad5" />
    <link rel="shortcut icon" href="~/favicon.ico?v=69kRrvbXdL" />
    <meta name="theme-color" content="#e5f6fa" />
</head>

<body>
    <div style="font-family: Verdana;">
        <h1>Georgia EPD Emissions Inventory System Changelog</h1>
        <h2>Updates made to Georgia EPD&#39;s EIS online application.</h2>

        <h3>Release Date: June 12, 2017</h3>
        <ul>
            <li>Updated control approach requirements: Capture Efficiency, Effectiveness, and Reduction efficiency are all required fields now.</li>
            <li>Calculated Overall Pollutant Reduction (%) is displayed in the pollutant table for each Control Approach.</li>
            <li>Summer-day emissions reporting requirements have been removed.</li>
        </ul>

        <h3>Release Date: January 29, 2016</h3>
        <ul>
            <li>
                <b>Facility Status question:</b> The question asking if the facility is currently operating has been removed. The user is asked only if the facility operated during any part of the EI year. </li>
            <li>
                <b>Fugitive Release Point Edit page:</b> The fugitive angle range of values has been revised in accordance with EPA&#39;s requirements. The new range is 0 through 89 degrees, inclusive.</li>
        </ul>

        <h3>Release Date: July 9, 2015</h3>
        <ul>
            <li>
                <b>EIS Emissions Edit page:</b> Max limit for Summer Day emissions no longer an error. A warning will be shown if the value entered is greater than 1/300 of the annual emissions.</li>
            <li>
                <b>EIS Pollutant Bulk page:</b> Error message changed to exclude summer day emission max limit.</li>
            <li>
                <b>EIS EI Help:</b> Updated to reflect summer day emissions max limit removal.</li>
        </ul>

        <h3>Release Date: March 31, 2015</h3>
        <ul>
            <li>
                <b>EIS Submit Checks:</b> PM error checks updated. Additional PM checks added. See EI Help emissions section for details.</li>
        </ul>

        <h3>Release Date: February 2, 2015</h3>
        <ul>
            <li>
                <b>EIS Submit Checks:</b> Error check for Process Throughput now includes check for missing unit of measure.
            </li>
        </ul>

        <h3>Release Date: July 17, 2014</h3>
        <ul>
            <li>
                <b>EIS Submit Checks:</b> Facilities with summer day emissions were being prevented from submitting thier EI data due to errors being indicated for process details when no errors existed. This issue has been resolved.</li>
            <li>
                <b>Facility Edit:</b> Simplified telephone number entry due to odd behavior in web browsers.
            </li>
        </ul>

        <h3>Release Date: July 11, 2014</h3>
        <ul>
            <li>
                <b>EIS Pollutant Bulk Entry:</b> Error handling and messages improved. Errors highlighted in light red in table.</li>
            <li>
                <b>Process Bulk Entry:</b> Number of blank values are indicated after updating completed.</li>
        </ul>

        <h3>Release Date: January 17, 2014</h3>
        <ul>
            <li>
                <b>EIS Submit Check:</b> Added error check for processes in the reporting period for whcih no emissions were reported. (Error ID E28)</li>
        </ul>

        <h3>Release Date: July 17, 2013</h3>
        <ul>
            <li>
                <b>Stack Edit:</b> Stack parameter checks improved.</li>
            <li>
                <b>Facility Inventory Help:</b> included more extensive information for stack parameter checks.
            </li>
        </ul>

        <h3>Release Date: June 26, 2013</h3>
        <ul>
            <li>
                <b>Stack Edit:</b> Exit Gas Flow Rate acceptable range is now &#177; 5% of value calculated using stack diameter and exit gas velocity; maximum value allowed is 200,000 ACFS.</li>
            <li>
                <b>EI Submit Check:</b> Added stack parameter checks - Error E03 now includes Exit Gas Flow Rate range checks and a check that Stack Diameter be less than Stack Height.</li>
            <li>
                <b>FI Help:</b> FI help updated to include information for above changes.</li>
        </ul>

        <h3>Release Date: June 17, 2013</h3>
        <ul>
            <li>
                <b>Bug Fixes:</b> Bug fixes on Facility Details EI entry page (NAICS Search), Error E07 in Submit Check, Facility Threshold page (Pb appearing in list when shouldn&#39;t) and other minor fixes.</li>
        </ul>

        <h3>Release Date: May 29, 2013</h3>
        <ul>
            <li>
                <b>Submit Emissions Inventory Data:</b> Bug in the Summer Day emissions error check fixed.
            </li>
            <li>
                <b>Edit Process Details:</b> SCC search button link fixed.</li>
        </ul>

        <h3>Release Date: April 2, 2013</h3>
        <ul>
            <li>
                <b>All pages:</b> Link to EIS Change Log added to left menu.</li>
        </ul>

        <h3>Release Date: March 11, 2013</h3>
        <ul>
            <li>
                <b>Facility Details:</b> This page now has a Google Maps excerpt of the facility&#39;s location. Remember that this should be the center of the production area, not the entrance to the facility.</li>
            <li>
                <b>Release Point Summary:</b> This page now has the
                <i>Deleted Release Points</i>
                list at the bottom of the page.</li>
            <li>
                <b>Fugitive Release Point Details:</b> Google Maps excerpt added. Shows the location of the fugitive release point.</li>
            <li>
                <b>Stack Release Point Details:</b> Google Maps excerpt added. Shows the location of the stack at the facility.</li>
            <li>
                <b>Emission Unit Summary:</b> This page now has the
                <i>Deleted Emission Units</i>
                list at the bottom of the page.</li>
            <li>
                <b>Edit Emission Unit Details:</b> Functionality of the
                <i>Operating Status</i>
                has been modified. When one changes the operating status of an emission unit from &quot;operating&quot; to any of the two shutdown statuses, a notification is displayed alerting the user that, on saving, any processes and emissions that are in the reporting period, and which are associated with the emission unit, will be removed from the current EI reporting period. Adding proceses and changes to existing processes for the emission unit will not be allowed.</li>
            <li>
                <b>Process Reporting Period Summary:</b> the list of Processes NOT in the Reporting Period no longer contains processes associated with emission units that are in a shutdown state.</li>
            <li>
                <b>Edit Process Operating Details:</b> If the process is fuel burning one may select Negligible for sulfur and/or ash content if those values are less than 0.01%.</li>
            <li>
                <b>Edit Emissions Details:</b> If the facility is located in the ozone non-attainmaent area and either VOC or NOx is selected additional two questions may appear: 1 -
                <i>Process operated May 1 - Sep 30?</i> If the process operated any time within that time frame, a second question is displayed:
                <i>2 - Summer Day VOC Emissions (tons)</i>. This asks for the average daily emissions in tons during the period May 1 through Sep 30. Entering Summer Day emissions will cause VOC and/or NOx to appear twice in the pollutants table on the page. The quantities will be differentiated by the terms
                <i>Annual</i> and
                <i>Summer Day</i> in the
                <i>Emissions Period</i> column.</li>
            <li>
                <b>Pollutant Bulk Entry:</b> If the facility is located in the ozone non-attainment area and has emissions of VOC and/or NOx during the period May 1 through Sep 30, Summer Day emissions will appear in the table and need to be updated. NOTE: the units for Summer Day emissions are tons per day (TPD).</li>
            <li>
                <b>Submit Emissions Inventory Data</b>: Lists of all possible EIS errors and warnings added for reference. EIS errors and warnings sorted by their reference numbers. List of encountered warnings collapsed by default. Expand by clicking the header.</li>
            <li>
                <b>Help Pages</b>: Help for recent changes added to respective areas.</li>
        </ul>
    </div>
</body>

</html>