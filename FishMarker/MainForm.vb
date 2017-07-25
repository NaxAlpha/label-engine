Imports System.IO
Imports System.IO.Compression
Imports System.Net
Imports System.Threading

Public Class MainForm

	Private app As New LabelApp

	Dim start As Point
	Dim ender As Point
	Dim creating As Boolean

	Dim aspect As Double = 0

	Private Async Function DownloadFile(url As String) As Task(Of String)
		Dim req As HttpWebRequest = WebRequest.Create(url)
		req.Accept = "application/vnd.github.v3+json"
		req.UserAgent = "Cool"
		Using res = req.GetResponse(), r = New StreamReader(res.GetResponseStream())
			Return Await r.ReadToEndAsync()
		End Using
	End Function

	Private Async Function DownloadData(url As String) As Task(Of Byte())
		Using wc As New WebClient()
			wc.Headers.Add("User-Agent", "Cool")
			'wc.Headers.Add("Accept", "application/vnd.github.v3+json")
			Return Await wc.DownloadDataTaskAsync(New Uri(url))
		End Using
	End Function

	Private Async Function AppUpdate(url As String) As Task
		lblUpdate.Text = "Downloading..."
		Dim appdata = Await DownloadData(url)
		Dim tmp = Path.GetTempFileName()
		File.WriteAllBytes(tmp, appdata)
		Dim tmp2 = Path.GetTempPath() + "LabelEngine-Update"
		If Directory.Exists(tmp2) Then
			Directory.Delete(tmp2, True)
		End If
		Directory.CreateDirectory(tmp2)
		lblUpdate.Text = "Extracting..."
		ZipFile.ExtractToDirectory(tmp, tmp2)
		File.Delete(tmp)
		lblUpdate.Text = "App restarts in 3 seconds..."
		Dim tmp3 = Path.GetTempPath() + "LabelEngine.cmd"
		Dim cmds = {"echo off",
			"cls",
			"echo Waiting for App to close...",
			"timeout 3",
			"echo Copying files",
			$"xcopy ""{tmp2}"" ""{Directory.GetCurrentDirectory()}"" /Y /E",
			"echo Starting app...",
			$"cd ""{Application.StartupPath}""",
			$"start /w /b {Path.GetFileName(Application.ExecutablePath)}",
			"exit"}
		File.WriteAllLines(tmp3, cmds)
		Using p = Process.Start(tmp3)
		End Using
		End
	End Function

	Private Async Sub CheckForUpdate()
		Await Task.Delay(1000)
		Try
			Dim txt = Await DownloadFile("https://api.github.com/repos/NaxAlpha/label-engine/releases/latest")
			Dim obj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(txt)
			Dim tag = obj("tag_name").ToString()
			Dim newver = Val(tag.Split("."c, "-"c)(2))
			Dim build = Val(Application.ProductVersion.Split("."c, "-"c)(2))
			If build < newver Then
				lblUpdate.Text = "Newer version is avaiable, Press Update button to update!"
			Else
				lblUpdate.Text = "You are using latest version of this software!"
				btnUpdate.Enabled = False
			End If
		Catch ex As Exception
			lblUpdate.Text = "Failed to check for update!"
		End Try
	End Sub

	Private Function GetOuterBounds(f As Fish) As Rectangle
		Dim r As New OpenCvSharp.RotatedRect(f.Center.ToPoint2f(), New OpenCvSharp.Size2f(f.Width, f.Height), f.Angle * 180 / Math.PI)
		Return r.BoundingRect().ToRectangle()
	End Function

	Private Sub RenderFishes(g As Graphics)
		For Each f In app.ListFishes()
			f = Transform(f)
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

	Private Sub img_Paint(sender As Object, e As PaintEventArgs) Handles img.Paint
		Dim g = e.Graphics
		g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
		g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
		g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
		g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

		RenderFishes(g)

		If creating Then
			g.DrawLine(Pens.Red, start, ender)
			'g.DrawRectangle(Pens.Green, New Fish(start, ender, Val(txtAspect.Text)).Bounding)
		End If
	End Sub

	Private Sub img_MouseDown(sender As Object, e As MouseEventArgs) Handles img.MouseDown
		If e.Button <> MouseButtons.Left OrElse Not app.IsLoaded Then Return
		start = e.Location
		ender = e.Location
		creating = True
	End Sub

	Private Sub img_MouseUp(sender As Object, e As MouseEventArgs) Handles img.MouseUp
		If Not creating OrElse e.Button <> MouseButtons.Left OrElse Not app.IsLoaded Then Return

		ender = e.Location
		creating = False

		Dim f = iTransform(New Fish(start, ender, 2 * progAr.Value / progAr.Maximum))
		If f.Width < 20 Then
			Return
		End If
		app.AddFish(f)
	End Sub

	Private Sub img_MouseMove(sender As Object, e As MouseEventArgs) Handles img.MouseMove
		If creating Then
			ender = e.Location
		End If
	End Sub

	Private Sub img_MouseClick(sender As Object, e As MouseEventArgs) Handles img.MouseClick
		If e.Button = MouseButtons.Right Then
			Dim mp = img.PointToClient(MousePosition)
			app.RemoveByPoint(iTransform(mp).ToPoint2f())
		End If
	End Sub

	Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
		Using ofd As New OpenFileDialog
			If ofd.ShowDialog() = DialogResult.OK Then
				If app.IsLoaded Then
					app.UnloadVideo()
				End If
				app.LoadVideo(ofd.FileName)
				progFid.Maximum = app.FrameCount
				lblProg.Text = $"Progress: {0}/{progFid.Maximum}"
				app.SkipToFrame(1)
				img.Image = app.CurrentFrame()
				app.TrackingEnabled = True
			End If
		End Using
	End Sub

	Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
		If Not app.IsLoaded Then Return
		app.ReadFrame()
		app.Save()
		Try
			img.Image = app.CurrentFrame()
		Catch ex As Exception
			img.Image = Nothing
		End Try
	End Sub

	Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		Text = Text + " " + Application.ProductVersion
		CheckForUpdate()
		app.TrackingEnabled = True
	End Sub

	Private Sub btnTrack_Click(sender As Object, e As EventArgs) Handles btnTrack.Click
		btnTrack.Checked = Not btnTrack.Checked
		btnTrack.Text = "Tracking " + If(btnTrack.Checked, "Enabled", "Disabled")
		app.TrackingEnabled = btnTrack.Checked
	End Sub

	Private Sub tmr_Tick(sender As Object, e As EventArgs) Handles tmr.Tick
		If img.Height <> mainPanel.Height - 25 Then
			img.Height = mainPanel.Height - 25
		End If
		If img.Image IsNot Nothing Then
			aspect = img.Height / img.Image.Height
			img.Width = img.Image.Width * aspect
		End If
		progFid.Value = app.FrameID
		lblAr.Text = $"Aspect Ratio: {2 * progAr.Value / progAr.Maximum}"
		lblProg.Text = $"Progress: {progFid.Value}/{progFid.Maximum}"
		img.Invalidate()
	End Sub

	Private Sub progFid_MouseMoveOrUp(sender As Object, e As MouseEventArgs) Handles progFid.MouseMove, progFid.MouseUp
		If e.Button = MouseButtons.Left Then
			Dim mp = progFid.Control.PointToClient(MousePosition)
			Dim vl = mp.X * progFid.Maximum / progFid.Width
			If vl <= 0 OrElse vl > progFid.Maximum - 2 Then Return
			progFid.Value = vl
			app.SkipToFrame(progFid.Value)
			Try
				img.Image = app.CurrentFrame()
			Catch ex As Exception
				img.Image = Nothing
			End Try
		End If
	End Sub

	Private Sub progAr_MouseMoveOrDown(sender As Object, e As MouseEventArgs) Handles progAr.MouseMove, progAr.MouseDown
		If e.Button = MouseButtons.Left Then
			Dim mp = progAr.Control.PointToClient(MousePosition)
			Dim vl = mp.X * progAr.Maximum / progAr.Width
			If vl < 0 OrElse vl >= progAr.Maximum Then Return
			progAr.Value = vl
		End If
	End Sub

	Private Function Transform(pt As Point) As Point
		Return New Point(pt.X * aspect, pt.Y * aspect)
	End Function

	Private Function iTransform(pt As Point) As Point
		Return New Point(pt.X / aspect, pt.Y / aspect)
	End Function

	Private Function Transform(f As Fish) As Fish
		Return New Fish(Transform(f.Start), Transform(f.End), f.Aspect)
	End Function

	Private Function iTransform(f As Fish) As Fish
		Return New Fish(iTransform(f.Start), iTransform(f.End), f.Aspect)
	End Function

	Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
		app.PreviousFrame()
		Try
			img.Image = app.CurrentFrame()
		Catch ex As Exception
			img.Image = Nothing
		End Try
	End Sub

	Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
		app.ListFishes().Clear()
	End Sub

	Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		app.Save()
	End Sub

	Private Sub txtFid_Click(sender As Object, e As EventArgs) Handles txtFid.Click
		txtFid.Text = Val(txtFid.Text)
	End Sub

	Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
		app.SkipToFrame(Val(txtFid.Text))
	End Sub

	Private Async Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
		btnUpdate.Enabled = False
		Try
			Dim txt = Await DownloadFile("https://api.github.com/repos/NaxAlpha/label-engine/releases/latest")
			Dim obj As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(txt)
			Dim tag = obj("tag_name").ToString()
			Dim newver = Val(tag.Split("."c, "-"c)(2))
			Dim build = Val(Application.ProductVersion.Split("."c, "-"c)(2))
			If build < newver Then
				lblUpdate.Text = "Newer version is avaiable, Updating please wait..."
				Dim assets As Newtonsoft.Json.Linq.JArray = obj("assets")
				Dim firstAsset = assets(0)
				Dim downloadUrl = firstAsset("browser_download_url").ToString()
				Await AppUpdate(downloadUrl)
			Else
				lblUpdate.Text = "You are using latest version of this software!"
			End If
		Catch ex As Exception
			lblUpdate.Text = "Failed to check for update!"
		End Try
		btnUpdate.Enabled = True
	End Sub

End Class
