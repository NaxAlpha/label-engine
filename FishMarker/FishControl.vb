Imports System.Runtime.InteropServices

Public Class FishControl

	Public Sub New(fish As RectangleF)

		InitializeComponent()

		Me.SizeMode = PictureBoxSizeMode.Normal
		Left = fish.Left
		Top = fish.Top
		Width = fish.Width
		Height = fish.Height
	End Sub

	Public Const WM_NCLBUTTONDOWN As Integer = &HA1
	Public Const HT_CAPTION As Integer = &H2

	<DllImport("user32.dll")>
	Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
	End Function
	<DllImport("user32.dll")>
	Public Shared Function ReleaseCapture() As Boolean
	End Function

	Protected Overrides Sub WndProc(ByRef m As Message)
		Const WM_NCHITTEST As UInt32 = &H84
		Const WM_MOUSEMOVE As UInt32 = &H200

		Const HTLEFT As UInt32 = 10
		Const HTRIGHT As UInt32 = 11
		Const HTBOTTOMRIGHT As UInt32 = 17
		Const HTBOTTOM As UInt32 = 15
		Const HTBOTTOMLEFT As UInt32 = 16
		Const HTTOP As UInt32 = 12
		Const HTTOPLEFT As UInt32 = 13
		Const HTTOPRIGHT As UInt32 = 14

		Const RESIZE_HANDLE_SIZE As Integer = 5
		Dim handled As Boolean = False

		If m.Msg = WM_NCHITTEST OrElse m.Msg = WM_MOUSEMOVE Then
			Dim formSize As Size = Me.Size
			Dim screenPoint As New Point(m.LParam.ToInt32())
			Dim clientPoint As Point = Me.PointToClient(screenPoint)

			Dim boxes As New Dictionary(Of UInt32, Rectangle)() From {
			{HTBOTTOMLEFT, New Rectangle(0, formSize.Height - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
			{HTBOTTOM, New Rectangle(RESIZE_HANDLE_SIZE, formSize.Height - RESIZE_HANDLE_SIZE, formSize.Width - 2 * RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
			{HTBOTTOMRIGHT, New Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, formSize.Height - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
			{HTRIGHT, New Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - 2 * RESIZE_HANDLE_SIZE)},
			{HTTOPRIGHT, New Rectangle(formSize.Width - RESIZE_HANDLE_SIZE, 0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
			{HTTOP, New Rectangle(RESIZE_HANDLE_SIZE, 0, formSize.Width - 2 * RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
			{HTTOPLEFT, New Rectangle(0, 0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE)},
			{HTLEFT, New Rectangle(0, RESIZE_HANDLE_SIZE, RESIZE_HANDLE_SIZE, formSize.Height - 2 * RESIZE_HANDLE_SIZE)}
		}
			For Each hitBox As KeyValuePair(Of UInt32, Rectangle) In boxes
				If hitBox.Value.Contains(clientPoint) Then
					m.Result = hitBox.Key
					handled = True
					Exit For
				End If
			Next
		End If

		If Not handled Then
			MyBase.WndProc(m)
		End If
	End Sub

	Protected Overrides Sub OnPaint(pe As PaintEventArgs)
		Dim g = pe.Graphics
		ButtonRenderer.DrawParentBackground(g, New Rectangle(0, 0, Width, Height), Me)
		g.DrawRectangle(New Pen(Color.Red, 2), New Rectangle(0, 0, Width, Height))
		MyBase.OnPaint(pe)
	End Sub

	Private Sub FishControl_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
		If e.Button = MouseButtons.Left Then
			ReleaseCapture()
			SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
		End If
	End Sub

	Private Sub FishControl_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
		If e.Button = MouseButtons.Right Then
			Dispose()
		End If
	End Sub

End Class
