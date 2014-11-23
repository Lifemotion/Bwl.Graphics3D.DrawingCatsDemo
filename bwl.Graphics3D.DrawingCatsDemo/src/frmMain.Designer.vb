<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.tMain = New System.Windows.Forms.Timer(Me.components)
        Me.tFPS = New System.Windows.Forms.Timer(Me.components)
        Me.lbInfo = New System.Windows.Forms.Label

        Me.SuspendLayout()
        '
        'tMain
        '
        Me.tMain.Interval = 20
        '
        'tFPS
        '
        Me.tFPS.Enabled = True
        Me.tFPS.Interval = 1000
        '
        'lbInfo
        '
        Me.lbInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbInfo.AutoSize = True
        Me.lbInfo.ForeColor = System.Drawing.Color.DarkGray
        Me.lbInfo.Location = New System.Drawing.Point(1, 469)
        Me.lbInfo.Name = "lbInfo"
        Me.lbInfo.Size = New System.Drawing.Size(16, 13)
        Me.lbInfo.TabIndex = 0
        Me.lbInfo.Text = "..."
        '
        'mplMusic
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(642, 483)

        Me.Controls.Add(Me.lbInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.Text = "BlueWolf, 2008: Рисование котов"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tMain As System.Windows.Forms.Timer
    Friend WithEvents tFPS As System.Windows.Forms.Timer
    Friend WithEvents lbInfo As System.Windows.Forms.Label

End Class
