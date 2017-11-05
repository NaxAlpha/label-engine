Imports System.IO
Imports OpenCvSharp
Imports OpenCvSharp.Extensions

Public Class LabelApp
	Implements IDisposable

#Region "Video Basics"

	Private capture As VideoCapture
	Private videoFile As String = ""
	Private fishFile As String = ""

	Public ReadOnly Property IsLoaded As Boolean
		Get
			Return capture IsNot Nothing
		End Get
	End Property

	Public ReadOnly Property FrameID As Integer
		Get
			If Not IsLoaded Then Return 0
			Return capture.PosFrames
		End Get
	End Property

	Public ReadOnly Property FrameCount As Integer
		Get
			If Not IsLoaded Then Return 0
			Return capture.Get(CaptureProperty.FrameCount) - 2
		End Get
	End Property

	Public Sub LoadVideo(fileName As String)
		If IsLoaded Then
			Throw New InvalidOperationException("Video already loaded!")
		End If
		videoFile = fileName
		fishFile = videoFile + ".box"
		If File.Exists(fishFile) Then fishes.Load(fishFile)
		capture = VideoCapture.FromFile(fileName)
	End Sub

	Public Sub UnloadVideo()
		capture?.Dispose()
		capture = Nothing
	End Sub

#End Region

#Region "Frame Basics"

	Private frame As New Mat

	Private Function BoundPoint(r As RectangleF) As RectangleF
		If r.X < 0 Then r.X = 0
		If r.X >= frame.Width Then r.Width = frame.Width - 1 - r.X
		If r.Y < 0 Then r.Y = 0
		If r.Bottom >= frame.Height Then r.Height = frame.Height - 1 - r.Y
		Return r
	End Function

	Public Sub ReadFrame()
		If FrameID >= FrameCount Then Return
		fishes.Move()
		capture.Read(frame)
	End Sub

	Public Sub ReloadVideo()
		If Not IsLoaded Then Return
		fishes.Reset()
		UnloadVideo()
		LoadVideo(videoFile)
	End Sub

	Public Sub SkipToFrame(fid As Integer)
		If IsLoaded Then
			If fid = FrameID Then Return
			If fid < FrameID Then ReloadVideo()
			While FrameID <> fid
				If FrameID = FrameCount Then Exit While
				capture.Read(frame)
				fishes.Move()
			End While
		End If
	End Sub

	Public Sub PreviousFrame()
		If FrameID > 0 Then
			SkipToFrame(FrameID - 1)
		End If
	End Sub

	Public Function CurrentFrame() As Bitmap
		Return frame.ToBitmap()
	End Function

#End Region

#Region "Fish Region"

	Private fishes As New FishCollection

	Public ReadOnly Property CurrentFrameCount As Integer
		Get
			Return fishes.Current.Count
		End Get
	End Property

	Public Sub AddFish(f As RectangleF)
		fishes.Current.Add(f)
	End Sub

	Public Sub RemoveByPoint(p As Point2f)
		For Each f In fishes.Current
			If f.Contains(p.ToPoint()) Then
				fishes.Current.Remove(f)
				Return
			End If
		Next
	End Sub

	Public Function ListFishes() As List(Of RectangleF)
		Return fishes.Current
	End Function

	Public Sub ClearFrame()
		fishes.Current.Clear()
	End Sub

	Public Sub Save()
		If Not IsLoaded Then Return
		fishes.Save(fishFile)
	End Sub

#End Region

#Region "Tracking Engine"

	Private tracker As New CvTracker

	Public Sub TrackToNextFrame()
		If FrameID >= FrameCount Then Return
		tracker.ClearAll()
		tracker.Init(frame)
		tracker.AddRegions(fishes.Current)
		ReadFrame()
		tracker.Track(frame)
		fishes.Current.Clear()
		fishes.Current.AddRange(tracker.ListRegions())
	End Sub

	Public Sub CopyToNextFrame()
		If FrameID >= FrameCount Then Return
		Dim current = fishes.Current
		ReadFrame()
		fishes.Current.Clear()
		fishes.Current.AddRange(current)
	End Sub

#End Region

#Region "IDisposable Support"
	Private disposedValue As Boolean

	Protected Overridable Sub Dispose(disposing As Boolean)
		If Not disposedValue Then
			If disposing Then
				capture?.Dispose()
				frame.Dispose()
			End If
			capture = Nothing
			frame = Nothing
			' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
			' TODO: set large fields to null.
		End If
		disposedValue = True
	End Sub

	' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
	'Protected Overrides Sub Finalize()
	'    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
	'    Dispose(False)
	'    MyBase.Finalize()
	'End Sub

	' This code added by Visual Basic to correctly implement the disposable pattern.
	Public Sub Dispose() Implements IDisposable.Dispose
		' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
		Dispose(True)
		' TODO: uncomment the following line if Finalize() is overridden above.
		' GC.SuppressFinalize(Me)
	End Sub

#End Region

End Class
