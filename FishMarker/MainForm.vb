Imports System.IO

Public Class MainForm

	Dim start As Point
	Dim ender As Point
	Dim creating As Boolean

	Private fx As New FishCollection

	Private videoFile As String = ""
	Private fishFile As String = ""
	Private engine As New CvEngine

	Dim play As Boolean = False

	Private Sub MoveSingleFrame()
		If engine.IsOpened Then
			engine.MoveNext()
			img.Image = engine.ToBitmap()
			lbl.Text = $"Progress: {engine.FrameID}/{engine.FrameCount}"
			fx.Move()
			If btnTrack.Checked Then
				fx.Current.Clear()
				engine.Track()
				For Each bb In engine.ListRegions()
					fx.Current.Add(New Fish With {.Start = bb.Location, .End = bb.Location + bb.Size, .Aspect = txtAspect.Text})
				Next
			End If
			fx.Save(fishFile)
		End If
	End Sub

	Private Sub SkipToFrame(fid As Integer)
		If engine.IsOpened Then
			If fid = engine.FrameID Then
				Return
			End If
			If fid < engine.FrameID Then
				ReloadVideo()
			End If
			While engine.FrameID <> fid
				engine.MoveNext()
				fx.Move()
			End While
			img.Image = engine.ToBitmap()
			lbl.Text = $"Progress: {engine.FrameID}/{engine.FrameCount}"
		End If
	End Sub

	Private Sub RenderFishes(g As Graphics)
		For Each f In fx.Current
			g.TranslateTransform(f.Center.X, f.Center.Y)
			'g.DrawEllipse(Pens.Green, New RectangleF(New PointF(-2, -2), New SizeF(4, 4)))

			g.RotateTransform(f.Angle * 180 / Math.PI)
			g.DrawEllipse(Pens.Blue, -f.Width / 2, -f.Height / 2, f.Width, f.Height)
			g.RotateTransform(-f.Angle * 180 / Math.PI)

			'Dim del = img.PointToClient(MousePosition) - f.Center
			'Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
			'Dim min = Math.Min(f.Width, f.Height)
			'Dim max = Math.Max(f.Width, f.Height)

			g.TranslateTransform(-f.Center.X, -f.Center.Y)

			If f.Bounding.Contains(img.PointToClient(MousePosition)) Then
				g.FillRectangle(New SolidBrush(Color.FromArgb(128, Color.Yellow)), f.Bounding)
			End If

			g.DrawRectangle(Pens.Orange, f.Bounding)
		Next
	End Sub

	Private Sub ReloadVideo() Handles ToolStripButton6.Click
		If engine.IsOpened Then
			engine.Close()
			engine.Open(videoFile)
			fx.Reset()
			img.Image = Nothing
		End If
	End Sub

	Private Sub img_Paint(sender As Object, e As PaintEventArgs) Handles img.Paint
		Dim g = e.Graphics
		g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
		g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
		g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
		g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

		RenderFishes(g)

		If creating Then
			g.DrawLine(Pens.Red, start, ender)
			g.DrawRectangle(Pens.Green, New Fish With {.Start = start, .End = ender}.Bounding)
		End If
	End Sub

	Private Sub img_MouseDown(sender As Object, e As MouseEventArgs) Handles img.MouseDown
		If e.Button <> MouseButtons.Left OrElse Not engine.IsOpened Then Return
		start = e.Location
		ender = e.Location
		creating = True
	End Sub

	Private Sub img_MouseUp(sender As Object, e As MouseEventArgs) Handles img.MouseUp
		If Not creating OrElse e.Button <> MouseButtons.Left OrElse Not engine.IsOpened Then Return

		ender = e.Location
		creating = False

		Dim f = New Fish With {.Start = start, .End = ender, .Aspect = txtAspect.Text}

		fx.Current.Add(f)

		If btnTrack.Checked Then
			engine.AddRegion(f.Bounding)
		End If
	End Sub

	Private Sub img_MouseMove(sender As Object, e As MouseEventArgs) Handles img.MouseMove
		If creating Then
			ender = e.Location
		End If
	End Sub

	Private Sub img_MouseClick(sender As Object, e As MouseEventArgs) Handles img.MouseClick
		If e.Button = MouseButtons.Right Then

			Dim mp = img.PointToClient(MousePosition)
			engine.RemoveRegion(mp.X, mp.Y)

			For Each f In fx.Current
				'Dim del = img.PointToClient(MousePosition) - f.Center
				'Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
				'Dim min = Math.Min(f.Width, f.Height)
				'Dim max = Math.Max(f.Width, f.Height)
				If f.Bounding.Contains(mp) Then
					fx.Current.Remove(f)
					Exit For
				End If

			Next

		End If
	End Sub

	Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
		Using ofd As New OpenFileDialog
			If ofd.ShowDialog() = DialogResult.OK Then
				If engine.IsOpened Then
					engine.Close()
					fx.Clear()
				End If
				videoFile = ofd.FileName
				engine.Open(videoFile)
				fishFile = videoFile + ".fish"
				If File.Exists(fishFile) Then
					fx.Load(fishFile)
				End If
				txtFileName.Text = Path.GetFileName(videoFile)
				lbl.Text = $"Progress: {0}/{engine.FrameCount}"
				img.Image = Nothing
			End If
		End Using
	End Sub

	Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
		If Not engine.IsOpened OrElse Not File.Exists(fishFile) Then Return
		fx.Load(fishFile)
	End Sub

	Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
		play = Not play
		ToolStripButton2.Checked = play
	End Sub

	Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
		If engine.IsOpened Then
			fx.Save(fishFile)
		End If
	End Sub

	Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
		MoveSingleFrame()
	End Sub

	Private Sub tmr_Tick(sender As Object, e As EventArgs) Handles tmr.Tick
		img.Invalidate()
		If play Then MoveSingleFrame()
	End Sub

	Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
		SkipToFrame(Val(txtFrm.Text))
	End Sub

	Private Sub txtFrm_Click(sender As Object, e As EventArgs) Handles txtFrm.Click
		txtFrm.Text = CUInt(Val(txtFrm.Text)).ToString()
	End Sub

	Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		'Dim vid = CVX.OpenVideo("C:\Users\Nax\Documents\Visual Studio 2017\Projects\fish_count\fish_count\ds\00.mp4")
		'Dim fx = CVX.CreateFrame()
		'Dim m As New Mat(fx)
		'CVX.ReadFrame(vid, fx)
		'Stop
		'Dim eng As New CvEngine()
		'eng.Open("C:\Users\Nax\Documents\Visual Studio 2017\Projects\fish_count\fish_count\ds\00.mp4")
		'eng.MoveNext()
		'eng.StartTracking()
		'eng.AddRegion(New Rectangle(2, 2, 20, 20))
		'eng.MoveNext()
		'eng.Track()
		'Dim lst = eng.ListRegions()

		'Dim bmp = eng.ToBitmap()
		'Stop
	End Sub

	Private Sub btnTrack_Click(sender As Object, e As EventArgs) Handles btnTrack.Click
		btnTrack.Checked = Not btnTrack.Checked
		btnTrack.Text = "Tracking " + If(btnTrack.Checked, "Enabled", "Disabled")
		If btnTrack.Checked Then
			engine.StartTracking()
			For Each f In fx.Current
				engine.AddRegion(f.Bounding)
			Next
		Else
			engine.StopTracking()
		End If
	End Sub

	Private Sub ToolStripButton9_Click(sender As Object, e As EventArgs)
		engine.StopTracking()
	End Sub

	Private Sub txtAspect_Click(sender As Object, e As EventArgs) Handles txtAspect.Click
		txtAspect.Text = Val(txtAspect.Text)
	End Sub

End Class
