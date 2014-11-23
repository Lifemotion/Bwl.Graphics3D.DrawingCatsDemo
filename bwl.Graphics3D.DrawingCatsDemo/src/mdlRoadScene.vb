Module mdlRoadScene
    Public roadScene As New Scene
    Public rain As New WeatherGen
    Private lampObj As Object3D
    Private lampLightObj As Object3D
    Private lampX, lampZ As Single
    Private Const moveSpeed As Integer = 2
    Private Const rotateSpeed As Single = 0.02
    Private aboutTV As Boolean
    Private tvMessageShown As Boolean
    Private isMainScene As Boolean
    Private tvTransition As Boolean
    Private tvShown As Boolean
    Public Sub Init()
        'настраиваем рендер
        InitScene()
        AddHandler mainScene.ScriptCommand, AddressOf HandleScriptEvents
        roadScene.ExecScript(Application.StartupPath + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "roadscene" + IO.Path.DirectorySeparatorChar + "loadscript.txt")
        LoadRain()
        lampObj = roadScene.FindObject("objLamp")
        lampLightObj = roadScene.FindObject("lamp")
        lampX = lampLightObj.positionX
        lampZ = lampLightObj.positionZ
        roadScene.ExecScript(Application.StartupPath + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "roadscene" + IO.Path.DirectorySeparatorChar + "startscript.txt")
    End Sub


    Private Sub HandleScriptEvents(ByVal line As String)
        If line = "STOPSOUND" Then
            waveout.Stop()
        End If
    End Sub
    Sub SetTargetMain()

    End Sub
    Private Sub InitScene()
        'настраиваем рендер
        roadScene.render = New Render3D
        With roadScene.render.presentSettings
            .TargetType = OutputTargetTypeEnum.graphics
            .TargetGraphics = frmMain.pbGraphics
            .WindowWidth = 640
            .WindowHeight = 480
        End With
        roadScene.camera = New Camera3D
        roadScene.Init()
    End Sub
    ''' <summary>
    ''' Процедура, вызывающая разные таймерные действия.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub TimedEvents()
        If mainMode = 0 And musicEnded Then
            musicEnded = False
            CycleMusic()
        End If
        If mainMode = 0 And Not tvShown Then
            ControlTV()
        End If
        CheckKeys()
        roadScene.Tick()
        rain.Move()
        MoveLamp()
    End Sub
    Sub CheckKeys()
        Dim moveFront As Single = 0
        Dim moveStrafe As Single = 0
        Static isInDebug As Boolean

        If Pressed(Keys.W) Then moveFront = moveSpeed
        If Pressed(Keys.S) Then moveFront = -moveSpeed
        If Pressed(Keys.A) Then moveStrafe = -moveSpeed
        If Pressed(Keys.D) Then moveStrafe = moveSpeed
        If Pressed(Keys.Left) Then roadScene.camera.Yaw += rotateSpeed
        If Pressed(Keys.Right) Then roadScene.camera.Yaw -= rotateSpeed
        If Pressed(Keys.X) Then
            Pressed(Keys.X) = False
            isInDebug = isInDebug Xor True
        End If
        If Not isInDebug Then
            If Pressed(Keys.Up) Then moveFront = moveSpeed
            If Pressed(Keys.Down) Then moveFront = -moveSpeed
        Else
            If Pressed(Keys.Up) Then roadScene.camera.Pitch -= rotateSpeed
            If Pressed(Keys.Down) Then roadScene.camera.Pitch += rotateSpeed
            If Pressed(Keys.Q) Then roadScene.camera.PositionY += -moveSpeed
            If Pressed(Keys.E) Then roadScene.camera.PositionY += +moveSpeed
        End If
        Dim z, x, y As Single
        z = roadScene.camera.PositionZ
        x = roadScene.camera.PositionX
        y = roadScene.camera.PositionY
        aboutTV = roadScene.GetDistanse(x, y, z, roadScene.FindObject("objTV")) < 150
        z = z + Math.Cos(-roadScene.camera.Yaw) * moveFront
        x = x + Math.Sin(-roadScene.camera.Yaw) * moveFront
        z = z + Math.Sin(roadScene.camera.Yaw) * moveStrafe
        x = x + Math.Cos(roadScene.camera.Yaw) * moveStrafe
        If roadScene.TestIntersect(x, y, z, 5, 20, 5) = False Then
            roadScene.camera.PositionZ = z
            roadScene.camera.PositionX = x
        End If
    End Sub
    Private Sub GoToMainScene()
        mdlMainScene.SetTargetMain()
        mainMode = 1
        tvTransition = True
        tvShown = True
        mdlMainScene.mainScene.ExecScript(Application.StartupPath + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "mainscene" + IO.Path.DirectorySeparatorChar + "transscript.txt")
        AddHandler mdlMainScene.mainScene.ScriptFinished, AddressOf TVTrasitionEnd
    End Sub
    Private Sub TVTrasitionEnd()
        If isMainScene Then
            isMainScene = False
            waveout.Init(New AudioFileReader(Application.StartupPath + "" + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "music" + IO.Path.DirectorySeparatorChar + "roadScene.wav"))
            waveout.Play()
            mdlMainScene.SetTargetTV()
            mainMode = 0
        End If
        If tvTransition = True Then
            tvTransition = False
            waveout.Init(New AudioFileReader(Application.StartupPath + "" + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "music" + IO.Path.DirectorySeparatorChar + "mainscene.wav"))
            waveout.Play()
            mainScene.ExecScript(Application.StartupPath + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "mainscene" + IO.Path.DirectorySeparatorChar + "movescript.txt")
            isMainScene = True
        End If
    End Sub
    Private Sub MoveLamp()
        Const lampStep As Single = 0.03
        Const lampLimit As Single = 2
        Const lampShowLimit As Single = 0.2
        Const lampLength As Single = 50
        Static lampPos As Single
        Static lampDirection As Integer
        If lampDirection = 0 Then
            lampPos += lampStep
        Else
            lampPos -= lampStep
        End If
        If lampPos > lampLimit Then lampDirection = 1
        If lampPos < -lampLimit Then lampDirection = 0
        If lampPos = 0 Then lampPos = 0.0001
        Dim lampResult As Single = Math.Sin(lampPos)
        If lampResult > lampShowLimit Then lampResult = lampShowLimit
        If lampResult < -lampShowLimit Then lampResult = -lampShowLimit
        'перемещаем
        lampObj.rotateY = lampResult
        lampLightObj.positionZ = lampZ - lampLength * Math.Cos(lampResult)
        lampLightObj.positionX = lampX + lampLength * Math.Sin(lampResult)
    End Sub
    ''' <summary>
    ''' Рисует сцену
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Draw()
        Static counter As Integer
        counter += 1
        If counter > 3 Then
            counter = 0
            info = "  X:" + CInt(roadScene.camera.PositionX).ToString
            info += "  Y:" + CInt(roadScene.camera.PositionY).ToString
            info += "  Z:" + CInt(roadScene.camera.PositionZ).ToString
            info += "  RY:" + roadScene.camera.Yaw.ToString
            info += "  Tri:" + roadScene.render.SceneDraw.InfoTrianglesDrawnCount.ToString + "/" + roadScene.render.SceneDraw.InfoTrianglesCount.ToString
            info += "  Pix:" + roadScene.render.SceneDraw.InfoPixelsDrawn.ToString + "/" + roadScene.render.SceneDraw.InfoPixelsDrawnRaw.ToString
            'mdlMainScene.Draw()
        End If
        If aboutTV And Not tvMessageShown Then
            tvMessageShown = True
            roadScene.ExecScript(Application.StartupPath + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "roadscene" + IO.Path.DirectorySeparatorChar + "abouttv.txt")
        End If
        '  aboutTV = True
        If aboutTV And Pressed(Keys.F) Then
            Pressed(Keys.F) = False
            GoToMainScene()
        End If
        roadScene.render.Clear()
        roadScene.render.SceneDraw.Clear()
        roadScene.Draw()
        rain.Draw()
        roadScene.render.SceneDraw.Render()
        roadScene.Draw2D()
        roadScene.render.Present()
    End Sub
    Sub ControlTV()
        Const phaseStep As Single = 0.01
        Const centerX As Single = 200
        Const centerY As Single = -140
        Static phase As Single
        phase += phaseStep
        If phase > 6.28 Then phase = 0
        mainScene.camera.Yaw = phase + 3.14
        mainScene.camera.PositionX = 100 * Math.Sin(phase) + centerX
        mainScene.camera.PositionZ = 100 * Math.Cos(phase) + centerY
    End Sub
    Sub LoadRain()
        Dim rainSpr As Sprite
        rainSpr = roadScene.FindSprite("sprRain")
        rain.sprite = rainSpr
        rain.areaLeft = -600
        rain.areaTop = -400
        rain.areaBottom = 400
        rain.areaRight = 400
        rain.floorLevel = 0
        rain.ceilLevel = 400
        rain.moveSpeed = -30
        rain.render = roadScene.render
        rain.ParticlesCount = 1000
        rain.Restart()
    End Sub
    Sub StartMusic()
        Try
            Dim music = New NAudio.Wave.AudioFileReader(Application.StartupPath + "" + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "music" + IO.Path.DirectorySeparatorChar + "roadscene_start.wav")

            waveout.Init(music)
            waveout.Play()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub CycleMusic()
        Try
            Dim music = New NAudio.Wave.AudioFileReader(Application.StartupPath + "" + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "music" + IO.Path.DirectorySeparatorChar + "roadscene.wav")
            waveout.Init(music)
            waveout.Play()
        Catch ex As Exception

        End Try

    End Sub
End Module


