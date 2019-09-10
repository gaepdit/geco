<%@ Page Language="VB" MasterPageFile="~/AnnualFees/AnnualFees.master" AutoEventWireup="false" Inherits="GECO.AnnualFees_Default" Title="GECO Emissions Fees" CodeBehind="Default.aspx.vb" %>

<%@ Import Namespace="GECO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <acs:ModalUpdateProgress ID="ModalUpdateProgress1" runat="server" DisplayAfter="1500"
        BackgroundCssClass="modalProgressGreyBackground">
        <ProgressTemplate>
            <div class="modalPopup">
                Please Wait...
               
                <img src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" align="middle" alt="working..." />
            </div>
        </ProgressTemplate>
    </acs:ModalUpdateProgress>

    <script type="text/javascript">
        function ActiveTabChanged(sender, e) {
            // use the client side active tab changed
            // event to trigger a callback thereby
            // triggering the server side event.
            __doPostBack('<%= UserTabs.ClientID %>',
                sender.get_activeTab().get_headerText());
        }
    </script>

    <div visible="false" style="display: none;">
        <asp:Label ID="feeyear" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="pertonrate" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="smfee" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="nspsfee" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="titlevfee" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="adminfee" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="adminfeedate" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="numaathres" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="numnathres" runat="server" Visible="false"></asp:Label>
    </div>
    <div align="center">
        <strong>Submission Deadline:</strong>
        <asp:Label ID="lblDeadline" ForeColor="DarkRed" Font-Bold="true" runat="server"></asp:Label><br />
        <br />
        <strong>Fee Year:</strong>
        <asp:DropDownList ID="ddlFeeYear" runat="server" CausesValidation="False" AutoPostBack="true"
            ForeColor="DarkRed" Font-Bold="true">
        </asp:DropDownList>
        <br />
        <br />
        <asp:LinkButton ID="lbtInvoice" Text="PRINT INVOICE" Visible="false" runat="server"
            CausesValidation="False" CssClass="linkHighlightButton"
            Font-Bold="True" OnClick="lbtInvoice_Click"></asp:LinkButton><br />
    </div>
    <p>
        <asp:Label ID="lblMessage" runat="server" ForeColor="#C00000" Visible="False"></asp:Label>
    </p>

    <act:TabContainer runat="server" ID="UserTabs" OnClientActiveTabChanged="ActiveTabChanged" OnActiveTabChanged="DoServerSideCode">

        <act:TabPanel runat="Server" ID="Welcome" HeaderText="Welcome">
            <ContentTemplate>
                <div align="right">
                    <asp:Button ID="btnProceed" runat="server" Font-Bold="True"
                        Text="Begin →" CausesValidation="False" Visible="false" />
                </div>

                <h1>Annual Permit/Emissions Fees Reporting Form</h1>
                <p>
                    The Georgia Air Protection Branch’s Emission Fee Reports are online.
                    Your facility is required to complete the on-line fee form and submit
                    your fee data electronically no matter the amount of fees the facility may owe.
                    If your facility does not owe a fee, you are still required to complete the on-line
                    fee form and submit the Georgia Air Emissions data electronically. However, you
                    will not be required to mail in any forms or coupons if you do not owe a fee.
               
                </p>

                <h2>Procedures</h2>
                <p>
                    Current and past Air Permit Fee Manuals, as well as fee calculation forms, are 
                    available on the 
                   
                    <a href="https://epd.georgia.gov/air-permit-fees" target="_blank">Air Permit Fees</a>
                    page. The Permit Fee Manual specifies the methods used to calculate the permit fees 
                    requiredunder Georgia Air Quality Control Rule 391-3-1-.03(9), “Permit Fees.” 
               
                </p>
                <p>
                    The fee calculation worksheets are for your facility records. Do not submit the
                    worksheets with your fee invoice. These forms are also used by the Air Protection
                    Branch to perform internal audits of our fee information. If you keep these in your
                    records, it will assist you if you are ever contacted for an audit.
               
                </p>
                <p>
                    If you would like to make changes to any previously submitted fee form, you may do 
                    so using the <a href="FeeAmendment.xls" download>Fee Amendment Form</a>.
               
                </p>

                <div runat="server" id="feeRatesSection" visible="false">
                    <h2>Calendar Year <% =feeyear.Text %> Emission Fees</h2>
                    <p>
                        (1) For major Part 70 sources paying the “calculated fees”, the cost per ton of
                        pollutant is <strong><% =pertonrate.Text %></strong>. If a stationary source with 
                        a Part 70 permit permanently ceases operation
                        prior to the calendar year in which the fees are based and requests that the Part
                        70 permit for that facility be revoked and the Division revokes the Part 70 permit
                        for the facility during or prior to the calendar year in which fees are based, the
                        Part 70 fee does not apply.
                   
                    </p>
                    <p>
                        (2) Any source for which a Part 70 (Title V) permit application is or will be required
                        to be submitted for the purpose of obtaining a Part 70 permit is required to pay
                        Part 70 Fees once a construction (SIP) permit is required under 391-3-1-.03(1) has
                        been issued for the construction of a new Part 70 source or the modification of
                        an existing source which results in the source becoming a Part 70 source.
                   
                    </p>
                    <p>
                        Synthetic Minor (SM) sources that are not also Part 70 sources owe a Synthetic Minor
                        Fee of <strong><% =smfee.Text %></strong>. True Minor (B) and Permit-by-Rule (PR) 
                        sources that are not also
                        Part 70 sources do not have to pay either the Part 70 or Synthetic Minor Fee. If
                        a stationary source with a synthetic minor permit permanently ceases operation and
                        requests that the synthetic minor operating permit for that facility be revoked
                        and the Division revokes the synthetic minor operating permit for the facility during
                        or prior to the calendar year in which the fees are based, the synthetic minor permit
                        fee does not apply.
                   
                    </p>
                    <p>
                        (3) Sources that are subject to at least one NSPS standard must pay the NSPS fee
                        of <strong><% =nspsfee.Text %></strong>, unless all of the NSPS standards the source 
                        is subject to are listed in section 2.1 of the Fee Manual.
                   
                    </p>
                    <p>
                        (4) The NSPS Fee is due in <strong><em>addition</em></strong> to any Part 70 or synthetic minor
                        fee that may be due.
                   
                    </p>
                    <p>
                        (5) If the total amount due for a facility is $10,000 or greater, the fee may be
                        paid in four equal quarterly payments.
                   
                    </p>
                </div>

                <h2>Notes</h2>
                <p>
                    The "Fee Calculations" and "Sign & Pay" tabs are only available when fees have not yet
                    been submitted for the selected year.
               
                </p>
                <p>
                    If your calculated emissions are below the thresholds listed in Section 3.16 of
                    the procedures manual, and the facility is either not subject to or is exempt from
                    the NSPS, synthetic minor, and Part 70 fees as described in Section 2.0, then you
                    do not owe any fees. For example, if the facility’s emissions were only one ton
                    and the facility is a minor (B) source and is not subject to any NSPS standards,
                    you would pay zero rather than $28.50 for one ton of emissions. However, you are
                    still required to submit the Georgia Air Emissions Fee Reporting Form on-line.
               
                </p>
                <p>
                    Please <strong>DO NOT</strong> send copies of your calculations with the payment coupon
                    and check. They will only be discarded. Keep the calculations information in your
                    files for a period of five years. Cover letters are required only in the event an
                    explanation is required to clear up discrepancies.
               
                </p>
                <p>
                    Please send the payment and the invoice in the same envelope.
                    Checks are frequently written without indication to which AIRS number they apply,
                    leading to double billing. We suggest that you have checks (that must be issued
                    from a different location than your own) mailed to your location, then mailed
                    to the Air Fees lockbox along with the appropriate form.
               
                </p>
                <p>
                    If you have questions regarding:
               
                </p>
                <ul>
                    <li>Calculations: Call the compliance engineer assigned to your facility or call 404-363-7000.</li>
                    <li>Other fees questions (due dates, deadline extensions, etc.): Call Tammy Tucker at 404-362-2521.</li>
                </ul>
            </ContentTemplate>
        </act:TabPanel>

        <act:TabPanel runat="Server" ID="Contact" HeaderText="Fee Contact">
            <ContentTemplate>
                <p>
                    These options will pre-load the contact form with either the current Fee Contact information or the 
                    contact information from your GECO user profile:
                </p>
                <asp:RadioButtonList ID="rblFeeContact" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="1">Use the Fee Contact information for the Permit Fees Contact</asp:ListItem>
                    <asp:ListItem Value="2">Use My GECO Contact information for the Permit Fees Contact</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Contact"
                    DisplayMode="BulletList" />
                <br />
                <br />
                <strong>Fee Contact</strong>
                <table id="Table1" class="sample">
                    <tr>
                        <td width="25%">First Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact" MaxLength="35"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFName" ErrorMessage="Type First Name"
                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Last Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact" MaxLength="35"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName" ErrorMessage="Type Last Name"
                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Title:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="unwatermarked" ValidationGroup="Contact" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTitle" ErrorMessage="Type Title"
                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Company Name (of Fee Contact):
                        </td>
                        <td>
                            <asp:TextBox ID="txtCoName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCoName" ErrorMessage="Type Company Name"
                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Street Address:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="unwatermarked" ValidationGroup="Contact" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                                ErrorMessage="Type Street Address" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="ZipCityState" runat="server">
                    <ContentTemplate>
                        <table id="citystate" class="sample">
                            <tr>
                                <td width="25%">Zip Code:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtZip" AutoPostBack="true" runat="server" CssClass="unwatermarked"
                                        MaxLength="5" ValidationGroup="Contact"></asp:TextBox><asp:Label ID="lblZipError"
                                            runat="server" ForeColor="Red" Visible="False" Font-Size="Small"></asp:Label><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtzip" ErrorMessage="Type 5-digit Zip Code"
                                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                    <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtZip"
                                        FilterType="Numbers">
                                    </act:FilteredTextBoxExtender>
                                    &nbsp;&nbsp;
                                   
                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="ZipCityState">
                                        <ProgressTemplate>
                                            <div class="progress">
                                                <img alt="loading..." src='<%= Page.ResolveUrl("~/assets/images/indicator_smallwaitanim.gif") %>' />
                                                Loading City & State...
                                           
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr>
                                <td>City:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="unwatermarked" ValidationGroup="Contact" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity" ErrorMessage="Type City Name"
                                        Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>State:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtState" runat="server" CssClass="unwatermarked" MaxLength="2"
                                        ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator12"
                                            runat="server" ControlToValidate="txtState" ErrorMessage="State Abbreviation"
                                            Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <table id="Table2" class="sample">
                    <tr>
                        <td width="25%">Telephone Number:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="unwatermarked" MaxLength="30" ValidationGroup="Contact" />
                        </td>
                    </tr>
                    <tr>
                        <td>Fax Number:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="unwatermarked" MaxLength="10" ValidationGroup="Contact"></asp:TextBox>
                            <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFax"
                                FilterType="Numbers">
                            </act:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>Email Address:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="unwatermarked" ValidationGroup="Contact" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtEmail" ErrorMessage="Type Email Address"
                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                    ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Type a valid Email address" Font-Size="Small" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Display="Dynamic" ValidationGroup="Contact"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <strong>Facility Information</strong><br />
                <table id="Tabl2" class="sample">
                    <tr>
                        <td width="25%">Facility Name*
                        </td>
                        <td>
                            <asp:Label ID="lblFacilityName" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Facility Location (Street)*
                        </td>
                        <td>
                            <asp:Label ID="lblFacilityStreet" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Facility Location (City)*
                        </td>
                        <td>
                            <asp:Label ID="lblFacilityCity" runat="server" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br>
                <strong>* Is the above Facility Name, Street, or City information correct?</strong>
                <asp:DropDownList ID="ddlFacilityInfoChange" runat="server" AutoPostBack="True">
                    <asp:ListItem>YES</asp:ListItem>
                    <asp:ListItem>NO</asp:ListItem>
                </asp:DropDownList>
                </P>
               
                <asp:Panel ID="pnlfacInfo" runat="server" Visible="False">
                    <p>
                        <strong>Please provide the correct facility information below:</strong>
                        <table id="Table3" class="sample">
                            <tr>
                                <td width="25%">Facility Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfacName" runat="server" CssClass="unwatermarked" MaxLength="90"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic"
                                        ErrorMessage="Enter the Correct Facility Name" ControlToValidate="txtfacName">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Facility Location (Street)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfacStreet" runat="server" CssClass="unwatermarked" MaxLength="90"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" Display="Dynamic"
                                        ErrorMessage="Enter the street address for the facility" ControlToValidate="txtfacStreet">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Facility Location (City)
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfacCity" runat="server" CssClass="unwatermarked" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" Display="Dynamic"
                                        ErrorMessage="Enter the city for the facility" ControlToValidate="txtfacCity">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <br>
                        <font size="2"><strong>Please NOTE that this change will be made upon review and approval
                            by our staff. If you also need to change this information on your permit, you must
                            submit&nbsp;the
                           
                            <asp:HyperLink ID="HyperLink4" runat="server" Target="_blank" NavigateUrl="https://epd.georgia.gov/documents/ssppsip-app-name-change-form">Facility Name Change Form</asp:HyperLink>.</strong></font>
                    </p>
                </asp:Panel>
                <br />
                <br />
                <asp:Button ID="btnUpdateContact" runat="server" Text="Save Fee Contact" OnClick="btnUpdateContact_Click"
                    ValidationGroup="Contact" />
                <asp:Label ID="lblContactMsg" runat="server" ForeColor="#C000C0" Visible="False"></asp:Label>
            </ContentTemplate>
        </act:TabPanel>

        <act:TabPanel runat="server" ID="Calculation" HeaderText="Fee Calculations">
            <ContentTemplate>
                <p>Source Classification options (for further clarification, please see the Fee Manual):</p>
                <ul>
                    <li>A - Major Source</li>
                    <li>SM - Synthetic Minor Source</li>
                    <li>B - Minor Source</li>
                    <li>PR - Permit-by-Rule Source</li>
                </ul>

                <table style="background: #ccffcc; border-collapse: collapse;" cellpadding="5" border="1">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkNonAttainment" runat="server" Font-Bold="True" Enabled="False"
                                Text="1-HOUR OZONE NON-ATTAINMENT"></asp:CheckBox>
                        </td>
                        <td>CLASS:
                           
                            <asp:TextBox ID="txtClass" runat="server" Width="50px" BackColor="#E0E0E0" ForeColor="#400000"
                                MaxLength="2" ReadOnly="True">CLASS:</asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkNSPS" runat="server" Font-Bold="True" Enabled="False" Text="NSPS"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <strong>Source Classification Change?</strong>
                        </td>
                        <td>CLASS:
                           
                            <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="A">A</asp:ListItem>
                                <asp:ListItem Value="SM">SM</asp:ListItem>
                                <asp:ListItem Value="B">B</asp:ListItem>
                                <asp:ListItem Value="PR">PR</asp:ListItem>
                                <asp:ListItem Value="C">C</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkNSPS1" runat="server" Font-Bold="True" Visible="False" Text="NSPS"
                                AutoPostBack="True" OnCheckedChanged="chkNSPS1_CheckedChanged"></asp:CheckBox>
                        </td>
                    </tr>
                </table>

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Calculation"
                    DisplayMode="BulletList" />
                <br />
                <asp:Panel ID="pnlDidNotOperate" runat="server" Visible="false">
                    Check here if this stationary source did not operate at all in calendar year
                   
                    <% =feeyear.Text%>
                    <br>
                    <asp:CheckBox ID="chkDidNotOperate" runat="server" AutoPostBack="True" Font-Bold="True"
                        Text="Did Not Operate" />
                    <asp:Label ID="lblNoOperateReason" runat="server" ForeColor="Red" Visible="False">You must select one option</asp:Label>
                    <asp:RadioButtonList ID="rblNoOperateReason" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="1">Source is permanently shutdown. Please revoke the Air Permit and remove this source from the Permit Fee mailing list.</asp:ListItem>
                        <asp:ListItem Value="2">Source may operate in the future. Please make no changes to the Air Permit or the Permit Fee mailing list. Select this if the source was only temporarily shutdown during all of the calendar year but may operate in the future, or if this source is a newly permitted source but has not yet commenced operation.</asp:ListItem>
                    </asp:RadioButtonList>
                </asp:Panel>
                <asp:Panel ID="pnlEmissions" runat="server" Visible="true">
                    <p>
                        If this is a major Part 70 source, complete the following. (If the total calculated emission fee is less than the 
                        minimum Part 70 Fee defined in the Permit Fee Manual, then the owner or operator must pay the minimum Part 70 Fee.)
                    </p>
                    <table style="border-collapse: collapse;" cellpadding="5">
                        <tr>
                            <td style="border: 1px solid">Calculated Annual Volatile Organic Compounds (VOC) Emissions in Tons</td>
                            <td style="border: 1px solid">
                                <asp:TextBox ID="txtVOCTons" runat="server" Width="75px" MaxLength="4" ValidationGroup="Calculation"
                                    AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtVOCTons" runat="server" Font-Size="Smaller"
                                    ControlToValidate="txtVOCTons" ErrorMessage="Please enter anuual VOC emissions"
                                    Display="Dynamic" ValidationGroup="Calculation">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rnvtxtVOCTons" runat="server" Font-Size="Smaller" ControlToValidate="txtVOCTons"
                                    ErrorMessage="The Annual VOC Emissions cannot be greater than 4000 tons and must be rounded to the nearest whole number"
                                    Display="Dynamic" Type="Integer" MaximumValue="4000" MinimumValue="0" ValidationGroup="Calculation"><= 4000</asp:RangeValidator>
                                <act:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtVOCTons"></act:FilteredTextBoxExtender>
                            </td>
                            <td style="border: 1px solid" align="right">Fee for VOC:
                            </td>
                            <td style="border: 1px solid" align="center">
                                <asp:Label ID="lblVOCFee" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid">Calculated Annual Nitrogen Oxides (NOx) Emissions in Tons
                            </td>
                            <td style="border: 1px solid">
                                <asp:TextBox ID="txtNOxTons" runat="server" Width="75px" MaxLength="4" ValidationGroup="Calculation"
                                    AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtNOxTons" runat="server" Font-Size="Smaller"
                                    ControlToValidate="txtNOxTons" ErrorMessage="Please enter anuual NOx emissions"
                                    Display="Dynamic" ValidationGroup="Calculation">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rnvtxtNOxTons" runat="server" Font-Size="Smaller" ControlToValidate="txtNOxTons"
                                    ErrorMessage="The Annual NOx Emissions cannot be greater than 4000 tons and must be rounded to the nearest whole number"
                                    Display="Dynamic" Type="Integer" MaximumValue="4000" MinimumValue="0" ValidationGroup="Calculation"><= 4000</asp:RangeValidator>
                                <act:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtNOxTons"></act:FilteredTextBoxExtender>
                            </td>
                            <td style="border: 1px solid" align="right">Fee for NOx:
                            </td>
                            <td style="border: 1px solid" align="center">
                                <asp:Label ID="lblNOxFee" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid">Calculated Annual Particulate Matter (PM) Emissions in Tons
                            </td>
                            <td style="border: 1px solid">
                                <asp:TextBox ID="txtPMTons" runat="server" Width="75px" MaxLength="4" ValidationGroup="Calculation"
                                    AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtPMTons" runat="server" Font-Size="Smaller"
                                    ControlToValidate="txtPMTons" ErrorMessage="Please enter anuual PM emissions"
                                    Display="Dynamic" ValidationGroup="Calculation">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rnvtxtPMTons" runat="server" Font-Size="Smaller" ControlToValidate="txtPMTons"
                                    ErrorMessage="The Annual PM Emissions cannot be greater than 4000 tons and must be rounded to the nearest whole number"
                                    Display="Dynamic" Type="Integer" MaximumValue="4000" MinimumValue="0" ValidationGroup="Calculation"><= 4000</asp:RangeValidator>
                                <act:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtPMTons"></act:FilteredTextBoxExtender>
                            </td>
                            <td style="border: 1px solid" align="right">Fee for PM:
                            </td>
                            <td style="border: 1px solid" align="center">
                                <asp:Label ID="lblPMFee" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid">Calculated Annual Sulfur Dioxide (SO2) Emissions in Tons
                            </td>
                            <td style="border: 1px solid">
                                <asp:TextBox ID="txtSO2Tons" runat="server" Width="75px" MaxLength="4" ValidationGroup="Calculation"
                                    AutoPostBack="true" CausesValidation="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxtSO2Tons" runat="server" Font-Size="Smaller"
                                    ControlToValidate="txtSO2Tons" ErrorMessage="Please enter anuual SO2 emissions"
                                    Display="Dynamic" ValidationGroup="Calculation">*</asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rnvtxtSO2Tons" runat="server" Font-Size="Smaller" ControlToValidate="txtSO2Tons"
                                    ErrorMessage="The Annual SO2 Emissions cannot be greater than 4000 tons and must be rounded to the nearest whole number"
                                    Display="Dynamic" Type="Integer" MaximumValue="4000" MinimumValue="0" ValidationGroup="Calculation"><= 4000</asp:RangeValidator>
                                <act:FilteredTextBoxExtender FilterType="Numbers" runat="server" TargetControlID="txtSO2Tons"></act:FilteredTextBoxExtender>
                            </td>
                            <td style="border: 1px solid" align="right">Fee for SO2:
                            </td>
                            <td style="border: 1px solid" align="center">
                                <asp:Label ID="lblSO2Fee" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td style="border: 1px solid" align="right" rowspan="1">
                                <strong>Total Emission Fee:</strong>
                            </td>
                            <td style="border: 1px solid" align="center">
                                <asp:Label ID="lblPart70Fee" runat="server" BackColor="#E0E0E0" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <p>
                    Select whether this source is a Part 70 or Synthetic Minor source.
                    If the source is both Part 70 and Synthetic Minor, check both boxes.
                </p>
                <asp:CheckBoxList ID="chkPart70SM" runat="server" Font-Bold="True">
                    <asp:ListItem Value="Part 70 Fee">Part 70 Source</asp:ListItem>
                    <asp:ListItem Value="Synthetic Minor Fee">Synthetic Minor Source</asp:ListItem>
                </asp:CheckBoxList>
                <p align="right">
                    Part 70/SM Fee:
                       
                        <asp:Label ID="lblpart70SMFee" runat="server" BackColor="#E0E0E0"></asp:Label>
                </p>
                <p>
                    If this stationary source is subject to NSPS and not exempt from the&nbsp;NSPS fee,&nbsp;$1500
                        will be entered for NSPS Fee. However, if the source is NSPS but exempt from the
                        NSPS fee&nbsp;in accordance with section 2.1 of the manual for all NSPS equipment
                        at facility, check the box for NSPS Exempt and select the reasons why it is exempt.
                   
                </p>
                <p align="right">
                    NSPS Fee:                       
                    <asp:Label ID="lblNSPSFee" runat="server" BackColor="#E0E0E0"></asp:Label>
                </p>
                <p>
                    <asp:CheckBox ID="chkNSPSExempt" runat="server" Font-Bold="True" Text="Exempt NSPS"
                        AutoPostBack="True"></asp:CheckBox>&nbsp;                       
                </p>
                <p>
                    <asp:Label ID="lblcblnspsreason" runat="server" ForeColor="red" Font-Size="smaller" />
                </p>
                <asp:CheckBoxList ID="cblNSPSExempt" runat="server" Font-Size="Small" Visible="False"></asp:CheckBoxList>
                <p align="right">
                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate" ValidationGroup="Calculation"></asp:Button>&nbsp;                   
                    <asp:Label ID="lblpart70" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblcalculated" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblsm" runat="server" Visible="False"></asp:Label>
                    Total Emissions Fee Due:
                       
                        <asp:Label ID="lblTotalFee" runat="server" BackColor="#E0E0E0" ForeColor="#000040"
                            Font-Bold="True"></asp:Label><br />
                    <asp:Label ID="lblAdminFeeText" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblAdminFeeAmount" runat="server" BackColor="#E0E0E0" ForeColor="#000040"
                        Font-Bold="True" Visible="false"></asp:Label>
                    <asp:HiddenField ID="hidAdminFee" Value="0" Visible="false" runat="server" />
                </p>
                <p>
                    <asp:Button ID="btnSavePnlFeeCalc" runat="server" ToolTip="Save this section and proceed to Payment Info"
                        Text="Save and Continue" ValidationGroup="Calculation"></asp:Button>
                </p>
            </ContentTemplate>
        </act:TabPanel>

        <act:TabPanel runat="server" ID="SignPay" HeaderText="Sign & Pay">
            <ContentTemplate>
                <asp:Panel ID="pnlSignandPay" runat="server">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="SignPay"
                        DisplayMode="BulletList" />

                    <p>You may select the payment option here. If the fee owed is $10,000 or greater, the fee may be paid in four equal quarterly payments.</p>
                    <p>
                        <strong>Payment is for:</strong>
                        <asp:TextBox ID="txtPayType" runat="server" Visible="False" MaxLength="100" Enabled="False"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPayType" runat="server" ErrorMessage="You must select the payment type"
                            ControlToValidate="rblPaymentType" ValidationGroup="SignPay">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:RadioButtonList ID="rblPaymentType" runat="server" Font-Size="Smaller" ValidationGroup="SignPay">
                            <asp:ListItem Value="Entire Annual Year">Entire Annual Year</asp:ListItem>
                            <asp:ListItem Value="Four Quarterly Payments">Four Quarterly Payments</asp:ListItem>
                        </asp:RadioButtonList>
                    </p>
                    <p>
                        <table id="Table7" cellspacing="1" cellpadding="1" border="0">
                            <tr>
                                <td>Total Emissions Fee Due: &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblPayment" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbladminfeeSign" runat="server" Visible="false"></asp:Label>&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblAdminfeeamtSign" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </p>
                    <p>
                        <font>The undersigned certifies that the permit fees have been calculated in accordance
                            with Georgia Air Quality Control <strong>Rule 391-3-1-.03(9)</strong> and the Division's
                            "Procedures for Calculating Air Permit Application & Annual Permit Fees" and that this form is complete and
                            correct to the best of their knowledge.</font>
                    </p>
                    <p>
                        <table id="Table8" cellspacing="1" cellpadding="1" border="0">
                            <tr>
                                <td align="right" style="width: 338px">
                                    <font size="2">Name of Owner or Authorized Official:</font>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwner" runat="server" Width="314px" MaxLength="100" ValidationGroup="SignPay"
                                        BackColor="#ffff99"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvOwner" runat="server" Display="Dynamic" ErrorMessage="Please enter the name of the authorized official"
                                        ControlToValidate="txtOwner" ValidationGroup="SignPay" InitialValue="">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                                        ErrorMessage="Please enter the name of the authorized official" ControlToValidate="txtOwner"
                                        ValidationGroup="SignPay" InitialValue="N/A">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 338px">
                                    <font size="2">Title:</font>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOwnerTitle" runat="server" Width="314px" MaxLength="50" ValidationGroup="SignPay"
                                        BackColor="#ffff99"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvOwnerTitle" runat="server" Display="Dynamic" ErrorMessage="Please enter the Title of the authorized official"
                                        ControlToValidate="txtOwnerTitle" ValidationGroup="SignPay" InitialValue="">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" Display="Dynamic"
                                        ErrorMessage="Please enter the Title of the authorized official" ControlToValidate="txtOwnerTitle"
                                        ValidationGroup="SignPay" InitialValue="N/A">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 338px">
                                    <font size="2">Date and Time:</font>
                                </td>
                                <td>
                                    <asp:Label ID="lblDate" runat="server" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </p>
                    <p>
                        Comments:<br />
                        <asp:TextBox ID="txtComments" runat="server" Width="90%" MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                    </p>
                    <p>
                        <asp:Button ID="btnSavepnlSign" runat="server" Text="Save and Continue" ValidationGroup="SignPay"></asp:Button>
                        Clicking on the Save and Continue button will take you to the final submission screen.
                    </p>
                </asp:Panel>
                <asp:Panel ID="pnlSubmit" runat="server" Visible="False">
                    <p><strong>Final Submission</strong></p>
                    <p>
                        You are about to make a final submission. Once submitted, you will
                        not be able to make any changes. However, you will be able to view
                        the submitted data for this and past years using the History tab above.
                    </p>
                    <p>
                        <asp:Button ID="btnSubmit" runat="server" Width="130px" CausesValidation="False" CssClass="button-large button-proceed"
                            Font-Bold="True" Text="Final Submit"></asp:Button>
                    </p>
                    <p>
                        <asp:Button ID="btnCancelSubmit" runat="server" Width="130px" CausesValidation="False" Text="Cancel"></asp:Button>
                    </p>
                </asp:Panel>
            </ContentTemplate>
        </act:TabPanel>

        <act:TabPanel runat="server" ID="Reports" HeaderText="History">
            <ContentTemplate>
                <p>
                    Following is annual permit fee information from the current year back to 2004. Past invoices and deposit information are located
                    in the separate
                   
                    <asp:HyperLink ID="hlPermitFees" runat="server" NavigateUrl="~/Fees/">Permit Fees Summary</asp:HyperLink>
                    section.)
               
                </p>

                <asp:GridView ID="grdFeeHistory" runat="server" CssClass="table-simple" Visible="true" AutoGenerateColumns="False"
                    UseAccessibleHeader="true" RowHeaderColumn="NUMFEEYEAR">
                    <Columns>
                        <asp:BoundField DataField="NUMFEEYEAR" HeaderText="Fee Year" />
                        <asp:TemplateField HeaderText="Fee Contact">
                            <ItemTemplate>
                                <%# String.Format("{0} {1}", Eval("STRCONTACTFIRSTNAME"), Eval("STRCONTACTLASTNAME")) %><br />
                                <%# Eval("STRCONTACTCOMPANYNAME") %><br />
                                <%# Eval("STRCONTACTADDRESS") %><br />
                                <%# String.Format("{0}, {1} {2}", Eval("STRCONTACTCITY"), Eval("STRCONTACTSTATE"), Eval("STRCONTACTZIPCODE")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Facility Status">
                            <ItemTemplate>
                                Classification:&nbsp;<b><%# Eval("STRCLASS") %></b><br />
                                Subject&nbsp;to&nbsp;NSPS:&nbsp;<b><%# Eval("STRNSPS") %></b><br />
                                NSPS&nbsp;Exempt:&nbsp;<b><%# Eval("STRNSPSEXEMPT") %></b><br />
                                <br />
                                Payment&nbsp;Type&nbsp;Selected:<br />
                                <b><%# Eval("STRPAYMENTPLAN") %></b>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reported Annual Emissions (tons)">
                            <ItemTemplate>
                                VOC:&nbsp;<b><%# Eval("INTVOCTONS") %></b><br />
                                NO<sub>x</sub>:&nbsp;<b><%# Eval("INTNOXTONS") %></b><br />
                                PM:&nbsp;<b><%# Eval("INTPMTONS") %></b><br />
                                SO<sub>2</sub>:&nbsp;<b><%# Eval("INTSO2TONS") %></b><br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Annual Emissions Fees">
                            <ItemTemplate>
                                Fee&nbsp;Rate:&nbsp;<b><%# String.Format("{0:c}", Eval("NUMPERTONRATE")) %></b><br />
                                Fee&nbsp;for&nbsp;VOC:&nbsp;<b><%# String.Format("{0:c}", NullableDecimalProduct(Eval("INTVOCTONS"), Eval("NUMPERTONRATE"))) %></b><br />
                                Fee&nbsp;for&nbsp;NO<sub>x</sub>:&nbsp;<b><%# String.Format("{0:c}", NullableDecimalProduct(Eval("INTNOXTONS"), Eval("NUMPERTONRATE"))) %></b><br />
                                Fee&nbsp;for&nbsp;PM:&nbsp;<b><%# String.Format("{0:c}", NullableDecimalProduct(Eval("INTPMTONS"), Eval("NUMPERTONRATE"))) %></b><br />
                                Fee&nbsp;for&nbsp;SO<sub>2</sub>:&nbsp;<b><%# String.Format("{0:c}", NullableDecimalProduct(Eval("INTSO2TONS"), Eval("NUMPERTONRATE"))) %></b><br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Fees">
                            <ItemTemplate>
                                Total&nbsp;Part&nbsp;70&nbsp;Fee:&nbsp;<b><%# String.Format("{0:c}", Eval("NUMPART70FEE")) %></b><br />
                                Part&nbsp;70/SM&nbsp;Fee:&nbsp;<b><%# String.Format("{0:c}", Eval("NUMSMFEE")) %></b><br />
                                NSPS&nbsp;Fee:&nbsp;<b><%# String.Format("{0:c}", Eval("NUMNSPSFEE")) %></b><br />
                                Admin&nbsp;Fee:&nbsp;<b><%# String.Format("{0:c}", Eval("NUMADMINFEE")) %></b><br />
                                <br />
                                Total&nbsp;Fee&nbsp;Due:&nbsp;<b><%# String.Format("{0:c}", Eval("NUMTOTALFEE")) %></b>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </ContentTemplate>
        </act:TabPanel>
    </act:TabContainer>
</asp:Content>
