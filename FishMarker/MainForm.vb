Imports System.IO
Imports System.Net
Imports System.Web.Configuration
Imports RestSharp

Public Class MainForm

	Dim start As Point
	Dim ender As Point
	Dim creating As Boolean

	Private fx As New FishCollection

	Private videoFile As String = ""
	Private fishFile As String = ""
	Private engine As New CvEngine

	Dim play As Boolean = False

	Private Async Function DownloadFile(url As String) As Task(Of String)
		Dim req As HttpWebRequest = WebRequest.Create(url)
		req.Accept = "application/vnd.github.v3+json"
		req.UserAgent = "Cool"
		Using res = req.GetResponse(), r = New StreamReader(res.GetResponseStream())
			Return Await r.ReadToEndAsync()
		End Using
	End Function

	Private Async Sub CheckForUpdate()
		Await Task.Delay(1)
		Try
			Dim txt = Await DownloadFile("https://api.github.com/repos/NaxAlpha/label-engine/releases/latest")
			Dim obj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(txt)
			Dim tag = obj("tag_name").ToString()
			Dim newver = Val(tag.Split("."c, "-"c)(2))
			Dim build = Val(Application.ProductVersion.Split("."c, "-"c)(2))
			If build < newver Then
				MessageBox.Show("Newer version is avaiable on Github: NaxAlpha/label-engine")
			End If
		Catch ex As Exception
			MessageBox.Show("Failed to check for update!")
		End Try
	End Sub

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
					fx.Current.Add(bb)
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

			engine.StopTracking()
			engine.StartTracking()
			For Each f In fx.Current
				engine.AddRegion(f)
			Next

			img.Image = engine.ToBitmap()
			lbl.Text = $"Progress: {engine.FrameID}/{engine.FrameCount}"
		End If
	End Sub

	Private Function GetOuterBounds(f As Fish) As Rectangle
		Dim r As New OpenCvSharp.RotatedRect(f.Center.ToPoint2f(), New OpenCvSharp.Size2f(f.Width, f.Height), f.Angle * 180 / Math.PI)
		Return r.BoundingRect().ToRectangle()
	End Function

	Private Sub RenderFishes(g As Graphics)
		For Each f In fx.Current
			g.TranslateTransform(f.Center.X, f.Center.Y)

			g.RotateTransform(f.Angle * 180 / Math.PI)
			g.DrawEllipse(Pens.Blue, -f.Width / 2, -f.Height / 2, f.Width, f.Height)
			g.RotateTransform(-f.Angle * 180 / Math.PI)

			Dim del = img.PointToClient(MousePosition) - f.Center
			Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
			Dim min = Math.Min(f.Width, f.Height)
			Dim max = Math.Max(f.Width, f.Height)

			g.DrawEllipse(Pens.Yellow, -min / 2, -min / 2, min, min)

			If dis < min / 2 Then
				g.FillEllipse(New SolidBrush(Color.FromArgb(128, Color.Gold)), -min / 2, -min / 2, min, min)
			End If

			g.TranslateTransform(-f.Center.X, -f.Center.Y)

			g.DrawRectangle(Pens.HotPink, GetOuterBounds(f))
			g.FillEllipse(Brushes.PaleGreen, f.Start.X - 3, f.Start.Y - 3, 6, 6)
			g.DrawLine(Pens.Orange, f.Start, f.End)
		Next
	End Sub

	Private Sub RenderFlow(g As Graphics)
		Dim pts = engine.GetFlowPoints()
		For i = 0 To pts.[new].Count - 1
			Dim pt = pts.[new](i)
			Dim ptOld = pts.old(i)
			g.FillEllipse(Brushes.White, ptOld.X - 1, ptOld.Y - 1, 2, 2)
			g.FillEllipse(Brushes.Black, pt.X - 1, pt.Y - 1, 2, 2)
			g.DrawLine(Pens.Gray, ptOld.X, ptOld.Y, pt.X, pt.Y)
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
		If btnTrack.Checked AndAlso fx.Current.Count < 3 Then
			RenderFlow(g)
		End If

		If creating Then
			g.DrawLine(Pens.Red, start, ender)
			'g.DrawRectangle(Pens.Green, New Fish(start, ender, Val(txtAspect.Text)).Bounding)
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

		Dim f = New Fish(start, ender, Val(txtAspect.Text))

		fx.Current.Add(f)

		If btnTrack.Checked Then
			engine.AddRegion(f)
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

			For Each f In fx.Current
				Dim del = mp - f.Center
				Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
				Dim min = Math.Min(f.Width, f.Height)
				Dim max = Math.Max(f.Width, f.Height)
				If dis < min / 2 Then
					fx.Current.Remove(f)
					engine.RemoveRegion(f)
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
		Text = Text + " " + Application.ProductVersion
		CheckForUpdate()
	End Sub

	Private Sub btnTrack_Click(sender As Object, e As EventArgs) Handles btnTrack.Click
		btnTrack.Checked = Not btnTrack.Checked
		btnTrack.Text = "Tracking " + If(btnTrack.Checked, "Enabled", "Disabled")
		If btnTrack.Checked Then
			engine.StartTracking()
			For Each f In fx.Current
				engine.AddRegion(f)
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
