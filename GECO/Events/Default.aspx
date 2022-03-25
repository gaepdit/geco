<%@ Page Title="Georgia EPD Upcoming Events" Language="VB" MasterPageFile="~/Main.master" Async="true"
    AutoEventWireup="false" Inherits="GECO.EpdEvents.Events_Default" CodeBehind="Default.aspx.vb" %>

<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    <h1>Upcoming classes and workshops offered by Georgia EPD</h1>

    <div class="announcement announcement-wide">
        <p>
            <strong>Notice:</strong>
            Event registration is now handled through Eventbrite. A GECO account is no longer needed to register for EPD Events.
        </p>

        <p>
            All events can also be viewed at our
            <a class="no-visited" href="https://www.eventbrite.com/o/georgia-environmental-protection-division-44099137123">Eventbrite page</a>.
        </p>
    </div>

    <asp:ListView ID="lvEventList" runat="server">
        <EmptyDataTemplate>
            <p class="message-update">There are no upcoming events.</p>
        </EmptyDataTemplate>

        <ItemTemplate>
            <div class="panel panel-card">
                <h2><a class="no-visited" href="<%# Eval("url") %>"><%# Eval("name.text") %></a></h2>
                <p>📅 <%# Eval("start.display_local_datetime") %></p>

                <blockquote>
                    <p><%# Eval("description.text") %></p>

                    <p runat="server" visible='<%# Eval("online_event") %>'>
                        <em>This is an online event. Information on joining the meeting will be shared with you before the event.</em>
                    </p>
                </blockquote>

                <p runat="server" class="message-update"
                    visible='<%# Eval("ticket_availability.has_registration_started") AndAlso Eval("ticket_availability.is_sold_out") %>'>
                    Sold out.
                </p>

                <p runat="server"
                    visible='<%# Eval("ticket_availability.has_registration_started") AndAlso Not Eval("ticket_availability.is_sold_out") %>'>
                    <a href="<%# Eval("url") %>" class="button button-large">Register</a>
                </p>

                <p runat="server" class="message-update" visible='<%# Not Eval("ticket_availability.has_registration_started") %>'>
                    Registration opens on <%# Eval("ticket_availability.start_sales_date.display_local_datetime") %>
                </p>
            </div>
        </ItemTemplate>
    </asp:ListView>

</asp:Content>
