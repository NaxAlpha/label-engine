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
		Me.tools = New System.Windows.Forms.ToolStrip()
		Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
		Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
		Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
		Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
		Me.progFid = New System.Windows.Forms.ToolStripProgressBar()
		Me.txtFid = New System.Windows.Forms.ToolStripTextBox()
		Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
		Me.tmr_slow = New System.Windows.Forms.Timer(Me.components)
		Me.mainPanel = New System.Windows.Forms.Panel()
		Me.status = New System.Windows.Forms.StatusStrip()
		Me.lblFishCount = New System.Windows.Forms.ToolStripStatusLabel()
		Me.lblUpdate = New System.Windows.Forms.ToolStripStatusLabel()
		Me.img = New System.Windows.Forms.PictureBox()
		Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton6 = New System.Windows.Forms.ToolStripButton()
		Me.btnTrack = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripButton5 = New System.Windows.Forms.ToolStripButton()
		Me.btnUpdate = New System.Windows.Forms.ToolStripButton()
		Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
		Me.tools.SuspendLayout()
		Me.mainPanel.SuspendLayout()
		Me.status.SuspendLayout()
		CType(Me.img, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'tools
		'
		Me.tools.ImageScalingSize = New System.Drawing.Size(40, 40)
		Me.tools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripSeparator1, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripSeparator5, Me.ToolStripButton6, Me.btnTrack, Me.ToolStripSeparator2, Me.ToolStripButton4, Me.ToolStripSeparator3, Me.ToolStripLabel1, Me.progFid, Me.txtFid, Me.ToolStripButton5, Me.ToolStripSeparator4, Me.btnUpdate})
		Me.tools.Location = New System.Drawing.Point(0, 0)
		Me.tools.Name = "tools"
		Me.tools.Size = New System.Drawing.Size(1185, 47)
		Me.tools.TabIndex = 0
		'
		'ToolStripSeparator1
		'
		Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
		Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 47)
		'
		'ToolStripSeparator2
		'
		Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
		Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 47)
		'
		'ToolStripSeparator3
		'
		Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
		Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 47)
		'
		'ToolStripLabel1
		'
		Me.ToolStripLabel1.Name = "ToolStripLabel1"
		Me.ToolStripLabel1.Size = New System.Drawing.Size(72, 44)
		Me.ToolStripLabel1.Text = "Frame ID:"
		'
		'progFid
		'
		Me.progFid.Name = "progFid"
		Me.progFid.Size = New System.Drawing.Size(400, 44)
		'
		'txtFid
		'
		Me.txtFid.Font = New System.Drawing.Font("Segoe UI Symbol", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtFid.Name = "txtFid"
		Me.txtFid.Size = New System.Drawing.Size(100, 47)
		Me.txtFid.Text = "0"
		'
		'ToolStripSeparator4
		'
		Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
		Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 47)
		'
		'tmr_slow
		'
		Me.tmr_slow.Enabled = True
		Me.tmr_slow.Interval = 500
		'
		'mainPanel
		'
		Me.mainPanel.AutoScroll = True
		Me.mainPanel.Controls.Add(Me.status)
		Me.mainPanel.Controls.Add(Me.img)
		Me.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill
		Me.mainPanel.Location = New System.Drawing.Point(0, 47)
		Me.mainPanel.Name = "mainPanel"
		Me.mainPanel.Size = New System.Drawing.Size(1185, 617)
		Me.mainPanel.TabIndex = 1
		'
		'status
		'
		Me.status.ImageScalingSize = New System.Drawing.Size(20, 20)
		Me.status.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblFishCount, Me.lblUpdate})
		Me.status.Location = New System.Drawing.Point(0, 588)
		Me.status.Name = "status"
		Me.status.Size = New System.Drawing.Size(1185, 29)
		Me.status.TabIndex = 3
		Me.status.Text = "StatusStrip1"
		'
		'lblFishCount
		'
		Me.lblFishCount.Name = "lblFishCount"
		Me.lblFishCount.Size = New System.Drawing.Size(92, 24)
		Me.lblFishCount.Text = "Fish Count: 0"
		'
		'lblUpdate
		'
		Me.lblUpdate.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
			Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
			Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
		Me.lblUpdate.Name = "lblUpdate"
		Me.lblUpdate.Size = New System.Drawing.Size(158, 24)
		Me.lblUpdate.Text = "Checking for Update..."
		'
		'img
		'
		Me.img.BackColor = System.Drawing.Color.White
		Me.img.Location = New System.Drawing.Point(0, 0)
		Me.img.Name = "img"
		Me.img.Size = New System.Drawing.Size(884, 570)
		Me.img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.img.TabIndex = 2
		Me.img.TabStop = False
		'
		'ToolStripButton1
		'
		Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ToolStripButton1.Image = Global.FishMarker.My.Resources.Resources._002_eject
		Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton1.Name = "ToolStripButton1"
		Me.ToolStripButton1.Size = New System.Drawing.Size(44, 44)
		Me.ToolStripButton1.Text = "Load Video"
		'
		'ToolStripButton2
		'
		Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ToolStripButton2.Image = Global.FishMarker.My.Resources.Resources._003_rewind
		Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton2.Name = "ToolStripButton2"
		Me.ToolStripButton2.Size = New System.Drawing.Size(44, 44)
		Me.ToolStripButton2.Text = "Previous Frame"
		'
		'ToolStripButton3
		'
		Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ToolStripButton3.Image = Global.FishMarker.My.Resources.Resources._004_fast_forward
		Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton3.Name = "ToolStripButton3"
		Me.ToolStripButton3.Size = New System.Drawing.Size(44, 44)
		Me.ToolStripButton3.Text = "Next Frame"
		'
		'ToolStripButton6
		'
		Me.ToolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ToolStripButton6.Image = Global.FishMarker.My.Resources.Resources.right
		Me.ToolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton6.Name = "ToolStripButton6"
		Me.ToolStripButton6.Size = New System.Drawing.Size(44, 44)
		Me.ToolStripButton6.Text = "btnCopyBoxes"
		Me.ToolStripButton6.ToolTipText = "Copy Boxes to Next Frame"
		'
		'btnTrack
		'
		Me.btnTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btnTrack.Image = Global.FishMarker.My.Resources.Resources.route
		Me.btnTrack.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btnTrack.Name = "btnTrack"
		Me.btnTrack.Size = New System.Drawing.Size(44, 44)
		Me.btnTrack.Text = "Do Track"
		'
		'ToolStripButton4
		'
		Me.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ToolStripButton4.Image = Global.FishMarker.My.Resources.Resources.delete
		Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton4.Name = "ToolStripButton4"
		Me.ToolStripButton4.Size = New System.Drawing.Size(44, 44)
		Me.ToolStripButton4.Text = "Clear This Frame"
		'
		'ToolStripButton5
		'
		Me.ToolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.ToolStripButton5.Image = Global.FishMarker.My.Resources.Resources.Button_Next_icon
		Me.ToolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.ToolStripButton5.Name = "ToolStripButton5"
		Me.ToolStripButton5.Size = New System.Drawing.Size(44, 44)
		Me.ToolStripButton5.Text = "Go to Frame"
		'
		'btnUpdate
		'
		Me.btnUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
		Me.btnUpdate.Image = Global.FishMarker.My.Resources.Resources.update_arrows
		Me.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta
		Me.btnUpdate.Name = "btnUpdate"
		Me.btnUpdate.Size = New System.Drawing.Size(44, 44)
		Me.btnUpdate.Text = "Update Now"
		'
		'ToolStripSeparator5
		'
		Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
		Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 47)
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1185, 664)
		Me.Controls.Add(Me.mainPanel)
		Me.Controls.Add(Me.tools)
		Me.Name = "MainForm"
		Me.ShowIcon = False
		Me.Text = "Fish Marker Engine"
		Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
		Me.tools.ResumeLayout(False)
		Me.tools.PerformLayout()
		Me.mainPanel.ResumeLayout(False)
		Me.mainPanel.PerformLayout()
		Me.status.ResumeLayout(False)
		Me.status.PerformLayout()
		CType(Me.img, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents tools As ToolStrip
	Friend WithEvents ToolStripButton1 As ToolStripButton
	Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
	Friend WithEvents ToolStripButton3 As ToolStripButton
	Friend WithEvents tmr_slow As Timer
	Friend WithEvents mainPanel As Panel
	Friend WithEvents img As PictureBox
	Friend WithEvents btnTrack As ToolStripButton
	Friend WithEvents ToolStripButton2 As ToolStripButton
	Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
	Friend WithEvents status As StatusStrip
	Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
	Friend WithEvents ToolStripLabel1 As ToolStripLabel
	Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
	Friend WithEvents progFid As ToolStripProgressBar
	Friend WithEvents ToolStripButton4 As ToolStripButton
	Friend WithEvents txtFid As ToolStripTextBox
	Friend WithEvents ToolStripButton5 As ToolStripButton
	Friend WithEvents btnUpdate As ToolStripButton
	Friend WithEvents lblUpdate As ToolStripStatusLabel
	Friend WithEvents lblFishCount As ToolStripStatusLabel
	Friend WithEvents ToolStripButton6 As ToolStripButton
	Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
End Class
