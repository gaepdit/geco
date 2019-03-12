<%@ Page Title="Facility Inventory Help" Language="VB" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.EIS_help_fi" Codebehind="help_fi.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Facility Inventory Guide
    </div>
    <%--Facility and Contact Information Guide--%>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeFacilityContactInfo" runat="server" TargetControlID="pnlFacilitContactInfo"
            ExpandControlID="pnlhdrFacilityContactInfo" CollapseControlID="pnlhdrFacilityContactInfo"
            Collapsed="true" TextLabelID="label1" ExpandedText="(...Hide Details)" CollapsedText="(Show Details...)"
            ImageControlID="Image1" CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrFacilityContactInfo" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; Facility &amp; Contact Information &nbsp; &nbsp;
            <asp:Label ID="Label1" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlFacilitContactInfo" runat="server" CssClass="collapsePanel">
            <div class="fieldguideseparator">
                <asp:Label ID="lblFacilityInfo" class="styledseparator" runat="server"
                    Text="Facility Name and Physical Address" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                The Facility Name and Facility Address information should be the name and address
                under which the facility is currently permitted. To verify this information, you
                can proceed to the Facility Summary tab on the
                <asp:HyperLink ID="HyperLink1" runat="server"
                    NavigateUrl="~/Facility/">GECO Facility Home</asp:HyperLink>
                page and verify the facility information. If the GECO Facility Summary information
                is incorrect, then you may need to complete the
                <asp:HyperLink ID="HyperLink2" runat="server"
                    NavigateUrl="https://epd.georgia.gov/air/documents/ssppsip-app-change-ownership-form"
                    Target="_blank">Name/Ownership Change document</asp:HyperLink>
                and follow the instructions provided in the document.
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label3" class="styledseparator" runat="server"
                    Text="Facility Mailing Address" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                The Facility Mailing Address should be the address where facility information should
                be delivered by mail.&nbsp; this may be different from the Facility Physical Address
                and even different from the Emissions Inventory Contact.&nbsp; All Emissions Inventory
                correspondence will be sent to the Emissions Inventory Contact.
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label4" class="styledseparator" runat="server"
                    Text="Facility Description" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                The Facility Description should be a brief depiction of the facility operations,
                e.g. Power Plant; Car Manufacturing.<br />
                <br />
                <b>Description: </b>Brief comment on the operationg of your facility. The field
                is limited to 100 characters.<br />
                <br />
                <b>Operating Status:</b> The facility status (for purposes of the EI only) is no
                longer editable on this form. The facility status may be changed in the Emissions
                Inventory System only when beginning the EI process. It can be changed on the Facility
                Operational Status form that is available only during the applicability determination
                part ofthe EI process each year. That form is not accessible after that point. To
                access the form after this point it is necessary to reset the EI and start over.<br />
                <br />
                If a facility operated during the calendar year for which data is being collected
                (EI Year), but is shutdown at the time the data is being provided, the facility
                will still participate in the EI process for the EI year if their emissions were
                over the threshold(s) for that EI Year.<br />
                &nbsp;
                <div style="margin-left: 15px">
                    <i>Operating </i>– the facility operated all or part of the calendar year.<br />
                    <br />
                    <i>Temporarily Shutdown </i>– the facility did not operate at all during the calendar
                    year, but may restart at sometime in the future.
                    <br />
                    <br />
                    <i>Permanently Shutdown </i>– the facility has been permanently shut down and will
                    no longer operate.&nbsp; Please note that if the facility operated at any time during
                    the relevant emissions inventory reporting (calendar) year, its status should not
                    be changed to “Permanently shutdown” until the following year.
                    <br />
                </div>
                <br />
                <b>NAICS Code: </b>&nbsp;The North American Industry Classification System codes
                are used by the EIS to provide an industrial classification. An NAICS tool is provided
                to search the NAICS list or you can use the NAICS website, 
                <a href="https://www.naics.com/search/">https://www.naics.com/search/</a>.&nbsp; 
                See <a href="https://www.census.gov/eos/www/naics/">https://www.census.gov/eos/www/naics/</a>
                for valid NAICS values and conversion tables from NAICS to SIC and from SIC to NAICS.<br />
                <br />
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label5" class="styledseparator" runat="server"
                    Text="Facility Geographic Coordinate Information" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                The Facility Geographic Coordinate Information is reported for the location of the
                center of the production area.&nbsp; Additional to the latitude and longitude, three
                Method Accuracy Description (MAD) codes are requested when submitting latitudes
                and longitudes.<br />
                <br />
                Because this information is marked as <i>protected</i> by EPA, changes must be made
                by Air Protection Branch personnel. If you need to change this data, please contact
                us using the Contact Us link in the menu above.<br />
                <br />
                <b>Horizontal Collection Method</b>: Describes the method used to determine the
                latitude and longitude for a point on earth. Choose the method from the dropdown
                list on the form. If a website is used, select <b>The geographic coordinate determination
                    method based on address matching-other.</b>
                <br />
                <br />
                <b>Accuracy Measure</b>: The measure of accuracy of the latitude and longitude coordinates
                in meters. If a website is used, enter <b>25 meters</b>.
                <br />
                <br />
                <b>Horizontal Datum Reference Code</b>: The code that represents the reference datum
                used to determine the latitude and longitude coordinates. If a website is used,
                enter <b>North American Datum of 1983</b>.
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label6" class="styledseparator" runat="server"
                    Text="Emissions Inventory Contact Information" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                Indicate on this form the person responsible for conducting the Emission Inventory
                at the facility. This could be an employee at your facility or the name of a consultant
                acting on behalf of the facility.&nbsp; If there are any problems with the submitted
                data, this is the individual that will be contacted.&nbsp; Additionally, any mailouts
                will be sent this individual.<br />
                <br />
                The company name, mailing address, and phone numbers should be for the contact listed
                on this form, i.e. if the contact is the facility consultant, then please indicate
                the consulting firm&#39;s name, mailing address and phone numbers and not that of
                the facility.
            </div>
        </asp:Panel>
    </div>
    <%--Release Point Guide--%>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeReleasePoints" runat="server" TargetControlID="pnlReleasePoints"
            ExpandControlID="pnlhdrReleasePoints" CollapseControlID="pnlhdrReleasePoints"
            Collapsed="true" TextLabelID="Label2" ExpandedText="(...Hide Details)" CollapsedText="(Show Details...)"
            ImageControlID="Image2" CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrReleasePoints" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; Release Points &nbsp; &nbsp;
            <asp:Label ID="Label2" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlReleasePoints" runat="server" CssClass="collapsePanel">
            <div class="fieldguidewrapper">
                The Emission Inventory System (EIS) separates Release Points into two categories,
                (1) Stacks and (2) Fugitives.&nbsp; Each category has its own set of required information
                and as such there are two distinct editing areas in the EIS application.&nbsp; Each
                release point category has a set of release point details and georgraphical information
                tha tis required to be submitted.<br />
                <br />
                Additionally, release points are used in the <b>Process Release Point Apportionment
                </b>to define the quanity of pollutants coming from the release point for that process.
                Note that if only one release point is left in the release point apportionment for
                a process, that apportionment cannot be deleted unless another release point is
                added or the process itself is deleted.<br />
                <br />
                <b>Stack/Fugitive ID</b>: Unique identifier that the facility provides to identify
                the release point in the system.&nbsp; This is limited to 6 characters.<br />
                <br />
                <b>Stack/Fugitive Descriptions</b>:Describes the point or area at which emissions
                are released into the environment, via a stack of fugitve release.&nbsp; A release
                point can define the area over which fugitve emissions occur<br />
                <br />
                <b>Stack/Fugitive Operating Status</b>: The operational status of the stack/emission
                release point. The possible selections are:
                <br />
                <div style="margin-left: 15px">
                    <i>Operating </i>– this stack/fugitive release point operated all or part of the
                    calendar year.
                    <br />
                    <br />
                    <i>Temporarily Shutdown </i>– this stack/fugitive emission release point did not
                    operate at all during the calendar year, but may restart at sometime in the future.
                    <br />
                    <br />
                    <i>Permanently Shutdown </i>– select this status if a stack/emission release point
                    has been removed, removed from service, or is permanently shutdown.
                    <br />
                    <br />
                    <b>NOTE</b>: If a release point is included in a release point apportionment its
                    status cannot be changed to one of the shutdown modes until it is removed from all
                    release point apportionments of which it is a part. If a release point is in a shutdown
                    mode it may not be attached to a process or release point apportionment.<br />
                </div>
                <br />
                <b>Stack/Fugitive Comments</b>: Any additional details regarding the release point
                that would be beneficial.<br />
                <br />
                <b>Stack/Fugitive Add</b>: A Release Point can be added from the summary or details
                pages.&nbsp; In order to add a release point, a release point ID and description
                must be provided.<br />
                <br />
                <b>Stack/Fugitive Duplicate</b>: Release points can be duplicated from the Details
                pages.&nbsp; In order to duplicate a release point, the Release Point ID and Description
                must be included. Note that release point apportionment is not included in the duplication
                process. The only action in the duplication process is the creation of a new stack
                or fugitive release point with the parameters of the copied stack or fugitive release
                point.<br />
                <br />
                <b>Stack/Fugitive Delete</b>: A Release Point can be deleted from the Edit pages
                only.&nbsp; If a release point has been submitted to EPA or it is currently in a
                Release Point Apportionment, then it cannot be deleted.
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label7" class="styledseparator" runat="server"
                    Text="Stack Release Points" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                <b>Stack Type</b>: The physical orientations of the stack release point. Types:
                Downward-Facing Vent, Goose Neck, Horizontal, Vertical, Vertical with Rain Cap
                <br />
                <br />
                <b>Stack Height</b>: The height above grade of the stack in feet. An error will
                be experienced if stack height is less than the stack diameter.<br />
                <br />
                <b>Stack Diameter</b>: Diameter of the stack in feet.&nbsp; For non-round stacks,
                the equivalent diameter equals the square root of the cross-sectional area in square
                feet multiplied by 1.128. For example, we need to determine the equivalent diameter
                for a rectangular stack exit that measures 3 feet x 2 feet. We find the area to
                be 6 square feet. The equivalent diameter is therefore<br />
                <div style="text-align: center;">
                    <asp:Image ID="imgEquivArea" runat="server" ImageUrl="~/assets/images/EIS/equivarea.jpg"
                        AlternateText="diameter formula" />
                </div>
                <br />
                For example, we need to determine the equivalent diameter for a rectangular stack
                exit that measures 3 feet x 2 feet. The cross sectional area is 6 square feet. The
                equivalent diameter is<br />
                <div style="text-align: center;">
                    <asp:Image ID="imgEquivAreaExample" runat="server" ImageUrl="~/assets/images/EIS/equivareaexample.jpg"
                        AlternateText="diameter formula" />
                </div>
                <br />
                NOTE: An error will be experienced if stack diameter is greater than the stack height.<br />
                <br />
                <b>Exit Gas Velocity</b>: The exit velocity of the exhaust stream in feet per second
                (fps)<br />
                <br />
                <b>Exit Gas Flow Rate</b>: Numeric value of the stack exit gas flow rate is in in
                actual cubc feet per second (acfs).&nbsp; Two checks are performed on the number
                to make sure it is within the following ranges:<br />
                <b>1. The reported value must be within 5% of the calculated value using the diameter
                    and exit gas velocity entered.
                    <br />
                    2. The Exit Gas Flow Rate also must be between 0.1 and 200,000 ACFS.<br />
                    <br />
                </b>&quot;<i>Calculated exit gas flow rate</i>&quot; means the value calculated
                using the following formula (D = diameter; v = exit gas velocity):<br />
                <div style="text-align: center;">
                    <asp:Image ID="Image6" runat="server" ImageUrl="~/assets/images/EIS/egfr_calc.jpg" />
                </div>
                See below for the list of ranges for stack parameters.<br />
                <br />
                <b>Fence Line Distance</b>: The measure of the horizontal distance in feet to the
                nearest fence line of a property within which the stack is located. (OPTIONAL)<br />
                <br />
                Stack Parameter Ranges<br />
                <br />
                <table width="500" border="1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="text-align: center; width: 149px;">
                            <b>Parameter </b>
                        </td>
                        <td style="text-align: center">
                            <b>Ranges and other Restrictions </b>
                        </td>
                        <td style="text-align: center">
                            <b>Units </b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 149px" valign="top">Stack Height</td>
                        <td valign="top">1.0 to 1300</td>
                        <td style="text-align: center" valign="top">feet</td>
                    </tr>
                    <tr>
                        <td style="width: 149px" valign="top">Stack Diameter</td>
                        <td valign="top">1 - 0.1 to 100<br />
                            2 - must be less than height</td>
                        <td style="text-align: center" valign="top">feet</td>
                    </tr>
                    <tr>
                        <td style="width: 149px" valign="top">Exit Gas Velocity</td>
                        <td valign="top">0.1 to 600</td>
                        <td style="text-align: center" valign="top">fps</td>
                    </tr>
                    <tr>
                        <td style="width: 149px" valign="top">Exit Gas Flow Rate</td>
                        <td valign="top">1 - 0.1 to 200,000<br />
                            2 - must also be ±5% of calculation using diameter and
                            exit gas velocity; if calculated value is small enough both upper and lower
                            limits can be the same value due to requirement for rounding to one decimal
                            place. i.e. 0.168 acfs and 0.187 acfs are both 0.2 acfs after rounding.</td>
                        <td style="text-align: center" valign="top">acfs</td>
                    </tr>
                    <tr>
                        <td style="width: 149px" valign="top">Exit Gas Temperature</td>
                        <td valign="top">−30 to 3500</td>
                        <td style="text-align: center" valign="top">&deg;F</td>
                    </tr>
                    <tr>
                        <td style="width: 149px" valign="top">Fenceline Distance</td>
                        <td valign="top">Optional, but if entered must be 1 to 99999</td>
                        <td style="text-align: center" valign="top">&nbsp;feet</td>
                    </tr>
                </table>
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label8" class="styledseparator" runat="server"
                    Text="Fugitive Release Point" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                <p>
                    Fugitive Release Point refers to an emission release point not routed
                    through a stack (e.g. stock piles, conveyors). The majority of the
                    information requested for fugitive release points is optional.
                </p>
                <p>
                    The latitude and longitude coordinates for the fugitive release point should be reported as 
                    those of the western-most corner. The fugitive angle is measured clockwise around that point 
                    from true (not magnetic) north. Fugitive width is the measure along the side that would run 
                    in the east-west direction if the angle were 0 degrees, and fugitive length is the measure 
                    along the side that would run north-south if the angle were 0 degrees.
                </p>
                <p>
                    In the example below, the release point coordinates are located at the push pin, 
                    the width is 680 feet, the length is 1897 feet, and the angle is 22 degrees.
                </p>
                <asp:Image ImageUrl="~/assets/images/EIS/fugitive-angle-diagram.png" runat="server" />
                <p>
                    <b>Fence Line Distance</b>: The measure of the horizontal distance in feet to the nearest 
                    fence line of a property within which the fugitive release is located. (OPTIONAL)
                </p>
                <p>
                    <b>Fugitive Height</b>: The height above the terrain of the fugitive emissions
                    in feet. (OPTIONAL)
                </p>
                <p>
                    <b>Fugitive Width</b>: The width of the fugitive release area measured along the side 
                    that would run in the east-west direction if the angle were 0 degrees. (OPTIONAL)
                </p>
                <p>
                    <b>Fugitive Length</b>: The length of the fugitive release area measured along the side 
                    that would run in the north-south direction if the angle were 0 degrees. (OPTIONAL)
                </p>
                <p>
                    <b>Fugitive Angle</b>: The angle for the fugitive release area, measured in the clockwise direction
                    from true (not magnetic) north. (OPTIONAL)
                </p>
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label9" class="styledseparator" runat="server"
                    Text="Release Point Geographic Coordinate Information" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                The Release Point Geographic Coordinate Information is reported for the location
                of the stack or the western-most corner of the fugitive source. Additional to the latitude
                and longitude, three Method Accuracy Description (MAD) codes are requested when
                submitting latitudes and longitudes.<br />
                <br />
                To assist the facilities with submitting this information, a mapping tool has been
                provided that will allow the selection of any latitude and longitude for the facility.&nbsp;
                If the mapping tool is used, the latitude and longitude will be transferred to the
                form as well as all the three MAD code data.<br />
                <br />
                <b>Horizontal Collection Method</b>: Describes the method used to determine the
                latitude and longitude for a point on earth. Choose the method from the dropdown
                list on the form. If a website is used, select <b>The geographic coordinate determination
                    method based on address matching-other.</b>
                <br />
                <br />
                <b>Accuracy Measure</b>: The measure of accuracy of the latitude and longitude coordinates
                in meters. If a website is used, enter <b>25 meters</b>.
                <br />
                <br />
                <b>Horizontal Datum Reference Code</b>: The code that represents the reference datum
                used to determine the latitude and longitude coordinates. If a website is used,
                enter <b>North American Datum of 1983</b>.
            </div>
        </asp:Panel>
    </div>
    <%--Emission Unit Guide--%>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeEmissionUnits" runat="server" TargetControlID="pnlEmissionUnits"
            ExpandControlID="pnlhdrEmissionUnits" CollapseControlID="pnlhdrEmissionUnits"
            Collapsed="true" TextLabelID="Label10" ExpandedText="(...Hide Details)" CollapsedText="(Show Details...)"
            ImageControlID="Image3" CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrEmissionUnits" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; Emission Units &nbsp; &nbsp;
            <asp:Label ID="Label10" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlEmissionUnits" runat="server" CssClass="collapsePanel">
            <div class="fieldguidewrapper">
                The Emission Unit forms allows the user to create, modify, and delete any emission
                units and emission unit control approaches from the facility inventory. An emission
                unit is a required piece of information for any facility inventory process.
                <br />
            </div>
            <div class="fieldguidewrapper">
                <b>Emisison Unit ID</b>: Unique code for the point of generation of emissions, typically
                a physical piece of equipment. This can be a maximum of 6 characters made up of
                lettes and numbers (A-Z, a-z, 0-9). If an emission unit is loaded into the form,
                and a new Emission Unit ID is entered and saved, a new emission unit will be created
                in the emission inventory for the facility. If the Emission Unit ID is changed to
                an existing Emission Unit ID, then the data in the form will save over the previous
                data.<br />
                <br />
                <b>Emission Unit Description</b>: Brief description of the emission unit. The description
                is limited to 100 characters.<br />
                <br />
                <b>Unit Type</b>: A description that uniquely identifies the emission unit.&nbsp;
                This is a new field and all emissions units migrated from past inventories were
                assigned a value of &#39;Unclassified&#39;.&nbsp; A warning will be given during
                submittal to update this information if Unclassified is selected.<br />
                <br />
                <b>Operating Status</b>: The operational status of the emission unit. The possible
                selections are:<br />
                <div style="margin-left: 15px">
                    <i>Operating </i>– this emission unit operated all or part of the calendar year
                    (Active at anytime during the year).<br />
                    <br />
                    <i>Temporarily Shutdown </i>– this emission unit did not operate at all during the
                    calendar year, but may restart at sometime in the future (no emissions during the
                    calendar year; Idle for the entire year).
                    <br />
                    <br />
                    <i>Permanently Shutdown</i>–select this status if an emission unit has been removed,
                    removed from service, or is permanently shutdown (no emissions during the calendar
                    year).<br />
                    <br />
                    <b>Note:</b> when an emission unit&#39;s status is changed to a shutdown state its
                    processes will be removed from the current reporting period and its processes will
                    not be available to add to the reporting period. Changes to its processes will not
                    be allowed and processes cannot be added to the emission unit.<br />
                </div>
                <br />
                <b>Date Unit Placed in Operation</b>: The date on which unit activity became operational.&nbsp;
                This was intentionally left balnk on all emission units migrated from a past emissions
                inventory.
                <br />
                <br />
                <b>Emission Unit Comments</b>: Comments relating to this emission unit, such as
                an explanation for a change in emissions or a like-kind replacement.
                <br />
                <br />
                <b>Emission Unit Control Approach</b>: An emission unit control approach applies
                to the emission unit and all processes associated with that emission unit. When
                an Emission Unit Control Approach is added no Process Control Approach can be added
                for any of the processes. &nbsp;In other words, if one adds an Emission Unit Control
                Approach for an emission unit then one cannot add a Process Control Approach for
                any process associated with that emission unit. Also, if one adds a Process Control
                Approach for a process then one cannot add an Emission Unit Control Approach for
                the emission unit associated with that process. For more information on <i>Control Approach</i>
                see the section below.<br />
                <br />
                <b>Processes Table</b>: List of processes that the emission unit is included.&nbsp;
                Selecting the process description will take you to the Process Details.&nbsp;
                <br />
                <br />
                <b>Emission Unit Add</b>: Emission Units can be added from the Summary or Details
                pages.&nbsp; In order to add an emission unit, the Emission Unit ID and Description
                must be included.<br />
                <br />
                <b>Emission Unit Duplicate</b>: Emission Units can be duplicated from the Details
                page.&nbsp; In order to duplicate an emission unit, the Emission Unit ID and Description
                must be included. Note that associated processes and control approaches are not
                included in the duplication; only the emission unit information is duplicated and
                the new emission unit ID and description are included.<br />
                <br />
                <b>Emission Unit Delete</b>: An emisison unit can only be deleted from the Edit
                page.&nbsp; If the emission unit has been submitted to EPA and has associated processes,
                then it cannot be deleted.&nbsp; Deleting an Emission Unit deletes any associated
                Emission Unit Control Approach, Control Measures, and Control Pollutants.
            </div>
        </asp:Panel>
    </div>
    <%--Process Guide--%>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeProcess" runat="server" TargetControlID="pnlProcess"
            ExpandControlID="pnlhdrProcess" CollapseControlID="pnlhdrProcess" Collapsed="true"
            TextLabelID="Label11" ExpandedText="(...Hide Details)" CollapsedText="(Show Details...)"
            ImageControlID="Image4" CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrProcess" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image4" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; Processes &nbsp; &nbsp;
            <asp:Label ID="Label11" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlProcess" runat="server" CssClass="collapsePanel">
            <div class="fieldguidewrapper">
                An Emissions Process is used to define how the emissions are generated.&nbsp; At
                the most basic, an emissions processes is made up of one emission unit and one release
                point and a defined operations process.&nbsp; This operations process is defined
                by a Source Classification Code (SCC) which tells EPA what type of process it is.&nbsp;
                The classic example is a boiler exhausting through one stack and using only one
                fuel.&nbsp; All the emissions for this unit would be placed in this one process.
                If two fuels are combusted, then a second process needs to be defined.<br />
                <br />
                Finally, during the Emissions Inventory, pollutants are added to the process.&nbsp;
                Only those emissions that are generated by that one process are included.&nbsp;
                So in our example with one fuel, all the emissions for the emissions unit would
                be provided in the single process.&nbsp; If a second fuel is combusted, then those
                emissions from the first fuel would be in the first process, and those emission
                generated from the second fuel would be in the second process.<br />
                <br />
                <b>Process ID</b>: Unique identifier for the process.&nbsp; This is provided during
                the initial Add of the process.<br />
                <br />
                <b>Process Description</b>: Brief description of the process, e.g. Natural Gas Fuel
                in B001. The description is limited to 100 characters.<br />
                <br />
                <b>Source Classification Code(SCC)</b>: The EPA Source Classificaiton Code describes
                an emissions process or activity.&nbsp; A search tool has been provided to assist
                in determining an SCC for a process.<br />
                <br />
                <b>Release Point Apportionment</b>:&nbsp; Release Point Apportionment allows multiple
                release points to be identified for a Process.&nbsp; If a facility has multiple
                release points for a given process, then each release point can be added to the
                Process and a percentage of the emissions assigned to each release point.&nbsp;
                The total apportionment for all release points must total 100%. Note that if only
                one release point is left in the release point apportionment for a process, that
                apportionment cannot be deleted unless another release point is added or the process
                itself is deleted.<br />
                <br />
                <b>Process Control Approach</b>: Defines a control approach for the process only.&nbsp;
                If a Process Control Approach is added for a process then an Emission Unit Control
                Approach cannot be added for the emission unit. Process Control Approaches cannot
                exist for processes associated with an emission unit that has an Emission Unit Control
                Approach.<br />
                <br />
                <b>Process Comment</b>: Comments relating to this process, such as an explanation
                for a change in SCC.<br />
                <br />
                <b>Process Add</b>: Processes can be added from the Process Summary and Process
                Detail pages.&nbsp; In order to add a Process, a Process ID, Process Description,
                and Release Point must be chosen.&nbsp; The Release Point will automatically be
                added to the Release Point Apportionment with 100% of the emissions exiting through
                that Release Point, to edit the Release Point Apportionment, select the Edit button
                on the Process Details page in Release Point Apportionment section.<br />
                <br />
                <b>Process Delete</b>: A Process can be deleted only from the Edit page.&nbsp; If
                a Process has been submitted to EPA or is currently in a Reporting Period, then
                it cannot be deleted.&nbsp; Deleting a Process deletes the process and all process
                control approach, control measure, and control pollutant information.&nbsp;
            </div>
        </asp:Panel>
    </div>
    <%--Control Approach Guide--%>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeControlApproach" runat="server" TargetControlID="pnlControlApproach"
            ExpandControlID="pnlhdrControlApproach" CollapseControlID="pnlhdrControlApproach"
            Collapsed="true" TextLabelID="Label14" ExpandedText="(...Hide Details)" CollapsedText="(Show Details...)"
            ImageControlID="Image5" CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrControlApproach" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image5" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; Control Approach &nbsp; &nbsp;
            <asp:Label ID="Label14" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlControlApproach" runat="server" CssClass="collapsePanel">
            <div class="fieldguidewrapper">
                A control approach in the new EIS defines the control device(s) and the pollutants
                controlled by those devices for either a process or an emission unit.<br />
                <br />
                A control approach defined at the emission unit level applies to all processes associated
                with that emission unit.&nbsp; A control approach defined at the process level only
                applies to that single process.&nbsp; If an emission unit has a control approach,
                then any processes associated with that emission unit cannot have a control approach
                and vice versa.<br />
                <br />
                A control approach is a strategy of control measures (devices) to control a single
                or group of pollutants.&nbsp; The EIS does not ask for how much each measure (decive)
                controls a particular pollutant, just how much does the entire approach control
                the pollutant.
            </div>
            <div class="fieldguideseparator">
                <asp:Label ID="Label15" class="styledseparator" runat="server"
                    Text="Control Approach Parameters" Style="color: #0033CC"></asp:Label>
            </div>
            <div class="fieldguidewrapper">
                <b>Control Approach Description</b>: Brief description of the control approach.<br />
                <br />
                <b>Percent Control Approach Capture Efficiency</b>: The percentage of air emission
                that is directed to the control equipment, or an estimate of that portion of an
                affected emissions stream that is collected and routed to the control measures,
                when the capture or collection system is operating as designed.&nbsp; A total enclosure
                or ducting would represent 100% capture.<br />
                <br />
                <b>Percent Control Approach Capture Effectiveness</b>: The amount of time, expressed
                as a percentage, that the captured air emissions were directed to the operating
                control scenario, uptime. The effectiveness should reflect control equipment downtime
                and maintenance degradation. If the emissions stream was directed to the control
                scenario for 1000 hours, and the control scenario only operated for 900 hours, then
                the effectiveness is 90%.<br />
                <br />
                <b>Control Measures</b>: Selection of different types of control equipment.&nbsp;
                Only one of each type can be selected even if there are more than one.<br />
                <br />
                <b>Control Pollutants</b>: Selection of pollutants controlled by the control approach.&nbsp;
                Even if multiple measures control the same pollutant, the pollutant is only added
                once and the overall reduction efficiency is applied to the pollutant.<br />
                <br />
                <b>Control Approach Reduction Efficiency</b>: The estimated average percent reduction
                achieved for the captured pollutant when all control measures are operating as designed.&nbsp;
                When operating, the control scenario receives 100 tons of pollutant and allows 25
                tons to be emitted to the atmosphere.&nbsp; Therefore the reduction efficiency is
                75% (75 tons not allowed to be emitted).&nbsp; Many control approaches do not have
                a permitted percentage to attain, therefore please use the manufacturer&#39;s specification.&nbsp;
                <br />
            </div>
        </asp:Panel>
    </div>

    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpaQA" runat="server" TargetControlID="pnlQA"
            ExpandControlID="pnlhdrQA" CollapseControlID="pnlhdrQA"
            Collapsed="true" TextLabelID="label2" ExpandedText="(...Hide Details)" CollapsedText="(Show Details...)"
            ImageControlID="Image3" CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrQA" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image7" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; EPA QA Checks &nbsp; &nbsp; 
            <asp:Label ID="Label12" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlQA" runat="server" CssClass="collapsePanel">
            <p>
                EPA has provided a spreadsheet listing the QA checks they perform on EIS data. Most of the same checks are performed on EPD 
                data, either upon entry or as part of the submittal QA process. Download the complete spreadsheet of 
                <a href="/EIS/files/qachecks.csv" download>EPA QA Checks</a>.
            </p>
        </asp:Panel>
    </div>
</asp:Content>
