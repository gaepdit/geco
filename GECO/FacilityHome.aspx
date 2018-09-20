<%@ Page Language="VB" MasterPageFile="~/MainMaster.master" AutoEventWireup="false" Inherits="GECO.FacilityHome" Title="GECO Facility Home" CodeBehind="FacilityHome.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FullContent" runat="Server">
    <h1>Facility Home</h1>
    <h2>
        <asp:Label ID="lblFacilityName" runat="server"></asp:Label></h2>
    <h2>AIRS No.
        <asp:Label ID="lblAIRS" runat="server"></asp:Label></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <act:TabContainer runat="server" ID="FacilityTabs" AutoPostBack="true" ActiveTabIndex="0">

                <act:TabPanel runat="server" ID="AppStatus" HeaderText="GECO Applications">
                    <ContentTemplate>
                        <asp:Table ID="AppTable" runat="server" CssClass="table-simple table-menu table-bordered">
                            <asp:TableHeaderRow ID="AppsHeader" runat="server" BackColor="#F0F0F6" CssClass="table-head">
                                <asp:TableHeaderCell Font-Bold="True" Text="Application" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="True" Text="Current Status" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="True" Text="Deadline" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell Font-Bold="True" Text="Current Contact<br>(click to edit)" runat="server"></asp:TableHeaderCell>
                            </asp:TableHeaderRow>

                            <asp:TableRow ID="AppsEmissionFees" runat="server">
                                <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                                    <asp:HyperLink ID="EFLink" runat="server" Text="Emissions Fees" NavigateUrl="~/Fees/Default.aspx"></asp:HyperLink>
                                </asp:TableHeaderCell>
                                <asp:TableCell runat="server">
                                    <asp:Literal ID="litEFText" runat="server"></asp:Literal>
                                </asp:TableCell>
                                <asp:TableCell runat="server">
                                    <asp:Label ID="lblEFDate" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell runat="server" CssClass="table-cell-link">
                                    <asp:LinkButton ID="lbtnEFContact" runat="server" CausesValidation="False"></asp:LinkButton>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow ID="AppsEmissionInventory" runat="server">
                                <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                                    <asp:HyperLink ID="EisLink" runat="server" Text="Emissions Inventory" NavigateUrl="~/EIS/Default.aspx"></asp:HyperLink>
                                </asp:TableHeaderCell>
                                <asp:TableCell runat="server">
                                    <asp:Label ID="lblEIText" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell runat="server">
                                    <asp:Label ID="lblEIDate" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell runat="server" CssClass="table-cell-link">
                                    <asp:LinkButton ID="lbtnEIContact" runat="server" CausesValidation="False"></asp:LinkButton>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow ID="AppsEmissionsStatement" runat="server">
                                <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                                    <asp:HyperLink ID="ESLink" runat="server" Text="Emissions Statement" NavigateUrl="~/ES/Default.aspx"></asp:HyperLink>
                                </asp:TableHeaderCell>
                                <asp:TableCell runat="server">
                                    <asp:Label ID="lblESText" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell runat="server">
                                    <asp:Label ID="lblESDate" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell runat="server" CssClass="table-cell-link">
                                    <asp:LinkButton ID="lbtnESContact" runat="server" CausesValidation="False"></asp:LinkButton>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow ID="AppsTestNotifications" runat="server">
                                <asp:TableHeaderCell runat="server" CssClass="table-cell-link">
                                    <asp:HyperLink ID="TNLink" runat="server" Text="Test Notifications" NavigateUrl="~/TN/Default.aspx"></asp:HyperLink>
                                </asp:TableHeaderCell>
                                <asp:TableCell runat="server">
                                    <asp:Label ID="TNText" runat="server"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell runat="server">
                                    <asp:Label ID="TNDate" runat="server" Text="N/A"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell runat="server" CssClass="table-cell-link">
                                    <asp:LinkButton ID="TNContact" runat="server" CausesValidation="False"></asp:LinkButton>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>

                        <asp:Panel ID="pnlContact" runat="server" Visible="False">
                            <asp:Label ID="lblContactHeader" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                            <asp:HiddenField ID="hidContactKey" runat="server" Visible="False" />
                            <asp:RadioButtonList ID="rblContact" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                            </asp:RadioButtonList>

                            <table>
                                <tr>
                                    <td width="25%">First Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFName"
                                            ErrorMessage="Type First Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Last Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName"
                                            ErrorMessage="Type Last Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Title:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTitle"
                                            ErrorMessage="Type Title" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Company Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCoName" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtCoName"
                                            ErrorMessage="Type Company Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Street Address:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtAddress"
                                            ErrorMessage="Type Street Address" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>City:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtCity"
                                            ErrorMessage="Type City Name" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
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
                                <tr>
                                    <td>Zip Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZip" runat="server" CssClass="unwatermarked" MaxLength="5" ValidationGroup="Contact" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtzip"
                                            ErrorMessage="Type 5-digit Zip Code" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtZip"
                                            FilterType="Numbers" Enabled="True">
                                        </act:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Telephone Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="unwatermarked" MaxLength="10"
                                            ValidationGroup="Contact"></asp:TextBox>
                                        &nbsp; Ext:
                                        <asp:TextBox ID="txtPhoneExt" runat="server" CssClass="unwatermarked" MaxLength="5"
                                            Width="64px" ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPhone" ErrorMessage="10-digit Phone Number"
                                                Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPhone"
                                            FilterType="Numbers" Enabled="True">
                                        </act:FilteredTextBoxExtender>
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPhoneExt"
                                            FilterType="Numbers" Enabled="True">
                                        </act:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Fax Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFax" runat="server" CssClass="unwatermarked" MaxLength="10" ValidationGroup="Contact"></asp:TextBox>
                                        <act:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtFax"
                                            FilterType="Numbers" Enabled="True">
                                        </act:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Email Address:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailContact" runat="server" CssClass="unwatermarked" ValidationGroup="Contact"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtEmailContact"
                                            ErrorMessage="Type Email Address" Font-Size="Small" ValidationGroup="Contact"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailContact"
                                                ErrorMessage="Type a valid Email address" Font-Size="Small" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                Display="Dynamic" ValidationGroup="Contact"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>

                            <asp:Button ID="btnUpdateContact" runat="server" Text="Save Contact" ValidationGroup="Contact" />
                            &nbsp; &nbsp;
                            <asp:Button ID="btnCloseContact" runat="server" Text="Close" ValidationGroup="Contact" />
                            <asp:Label ID="lblContactMsg" runat="server" ForeColor="#C000C0" Visible="False"></asp:Label>
                        </asp:Panel>
                    </ContentTemplate>
                </act:TabPanel>

                <act:TabPanel runat="Server" ID="FacilitySummary" HeaderText="Facility Summary">
                    <ContentTemplate>

                        <h3>Facility Location</h3>

                        <table class="table-simple table-list">
                            <tbody>
                                <tr>
                                    <th scope="row">Address:</th>
                                    <td>
                                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <td>
                                        <asp:Label ID="lblCityStateZip" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">County:</th>
                                    <td>
                                        <asp:Label ID="lblCounty" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">District:</th>
                                    <td>
                                        <asp:Label ID="lblDistrict" runat="server"></asp:Label>
                                        &nbsp; &nbsp;
                                                <asp:HyperLink ID="hlDistrict" runat="server" Target="_blank" Text="District Responsible Source"
                                                    NavigateUrl="https://epd.georgia.gov/district-office-locations" />
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Office:</th>
                                    <td>
                                        <asp:Label ID="lblOffice" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Longitude:</th>
                                    <td>
                                        <asp:Label ID="lblLongitude" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Latitude:</th>
                                    <td>
                                        <asp:Label ID="lblLatitude" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <p>
                            <a href="FacilityMap.aspx?name=<% =lblFacilityName.Text %>&address=<% =lblAddress.Text %>&city=<% =lblCityStateZip.Text %>&lat=<% =lblLatitude.Text %>&lon=<% =lblLongitude.Text %>"
                                target="_blank">Open map</a>
                        </p>

                        <h3>Facility Status</h3>

                        <table class="table-simple table-list">
                            <tbody>
                                <tr>
                                    <th scope="row">Classification:</th>
                                    <td>
                                        <asp:Label ID="lblClassification" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Operating Status:</th>
                                    <td>
                                        <asp:Label ID="lblOpStatus" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">SIC Code:</th>
                                    <td>
                                        <asp:Label ID="lblSICCode" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Startup Date:</th>
                                    <td>
                                        <asp:Label ID="lblStartUp" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Date Closed:</th>
                                    <td>
                                        <asp:Label ID="lblClosed" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">CMS Status:</th>
                                    <td>
                                        <asp:Label ID="lblCMSStatus" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Air Program Codes:</th>
                                    <td>
                                        <asp:Label ID="lblAirProgramCodes" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <h3>State Contacts</h3>

                        <table class="table-simple">
                            <thead>
                                <tr>
                                    <th scope="col">Program</th>
                                    <th scope="col">Contact Name</th>
                                    <th scope="col">Contact Phone</th>
                                    <th scope="col">Contact Email</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <th scope="row">Permitting</th>
                                    <td>
                                        <asp:Label ID="lblPermitContactName" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPermitContactPhone" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hlPermitContactEmail" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Compliance</th>
                                    <td>
                                        <asp:Label ID="lblComplianceContactName" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblComplianceContactPhone" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hlComplianceContactEmail" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row">Monitoring</th>
                                    <td>
                                        <asp:Label ID="lblMonitoringContactName" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMonitoringContactPhone" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hlMonitoringContactEmail" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </act:TabPanel>

                <act:TabPanel runat="server" ID="AirProgram" HeaderText="Air Programs">
                    <ContentTemplate>
                        <act:TabContainer runat="server" ID="AirProgramDetails" ActiveTabIndex="0">
                            <act:TabPanel runat="server" ID="Permit" HeaderText="Permit Applications">
                                <ContentTemplate>
                                    <asp:Label ID="lblGridView1" runat="server" Text="There are no Applications for this facility at this time." Visible="false"></asp:Label>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                        CssClass="table-simple" OnRowDataBound="GridView1_RowDataBound" PageSize="20" AllowPaging="true"
                                        OnSorting="GridView1_Sorting" OnPageIndexChanging="GridView1_PageIndexChanging"
                                        EmptyDataText="NO Applications Found" DataKeyNames="strApplicationNumber">
                                        <AlternatingRowStyle CssClass="alternatingrowstyle" />
                                        <HeaderStyle CssClass="headerstyle" />
                                        <PagerSettings Position="TopAndBottom" />
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Appl. No." SortExpression="strApplicationNumber">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlAppDetails" runat="server" Target="_blank" Text='<%# Eval("strApplicationNumber") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="strFacilityName" HeaderText="Facility Name" SortExpression="strFacilityName" />
                                            <asp:TemplateField HeaderText="Permit Number" SortExpression="strPermitNumber">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlFinalPermit" runat="server" Target="_blank" Text='<%# Eval("strPermitNumber") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="strApplicationType" HeaderText="Appl. Type" SortExpression="strApplicationType" />
                                            <asp:BoundField DataField="AppStatus" HeaderText="Appl. Status" SortExpression="AppStatus" />
                                            <asp:BoundField DataField="StatusDate" HeaderText="Last Action" SortExpression="StatusDate" />
                                            <asp:TemplateField HeaderText="Staff Responsible" SortExpression="StaffResponsible">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("StaffResponsible") %>'
                                                        NavigateUrl='<%# (If(String.IsNullOrWhiteSpace(Eval("strUserEmail").ToString), Nothing, Eval("strUserEmail", "mailto:{0}"))) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </act:TabPanel>

                            <act:TabPanel ID="FeeHistory" runat="server" HeaderText="Emissions Fees Data">
                                <ContentTemplate>
                                    <act:TabContainer runat="server" ID="FeesData" ActiveTabIndex="0">
                                        <act:TabPanel ID="Deposits" runat="server" HeaderText="Deposits">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdDeposits" DataKeyNames="STRAIRSNUMBER" OnRowDataBound="GridView_RowDataBound"
                                                    AutoGenerateColumns="false" runat="Server" AllowSorting="true" CssClass="table-simple"
                                                    PageSize="20" AllowPaging="true" OnSorting="grdDeposits_Sorting" OnPageIndexChanging="grdDeposits_PageIndexChanging">
                                                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerSettings Position="TopAndBottom" />
                                                    <PagerStyle HorizontalAlign="Center" />
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="intYear" HeaderText="Fee Year" SortExpression="intYear" />
                                                        <asp:BoundField DataField="numpayment" HeaderText="Amount Deposited" HtmlEncode="false"
                                                            DataFormatString="{0:C}" SortExpression="numpayment" />
                                                        <asp:BoundField DataField="strcheckno" HeaderText="Check No." SortExpression="strcheckno" />
                                                        <asp:BoundField DataField="datpaydate" HeaderText="Date Deposited" HtmlEncode="false"
                                                            DataFormatString="{0:d}" SortExpression="datpaydate" />
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </act:TabPanel>

                                        <act:TabPanel ID="Invoices" runat="server" HeaderText="Invoices">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdInvoices" DataKeyNames="STRAIRSNUMBER" OnRowDataBound="GridView_RowDataBound"
                                                    AutoGenerateColumns="false" runat="Server" AllowSorting="true" CssClass="table-simple"
                                                    PageSize="20" AllowPaging="true" OnSorting="grdInvoices_Sorting" OnPageIndexChanging="grdInvoices_PageIndexChanging">
                                                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerSettings Position="TopAndBottom" />
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="intYear" HeaderText="Fee Year" SortExpression="intYear" />
                                                        <asp:BoundField DataField="strInvoiceNumber" HeaderText="Invoice Number" SortExpression="strInvoiceNumber" />
                                                        <asp:BoundField DataField="numTotalfee" HeaderText="Invoice Amount" HtmlEncode="false"
                                                            DataFormatString="{0:C}" SortExpression="numTotalfee" />
                                                        <asp:BoundField DataField="datesubmit" HeaderText="Date Submitted" HtmlEncode="false"
                                                            DataFormatString="{0:d}" SortExpression="datesubmit" />
                                                        <asp:BoundField DataField="strpaymentType" HeaderText="Payment Type" SortExpression="strpaymentType" />
                                                        <asp:BoundField DataField="strofficialname" HeaderText="Submitted by" SortExpression="strofficialname" />
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </act:TabPanel>
                                    </act:TabContainer>
                                </ContentTemplate>
                            </act:TabPanel>
                        </act:TabContainer>
                    </ContentTemplate>
                </act:TabPanel>

                <act:TabPanel runat="Server" ID="AdminTools" HeaderText="Admin/User Tools">
                    <ContentTemplate>
                        <p>
                            Users with access rights to this facility in GECO:
                        </p>
                        <asp:GridView ID="grdUsers" DataKeyNames="NUMUSERID,STRAIRSNUMBER" CssClass="table-simple"
                            AutoGenerateColumns="false" AutoGenerateEditButton="true" AutoGenerateDeleteButton="false"
                            runat="Server" CellPadding="5">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete"
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                            Text="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NUMUSERID" Visible="False" />
                                <asp:BoundField DataField="STRAIRSNUMBER" Visible="False" />
                                <asp:BoundField DataField="strUserEmail" HeaderText="User ID" ReadOnly="True" />
                                <asp:CheckBoxField HeaderText="Admin Rights" DataField="intAdminAccess">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField HeaderText="Emissions Fees" DataField="intFeeAccess">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField HeaderText="Emissions Inventory System" DataField="intEIAccess">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField HeaderText="Emissions Statement" DataField="intESAccess">
                                    <ItemStyle HorizontalAlign="Center" CssClass="table-cell-checkbox" />
                                </asp:CheckBoxField>
                            </Columns>
                            <EditRowStyle BackColor="#C0FFC0" BorderColor="Green" BorderStyle="Ridge" />
                            <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#008A8C" BorderColor="Red" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle CssClass="table-head" />
                            <AlternatingRowStyle BackColor="#f3f3f7" />
                        </asp:GridView>
                        <br />
                        <asp:Panel runat="server" ID="pnlAddNewUser">
                            User Email:
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            <asp:Button ID="btnAddUser" runat="server" BackColor="Control" Text="Add New User" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Valid email address is required." Font-Size="Small"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#C00000" Visible="False">
                                The user you are trying to add does not have a GECO account.
                            </asp:Label>
                        </asp:Panel>
                    </ContentTemplate>
                </act:TabPanel>
            </act:TabContainer>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter">
                    </div>
                    <div id="progressMessage">
                        Please Wait...
                        <br />
                        <img alt="Loading" src="<%= Page.ResolveUrl("~/assets/images/progressbar_green.gif") %>" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
