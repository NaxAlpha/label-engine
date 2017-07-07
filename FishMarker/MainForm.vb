Public Class MainForm

	Private fishes As New List(Of Fish)

	Private videoFile As String = ""
	Private fishFile As String = ""

	Dim start As Point
	Dim ender As Point
	Dim being As Boolean

	Private Sub img_Paint(sender As Object, e As PaintEventArgs) Handles img.Paint
		Dim g = e.Graphics
		g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
		g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
		g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
		g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

		For Each f In fishes
			g.TranslateTransform(f.Center.X, f.Center.Y)
			g.DrawEllipse(Pens.Green, New RectangleF(New Point(-2, -2), New Size(4, 4)))

			g.RotateTransform(f.Angle * 180 / Math.PI)
			g.DrawEllipse(Pens.Blue, -f.Width / 2, -f.Height / 2, f.Width, f.Height)
			g.RotateTransform(-f.Angle * 180 / Math.PI)

			Dim del = img.PointToClient(MousePosition) - f.Center
			Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
			Dim min = Math.Min(f.Width, f.Height)
			Dim max = Math.Max(f.Width, f.Height)

			If dis <= min / 2 Then
				g.FillEllipse(New SolidBrush(Color.FromArgb(128, Color.Yellow)), New RectangleF(-min / 2, -min / 2, min, min))
				'g.FillEllipse(New SolidBrush(Color.FromArgb(128, Color.Tomato)), New RectangleF(-max / 2, -max / 2, max, max))
			End If

			g.TranslateTransform(-f.Center.X, -f.Center.Y)
		Next

		If being Then
			g.DrawLine(Pens.Red, start, ender)
			Dim center = (start + ender)
			center.X /= 2 : center.Y /= 2
			Dim diff = ender - start
			Dim dist = Math.Sqrt(diff.X ^ 2 + diff.Y ^ 2)
			Dim heis = dist * Val(txtHeight.Text)
			Dim widt = dist * (1 + Val(txtWidth.Text))

			g.TranslateTransform(center.X, center.Y)
			g.DrawEllipse(Pens.Green, New RectangleF(New Point(-2, -2), New Size(4, 4)))

			Dim ang = Math.Atan2(diff.Y, diff.X)
			g.RotateTransform(ang * 180 / Math.PI)

			g.DrawEllipse(Pens.Blue, CSng(-widt / 2), CSng(-heis / 2), CSng(widt), CSng(heis))
		End If
	End Sub

	Private Sub tmr_Tick(sender As Object, e As EventArgs) Handles tmr.Tick
		img.Invalidate()
	End Sub

	Private Sub img_MouseDown(sender As Object, e As MouseEventArgs) Handles img.MouseDown
		If e.Button <> MouseButtons.Left Then Return
		start = e.Location
		ender = e.Location
		being = True
	End Sub

	Private Sub img_MouseUp(sender As Object, e As MouseEventArgs) Handles img.MouseUp
		If e.Button <> MouseButtons.Left Then Return

		ender = e.Location
		being = False

		Dim center = (start + ender)
		center.X /= 2 : center.Y /= 2
		Dim diff = ender - start
		Dim dist = Math.Sqrt(diff.X ^ 2 + diff.Y ^ 2)
		Dim heis = dist * Val(txtHeight.Text)
		Dim widt = dist * (1 + Val(txtWidth.Text))
		Dim ang = Math.Atan2(diff.Y, diff.X)

		fishes.Add(New Fish With {.Angle = ang, .Center = center, .Height = heis, .Width = widt})
	End Sub

	Private Sub img_MouseMove(sender As Object, e As MouseEventArgs) Handles img.MouseMove
		If being Then
			ender = e.Location
		End If
	End Sub

	Private Sub txtWidth_TextChanged(sender As Object, e As EventArgs) Handles txtWidth.TextChanged
		txtWidth.Text = Val(txtWidth.Text)
	End Sub

	Private Sub txtHeight_TextChanged(sender As Object, e As EventArgs) Handles txtHeight.TextChanged
		txtHeight.Text = Val(txtHeight.Text)
	End Sub

	Private Sub img_MouseClick(sender As Object, e As MouseEventArgs) Handles img.MouseClick
		If e.Button = MouseButtons.Right Then

			For Each f In fishes
				Dim del = img.PointToClient(MousePosition) - f.Center
				Dim dis = Math.Sqrt(del.X ^ 2 + del.Y ^ 2)
				Dim min = Math.Min(f.Width, f.Height)
				Dim max = Math.Max(f.Width, f.Height)

				If dis <= min / 2 Then
					fishes.Remove(f)
					Exit For
				End If

			Next

		End If
	End Sub

	Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

	End Sub
End Class

Public Class Fish
	Public Property Center As Point
	Public Property Angle As Single
	Public Property Width As Single
	Public Property Height As Single
End Class