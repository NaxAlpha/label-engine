Imports System.IO

Public Class Fish
	Public Property Start As Point
	Public Property [End] As Point
	Public Property Aspect As Single
	Public ReadOnly Property Bounding As Rectangle
		Get
			Dim sx = Math.Min(Start.X, [End].X)
			Dim sy = Math.Min(Start.Y, [End].Y)
			Dim ex = Math.Max(Start.X, [End].X)
			Dim ey = Math.Max(Start.Y, [End].Y)
			Return New Rectangle(sx, sy, ex - sx, ey - sy)
		End Get
	End Property
	Public ReadOnly Property Center As Point
		Get
			Return New Point((Start.X + [End].X) / 2, (Start.Y + [End].Y) / 2)
		End Get
	End Property
	Public ReadOnly Property Angle As Single
		Get
			Return Math.Atan2([End].Y - Start.Y, [End].X - Start.X)
		End Get
	End Property
	Public ReadOnly Property Width As Single
		Get
			Return Math.Sqrt(([End].X - Start.X) ^ 2 + ([End].Y - Start.Y) ^ 2)
		End Get
	End Property
	Public ReadOnly Property Height As Single
		Get
			Return Width * Aspect
		End Get
	End Property
End Class
Public Class FishCollection
	Inherits List(Of List(Of Fish))

	Private id As Integer = 0
	Public ReadOnly Property Current As List(Of Fish)
		Get
			If Me.Count = id Then
				Me.Add(New List(Of Fish))
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
					fx.Write(fish.Start.X.ToString() + " ")
					fx.Write(fish.Start.Y.ToString() + " ")
					fx.Write(fish.End.X.ToString() + " ")
					fx.Write(fish.End.Y.ToString() + " ")
					fx.Write(fish.Aspect.ToString() + " ")
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
					Add(New List(Of Fish))
				End If
				Dim nItems = 5
				For i = 0 To nx - 1
					Current.Add(New Fish With {
								.Start = New Point(w(2 + i * nItems), w(3 + i * nItems)),
								.End = New Point(w(4 + i * nItems), w(5 + i * nItems)),
								.Aspect = Val(w(6 + i * nItems))})
				Next
				Move()
			End While
		End Using
		Me.id = 0
	End Sub

	Public Sub Reset()
		id = 0
	End Sub

End Class