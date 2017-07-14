Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class CvEngine

	Private videoCapture As IntPtr = 0
	Private multiTracker As IntPtr = 0
	Private currentFrame As IntPtr = 0
	Private frameNumber As Integer = 0

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

	Public ReadOnly Property FrameCount As Integer
		Get
			Return frameNumber
		End Get
	End Property

	Public Sub Close()
		If IsOpened Then
			CVX.CloseVideo(videoCapture)
			CVX.DestroyFrame(currentFrame)
			frameNumber = 0
		End If
	End Sub

	Public Sub Open(fileName As String)
		If IsOpened Then
			Close()
		End If
		frameNumber = 0
		currentFrame = CVX.CreateFrame()
		videoCapture = CVX.OpenVideo(fileName)
	End Sub

	Public Sub MoveNext()
		If IsOpened Then
			CVX.ReadFrame(videoCapture, currentFrame)
		End If
	End Sub

	Public Sub StartTracking()
		If Not IsOpened Then Return
		If IsTracking Then Return
		multiTracker = CVX.StartTracking()
	End Sub

	Public Sub StopTracking()
		If IsTracking Then
			CVX.StopTracking(multiTracker)
		End If
	End Sub

	<DllImport("kernel32.dll", SetLastError:=True, EntryPoint:="CopyMemory")>
	Private Shared Sub CopyMemory(destination As IntPtr, source As IntPtr, length As UInteger)
	End Sub

End Class
