Module mdlMainScene
    Public mainScene As New Scene
    Public waterGen As New WaterGen
    Public water As New Model
    Public waterObj As Object3D
    Public waterFrame As Integer
    Public sceneRender As Render3D
    Private Const moveSpeed As Integer = 10
    Private Const rotateSpeed As Single = 0.05
    Public Sub Init()
        sceneRender = New Render3D
        'настраиваем рендер
        InitScene()
        'создаем воду
        CreateWater()
        ' frmTest.Show()
    End Sub
    Sub StartMainScript()
        mainScene.ExecScript(Application.StartupPath + "" + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "mainscene" + IO.Path.DirectorySeparatorChar + "movescript.txt")
    End Sub
    Sub SetTargetTV()
        Dim screen As Model = roadScene.FindModel("mdlTVscreen")
        Dim screenMat As Material = Nothing
        Dim i As Integer
        For i = 0 To screen.materialsLib.GetUpperBound(0)
            If screen.materialsLib(i).name = "SCREEN" Then screenMat = screen.materialsLib(i) : Exit For
        Next
        mainScene.render = sceneRender
        With mainScene.render.presentSettings
            .TargetType = OutputTargetTypeEnum.material
            .TargetMaterial = screenMat
            .TargetGraphics = frmMain.pbGraphics
            .WindowWidth = 512
            .WindowHeight = 512
        End With
        mainScene.Init()
        StartTVSCript()
    End Sub
    Sub StartTVSCript()
        mainScene.ExecScript(Application.StartupPath + "" + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "mainscene" + IO.Path.DirectorySeparatorChar + "tvscript.txt")
    End Sub
    Sub SetTargetMain()
        mainScene.render = sceneRender
        With mainScene.render.presentSettings
            .TargetType = OutputTargetTypeEnum.graphics
            .TargetGraphics = frmMain.pbGraphics
            .WindowWidth = 640
            .WindowHeight = 480
        End With
        mainScene.Init()
    End Sub
    Private Sub InitScene()
        'настраиваем рендер
        mainScene.render = New Render3D
        With mainScene.render.presentSettings
            .TargetType = OutputTargetTypeEnum.graphics
            .TargetGraphics = frmMain.pbGraphics
            .WindowWidth = 640
            .WindowHeight = 480
        End With
        mainScene.camera = New Camera3D
        mainScene.Init()
        'добавляем модель воды в сцену
        mainScene.AddModel(water, "mdlWater")
        'выполняет основную загрузку из скрипта
        mainScene.ExecScript(Application.StartupPath + "" + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "mainscene" + IO.Path.DirectorySeparatorChar + "loadscript.txt")
    End Sub
    Private Sub CreateWater()
        Dim waterTex As PixelSurface
        'ищем в сцене объект воды
        waterObj = mainScene.FindObject("objWater")
        'и текстуру воды
        waterTex = mainScene.FindTexture("texWater")
        'создаем материал для воды
        Dim waterMat As New Material
        waterMat.name = "matWater"
        waterMat.texturePixels(0) = waterTex
        waterMat.maximumMipMapLevel = 0
        'запускаем генератор 
        waterGen.material = waterMat
        With waterGen
            .framesCount = 200
            .disturbMaxFrame = 190
            .gridSizeX = 30
            .gridSizeZ = 15
            .gridStepX = 50
            .gridStepZ = 50
            .textureMoveX = 0.02
            .textureMovez = 0.0
            .textureTileX = 2
            .textureTileZ = 1
            .wavesAmplitude = 3
            .wavesKoeffX = 0.4
            .wavesKoeffZ = 0.4
            .yMultiplier = 2
            .disturbProbability = 0.05
        End With
        water = waterGen.MakeModel
        waterObj.model = water
        waterObj.modelMesh = 1
    End Sub
    ''' <summary>
    ''' Процедура, вызывающая разные таймерные действия.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub TimedEvents()
        Static slow10 As Integer
        Static slow2 As Integer
        CheckKeys()
        mainScene.Tick()
        slow2 += 1
        slow10 += 1
        If slow2 > 2 Then
            slow2 = 1
            SetWaterFrame()
        End If
        If slow10 > 10 Then
            slow10 = 1
        End If
    End Sub
    Sub CheckKeys()
        If mainMode <> 0 Then
            Dim moveFront As Integer = 0
            Dim moveStrafe As Integer = 0
            If Pressed(Keys.Space) Then
                Pressed(Keys.Space) = False
                ' mainScene.ExecScript(".." + IO.Path.DirectorySeparatorChar +"mainscene" + IO.Path.DirectorySeparatorChar +"movescript.txt")
            End If
            '   If Pressed(Keys.Left) Then mainScene.camera.PositionX -= moveSpeed
            '   If Pressed(Keys.Right) Then mainScene.camera.PositionX += moveSpeed
            If Pressed(Keys.W) Then moveFront = moveSpeed
            If Pressed(Keys.S) Then moveFront = -moveSpeed
            If Pressed(Keys.A) Then moveStrafe = -moveSpeed
            If Pressed(Keys.D) Then moveStrafe = moveSpeed
            If Pressed(Keys.Q) Then mainScene.camera.PositionY += -moveSpeed
            If Pressed(Keys.E) Then mainScene.camera.PositionY += +moveSpeed
            If Pressed(Keys.Left) Then mainScene.camera.Yaw += rotateSpeed
            If Pressed(Keys.Right) Then mainScene.camera.Yaw -= rotateSpeed
            If Pressed(Keys.Up) Then moveFront = moveSpeed
            If Pressed(Keys.Down) Then moveFront = -moveSpeed

            mainScene.camera.PositionZ = mainScene.camera.PositionZ + Math.Cos(-mainScene.camera.Yaw) * moveFront
            mainScene.camera.PositionX = mainScene.camera.PositionX + Math.Sin(-mainScene.camera.Yaw) * moveFront
            mainScene.camera.PositionZ = mainScene.camera.PositionZ + Math.Sin(mainScene.camera.Yaw) * moveStrafe
            mainScene.camera.PositionX = mainScene.camera.PositionX + Math.Cos(mainScene.camera.Yaw) * moveStrafe
        End If
    End Sub
    ''' <summary>
    ''' Переключает кадры воды для ее анимации
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWaterFrame()
        Static waterFrame As Integer
        waterFrame += 1
        If waterFrame > Water.MeshesCount - 1 Then waterFrame = 0
        waterObj.modelMesh = waterFrame
    End Sub
    ''' <summary>
    ''' Рисует сцену
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Draw()
        mainScene.render.Clear()
        mainScene.render.SceneDraw.Clear()
        mainScene.Draw()
        mainScene.render.SceneDraw.Render()
        mainScene.Draw2D()
        mainScene.render.Present()
        If mainMode <> 0 Then
            Static counter As Integer
            counter += 1
            If counter = 10 Then
                counter = 0
                info = "  X:" + CInt(mainScene.camera.PositionX).ToString
                info += "  Y:" + CInt(mainScene.camera.PositionY).ToString
                info += "  Z:" + CInt(mainScene.camera.PositionZ).ToString
                info += "  RY:" + mainScene.camera.Yaw.ToString
                info += "  Tri:" + mainScene.render.SceneDraw.InfoTrianglesDrawnCount.ToString + "/" + mainScene.render.SceneDraw.InfoTrianglesCount.ToString
                info += "  Pix:" + mainScene.render.SceneDraw.InfoPixelsDrawn.ToString + "/" + mainScene.render.SceneDraw.InfoPixelsDrawnRaw.ToString
            End If
        End If
    End Sub
End Module
