Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports OpenCvSharp
Imports OpenCvSharp.Extensions

Public Class CvEngine

	Private cap As VideoCapture
	Private frame As New Mat
	Private trc As New CvTracker

	Public ReadOnly Property IsOpened As Boolean
		Get
			If cap IsNot Nothing Then Return cap.IsOpened() Else Return False
		End Get
	End Property

	Public ReadOnly Property FrameID As Integer
		Get
			If cap IsNot Nothing Then Return cap.PosFrames Else Return 0
		End Get
	End Property

	Public ReadOnly Property FrameCount As Integer
		Get
			If cap IsNot Nothing Then Return cap?.Get(CaptureProperty.FrameCount) Else Return False
		End Get
	End Property

	Public Sub Close()
		cap?.Release()
	End Sub

	Public Sub Open(fileName As String)
		Close()
		cap = VideoCapture.FromFile(fileName)
	End Sub

	Public Sub MoveNext()
		cap?.Read(frame)
	End Sub

	Public Function ToBitmap() As Bitmap
		Try
			Return frame.ToBitmap()
		Catch ex As Exception
			Return Nothing
		End Try
	End Function

	Public Sub StartTracking()
		trc.Init(frame)
	End Sub

	Public Sub StopTracking()
		trc.ClearAll()
	End Sub

	Public Sub Track()
		trc.Track(frame)
	End Sub

	Public Sub AddRegion(region As Rectangle)
		trc.AddRegion(R2RR(region))
	End Sub

	Public Sub RemoveRegion(x As Integer, y As Integer)
		trc.RemoveRegion(New Point2f(x, y))
	End Sub

	Public Function ListRegions() As Rectangle()
		Return trc.ListRegions().Select(Function(r) R2R(r)).ToArray()
	End Function

	Private Function R2R(r As Rect) As Rectangle
		Return New Rectangle(r.X, r.Y, r.Width, r.Height)
	End Function

	Private Function R2RR(r As Rectangle) As Rect
		Return New Rect(r.X, r.Y, r.Width, r.Height)
	End Function

End Class
