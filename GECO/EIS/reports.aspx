<%@ Page Title="GECO Emissions Inventory Reports" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_reports" CodeBehind="reports.aspx.vb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <style type="text/css">
        .rpt-buttons {
            list-style: none;
            margin: 0;
            padding: 0;
        }

            .rpt-buttons li {
                display: inline-block;
            }

                .rpt-buttons li a {
                    display: inline-block;
                    padding: 8px 16px;
                    margin: 8px 4px;
                    background: #eee;
                    border: 1px solid #ccc;
                    border-radius: 4px;
                    color: blue;
                    text-decoration: none;
                }

                    .rpt-buttons li a:hover, .rpt-buttons li a:focus {
                        background: #ddd;
                        text-decoration: underline;
                    }
    </style>
    <div style="text-align: center">
        <h2>Emissions Inventory Reports</h2>

        <p>
            Click one of the buttons below to go the the page for the indicated report.<br />
            Reports can be printed and exported to Microsoft Excel.
        </p>

        <div style="margin: 30px 0 30px;">
            <h3>Facility Inventory Reports</h3>
            <ul class="rpt-buttons">
                <li>
                    <asp:HyperLink ID="btnReleasePoints" runat="server"
                        Text="Release Points" CausesValidation="False"
                        NavigateUrl="report_releasepoints.aspx"></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="btnEmissionUnits" runat="server"
                        Text="Emission Units" CausesValidation="False"
                        NavigateUrl="report_emissionunits.aspx"></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="btnProcesses" runat="server"
                        Text="Processes" CausesValidation="False"
                        NavigateUrl="report_processes.aspx"></asp:HyperLink>
                </li>
            </ul>
        </div>

        <div style="margin: 30px 0 30px;">
            <h3>Emissions Inventory Reports</h3>
            <ul class="rpt-buttons">
                <li>
                    <asp:HyperLink ID="btnProcessThroughput" runat="server"
                        Text="Process Reporting Period" CausesValidation="False"
                        NavigateUrl="report_proctputdetails.aspx"></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="btnPollutant" runat="server"
                        Text="Pollutant Report" CausesValidation="False"
                        NavigateUrl="report_pollutantdetails.aspx"></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="btnEmissions" runat="server"
                        Text="Facility-Wide Emissions Summary" CausesValidation="False"
                        NavigateUrl="report_fw_emsummary.aspx"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
