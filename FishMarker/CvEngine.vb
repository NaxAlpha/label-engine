Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports OpenCvSharp
Imports OpenCvSharp.Extensions

Public Class CvEngine

	Private videoCapture As IntPtr = 0
	Private multiTracker As IntPtr = 0
	Private currentFrame As IntPtr = 0
	Private frameNumber As Integer = 0
	Private framesCount As Integer = 0

	Private frame As Mat

	Public ReadOnly Property IsOpened As Boolean
		Get
			Return (videoCapture <> 0)
		End Get
	End Property

	Public ReadOnly Property IsTracking As Boolean
		Get
			Return (multiTracker <> 0)
		End Get
	End Property

	Public ReadOnly Property FrameID As Integer
		Get
			Return frameNumber
		End Get
	End Property

	Public ReadOnly Property FrameCount As Integer
		Get
			Return framesCount
		End Get
	End Property

	Public Sub Close()
		If IsOpened Then
			CVX.CloseVideo(videoCapture)
			CVX.DestroyFrame(currentFrame)

			frameNumber = 0

			videoCapture = 0
			currentFrame = 0
		End If
	End Sub

	Public Sub Open(fileName As String)
		If IsOpened Then
			Close()
		End If
		frameNumber = 0
		currentFrame = CVX.CreateFrame()
		frame = New Mat(currentFrame)
		videoCapture = CVX.OpenVideo(fileName)
		framesCount = CVX.VideoLength(videoCapture)
	End Sub

	Public Sub MoveNext()
		If IsOpened Then
			CVX.ReadFrame(videoCapture, currentFrame)
			frameNumber += 1
		End If
	End Sub

	Public Function ToBitmap() As Bitmap
		Try
			Return frame.ToBitmap()
		Catch ex As Exception
			Return Nothing
		End Try
	End Function

	Public Sub StartTracking()
		If Not IsOpened Then Return
		If IsTracking Then Return
		multiTracker = CVX.StartTracking()
	End Sub

	Public Sub Track()
		If IsTracking Then
			CVX.UpdateTracker(multiTracker, currentFrame)
		End If
	End Sub

	Public Sub StopTracking()
		If IsTracking Then
			CVX.StopTracking(multiTracker)
		End If
	End Sub

	Public Sub AddRegion(region As Rectangle)
		If IsTracking Then
			CVX.CreateTracker(multiTracker, currentFrame, region)
		End If
	End Sub

	Public Sub RemoveRegion(x As Integer, y As Integer)
		If IsTracking Then
			multiTracker = CVX.DestroyTracker(multiTracker, currentFrame, x, y)
		End If
	End Sub

	Public Function ListRegions() As Rectangle()
		Dim count As Integer
		Dim ptr = CVX.GetObjectList(multiTracker, count)
		Dim regions(count - 1) As Rectangle
		Dim sz = Marshal.SizeOf(Of Rectangle)
		For i = 0 To count - 1
			regions(i) = Marshal.PtrToStructure(Of CvRect)(ptr + i * sz)
		Next
		CVX.DeleteObjectList(ptr)
		Return regions
	End Function

	<DllImport("kernel32.dll", SetLastError:=True, EntryPoint:="CopyMemory")>
	Private Shared Sub CopyMemory(destination As IntPtr, source As IntPtr, length As UInteger)
	End Sub

End Class
