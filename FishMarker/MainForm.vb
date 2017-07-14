Imports System.IO
Imports OpenCvSharp
Imports OpenCvSharp.Extensions

Public Class MainForm

	Dim start As Drawing.Point
	Dim ender As Drawing.Point
	Dim creating As Boolean

	Private fx As New FishCollection

	Private videoFile As String = ""
	Private fishFile As String = ""
	Private capture As VideoCapture

	Private current As New Mat

	Dim play As Boolean = False

	Private Sub MoveSingleFrame()
		If capture IsNot Nothing Then
			capture.Read(current)
			img.Image = current.ToBitmap()
			prog.Value = capture.PosFrames
			lbl.Text = $"Progress: {prog.Value}/{prog.Maximum}"
			fx.Move()
			fx.Save(fishFile)
		End If
	End Sub

	Private Sub SkipToFrame(fid As Integer)
		If capture IsNot Nothing Then
			If fid = capture.PosFrames Then
				Return
			End If
			If fid < capture.PosFrames Then
				ReloadVideo()
			End If
			While capture.PosFrames <> fid
				capture.Read(current)
				fx.Move()
			End While
			img.Image = current.ToBitmap()
			prog.Value = capture.PosFrames
			lbl.Text = $"Progress: {prog.Value}/{prog.Maximum}"
		End If
	End Sub

	Private Sub RenderFishes(g As Graphics)
		For Each f In fx.Current
			'g.TranslateTransform(f.Center.X, f.Center.Y)
			'g.DrawEllipse(Pens.Green, New RectangleF(New PointF(-2, -2), New SizeF(4, 4)))

			'g.RotateTransform(f.Angle * 180 / Math.PI)
			'g.DrawEllipse(Pens.Blue, -f.Width / 2, -f.Height / 2, f.Width, f.Height)
			'g.RotateTransform(-f.Angle * 180 / Math.PI)

			'Dim del = img.PointToClient(MousePosition) - f.Center
			'Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
			'Dim min = Math.Min(f.Width, f.Height)
			'Dim max = Math.Max(f.Width, f.Height)

			If f.Bounding.Contains(img.PointToClient(MousePosition)) Then
				g.FillRectangle(New SolidBrush(Color.FromArgb(128, Color.Yellow)), f.Bounding)
			End If

			'g.TranslateTransform(-f.Center.X, -f.Center.Y)
			g.DrawRectangle(Pens.Orange, f.Bounding)
		Next
	End Sub

	Private Sub ReloadVideo() Handles ToolStripButton6.Click
		If capture IsNot Nothing Then
			capture.Dispose()
			capture = VideoCapture.FromFile(videoFile)
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
		If e.Button <> MouseButtons.Left OrElse capture Is Nothing Then Return
		start = e.Location
		ender = e.Location
		creating = True
	End Sub

	Private Sub img_MouseUp(sender As Object, e As MouseEventArgs) Handles img.MouseUp
		If Not creating OrElse e.Button <> MouseButtons.Left OrElse capture Is Nothing Then Return

		ender = e.Location
		creating = False

		fx.Current.Add(New Fish With {.Start = start, .End = ender})
	End Sub

	Private Sub img_MouseMove(sender As Object, e As MouseEventArgs) Handles img.MouseMove
		If creating Then
			ender = e.Location
		End If
	End Sub

	Private Sub img_MouseClick(sender As Object, e As MouseEventArgs) Handles img.MouseClick
		If e.Button = MouseButtons.Right Then

			For Each f In fx.Current
				'Dim del = img.PointToClient(MousePosition) - f.Center
				'Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
				'Dim min = Math.Min(f.Width, f.Height)
				'Dim max = Math.Max(f.Width, f.Height)

				If f.Bounding.Contains(img.PointToClient(MousePosition)) Then
					fx.Current.Remove(f)
					Exit For
				End If

			Next

		End If
	End Sub

	Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
		Using ofd As New OpenFileDialog
			If ofd.ShowDialog() = DialogResult.OK Then
				If capture IsNot Nothing Then
					capture.Dispose()
					fx.Clear()
				End If
				videoFile = ofd.FileName
				capture = VideoCapture.FromFile(videoFile)
				fishFile = videoFile + ".fish"
				If File.Exists(fishFile) Then
					fx.Load(fishFile)
				End If
				txtFileName.Text = Path.GetFileName(videoFile)
				prog.Maximum = capture.Get(CaptureProperty.FrameCount)
				lbl.Text = $"Progress: {0}/{prog.Maximum}"
				img.Image = Nothing
			End If
		End Using
	End Sub

	Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
		If capture Is Nothing OrElse Not File.Exists(fishFile) Then Return
		fx.Load(fishFile)
	End Sub

	Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
		play = Not play
		ToolStripButton2.Checked = play
	End Sub

	Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
		If capture IsNot Nothing Then
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
		Dim eng As New CvEngine()
		eng.Open("C:\Users\Nax\Documents\Visual Studio 2017\Projects\fish_count\fish_count\ds\00.mp4")
		eng.MoveNext()
		Dim bmp = eng.GetCurrentFrame()
		Stop
	End Sub

End Class
