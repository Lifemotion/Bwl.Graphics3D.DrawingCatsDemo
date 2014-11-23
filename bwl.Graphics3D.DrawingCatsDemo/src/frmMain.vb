Public Class frmMain
    Public pbGraphics As Drawing.Graphics

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        running = False
    End Sub
    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Pressed(e.KeyValue) = True
    End Sub
    Private Sub frmMain_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Pressed(e.KeyValue) = False
    End Sub
    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pbGraphics = Me.CreateGraphics
    End Sub
    Private Sub tMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tMain.Tick

    End Sub
    Private Sub tFPS_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tFPS.Tick
        RerfeshInfo()
    End Sub


End Class
