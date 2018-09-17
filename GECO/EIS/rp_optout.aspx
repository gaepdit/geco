<%@ Page Title="GECO Emission Inventory" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.EIS_rp_optout" Codebehind="rp_optout.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server"
            Text="Opt Out Form" Font-Bold="True"></asp:Label>
    </div>
    <div style="text-align: center">
        <table style="width: 100%">
            <tr>
                <td>
                    <strong><span style="font-size: 11pt">Calendar Year
                    <asp:Label ID="lblEIYear1" runat="server"></asp:Label>
                        &nbsp;Thresholds<br />
                    </span><span style="color: #FF0000">Note: Thresholds are TPY of POTENTIAL Emissions</span></strong></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="dgvThresholds" runat="server"
                        AutoGenerateColumns="False" CssClass="tablestyle" DataSourceID="sqldsThresholds"
                        EmptyDataText="NA" HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundField DataField="strPollutant" HeaderText="Pollutant">
                                <HeaderStyle Font-Size="Small" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="numThreshold" HeaderText="Threshold (tpy)"
                                NullDisplayText="NA">
                                <HeaderStyle Font-Size="Small" HorizontalAlign="Center"
                                    VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="numThresholdNAA" HeaderText="NAA* Threshold (tpy)"
                                NullDisplayText="NA">
                                <HeaderStyle Font-Size="Small" HorizontalAlign="Center"
                                    VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                            </asp:BoundField>
                        </Columns>
                        <AlternatingRowStyle CssClass="alternatingrowstyle" />
                        <HeaderStyle BackColor="#D3DBE7" CssClass="headerstyle"
                            HorizontalAlign="Center" VerticalAlign="Middle" />
                        <FooterStyle HorizontalAlign="Center" />
                        <RowStyle CssClass="rowstyle" />
                    </asp:GridView>
                    <asp:Label ID="lblNAA" runat="server" Text="*NAA - Ozone Nonattainment Area"
                        Font-Size="Small"
                        ToolTip="Cherokee, Clayton, Cobb, Coweta, DeKalb, Douglas, Fayette, Forsyth, Fulton, Gwinnett, Henry, Paulding, and Rockdale counties"></asp:Label>
                    <br />
                    <asp:SqlDataSource ID="sqldsThresholds" runat="server"></asp:SqlDataSource>
                </td>
            </tr>
            <tr style="font-size: 10pt">
                <td align="center">
                    <table style="width: 100%">
                        <tr>
                            <td align="left" valign="top">&nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <span style="font-family: Arial;">Selecting "YES" below and submitting this form is a certification
                        that the facility potential emissions of all pollutants shown in the table above are below
                        the indicated thresholds and, because none of these thresholds were exceeded, the facility does
                        not need to and does not wish to participate in emission inventory data collection
                        for the calendar year
                        <asp:Label ID="lblEIYear2" runat="server" Font-Names="Arial"
                            Font-Size="Small"></asp:Label><span
                                id="Label2"></span></span><font face="Arial" size="2">.</font></td>
                        </tr>
                        <tr>
                            <td align="center" valign="top">
                                <strong><span style="color: #0000ff">Would you like to opt out of the Emissions Inventory
                                    for this facility?<br />
                                </span>
                                    <asp:RadioButtonList ID="rbtnYesNo" runat="server" Font-Names="Arial" Font-Size="Small"
                                        RepeatDirection="Horizontal" RepeatLayout="Flow" Width="85px">
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                    &nbsp;&nbsp; &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" /></strong>
                                <asp:RequiredFieldValidator ID="reqvYesNo" runat="server" ControlToValidate="rbtnYesNo"
                                    ErrorMessage="Select Yes or No before submitting." Font-Names="Arial"
                                    Font-Size="Small"></asp:RequiredFieldValidator></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="font-size: 10pt">
                <td align="left">&nbsp;
                    &nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>