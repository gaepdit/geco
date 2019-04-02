<%@ Page Title="Emissions Inventory Help" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_help_ei" Codebehind="help_ei.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="pageheader">
        Emission Inventory Guide
    </div>
    <%--Facility and Contact Information Guide--%>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeReportingPeriod" runat="server"
            TargetControlID="pnlReportingPeriod"
            ExpandControlID="pnlhdrReportingPeriod"
            CollapseControlID="pnlhdrReportingPeriod"
            Collapsed="true"
            TextLabelID="Label1"
            ExpandedText="(...Hide Details)"
            CollapsedText="(Show Details...)"
            ImageControlID="Image1"
            CollapsedImage="~/assets/images/expand.jpg"
            ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrReportingPeriod" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; Reporting Period &nbsp; &nbsp;
            <asp:Label ID="Label1" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlReportingPeriod" runat="server" CssClass="collapsePanel">
            <div class="fieldguidewrapper">
                The EIS consists of the Facility Inventory and the Emission Inventory.&nbsp; The
                Emission Inventory is the submission of process emission information for a given
                reporting period.&nbsp; For the EIS, the reporting period is always an annual
                submission.&nbsp; All emission information is associated with a Process from the
                Facility Inventory.&nbsp; A Process must be added in the Facility Inventory and added
                to the annual reporting period before emissions from that process can be
                included.<br />
                <br />
                The Georgia EIS allows for the pre-population of the annual reporting period
                from a past submission.&nbsp; However, from the past different pre-population tool,
                the process throughput and the emission values are not pre-populated.&nbsp; All other
                reporting period information is pre-populated.<br />
                <br />
                There are two bulk entry tools for the reporting period information.&nbsp; The
                Process Bulk Entry allows for the entry of the annual process throughput for
                multiple processes.&nbsp; The Pollutant Bulk Entry allows for the entry of annual
                pollutant values for multiple processes.&nbsp; On the Bulk Entry pages, a previous
                year value is shown next to the current year value in the entry table.
            </div>
            <div class="fieldguideseparator">
                Emission Reporting
            </div>
            <div class="fieldguidewrapper">
                The Emission Reporting page lists all the Processes in the Facility Inventory in
                two distinct tables.<br />
                <br />
                <b>Processes NOT in the Annual Reporting Period:</b>&nbsp; Lists those Processes that
                are currently <b>not</b> in the current Reporting Period.&nbsp; Clicking the <b>Add</b>
                button in this table for a Process places that Process in the Reporting Period
                and you are taken to the <b>Edit Process Operating Details</b> page to begin
                adding details.&nbsp; The <b>Prepopulate</b> button will place the Process and its
                underlying emissions from the last year that process was included in the EI into
                the current EI Reporting Period. After clicking <b>Prepopulate</b> the Process
                will appear in the table &quot;<b>Processes in the&nbsp; Annual Reporting Period</b>.&quot; One
                may click on the process in that table to make any changes. The <b>Prepopulate</b>
                button is not active for any processes that have never been included in an EI
                reporting period.<br />
                <br />
                <b>Processes in the Annual Reporting Period:</b>&nbsp; Lists those Processes that are
                already in the Reporting Period from the pre-population or that have already
                been added. You may remove a Process from the current Reporting Period by
                choosing the Processs, going to the <b>Emission Inventory Detail</b>s page then
                clicking the <b>Edit</b> button for the <i>Process Operating Details</i> and,
                lastly, clicking the <b>Delete</b> button on the <b>Edit Process Operating
                Details</b> page.
            </div>
        </asp:Panel>
    </div>
    <%--Facility and Contact Information Guide--%>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeProcessDetails" runat="server"
            TargetControlID="pnlProcessDetails"
            ExpandControlID="pnlhdrProcessDetails"
            CollapseControlID="pnlhdrProcessDetails"
            Collapsed="true" TextLabelID="Label2"
            ExpandedText="(...Hide Details)"
            CollapsedText="(Show Details...)"
            ImageControlID="Image2"
            CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrProcessDetails" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp;Process Operating Details&nbsp; &nbsp;
            <asp:Label ID="Label2" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlProcessDetails" runat="server" CssClass="collapsePanel">
            <div class="fieldguidewrapper">
                The Process Operating Details consist of the following:<br />
                <div style="margin-left: 15px">
                    - Process Operating Details<br />
                    - Daily, Weekly &amp; Annual Information<br />
                    -
                    Seasonal Operation Percentages
                    <br />
                    - Fuel Burning Information (if needed)
                </div>
                <br />
                <b>Delete Process Operating Details:</b> This Delete removes the entire
                Reporting Period Process and Emissions from the reporting period.&nbsp; All Process
                Operating Details; Daily, Weekly, &amp; Annual Information; Seasonal Operation
                Percentages; Fuel Burning Information; and Emission Details.
            </div>
            <div class="fieldguideseparator">
                Process Operating Details
            </div>
            <div class="fieldguidewrapper">
                <b>Calculation Paramter Type:</b> Selects the type of process information the
                annual throughput represents.&nbsp; the three possible choices are <i>Input</i>, <i>Output</i>, or <i>Existing</i>.&nbsp; All informaiton that was migrated from the old
                emission inventory was given a value of Existing.<br />
                <br />
                <b>Actual Annual Throughput/Activity:</b>&nbsp; The quantity of throughput/activity
                for the process in units per year.&nbsp; This value can also be entered on the
                Process Bulk Entry pages.<br />
                <br />
                <b>Annual Throughput/Activity Units:</b>&nbsp; Describes the proces annual throughput
                number based on the options available.<br />
                <br />
                <b>Material Processed or Fuel Used:</b>&nbsp; Select the best product or fuel
                description from the drop-down list that best represents the process meterial
                input/ourput or fuel consumed.<br />
                <br />
            </div>
            <div class="fieldguideseparator">
                Daily, Weekly, Annual Information
            </div>
            <div class="fieldguidewrapper">
                <b>Average Hours per Day:</b> Typical hours per day that the emitting process
                operates during the inventory period.<br />
                <br />
                <b>Average Days Per Week:</b> Typical days per week that the emitting process
                operates during the inventory period.<br />
                <br />
                <b>Average Weeks Per Year:</b> Typical weeks per year that the emitting process
                operates during the inventory period.<br />
                <br />
                <b>A<span id="ctl00_ContentPlaceHolder3_lblActualHoursPerYear" class="styled">ctual
                Hours Per Year</span>:</b> Represents the total actual hours the process
                operated during the calendar year for which the emissions inventory is being
                submitted.<br />
                <br />
            </div>
            <div class="fieldguideseparator">
                Seasonal Operational Percentages
            </div>
            <div class="fieldguidewrapper">
                The fraction of the total operation that occurs in each season. This expresses
                the part of the actual annual activity information based on the four seasons
                (e.g., production in summer is 40%, i.e. 40% of the year’s production). The
                total Seasonal Operation shall equal 1.00, i.e., 100%.<br />
                <br />
                <div style="margin-left: 15px">
                    <b>Winter</b> —The percentage of the total annual activity that a process operates
                    during the Winter months (December, January, February, all from the same
                    reporting year, e.g., Winter&nbsp;2010 = January&nbsp;2010 + February&nbsp;2010 + 
                    December&nbsp;2010).
                    <br />
                    <br />
                    <b>Spring</b> — The percentage of the total annual activity that a process
                operates during the Spring months (March, April, May).<br />
                    <br />
                    <b>Summer</b> —The percentage of the total annual activity that a process
                operates during the Summer months (June, July, August).<br />
                    <br />
                    <b>Fall</b> —The percentage of the total annual activity that a process operates
                during the Fall months (September, October, November).<br />
                </div>
                <br />
            </div>
            <div class="fieldguideseparator">
                Fuel Burning Information
            </div>
            <div class="fieldguidewrapper">
                Report only if the process consumes a solid, liquid, or gaseous fuel.<br />
                <br />
                <b>Fuel Heat Content:</b> The heat content of a fuel in British Thermal Units
                per the standard unit of measure for that type of fuel, that is:
                <br />
                <div style="margin-left: 15px">
                    BTU per Standard Cubic Feet (gaseous fuels)
                <br />
                    BTU per Ton (solid fuels)
                <br />
                    BTU per Gallon of oil (liquid fuels)<br />
                    <br />
                    NOTE: To avoid errors please use the correct value and unit combinations.
                    &quot;E6BTU&quot; means &quot;million Btu.&quot; The
                    <span style="color: #FF0000; font-weight: bold">numerator is always million BTU
                    (E6BTU)</span>. Choose the a combination of heat content value and denominator
                    to make the heat content value correct. For example, the heat content of natural
                    gas ia about 1000 BTU/SCF. The units cannot be entered in that form. On the form
                    one would enter &quot;1000 E6BTU/MILLION STANDARD CUBIC FEET,&quot; which is the same as
                    1000 BTU/SCF.<br />
                    <br />
                    The following table shows examples of fuel heating values and the value and unit
                    to be used on the form.
                </div>
                <br />
                <table align="center"
                    style="border-style: solid; border-width: 1px; width: 600px;">
                    <tr>
                        <td style="text-align: center; background-color: #6699FF; border-width: 1px;">
                            <b>Fuel</b></td>
                        <td style="width: 144px; text-align: center; background-color: #6699FF; border-width: 1px;">
                            <b>Typical Heating Value (Heat Content)</b></td>
                        <td style="text-align: center; background-color: #6699FF">
                            <b>Heat Content for Form</b></td>
                    </tr>
                    <tr>
                        <td>Fuel Oil</td>
                        <td style="width: 144px; text-align: center;">145,000 Btu/gal</td>
                        <td style="text-align: center">145 E6BTU/1000 Gallons <b>OR</b> 0.15 E6BTU/Gallon</td>
                    </tr>
                    <tr>
                        <td style="background-color: #99CCFF">Natural gas</td>
                        <td style="width: 144px; text-align: center; background-color: #99CCFF;">1030 Btu/scf</td>
                        <td style="text-align: center; background-color: #99CCFF">1000 E6BTU/Million Standard Cubic Feet</td>
                    </tr>
                    <tr>
                        <td>Coal</td>
                        <td style="width: 144px; text-align: center;">13,000 Btu/lb</td>
                        <td style="text-align: center">26 E6BTU/Ton</td>
                    </tr>
                    <tr>
                        <td style="background-color: #99CCFF">Wood Pellets</td>
                        <td style="width: 144px; text-align: center; background-color: #99CCFF;">8,000 Btu/lb</td>
                        <td style="text-align: center; background-color: #99CCFF">16 E6BTU/Ton</td>
                    </tr>
                </table>
                <br />
                <br />
                <b>Sulfur %:</b> Percent Sulfur, by weight, in the process fuel. Enter sulfur
                percentage if the percentage is 0.01% or greater. For fuels with sulfur content
                below 0.01% place a checkmark in the &quot;Negligible&quot; checkbox. This will clear any
                value entered for the sulfur content and allow the operating details to be
                submitted without a value for sulfur content.<br />
                <br />
                <b>Ash %:</b> Percent Ash, by weight, in the process fuel. Enter ash content
                percentage if the percentage is 0.01% or greater. For fuels with ash content
                below 0.01% place a checkmark in the &quot;Negligible&quot; checkbox. This will clear any
                value entered for the ash content and allow the operating details to be
                submitted without a value for ash content.
            </div>
        </asp:Panel>
    </div>
    <div class="fieldcpewrapper">
        <act:CollapsiblePanelExtender ID="cpeEmissions" runat="server"
            TargetControlID="pnlEmissions"
            ExpandControlID="pnlhdrEmissions"
            CollapseControlID="pnlhdrEmissions"
            Collapsed="true" TextLabelID="Label3"
            ExpandedText="(...Hide Details)"
            CollapsedText="(Show Details...)"
            ImageControlID="Image3"
            CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrEmissions" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; Emissions Reporting &nbsp; &nbsp;
            <asp:Label ID="Label3" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlEmissions" runat="server" CssClass="collapsePanel">
            <div class="fieldguidewrapper">
                <p>
                    The primary goal of the annual emission inventory is to obtain the amount of
                    emissions generated by a process.&nbsp; This section allows for the entry of the
                    pollutant, emission calculation method information, and the actual amount of
                    emissions.<br />
                    <br />
                    <b>Pollutant:</b> Name of the substance, compound or compound group of which
                    emissions are produced.<br />
                    <br />
                    <b>Emission Calculation Method:</b> The method used for computing or measuring
                    emissions.<br />
                    <br />
                    <b>Emission Factor:</b> The ratio relating emissions of a specific pollutant to
                    an activity or material process rate. The numeric value of this field is
                    dependent upon the calculation method and the emission factor numerator and
                    denominator units. A pollutant-specific emission factor is typically expressed
                    as pounds per process rate units. Uncontrolled emission factors should be
                    reported here. Where applicable, reduce the resulting amount of emissions by the
                    actual capture and control efficiencies of the control approach.<br />
                    <br />
                    <b>Emission Factor Numerator:</b> Usually pounds but can be another unit, such
                    as grams where the emission factor is grams per horsepower-hour (g/Hp-Hr).<br />
                    <br />
                    <b>Emission Factor Denominator:</b> These units are usually the same as the
                    process rate units. For example, for a natural gas boiler the process rate is
                    measured in million standard cubic feet; the emission factor unit numerator
                    would be pounds and the denominator would be million standard cubic feet
                    (lbs/mmscf).<br />
                    <br />
                    <b>Actual Emissions:</b> Amount of actual annual emissions of the pollutant,
                    after application of air pollution control equipment, reported in tons per year.
                    The actual emissions for a process—measured or calculated—that represents a
                    calendar year. This total shall include emissions from fugitives, upsets,
                    startups, shutdowns, malfunctions, and excess emissions.<br />
                    <br />
                    <b>Summer Day Emissions (Process operated May&nbsp;1 - Sep&nbsp;30): </b>Summer Day
                    emissions of VOC and NOx are now being collected for facilities located in the
                    Atlanta ozone non-attainment area. The question only appears if your facility is
                    located in the ozone non-attainment area and NOx and/or VOC emissions are
                    greater than 0.3 TPY. The Summer Day emissions quantity is the average daily
                    emissions of VOC or NOx that the emission unit/process emits during the period
                    May&nbsp;1 through September&nbsp;30. This period is different from the definition of
                    Summer for a process.<br />
                    <br />
                    The Summer Day emissions is the average daily emissions of NOx and/or VOC for
                    the period May&nbsp;1 through September&nbsp;30. If a unit/process operated 7 days per
                    week during that period and had total VOC emissions of 18 tons, the summer day
                    emissions reported should be 18 tons divided by 153 days. That is 0.118 TPD
                    (18/153). If the unit/process does not operate 7 days per week, the summer day
                    emission reported should be the total emission from May&nbsp;1 through September&nbsp;30
                    divided by the number of days the unit/process operated during that period. The
                    smallest value allowed to be entered is 0.001 TPD.<p>
                        If the value entered is greater than 1/300 of the total emissions of VOC or NOx
                        for the process the value will be saved and a warning will be shown that the
                        value was saved, but is larger than expected.<b><br />
                            <br />
                            Emission Factor Explanation:</b> Any additional information regarding the
                        emission factor.<br />
                        <br />
                        <b>Delete Emissions:</b> Deleting emissions will remove all pollutant and
                        emission calculation information for the selected pollutant.&nbsp; In order to delete
                        all pollutants from the Emission Inventory submittal process, the entire Process
                        Reporting period information must be deleted from the Edit Process Operating
                        Details page using the Delete button on that page.
                        <br />
                        <br />
                        <b>Particulate Matter (PM) Reporting <span style="color: #CC0000">Errors</span>:
                        </b>The following rules must be adhered to when reporting PM emissions.<br />
                        For
                        each combination of Emission Unit and Process:<br />
                        <ol>
                            <li>If PM10-FIL and PM10-PRI are submitted, PM10-PRI must be greater than or equal
                                to PM10-FIL</li>
                            <li>If PM25-FIL and PM25-PRI are submitted, PM25-PRI must be greater than or equal
                                to PM25-FIL</li>
                            <li>If PM-CON and PM10-PRI are submitted, PM10-PRI must be greater than or equal to
                                PM-CON</li>
                            <li>If PM-CON and PM25-PRI are submitted, PM25-PRI must be greater than or equal to
                                PM-CON</li>
                            <li>If PM10-FIL and PM25-FIL are submitted, PM10-FIL must be greater than or equal
                                to PM25-FIL</li>
                            <li>If PM10-PRI and PM25-PRI are submitted, PM10-PRI must be greater than or equal
                                to PM25-PRI</li>
                            <li>If PM10-FIL, PM-CON, and PM10-PRI are submitted, PM10-FIL + PM-CON = PM10-PRI
                                (+/- 1 Ton)</li>
                            <li>If PM25-FIL, PM-CON, and PM25-PRI are submitted, PM25-FIL + PM-CON = PM25-PRI
                                (+/- 1 Ton) </li>
                        </ol>
                        <b>Particulate Matter (PM) Reporting <span style="color: #CC0000">Warnings</span>:
                        </b>The following rules should be adhered to when reporting PM emissions.<br />
                        For
                        each combination of Emission Unit and Process:
                        <br />
                        <ol>
                            <li>If PM10 Primary emissions are reported then PM2.5 Primary should also be
                                reported</li>
                            <li>If PM10 Filterable emissions are reported then PM2.5 Filterable should also be
                                reported</li>
                            <li>If PM Condensible emissions are reported then PM2.5 Filterable and/or PM10
                                Filterable should also be reported</li>
                        </ol>
                        <p>
                        </p>
                    </p>
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