Imports System.IO
Imports OpenCvSharp

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
			Return capture.Get(CaptureProperty.FrameCount)
		End Get
	End Property

	Public Sub LoadVideo(fileName As String)
		If IsLoaded Then
			Throw New InvalidOperationException("Video already loaded!")
		End If
		videoFile = fileName
		fishFile = videoFile + ".fish"
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

	Public Sub ReadFrame()
		fishes.Move()
		capture.Read(frame)
		If trackerEnabled Then
			fishes.Current.Clear()
			tracker.Track(frame)
			fishes.Current.AddRange(tracker.ListRegions())
		End If
	End Sub

	Public Sub ReloadVideo()
		If Not IsLoaded Then Return
		fishes.Reset()
		LoadVideo(videoFile)
	End Sub

	Public Sub SkipToFrame(fid As Integer)
		If IsLoaded Then
			If fid = FrameID Then Return
			If fid < FrameID Then ReloadVideo()
			While FrameID <> fid
				capture.Read(frame)
				fishes.Move()
			End While
			If TrackingEnabled Then
				trackerEnabled = True 'Force Re-Enable
			End If
		End If
	End Sub

#End Region

#Region "Fish Region"

	Private fishes As New FishCollection

	Public Sub AddFish(f As Fish)
		fishes.Current.Add(f)
	End Sub

	Public Sub RemoveByPoint(p As Point2f)
		For Each f In fishes.Current
			If f.Bounding.Contains(p.ToPoint()) Then
				fishes.Current.Remove(f)
				Return
			End If
		Next
	End Sub

#End Region

#Region "Tracking Engine"

	Private tracker As New CvTracker
	Private trackerEnabled As Boolean = False

	Public Property TrackingEnabled As Boolean
		Get
			Return trackerEnabled
		End Get
		Set(value As Boolean)
			trackerEnabled = value
			If trackerEnabled Then
				tracker.ClearAll()
				tracker.Init(frame)
				For Each f In fishes.Current
					tracker.AddRegion(f)
				Next
			Else
				tracker.ClearAll()
			End If
		End Set
	End Property

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
