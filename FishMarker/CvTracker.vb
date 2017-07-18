Imports OpenCvSharp

Public Class CvTracker

	'Config

	Private nPointsToTrack As Integer = 100

	'Fields
	Private regions As New List(Of Rect)

	Private oldPoints As Point2f()
	Private newPoints As Point2f()

	Private current As New Mat
	Private previous As New Mat

	Public Sub Init(current As Mat)
		current.CopyTo(Me.current)
	End Sub

	Public Sub AddRegion(r As Rect)
		regions.Add(r)
	End Sub

	Public Sub ClearAll()
		regions.Clear()
	End Sub

	Public Sub RemoveRegion(at As Point2f)
		For Each p In regions
			If p.Contains(at) Then
				regions.Remove(p)
				Return
			End If
		Next
	End Sub

	Private Function GetPoints(r As Rect) As List(Of Point2f)
		Dim pts As New List(Of Point2f)
		Dim nPoints1d As Integer = Math.Sqrt(nPointsToTrack)

		Dim xstep As Single = (1.0 * r.Width) / nPoints1d
		Dim xoffset As Single = 0.5 * xstep
		Dim ystep As Single = (1.0 * r.Height) / nPoints1d
		Dim yoffset As Single = 0.5 * ystep

		For i As Integer = 0 To nPoints1d - 1
			For j As Integer = 0 To nPoints1d - 1
				Dim xPos As Single = r.X + (xstep * j) + xoffset
				Dim yPos As Single = r.Y + (ystep * i) + yoffset
				pts.Add(New Point2f(xPos, yPos))
			Next
		Next
		Return pts
	End Function

	Private Function CalcDisplacement(v1 As Point2f(), v2 As Point2f(), status As Byte()) As List(Of Point2f)
		Dim di As New List(Of Point2f)
		Dim nTracked As Integer = 0
		For i As Integer = 0 To v1.Count - 1
			If status(i) = 1 Then
				di.Add(v2(i) - v1(i))
				nTracked += 1
			End If
		Next
		Return di
	End Function

	Private Function GetMedian(tx As List(Of Single), Optional count As Integer = -1) As Single
		If count < 0 Then
			count = tx.Count
		End If
		If count = 0 Then
			Return 0
		End If

		Dim tmp As New List(Of Single)(tx.Take(count))
		tmp.Sort()

		If tmp.Count Mod 2 = 0 Then
			Dim cbt = tmp.Count / 2
			Return (tmp(cbt - 1) + tmp(cbt)) / 2
		Else
			Return tmp((tmp.Count - 1) / 2)
		End If
	End Function

	Private Function ValidateFB() As List(Of Byte)
		Dim status As Byte() = New Byte(oldPoints.Length - 1) {}
		Dim err As Single() = New Single(oldPoints.Length - 1) {}
		Dim mProj As Point2f() = New Point2f(oldPoints.Length - 1) {}
		Dim mNew As Point2f() = newPoints.ToArray()

		Dim fberr As New List(Of Single)

		Cv2.CalcOpticalFlowPyrLK(current, previous, mNew, mProj, status, err) 'Need to specify params

		For i = 0 To oldPoints.Count - 1
			fberr.Add((oldPoints(i) - mProj(i)).DistanceTo(New Point2f))
		Next

		Dim med = GetMedian(fberr)

		For i = 0 To status.Length - 1
			status(i) = (fberr(i) < med)
		Next

		Return status.ToList()
	End Function

	Private Sub ValidateNCC(status As Byte())
		Dim NCC As New List(Of Single)

		Dim patch As New Size(30, 30)
		Dim patchLength As Integer = 30 * 30

		Dim p1 As New Mat, p2 As New Mat

		For i = 0 To oldPoints.Count - 1
			Cv2.GetRectSubPix(previous, patch, oldPoints(i), p1)
			Cv2.GetRectSubPix(current, patch, newPoints(i), p2)

			Dim s1 As Double = Cv2.Sum(p1)(0), s2 As Double = Cv2.Sum(p2)(0)
			Dim n1 As Double = Cv2.Norm(p1), n2 As Double = Cv2.Norm(p2)
			Dim prod As Double = p1.Dot(p2)
			Dim sq1 As Double = Math.Sqrt(n1 * n1 - s1 * s1 / patchLength), sq2 As Double = Math.Sqrt(n2 * n2 - s2 * s2 / patchLength)
			Dim ares As Double = If((sq2 = 0), sq1 / Math.Abs(sq1), (prod - s1 * s2 / patchLength) / sq1 / sq2)

			NCC.Add(ares)
		Next
		Dim NCCmedian As Single = GetMedian(NCC)

		For i = 0 To oldPoints.Count - 1
			status(i) = status(i) AndAlso (NCC(i) > NCCmedian)
		Next
	End Sub

	Private Function ComputeNewRect(old As Rect, ByRef disp As Point2f) As Rect
		Dim newCenter As New Point2d(old.X + old.Width / 2.0, old.Y + old.Height / 2.0)
		Dim n As Integer = oldPoints.Count
		Dim buf = New Single(Math.Max(n * (n - 1) / 2, 3) - 1) {}
		Dim rNew As New Rect
		If oldPoints.Count = 1 Then
			rNew.X = old.X + newPoints(0).X - oldPoints(0).X
			rNew.Y = old.Y + newPoints(0).Y - oldPoints(0).Y
			rNew.Width = old.Width
			rNew.Height = old.Height
			Return rNew
		End If

		For j = 0 To n - 1
			buf(j) = newPoints(j).X - oldPoints(j).X
		Next
		Dim xshift As Double = GetMedian(buf.ToList(), n)
		newCenter.X += xshift

		For j As Integer = 0 To n - 1
			buf(j) = newPoints(j).Y - oldPoints(j).Y
		Next
		Dim yshift As Double = GetMedian(buf.ToList(), n)
		newCenter.Y += yshift

		Dim nd As Double, od As Double
		Dim i As Integer = 0, ctr As Integer = 0
		While i < n
			For j As Integer = 0 To i - 1
				nd = (newPoints(i) - newPoints(j)).DistanceTo(New Point2f)
				od = (oldPoints(i) - oldPoints(j)).DistanceTo(New Point2f)
				buf(ctr) = If((od = 0.0), 0.0, (nd / od))
				ctr += 1
			Next
			i += 1
		End While

		Dim scale As Double = GetMedian(buf.ToList, n * (n - 1) / 2)
		scale = Math.Min(1.1, Math.Max(scale, 0.9))

		rNew.X = newCenter.X - scale * old.Width / 2.0
		rNew.Y = newCenter.Y - scale * old.Height / 2.0
		rNew.Width = scale * old.Width
		rNew.Height = scale * old.Height

		disp = New Point2f(xshift, yshift)
		Return rNew
	End Function

	Private Function ShiftPoints(pt As List(Of Point2f), del As Point2f) As List(Of Single)
		Dim disp As New List(Of Single)
		For i = 0 To pt.Count - 1
			pt(i) -= del
			disp.Add(Math.Sqrt(pt(i).DotProduct(pt(i))))
		Next
		Return disp
	End Function

	Public Sub Track(nextFrame As Mat)
		current.CopyTo(previous)
		nextFrame.CopyTo(current)

		Dim removeables As New List(Of Integer)

		For i = 0 To regions.Count - 1
			Dim r = regions(i)

			oldPoints = GetPoints(r).ToArray()
			Dim stat As Byte() = New Byte(oldPoints.Length - 1) {}
			Dim errs As Single() = New Single(oldPoints.Length - 1) {}
			newPoints = New Point2f(oldPoints.Length - 1) {}

			Cv2.CalcOpticalFlowPyrLK(previous, current, oldPoints, newPoints, stat, errs)

			Dim disp = CalcDisplacement(oldPoints, newPoints, stat)
			Dim filt = ValidateFB()
			ValidateNCC(filt.ToArray())

			Dim del As Point2f
			Dim rNew = ComputeNewRect(r, del)

			Dim disx = ShiftPoints(disp, del)

			If GetMedian(disx) > 10 Then
				removeables.Add(i)
				Continue For
			End If

			regions(i) = rNew
		Next
	End Sub

	Public Function ListRegions() As IEnumerable(Of Rect)
		Return regions
	End Function

End Class
