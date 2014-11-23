Public Class frmSplash
    Private Sub frmSplash_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackgroundImage = Bitmap.FromFile(Application.StartupPath + IO.Path.DirectorySeparatorChar + ".." + IO.Path.DirectorySeparatorChar + "data" + IO.Path.DirectorySeparatorChar + "loader2.jpg")
    End Sub
End Class