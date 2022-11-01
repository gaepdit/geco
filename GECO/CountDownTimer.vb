Imports System.Timers

Public Class CountDownTimer
    Implements IDisposable

    Public _stopWatch As New Stopwatch()

    Public TimeChanged As Action
    Public CountDownFinished As Action

    Public ReadOnly Property IsRunning() As Boolean
        Get
            Return timer.Enabled
        End Get
    End Property

    Public Property StepMs() As Double
        Get
            Return timer.Interval
        End Get
        Set
            timer.Interval = Value
        End Set
    End Property

    Private ReadOnly timer As New Timer()

    Private _max As TimeSpan = TimeSpan.FromMilliseconds(30000)

    Public ReadOnly Property TimeLeft() As TimeSpan
        Get
            Return If((
                _max.TotalMilliseconds - _stopWatch.ElapsedMilliseconds) > 0,
                      TimeSpan.FromMilliseconds(_max.TotalMilliseconds - _stopWatch.ElapsedMilliseconds),
                      TimeSpan.FromMilliseconds(0))
        End Get
    End Property

    Private ReadOnly Property _mustStop() As Boolean
        Get
            Return (_max.TotalMilliseconds - _stopWatch.ElapsedMilliseconds) < 0
        End Get
    End Property

    Public ReadOnly Property TimeLeftInSeconds() As String
        Get
            Return TimeLeft.ToString("ss")
        End Get
    End Property

    Public ReadOnly Property TimeLeftMinutesSecondsMilliSeconds() As String
        Get
            Return TimeLeft.ToString("mm\:ss\.fff")
        End Get
    End Property

    Private Sub TimerTick(sender As Object, e As EventArgs)

        TimeChanged?.Invoke()
        If _mustStop Then
            CountDownFinished?.Invoke()
            _stopWatch.Stop()
            timer.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' Setup with minutes and seconds
    ''' </summary>
    ''' <param name="minutes"></param>
    ''' <param name="seconds"></param>
    Public Sub New(minutes As Integer, seconds As Integer)
        SetTime(minutes, seconds)
        Initialize()
    End Sub
    ''' <summary>
    ''' Setup with TimeSpan
    ''' </summary>
    ''' <param name="ts"><see cref="TimeSpan"/></param>
    Public Sub New(ts As TimeSpan)
        SetTime(ts)
        Initialize()
    End Sub

    Public Sub New()
        Initialize()
    End Sub

    Private Sub Initialize()
        StepMs = 1000
        AddHandler timer.Elapsed, AddressOf TimerTick
    End Sub

    Public Sub SetTime(ts As TimeSpan)
        _max = ts
        TimeChanged?.Invoke()
    End Sub

    Public Sub SetTime(minutes As Integer, Optional seconds As Integer = 0)
        SetTime(TimeSpan.FromSeconds(minutes * 60 + seconds))
    End Sub

    Public Sub Start()
        timer.Start()
        _stopWatch.Start()
    End Sub

    Public Sub Pause()
        timer.Stop()
        _stopWatch.Stop()
    End Sub

    Public Sub [Stop]()
        Reset()
        Pause()
    End Sub

    Public Sub Reset()
        _stopWatch.Reset()
    End Sub

    Public Sub Restart()
        _stopWatch.Reset()
        timer.Start()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        timer.Dispose()
    End Sub
End Class
