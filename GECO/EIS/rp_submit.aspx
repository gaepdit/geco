<%@ Page Title="Reporting Period Submittal - GECO Emissions Inventory" Language="VB"
    MaintainScrollPositionOnPostback="true" MasterPageFile="eismaster.master"
    AutoEventWireup="false" Inherits="GECO.EIS_rp_submit" Codebehind="rp_submit.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></act:ToolkitScriptManager>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" CssClass="styledseparator" runat="server" Text="Submit Emissions Inventory Data"
            Font-Bold="True"></asp:Label><br />
        <br />
        <asp:Label ID="lblEISCheckResults" runat="server"
            Text="Emissions Inventory Submittal Check Results" Font-Bold="False"
            Font-Names="Verdana" Font-Size="Large"></asp:Label>
    </div>
    <br />
    <asp:Panel ID="pnlStart" runat="server">
        <div style="text-align: left; width: 600px; margin-left: 200px; font-size: small; color: #000000; font-weight: normal">
            Click the button below to begin the submittal process for the facility&#39;s Emissions
            Inventory data for
            <asp:Label ID="lblEIYear" runat="server"></asp:Label>
            . The data will be checked for errors before allowing submittal. To cancel and return
            to the Emissions Inventory home page, click cancel.<br />
            <br />
            If there are no errors, the button to allow submittal will appear. If alerted that
            warnings are present please review those areas before submitting to prevent the
            possibility of having EPD cancel your submittal due to bad or incorrect data.<br />
            <br />
            After submitting the data a confirmation number will be shown. Prior to EPD&#39;s
            review process you will be able to withdraw the submittal and make changes. Withdrawing
            the submittal will not delete the data already entered; you will lose the confirmation
            number and date of submittal. However, EPD does retain the date that the facility
            initially submitted the Emissions Inventory data.
        </div>
        <div class="buttonwrapper">
            <br />
            <br />

            <asp:Button ID="btnSubmit" runat="server" Text="Begin Submittal QA Process" CausesValidation="False"
                Height="30px" Font-Bold="True" />
            &nbsp;&nbsp;
            <asp:Button ID="btnCancel1" runat="server" CausesValidation="False" Text="Cancel"
                Height="30px" />
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlErrorPresent" runat="server">
        <div style="text-align: left; width: 600px; margin-left: 200px; font-size: small; color: #000000; font-weight: normal">
            Errors were found in the Emissions Inventory data for the facility. The errors are
                listed below and need to be addressed and fixed prior to submittal being allowed.<br />
            <br />
            If any warnings are indicated it is strongly suggested to review those areas to
                avoid the possibility of having EPD cancel your submittal due to bad or incorrect
                data.<br />
            <br />
            Print this page to have the list available while reviewing the data.
        </div>
        <br />
        <div style="text-align: center;">
            <asp:Label ID="lblMessage1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Large"
                ForeColor="Red"></asp:Label>
        </div>
        <br />
        <div class="buttonwrapper">
            <asp:Button ID="btnEIHome" runat="server" Text="Go to EI Home Page" CausesValidation="False"
                Height="30px" />
        </div>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlNoErrors" runat="server">
        <div style="text-align: left; width: 600px; margin-left: 200px; font-size: small; color: #000000; font-weight: normal">
            No errors were found in the Emissions Inventory data submitted. However, please
                review any warnings indicated below before submitting to avoid the possibility of
                having EPD cancel your submittal due to bad or incorrect data.<br />
            <br />
            After submitting the data a confirmation number will be shown. Prior to EPD&#39;s
                review process you will be able to withdraw the submittal and make changes. Withdrawing
                the submittal will not delete the data already entered; you will lose the confirmation
                number and date of submittal. However, EPD does retain the date that the facility
                initially submitted the Emissions Inventory data. Once the data has been submitted
                to EPA access to the Emissions Inventory area will be restricted.
        </div>
        <br />
        <br />
        <div style="text-align: center;">
            <asp:Label ID="lblMessage2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Large"
                ForeColor="#00CC66"></asp:Label>
        </div>
        <br />
        <div class="buttonwrapper">
            <asp:Button ID="btnConfSubmit" runat="server" Text="Submit EI Data" Font-Size="Medium"
                CausesValidation="False" Font-Bold="True" Height="30px" />
            &nbsp;&nbsp;
                <asp:Button ID="btnContinue" runat="server" Text="Continue" Visible="False" Height="30px" />
        </div>
        <br />
    </asp:Panel>

    <asp:Panel ID="pnlErrorList_Outer" runat="server" CssClass="gridview_errorlist">
        <asp:Panel ID="pnlErrorList" runat="server">
            <div class="fieldwrapperseparator">
                <asp:Label ID="Label3" class="styledseparator" runat="server" Text="Error Lists" Font-Bold="True" Font-Size="Large"></asp:Label>
            </div>
            <asp:Panel ID="pnlGeneralErrors" runat="server">
                <p>
                    <asp:Label ID="lblNoRPData" runat="server" Text="" Font-Bold="True"></asp:Label><br />
                    <asp:HyperLink ID="hlnkNORPData" runat="server" NavigateUrl="~/EIS/rp_summary.aspx">Click here to go to Reporting Period Summary</asp:HyperLink>
                </p>
                <p>
                    <asp:Label ID="lblBadNaics" runat="server" Text="" Font-Bold="True"></asp:Label><br />
                    <asp:HyperLink ID="hlBadNaics" runat="server" NavigateUrl="~/EIS/facility_edit.aspx">Click here to edit NAICS</asp:HyperLink>
                </p>
                <p>
                    <asp:Label ID="lblInvalidSiteStatus" runat="server" Text="" Font-Bold="True"></asp:Label><br />
                </p>
                <p>
                    <asp:Label ID="lblMissingSiteAddress" runat="server" Text="" Font-Bold="True"></asp:Label><br />
                </p>
                <p>
                    <asp:Label ID="lblMissingFacilityGeo" runat="server" Text="" Font-Bold="True"></asp:Label><br />
                </p>
            </asp:Panel>
            <br />

            <asp:Label ID="lblEmissionUnitErrors" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionUnit" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblStackErrors" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwStackErrors" runat="server"
                DataKeyNames="ReleasePointID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" BorderStyle="Inset" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="stack_edit.aspx?stk={0}"
                        DataTextField="ReleasePointID" HeaderText="Stack ID" NavigateUrl="~/eis/stack_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="stack_edit.aspx?stk={0}"
                        DataTextField="strRPDescription" HeaderText="Description" NavigateUrl="~/eis/stack_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="numRPStackHeightMeasure" HeaderText="Stack Height (ft)" />
                    <asp:BoundField DataField="numRPStackDiameterMeasure" HeaderText="Stack Diameter (ft)" />
                    <asp:BoundField DataField="numRPExitGasVelocityMeasure" HeaderText="Exit Gas Velocity (fps)" />
                    <asp:BoundField DataField="NUMRPEXITGASTEMPMEASURE" HeaderText="Exit Gas Temperature" />
                    <asp:BoundField DataField="numRPExitGasFlowRateMeasure" HeaderText="Exit Gas Flow Rate (acfs)" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblE03b" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwE03b" runat="server"
                DataKeyNames="ReleasePointID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" BorderStyle="Inset" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="stack_edit.aspx?stk={0}"
                        DataTextField="ReleasePointID" HeaderText="Stack ID" NavigateUrl="~/eis/stack_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="stack_edit.aspx?stk={0}"
                        DataTextField="strRPDescription" HeaderText="Description" NavigateUrl="~/eis/stack_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="numrpfencelinedistmeasure" HeaderText="Fence line distance (ft)" />
                    <asp:BoundField DataField="numLatitudeMeasure" HeaderText="Latitude">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="numLongitudeMeasure" HeaderText="Longitude">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="strrpstatuscode" HeaderText="Release point operating status" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblFugitiveErrors" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwFugitiveErrors" runat="server"
                DataKeyNames="ReleasePointID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="fugitive_edit.aspx?fug={0}"
                        DataTextField="ReleasePointID" HeaderText="Fugitive ID" NavigateUrl="~/eis/fugitive_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="fugitive_edit.aspx?fug={0}"
                        DataTextField="strRPDescription" HeaderText="Description" NavigateUrl="~/eis/fugitive_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="numLatitudeMeasure" HeaderText="Latitude">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="numLongitudeMeasure" HeaderText="Longitude">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblRPApportionmentErrors" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwRPApportionmentTotals" runat="server"
                DataKeyNames="EmissionsUnitID,ProcessID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist" EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="ProcessID" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rpapportionment_edit.aspx?eu={0}&amp;ep={1}"
                        HeaderText="Process ID" NavigateUrl="~/EIS/rpapportionment_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="strProcessDescription" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rpapportionment_edit.aspx?eu={0}&amp;ep={1}"
                        HeaderText="Process Description" NavigateUrl="~/EIS/rpapportionment_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="TotalApportionment" HeaderText="Apportionment Total">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionUnitCtrlApp" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionUnitCtrlApp" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="eucontrolapproach_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="eucontrolapproach_edit.aspx?eu={0}"
                        DataTextField="strControlApproachDescription" HeaderText="Unit Control Approach Description"
                        NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionUnitCtrlAppData" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionUnitCtrlAppData" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="eucontrolapproach_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="eucontrolapproach_edit.aspx?eu={0}"
                        DataTextField="strControlApproachDescription" HeaderText="Unit Control Approach Description"
                        NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionUnitCtrlAppPollData" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionUnitCtrlAppPollData" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="eucontrolapproach_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="eucontrolapproach_edit.aspx?eu={0}"
                        DataTextField="strControlApproachDescription" HeaderText="Unit Control Approach Description"
                        NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblProcessCtrlApp" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwProcessCtrlApp" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Class="gridview"
                PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="EmissionsUnitID" HeaderText="Emission Unit ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="strControlApproachDesc" HeaderText="Process Control Approach Description"
                        NavigateUrl="~/eis/processcontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblProcessCtrlAppData" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwProcessCtrlAppData" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Class="gridview"
                PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="EmissionsUnitID" HeaderText="Emission Unit ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="strControlApproachDesc" HeaderText="Process Control Approach Description"
                        NavigateUrl="~/eis/processcontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblProcessCtrlAppPollData" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwProcessCtrlAppPollData" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Class="gridview"
                PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="EmissionsUnitID" HeaderText="Emission Unit ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/eucontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="strControlApproachDesc" HeaderText="Process Control Approach Description"
                        NavigateUrl="~/eis/processcontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblPMControlPollutantDependancy" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwPMControlPollutantDependancy" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Class="gridview"
                PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="EmissionsUnitID" HeaderText="Emission Unit ID" NavigateUrl="~/eis/processcontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="processcontrolapproach_edit.aspx?eu={0}&ep={1}"
                        DataTextField="ProcessID" HeaderText="Process ID" NavigateUrl="~/eis/processcontrolapproach_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblStackRPGeoCoordDupes" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwStackRPGeoCoordDupes" runat="server" AutoGenerateColumns="False"
                BorderStyle="Inset"
                CellPadding="4" class="gridview" CssClass="gridview_errorlist" DataKeyNames="ReleasePointID"
                Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" PageSize="20">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="stack_details.aspx?stk={0}"
                        DataTextField="ReleasePointID" HeaderText="Stack ID" NavigateUrl="~/eis/stack_details.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="stack_details.aspx?stk={0}"
                        DataTextField="strRPDescription" HeaderText="Description" NavigateUrl="~/eis/stack_details.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="numLatitudeMeasure" HeaderText="Latitude" />
                    <asp:BoundField DataField="numLongitudeMeasure" HeaderText="Longitude" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblFugitiveRPGeoCoordDupes" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwFugitiveRPGeoCoordDupes" runat="server"
                AutoGenerateColumns="False" BorderStyle="Inset"
                CellPadding="4" class="gridview" CssClass="gridview_errorlist" DataKeyNames="ReleasePointID"
                Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" PageSize="20">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="fugitive_details.aspx?fug={0}"
                        DataTextField="ReleasePointID" HeaderText="Fugitive ID" NavigateUrl="~/eis/fugitive_details.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="fugitive_details.aspx?fug={0}"
                        DataTextField="strRPDescription" HeaderText="Description" NavigateUrl="~/eis/fugitive_details.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="numLatitudeMeasure" HeaderText="Latitude" />
                    <asp:BoundField DataField="numLongitudeMeasure" HeaderText="Longitude" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblProcessSCC" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwProcessSCC" runat="server" DataKeyNames="emissionsunitid,processid"
                CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False" class="gridview" PageSize="20"
                CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="process_edit.aspx?eu={0}&ep={1}"
                        DataTextField="processid" HeaderText="Process ID" NavigateUrl="~/eis/process_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="process_edit.aspx?eu={0}&ep={1}"
                        DataTextField="strProcessDescription" HeaderText="Description" NavigateUrl="~/eis/process_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionUnitsNotInRptPeriod" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionUnitsNotInRptPeriod" runat="server"
                DataKeyNames="EmissionsUnitID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist"
                EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblProcessThroughputNullErrors" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwProcessThroughputNullErrors" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="rp_operscp_edit.aspx?eu={0}&ep={1}"
                        DataTextField="processid" HeaderText="Process ID" NavigateUrl="~/eis/rp_operscp_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid,processid" DataNavigateUrlFormatString="rp_operscp_edit.aspx?eu={0}&ep={1}"
                        DataTextField="strProcessDescription" HeaderText="Description" NavigateUrl="~/eis/rp_operscp_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblProcessCalcParamType" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwProcessCalcParamType" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4"
                Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist"
                EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="ProcessID" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_operscp_edit.aspx?eu={0}&amp;ep={1}" HeaderText="Process ID"
                        NavigateUrl="~/EIS/rp_operscp_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="strProcessDescription" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_operscp_edit.aspx?eu={0}&amp;ep={1}" HeaderText="Process Description"
                        NavigateUrl="~/EIS/rp_operscp_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="strCPTypeDesc" HeaderText="Calculation Parameter type"
                        Visible="False" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionNullErrors" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionNullErrors" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="processid" HeaderText="ProcessID" NullDisplayText="No Data">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Process Description" DataField="strprocessdescription">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PollutantCode" HeaderText="PollutantCode" Visible="False" />
                    <asp:HyperLinkField DataTextField="strPollutant" DataNavigateUrlFields="EmissionsUnitID,ProcessID,PollutantCode"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em={2}"
                        HeaderText="Pollutant" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionFactors" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionFactors" runat="server" s
                DataKeyNames="EmissionsUnitID,ProcessID,PollutantCode" CellPadding="4" Font-Names="Arial"
                Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="EmissionsUnitID">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProcessID" HeaderText="Process ID">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="strPollutant" DataNavigateUrlFields="EmissionsUnitID,ProcessID,PollutantCode"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em={2}"
                        HeaderText="Pollutant" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblSummerDay" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwSummerDay" runat="server"
                DataKeyNames="emissionsunitid"
                CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False" class="gridview" PageSize="20"
                CssClass="gridview_errorlist" EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="EmissionsUnitID" HeaderText="Emission Unit ID" Visible="True" />
                    <asp:BoundField DataField="ProcessID" HeaderText="Process ID" Visible="True" />
                    <asp:BoundField DataField="strProcessDescription" HeaderText="Process Description">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="PollutantCode" DataNavigateUrlFields="EmissionsUnitID,ProcessID,PollutantCode"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em={2}"
                        HeaderText="Pollutant" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="EmissionSum" HeaderText="Emissions TPY" Visible="true" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblSeasonTotals" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwSeasonTotals" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4"
                Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist"
                EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="ProcessID" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_operscp_edit.aspx?eu={0}&amp;ep={1}" HeaderText="Process ID"
                        NavigateUrl="~/EIS/rp_operscp_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="strProcessDescription" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_operscp_edit.aspx?eu={0}&amp;ep={1}" HeaderText="Process Description"
                        NavigateUrl="~/EIS/rp_operscp_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="SeasonTotal" HeaderText="Total for All Seasons" Visible="False" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblOperatingDetailsAppData" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwOperatingDetailsAppData" runat="server"
                DataKeyNames="emissionsunitid,processid" CellPadding="4"
                Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist"
                EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="ProcessID" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_operscp_edit.aspx?eu={0}&amp;ep={1}" HeaderText="Process ID"
                        NavigateUrl="~/EIS/rp_operscp_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblPMTotalEmissionsError" runat="server" Font-Bold="True"></asp:Label><br />
            <asp:GridView ID="gvwPMTotalEmissionsError" runat="server"
                DataKeyNames="EmissionsUnitID,ProcessID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="ProcessID" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em="
                        HeaderText="Process ID" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="strProcessDescription" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em="
                        HeaderText="Description" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="TotalPM25Pri" HeaderText="PM-25 Pri">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPM10Pri" HeaderText="PM-10 Pri">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPM25Fil" HeaderText="PM-25 Fil">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPM10Fil" HeaderText="PM-10 Fil">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPMCon" HeaderText="PM Cond">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ErrorType" HeaderText="Error Type" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionsNotReported" runat="server" Font-Bold="True"></asp:Label><br />
            <asp:GridView ID="gvwEmissionsNotReported" runat="server"
                DataKeyNames="EmissionsUnitID,ProcessID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="ProcessID" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em="
                        HeaderText="Process ID" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="strProcessDescription" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em="
                        HeaderText="Description" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblE31" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwE31" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblE32" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwE32" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblE33" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwE33" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblE34b" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwE34b" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                Class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblE38" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwE38" runat="server"
                DataKeyNames="EMISSIONSUNITID,PROCESSID" CellPadding="4"
                Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist"
                EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="EMISSIONSUNITID">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="PROCESSID" DataNavigateUrlFields="EMISSIONSUNITID,PROCESSID"
                        DataNavigateUrlFormatString="~/EIS/rp_operscp_edit.aspx?eu={0}&amp;ep={1}" HeaderText="Process ID"
                        NavigateUrl="~/EIS/rp_operscp_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="STRPROCESSDESCRIPTION" DataNavigateUrlFields="EMISSIONSUNITID,PROCESSID"
                        DataNavigateUrlFormatString="~/EIS/rp_operscp_edit.aspx?eu={0}&amp;ep={1}" HeaderText="Process Description"
                        NavigateUrl="~/EIS/rp_operscp_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="HEATCONTENT" HeaderText="Heat Content">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HCNUMER" HeaderText="Heat Content Numerator">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HCDENOM" HeaderText="Heat Content Denominator">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SULFURCONTENT" HeaderText="Sulfur Content (%)">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ASHCONTENT" HeaderText="Ash Content (%)">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblEmissionUnitType" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwEmissionUnitType" runat="server"
                DataKeyNames="emissionsunitid" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="emissionsunitid" HeaderText="Emission Unit ID" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="emissionsunitid" DataNavigateUrlFormatString="emissionunit_edit.aspx?eu={0}"
                        DataTextField="strUnitDescription" HeaderText="Description" NavigateUrl="~/eis/emissionunit_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblE39" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwE39" runat="server"
                DataKeyNames="EMISSIONSUNITID,PROCESSID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit" DataField="EmissionUnit">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataNavigateUrlFields="EMISSIONSUNITID,PROCESSID" DataNavigateUrlFormatString="rp_operscp_edit.aspx?eu={0}&ep={1}"
                        DataTextField="Process" HeaderText="Process" NavigateUrl="~/eis/rp_operscp_edit.aspx">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlWarningList_Outer" runat="server" CssClass="gridview_errorlist">
        <asp:Panel ID="pnlWarningList" runat="server">
            <div class="fieldwrapperseparator">
                <asp:Label ID="Label1" class="styledseparator" runat="server" Text="Warning Lists" Font-Bold="True" Font-Size="Large"></asp:Label>
            </div>

            <asp:Label ID="lblFugitiveWarning" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwFugitiveWarning" runat="server"
                DataKeyNames="ReleasePointID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist" Width="300px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="fugitive_edit.aspx?fug={0}"
                        DataTextField="ReleasePointID" HeaderText="Fugitive ID" NavigateUrl="~/eis/fugitive_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataNavigateUrlFields="ReleasePointID" DataNavigateUrlFormatString="fugitive_edit.aspx?fug={0}"
                        DataTextField="strRPDescription" HeaderText="Description" NavigateUrl="~/eis/fugitive_edit.aspx">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblPMTotalEmissionsWarning" runat="server" Font-Bold="True"></asp:Label><br />
            <asp:GridView ID="gvwPMTotalEmissionsWarning" runat="server"
                DataKeyNames="EmissionsUnitID,ProcessID" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="emissionsunitid">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="ProcessID" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em="
                        HeaderText="Process ID" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="strProcessDescription" DataNavigateUrlFields="EmissionsUnitID,ProcessID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}&amp;em="
                        HeaderText="Description" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="TotalPM25Pri" HeaderText="PM-25 Pri">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPM10Pri" HeaderText="PM-10 Pri">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPM25Fil" HeaderText="PM-25 Fil">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPM10Fil" HeaderText="PM-10 Fil">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalPMCon" HeaderText="PM Cond">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

            <asp:Label ID="lblTwentyPercent" runat="server" Font-Bold="True"></asp:Label>
            <asp:GridView ID="gvwTwentyPercent" runat="server"
                DataKeyNames="" CellPadding="4" Font-Names="Arial" Font-Size="Small"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" AllowSorting="True"
                class="gridview" PageSize="20" CssClass="gridview_errorlist" Width="300px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Emission Unit ID" DataField="EMISSIONSUNITID">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataTextField="PROCESSID" DataNavigateUrlFields="EMISSIONSUNITID,PROCESSID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}"
                        HeaderText="Process ID" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                    <asp:HyperLinkField DataTextField="STRPROCESSDESCRIPTION" DataNavigateUrlFields="EMISSIONSUNITID,PROCESSID"
                        DataNavigateUrlFormatString="~/EIS/rp_emissions_edit.aspx?eu={0}&amp;ep={1}"
                        HeaderText="Process Description" NavigateUrl="~/EIS/rp_emissions_edit.aspx">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:HyperLinkField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>

        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlQALists" runat="server" CssClass="gridview_errorlist">
        <act:CollapsiblePanelExtender ID="cpaQA" runat="server" TargetControlID="pnlQA"
            ExpandControlID="pnlhdrQA" CollapseControlID="pnlhdrQA"
            Collapsed="true" TextLabelID="label2" ExpandedText="(...Hide Details)" CollapsedText="(Show Details...)"
            ImageControlID="Image3" CollapsedImage="~/assets/images/expand.jpg" ExpandedImage="~/assets/images/collapse.jpg"
            SuppressPostBack="true">
        </act:CollapsiblePanelExtender>
        <asp:Panel ID="pnlhdrQA" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/assets/images/expand.jpg" />
            &nbsp; &nbsp; QA Checks Descriptions &nbsp; &nbsp; 
            <asp:Label ID="Label2" class="styledseparator" runat="server" Text="(Show Details...)"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlQA" runat="server" CssClass="collapsePanel">
            <h3>EPA QA Checks</h3>
            <p>
                EPA has provided a spreadsheet listing the QA checks they perform on EIS data. Most of the same checks are performed on EPD 
            data, either upon entry or as part of this submittal QA process.
            </p>
            <p>
                A simplified list of errors and warnings is show below. You may also download the complete spreadsheet of <a href="/EIS/files/epa_qa_checks.xlsx" download>EPA QA Checks</a>.
            </p>
            <h3>List of Possible EIS Errors</h3>
            <asp:GridView ID="gvwEISErrors" runat="server" CellPadding="4"
                Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False"
                class="gridview" PageSize="20" CssClass="gridview_errorlist"
                EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Error ID" DataField="ErrorID">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Small" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Basis for Error" DataField="strConditionForError">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Small" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="true" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Corrective Action" DataField="strCorrectiveAction">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Small" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="true" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <h3>List of Possible EIS Warnings</h3>
            <asp:GridView ID="gvwEISWarnings" runat="server"
                CellPadding="4" Font-Names="Arial" Font-Size="Small" ForeColor="#333333" GridLines="None"
                AutoGenerateColumns="False" class="gridview" PageSize="20" CssClass="gridview_errorlist"
                EnableModelValidation="True">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField HeaderText="Warning ID" DataField="WarningID">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Small" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Basis for Warning" DataField="strConditionForWarning">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Small" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="true" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Corrective Action" DataField="strCorrectiveAction">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Small" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="true" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <br />
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlPMTotalEmissions" runat="server" Width="540px" BackColor="LightBlue"
        Style="display: none; font-family: Verdana; font-size: small; width: 540px;">
        <b>PM Reporting Errors</b>
        <ol>
            <li>If PM10-FIL and PM10-PRI are submitted, PM10-PRI must be greater than or equal to PM10-FIL</li>
            <li>If PM25-FIL and PM25-PRI are submitted, PM25-PRI must be greater than or equal to PM25-FIL</li>
            <li>If PM-CON and PM10-PRI are submitted, PM10-PRI must be greater than or equal to PM-CON</li>
            <li>If PM-CON and PM25-PRI are submitted, PM25-PRI must be greater than or equal to PM-CON</li>
            <li>If PM10-FIL and PM25-FIL are submitted, PM10-FIL must be greater than or equal to PM25-FIL</li>
            <li>If PM10-PRI and PM25-PRI are submitted, PM10-PRI must be greater than or equal to PM25-PRI</li>
            <li>If PM10-FIL, PM-CON, and PM10-PRI are submitted, PM10-FIL + PM-CON = PM10-PRI (+/- 1 Ton)</li>
            <li>If PM25-FIL, PM-CON, and PM25-PRI are submitted, PM25-FIL + PM-CON = PM25-PRI (+/- 1 Ton) </li>
        </ol>
        <b>PM Reporting Warnings</b>
        <ol>
            <li>If PM10 Pri reported, PM2.5 Pri should be reported.</li>
            <li>If PM10 Fil reported, PM2.5 Fil should be reported.</li>
            <li>If PM Cond reported, PM10 Fil and/or PM2.5 Fil should be reported.</li>
        </ol>
    </asp:Panel>
</asp:Content>
