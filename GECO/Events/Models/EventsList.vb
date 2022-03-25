Namespace EpdEvents

    ' Generated using JSON to C# converter: https://json2csharp.com/

    Public Class EventsList
        Public Property pagination As Pagination
        Public ReadOnly Property events As New List(Of [Event])

    End Class

    Public Class Pagination
        Public Property object_count As Integer
        Public Property page_number As Integer
        Public Property page_size As Integer
        Public Property page_count As Integer
        Public Property has_more_items As Boolean
    End Class

    Public Class TextOrHtml
        Public Property text As String
        Public Property html As String
    End Class

    Public Class DateTimeInfo
        Public Property timezone As String
        Public Property local As Date
        Public Property utc As Date

        Public ReadOnly Property display_local_datetime As String
            Get
                Return local.ToFullDateTime()
            End Get
        End Property
    End Class

    Public Class TopLeft
        Public Property x As Integer
        Public Property y As Integer
    End Class

    Public Class CropMask
        Public Property top_left As TopLeft
        Public Property width As Integer
        Public Property height As Integer
    End Class

    Public Class Original
        Public Property url As String
        Public Property width As Integer
        Public Property height As Integer
    End Class

    Public Class Logo
        Public Property crop_mask As CropMask
        Public Property original As Original
        Public Property id As String
        Public Property url As String
        Public Property aspect_ratio As String
        Public Property edge_color As String
        Public Property edge_color_set As Boolean
    End Class

    Public Class TicketPriceInfo
        Public Property currency As String
        Public Property major_value As String
        Public Property value As Integer
        Public Property display As String
    End Class

    Public Class TicketAvailability
        Public Property has_available_tickets As Boolean
        Public Property minimum_ticket_price As TicketPriceInfo
        Public Property maximum_ticket_price As TicketPriceInfo
        Public Property is_sold_out As Boolean
        Public Property start_sales_date As DateTimeInfo
        Public Property waitlist_available As Boolean

        Public ReadOnly Property has_registration_started As Boolean
            Get
                Return start_sales_date.local < Date.Now
            End Get
        End Property
    End Class

    Public Class [Event]
        Public Property name As TextOrHtml
        Public Property description As TextOrHtml
        Public Property url As String
        Public Property start As DateTimeInfo
        Public Property [end] As DateTimeInfo
        Public Property organization_id As String
        Public Property created As Date
        Public Property changed As Date
        Public Property published As Date
        Public Property capacity As Integer
        Public Property capacity_is_custom As Boolean
        Public Property status As String
        Public Property currency As String
        Public Property listed As Boolean
        Public Property shareable As Boolean
        Public Property invite_only As Boolean
        Public Property online_event As Boolean
        Public Property show_remaining As Boolean
        Public Property tx_time_limit As Integer
        Public Property hide_start_date As Boolean
        Public Property hide_end_date As Boolean
        Public Property locale As String
        Public Property is_locked As Boolean
        Public Property privacy_setting As String
        Public Property is_series As Boolean
        Public Property is_series_parent As Boolean
        Public Property inventory_type As String
        Public Property is_reserved_seating As Boolean
        Public Property show_pick_a_seat As Boolean
        Public Property show_seatmap_thumbnail As Boolean
        Public Property show_colors_in_seatmap_thumbnail As Boolean
        Public Property source As String
        Public Property is_free As Boolean
        Public Property version As Object
        Public Property summary As String
        Public Property facebook_event_id As Object
        Public Property logo_id As String
        Public Property organizer_id As String
        Public Property venue_id As Object
        Public Property category_id As String
        Public Property subcategory_id As String
        Public Property format_id As String
        Public Property id As String
        Public Property resource_uri As String
        Public Property is_externally_ticketed As Boolean
        Public Property logo As Logo
        Public Property ticket_availability As TicketAvailability
    End Class

End Namespace
