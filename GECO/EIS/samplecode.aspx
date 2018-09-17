<%@ Page Title="" Language="VB" MasterPageFile="eismaster.master" AutoEventWireup="false" Inherits="GECO.eis_template" Codebehind="samplecode.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <%-- Page header --%>
    <div class="pageheader">
        Title for Page
    </div>

    <%-- Page subheader --%>
    <div class="pagesubheader">
        Page Title Subheader
    </div>
    <br />

    <%-- <<<<<< BEGIN SEPARATORS >>>>>> --%>

    <%-- Separator WITHOUT Add/Edit button and text not visible--%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorOnly" class="styledseparator" runat="server" Text="_" Font-Size="Small" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
    </div>

    <%-- Separator with text but no Add or Edit button --%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparator" class="styledseparator" runat="server" Text="lblSeparatorWithText"></asp:Label>
    </div>

    <%-- Separator with text plus Add and Edit buttons --%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label2" class="styledseparator" runat="server" Text="lblSeparatorTextWithAddandEditButtons"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAdd1" runat="server" Text="Add" ToolTip="" Font-Size="Small" />
            &nbsp;<asp:Button ID="btnEdit1" runat="server" Text="Edit" Font-Size="Small" />
        </div>
    </div>

    <%-- Separator with Add button only --%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label4" class="styledseparator" runat="server" Text="lblSeparatorTextWithAddButtonOnly"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAdd2" runat="server" Text="Add" ToolTip="" Font-Size="Small" /></div>
    </div>

    <%-- Separator with Edit button only --%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label5" class="styledseparator" runat="server" Text="lblSeparatorTextWithEditButtonOnly"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnEdit2" runat="server" Text="Edit" ToolTip="" Font-Size="Small" /></div>
    </div>

    <%-- Separator with Add/Edit button and invisible header text--%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorNoText" class="styledseparator" runat="server" Text="lblSeparatorwithAddandEditNoText" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="Button1" runat="server" Text="Add" ToolTip="" Font-Size="Small" />
            &nbsp;<asp:Button ID="Button2" runat="server" Text="Edit" ToolTip="" Font-Size="Small" />
        </div>
    </div>

    <%-- Separator with Add button only and invisible header text--%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label3" class="styledseparator" runat="server" Text="lblSeparatorwithAddNoText" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="Button3" runat="server" Text="Add" ToolTip="" Font-Size="Small" /></div>
    </div>

    <%-- Separator with Edit button and invisible header text--%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="Label6" class="styledseparator" runat="server" Text="lblSeparatorwithEditNoText" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="Button6" runat="server" Text="Edit" ToolTip="" Font-Size="Small" /></div>
    </div>

    <%-- Separator with Save button and header text--%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSave1" class="styledseparator" runat="server" Text="lblSeparatorwithSaveandText"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnSave1" runat="server" Text="Save" ToolTip="" Font-Size="Small" /></div>
    </div>

    <%-- Separator with Save button and invisible header text--%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSave2" class="styledseparator" runat="server" Text="lblSeparatorwithSaveandNoText" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnSave2" runat="server" Text="Save" ToolTip="" Font-Size="Small" /></div>
    </div>

    <%-- Separator with Save and Add buttons and invisible header text--%>
    <div class="fieldwrapperseparator">
        <asp:Label ID="lblSeparatorAddSave" class="styledseparator" runat="server" Text="lblSeparatorwithAddandSaveandNoText" BackColor="#ffffff" ForeColor="#ffffff"></asp:Label>
        <div class="sepbuttons">
            <asp:Button ID="btnAdd" runat="server" Text="Add" ToolTip="" Font-Size="Small" />&nbsp;
        <asp:Button ID="btnSave3" runat="server" Text="Save" ToolTip="" Font-Size="Small" />
        </div>
    </div>

    <%-- <<<<<< END OF SEPARATORS >>>>>> --%>

    <%-- <<<<<< BEGIN ONE LINE FIELDS AND DROPDOWN LIST >>>>>> --%>

    <%-- Label and textbox for editable text boxes --%>
    <div class="fieldwrapper">
        <asp:Label ID="lblID1" class="styled" runat="server" Text="lblEditableText"></asp:Label>
        <asp:TextBox ID="txtID_editable" runat="server" class="editable" Text=""
            Width="300px"></asp:TextBox>
    </div>

    <%-- Label and textbox for ReadOnly text boxes --%>
    <div class="fieldwrapper">
        <asp:Label ID="lblID2" class="styled" runat="server" Text="lblReadOnlyText"></asp:Label>
        <asp:TextBox ID="txtID_readonly" class="readonly" runat="server" Text="Delete this text"
            ReadOnly="True" Width="300px"></asp:TextBox>
    </div>

    <%-- Label and editable textbox with note below textbox; ReadOnly textboxes do not need warning notes --%>
    <div class="fieldwrapper">
        <asp:Label ID="lblID" CssClass="styled" runat="server" Text="lblforTextWithNote"></asp:Label>
        <div class="editable">
            <asp:TextBox ID="txtID_editablewithnote" runat="server" Text="EditableText" Width="300px"></asp:TextBox><br />
            <span style="font-size: 70%; font-family: Verdana; color: Red;">*Note or warning goes
                here!</span>
        </div>
    </div>

    <%-- Label and dropdown list; ReadOnly pages do not use dropdown lists - replace with readonly textbox on readonly pages --%>
    <div class="fieldwrapper">
        <asp:Label ID="lblddlID" CssClass="styled" runat="server" Text="lblForDropdownList"></asp:Label>
        <asp:DropDownList ID="ddlID" runat="server" class="">
        </asp:DropDownList>
    </div>

    <%-- Centered   --%>
    <%-- END ONE LINE FIELDS AND DROPDOWN LIST --%>

    <%-- Label and Editable Comment field --%>
    <div class="fieldwrapper">
        <asp:Label ID="lblCommentID" runat="server" CssClass="styled" Text="lblForEditableComment"></asp:Label>
        <asp:TextBox ID="txtCommentID" runat="server" class="editable" TextMode="MultiLine" Rows="4" 
            Text="Editable comment goes here" Width="400px"></asp:TextBox>
    </div>

    <%-- Label and ReadOnly Comment field --%>
    <div class="fieldwrapper">
        <asp:Label ID="Label1" runat="server" CssClass="styled" Text="lblForReadOnlyComment"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True" class="readonly" TextMode="MultiLine" Rows="4" 
            Text="ReadOnly comment goes here" Width="400px"></asp:TextBox>
    </div>

    <%-- Regular GridView --%>
    <div class="gridview">
        Gridview (normal size - one to about 6 columns) is below here, but is empty if
                viewed in browser so does not show (go to source view)
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSourceID1"
            CellPadding="4" Font-Names="Arial"
            Font-Size="Small" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="emissionsunitid" HeaderText="Column Header 1"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="strunitdescription" HeaderText="Column Header 2"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
                <asp:BoundField DataField="strunittypecode" HeaderText="Column Header 3"
                    NullDisplayText="No Data">
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:BoundField>
            </Columns>
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSourceID1" runat="server"></asp:SqlDataSource>
    </div>

    <%-- Wide GridView --%>
    <%--         <div class="gridview_wide">
                Wide
                Gridviews (over 6 columns or so) is below here, but is empty if
                viewed in borwser so does not show (go to source view)
        <asp:GridView ID="grv" runat="server" CellPadding="4" Font-Names="Verdana"
                    Font-Size="Small" ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSourceID2" runat="server" ConnectionString="Data Source=DEV; Persist Security Info=True; User ID=airbranch; Password=smogalert;Unicode=True"
                            ProviderName="System.Data.OracleClient">
                        </asp:SqlDataSource>
            </div>
    --%>

    <%-- Button area --%>
    <div class="buttonwrapper">
        <asp:Button runat="server" ID="btnID1" CssClass="buttondiv" Text="btnText" />
        <asp:Button runat="server" ID="btnID2" CssClass="buttondiv" Text="btnText" />
        <asp:Button runat="server" ID="btnID3" CssClass="buttondiv" Text="btnText" />
        <asp:Button runat="server" ID="btnID4" CssClass="buttondiv" Text="btnText" />
    </div>
</asp:Content>