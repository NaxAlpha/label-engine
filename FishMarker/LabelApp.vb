Imports OpenCvSharp

Public Class LabelApp
	Implements IDisposable

#Region "Video Basics"

	Private capture As VideoCapture

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
		capture.Read(frame)
	End Sub

#End Region

#Region "IDisposable Support"
	Private disposedValue As Boolean ' To detect redundant calls

	' IDisposable
	Protected Overridable Sub Dispose(disposing As Boolean)
		If Not disposedValue Then
			If disposing Then
				' TODO: dispose managed state (managed objects).
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
