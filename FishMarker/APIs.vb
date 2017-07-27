Imports System.IO
Imports System.Runtime.CompilerServices
Imports OpenCvSharp

Module PointExtensions

	<Extension>
	Public Function ToPoint2f(this As Drawing.Point) As Point2f
		Return New Point2f(this.X, this.Y)
	End Function

	<Extension>
	Public Function ToPoint(this As Point2f) As Drawing.PointF
		Return New Drawing.Point(this.X, this.Y)
	End Function

	<Extension>
	Public Function ToRect(this As RectangleF) As Rect
		Return New Rect(this.X, this.Y, this.Width, this.Height)
	End Function

	<Extension>
	Public Function ToRectangle(this As Rect) As RectangleF
		Return New RectangleF(this.X, this.Y, this.Width, this.Height)
	End Function

	<Extension>
	Public Function Shift(this As Point2f, delta As Point2f) As Point2f
		Return this - delta
	End Function

	<Extension>
	Public Function Rotate(this As Point2f, angle As Single) As Point2f
		Return New Point2f(this.X * Math.Cos(angle) - this.Y * Math.Sin(angle),
						   this.X * Math.Sin(angle) + this.Y * Math.Cos(angle))
	End Function

	<Extension>
	Public Function ToRectangle(this As RectangleF) As Rectangle
		Return New Rectangle(this.X, this.Y, this.Width, this.Height)
	End Function

End Module
Public Class FishCollection
	Inherits List(Of List(Of RectangleF))

	Private id As Integer = 0

	Public ReadOnly Property Current As List(Of RectangleF)
		Get
			If Count = id Then
				Add(New List(Of RectangleF))
			End If
			Return Me(id)
		End Get
	End Property

	Public Sub Move()
		Dim z = Current
		id += 1
	End Sub

	Public Sub Save(out As String)
		Using fx As New StreamWriter(out)
			Dim id = 0
			For Each fishes In Me
				fx.Write(id.ToString() + " ")
				fx.Write(fishes.Count.ToString() + " ")
				For Each fish In fishes
					fx.Write(fish.X.ToString() + " ")
					fx.Write(fish.Y.ToString() + " ")
					fx.Write(fish.Right.ToString() + " ")
					fx.Write(fish.Bottom.ToString() + " ")
				Next
				fx.WriteLine()
				id += 1
			Next
		End Using
	End Sub

	Public Sub Load(inp As String)
		Me.Clear()
		Me.id = 0
		Using fx As New StreamReader(inp)
			While Not fx.EndOfStream
				Dim ln = fx.ReadLine()
				If ln = "" Then Continue While
				Dim w = ln.Split(" ").Select(Function(s) Val(s)).ToArray()
				Dim id = w(0)
				Dim nx = w(1)
				If nx = 0 Then
					Add(New List(Of RectangleF))
				End If
				Dim nItems = 4
				For i = 0 To nx - 1
					Dim x = w(2 + i * nItems)
					Dim y = w(3 + i * nItems)
					Dim r = w(4 + i * nItems)
					Dim b = w(5 + i * nItems)
					Current.Add(New RectangleF(x, y, r - x, b - y))
				Next
				Move()
			End While
		End Using
		id = 0
	End Sub

	Public Sub Reset()
		id = 0
	End Sub

End Class