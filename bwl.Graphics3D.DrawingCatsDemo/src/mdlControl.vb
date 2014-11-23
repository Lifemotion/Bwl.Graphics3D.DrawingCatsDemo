Imports System.Threading
''' <summary>
''' Основной модуль.
''' </summary>
''' <remarks></remarks>
Public Module mdlControl
    Public Const globalTickPeriod As Integer = 20
    'массив нажатых кнопок
    Public Pressed(256) As Boolean
    'таймер, управляющий всеми времязависимыми действиями
    Public ActionTimer As New Timers.Timer
    Private thread As Thread
    Public running As Boolean
    Public info As String
    Private fps As Integer
    Public musicEnded As Boolean
    Public mainMode As Integer
    Public waveout As New WaveOut

    ''' <summary>
    ''' Процедура запуска.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Main()
        frmSplash.Show()
        frmMain.Show()
        frmMain.Hide()
        Application.DoEvents()
        InitTimer()
        InitMusic()
        mdlRoadScene.Init()
        mdlMainScene.Init()
        frmSplash.Hide()
        frmMain.Show()
        frmMain.Show()
        Application.DoEvents()
        StartMusic()
        mdlMainScene.SetTargetMain()
        mdlMainScene.SetTargetTV()
        mainMode = 0
        MainCycle()
    End Sub
    Private Sub InitTimer()
        AddHandler ActionTimer.Elapsed, AddressOf ActionTimerEvent
        ActionTimer.Interval = globalTickPeriod
        ActionTimer.Enabled = True
    End Sub
    ''' <summary>
    ''' Вызывает все обработчики клавиш, перемещения объектов и т.п.
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ActionTimerEvent(ByVal source As Object, ByVal e As Timers.ElapsedEventArgs)
        Static working As Boolean
        If running = True Then
            '  If Not working Then
            working = True
            AllTimedEvents()
            working = False
            'End If
            'Dim thread As New Threading.Thread(AddressOf AllTimedEvents)
            'thread.Priority = Threading.ThreadPriority.Highest
            'thread.Start()
            'thread.Join()
        End If
    End Sub
    Public Sub RerfeshInfo()
        frmMain.lbInfo.Text = "КВС:" + fps.ToString + info
        fps = 0
    End Sub
    Sub AllTimedEvents()
        mdlMainScene.TimedEvents()
        mdlRoadScene.TimedEvents()
    End Sub
    Public Sub MainCycle()
        running = True
        '  thread = New Thread(AddressOf mdlMainScene.Draw)
        '  thread.Priority = ThreadPriority.Lowest
        Do While running
            'проверяем код выхода
            If Pressed(Keys.Escape) Then running = False
            'выполняем отрисовочки и прочее
            If mainMode = 0 Then
                mdlRoadScene.Draw()
            Else
                mdlMainScene.Draw()
            End If
            thread = New Thread(AddressOf mdlMainScene.Draw)
            thread.Priority = ThreadPriority.Lowest
            thread.Start()
            thread.Join()
            fps += 1
            Application.DoEvents()
        Loop
    End Sub
    Private Sub InitMusic()

    End Sub
End Module
