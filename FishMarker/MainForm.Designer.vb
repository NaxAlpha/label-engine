<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
		Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
		Me.txtFileName = New System.Windows.Forms.ToolStripTextBox()
		Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
		Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton6 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
		Me.ToolStripButton5 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
		Me.lbl = New System.Windows.Forms.ToolStripLabel()
		Me.prog = New System.Windows.Forms.ToolStripProgressBar()
		Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
		Me.txtFrm = New System.Windows.Forms.ToolStripTextBox()
		Me.ToolStripButton7 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
		Me.btnTrack = New System.Windows.Forms.ToolStripButton()
		Me.tmr = New System.Windows.Forms.Timer(Me.components)
		Me.Panel1 = New System.Windows.Forms.Panel()
		Me.img = New System.Windows.Forms.PictureBox()
		Me.ToolStrip1.SuspendLayout()
		Me.Panel1.SuspendLayout()
		CType(Me.img, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'ToolStrip1
		'
		Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
		Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.txtFileName, Me.ToolStripSeparator1, Me.ToolStripButton2, Me.ToolStripSeparator2, Me.ToolStripButton3, Me.ToolStripButton6, Me.ToolStripSeparator3, Me.ToolStripButton5, Me.ToolStripButton4, Me.ToolStripSeparator4, Me.lbl, Me.prog, Me.ToolStripSeparator7, Me.txtFrm, Me.ToolStripButton7, Me.ToolStripSeparator5, Me.btnTrack})
		Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
		Me.ToolStrip1.Name = "ToolStrip1"
		Me.ToolStrip1.Size = New System.Drawing.Size(1332, 27)
		Me.ToolStrip1.TabIndex = 0
		Me.ToolStrip1.Text = "ToolStrip1"
		'
		'ToolStripButton1
		'
		Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
		Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton1.Name = "ToolStripButton1"
		Me.ToolStripButton1.Size = New System.Drawing.Size(89, 24)
		Me.ToolStripButton1.Text = "Load Video"
		'
		'txtFileName
		'
		Me.txtFileName.Name = "txtFileName"
		Me.txtFileName.ReadOnly = True
		Me.txtFileName.Size = New System.Drawing.Size(100, 27)
		'
		'ToolStripSeparator1
		'
		Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
		Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 27)
		'
		'ToolStripButton2
		'
		Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
		Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton2.Name = "ToolStripButton2"
		Me.ToolStripButton2.Size = New System.Drawing.Size(83, 24)
		Me.ToolStripButton2.Text = "Pause/Play"
		'
		'ToolStripSeparator2
		'
		Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
		Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 27)
		'
		'ToolStripButton3
		'
		Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
		Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton3.Name = "ToolStripButton3"
		Me.ToolStripButton3.Size = New System.Drawing.Size(89, 24)
		Me.ToolStripButton3.Text = "Next Frame"
		'
		'ToolStripButton6
		'
		Me.ToolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton6.Image = CType(resources.GetObject("ToolStripButton6.Image"), System.Drawing.Image)
		Me.ToolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton6.Name = "ToolStripButton6"
		Me.ToolStripButton6.Size = New System.Drawing.Size(103, 24)
		Me.ToolStripButton6.Text = "Reload Video"
		'
		'ToolStripSeparator3
		'
		Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
		Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 27)
		'
		'ToolStripButton5
		'
		Me.ToolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton5.Image = CType(resources.GetObject("ToolStripButton5.Image"), System.Drawing.Image)
		Me.ToolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton5.Name = "ToolStripButton5"
		Me.ToolStripButton5.Size = New System.Drawing.Size(89, 24)
		Me.ToolStripButton5.Text = "Load Fishes"
		'
		'ToolStripButton4
		'
		Me.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton4.Image = CType(resources.GetObject("ToolStripButton4.Image"), System.Drawing.Image)
		Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton4.Name = "ToolStripButton4"
		Me.ToolStripButton4.Size = New System.Drawing.Size(87, 24)
		Me.ToolStripButton4.Text = "Save Fishes"
		'
		'ToolStripSeparator4
		'
		Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
		Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 27)
		'
		'lbl
		'
		Me.lbl.Name = "lbl"
		Me.lbl.Size = New System.Drawing.Size(91, 24)
		Me.lbl.Text = "Progress 0/0"
		'
		'prog
		'
		Me.prog.Name = "prog"
		Me.prog.Size = New System.Drawing.Size(100, 24)
		'
		'ToolStripSeparator7
		'
		Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
		Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 27)
		'
		'txtFrm
		'
		Me.txtFrm.Name = "txtFrm"
		Me.txtFrm.Size = New System.Drawing.Size(100, 27)
		Me.txtFrm.Text = "0"
		'
		'ToolStripButton7
		'
		Me.ToolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.ToolStripButton7.Image = CType(resources.GetObject("ToolStripButton7.Image"), System.Drawing.Image)
		Me.ToolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton7.Name = "ToolStripButton7"
		Me.ToolStripButton7.Size = New System.Drawing.Size(97, 24)
		Me.ToolStripButton7.Text = "Go To Frame"
		'
		'ToolStripSeparator5
		'
		Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
		Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 27)
		'
		'btnTrack
		'
		Me.btnTrack.Checked = True
		Me.btnTrack.CheckState = System.Windows.Forms.CheckState.Checked
		Me.btnTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
		Me.btnTrack.Image = CType(resources.GetObject("btnTrack.Image"), System.Drawing.Image)
		Me.btnTrack.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btnTrack.Name = "btnTrack"
		Me.btnTrack.Size = New System.Drawing.Size(126, 24)
		Me.btnTrack.Text = "Tracking Enabled"
		'
		'tmr
		'
		Me.tmr.Enabled = True
		Me.tmr.Interval = 16
		'
		'Panel1
		'
		Me.Panel1.AutoScroll = True
		Me.Panel1.Controls.Add(Me.img)
		Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.Panel1.Location = New System.Drawing.Point(0, 27)
		Me.Panel1.Name = "Panel1"
		Me.Panel1.Size = New System.Drawing.Size(1332, 618)
		Me.Panel1.TabIndex = 1
		'
		'img
		'
		Me.img.BackColor = System.Drawing.Color.White
		Me.img.Location = New System.Drawing.Point(0, 0)
		Me.img.Name = "img"
		Me.img.Size = New System.Drawing.Size(215, 200)
		Me.img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
		Me.img.TabIndex = 2
		Me.img.TabStop = False
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1332, 645)
		Me.Controls.Add(Me.Panel1)
		Me.Controls.Add(Me.ToolStrip1)
		Me.Name = "MainForm"
		Me.ShowIcon = False
		Me.Text = "Fish Marker Engine"
		Me.ToolStrip1.ResumeLayout(False)
		Me.ToolStrip1.PerformLayout()
		Me.Panel1.ResumeLayout(False)
		Me.Panel1.PerformLayout()
		CType(Me.img, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents ToolStrip1 As ToolStrip
	Friend WithEvents ToolStripButton1 As ToolStripButton
	Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
	Friend WithEvents ToolStripButton2 As ToolStripButton
	Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
	Friend WithEvents ToolStripButton3 As ToolStripButton
	Friend WithEvents tmr As Timer
	Friend WithEvents txtFileName As ToolStripTextBox
	Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
	Friend WithEvents lbl As ToolStripLabel
	Friend WithEvents prog As ToolStripProgressBar
	Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
	Friend WithEvents ToolStripButton5 As ToolStripButton
	Friend WithEvents ToolStripButton4 As ToolStripButton
	Friend WithEvents ToolStripButton6 As ToolStripButton
	Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
	Friend WithEvents txtFrm As ToolStripTextBox
	Friend WithEvents ToolStripButton7 As ToolStripButton
	Friend WithEvents Panel1 As Panel
	Friend WithEvents img As PictureBox
	Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
	Friend WithEvents btnTrack As ToolStripButton
End Class
