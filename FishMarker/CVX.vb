Imports System.Runtime.InteropServices

Friend Class CVX

	Private Const DLLPath As String = "C:\Users\Nax\Documents\Visual Studio 2017\Projects\cvx\x64\Release\cvx.dll"

	' Mat APIs

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function CreateFrame() As IntPtr
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function GetFrameProps(frame As IntPtr) As MatProps
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Sub DestroyFrame(frame As IntPtr)
	End Sub

	' Video APIs

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function OpenVideo(fn As String) As IntPtr
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function VideoLength(capture As IntPtr) As Integer
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Sub CloseVideo(capture As IntPtr)
	End Sub

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Sub ReadFrame(vid As IntPtr, outFrame As IntPtr)
	End Sub

	' Tracking APIs

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function StartTracking() As IntPtr
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Sub StopTracking(engine As IntPtr)
	End Sub

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function CreateTracker(engine As IntPtr, currentFrame As IntPtr, region As Rectangle) As IntPtr
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Sub UpdateTracker(engine As IntPtr, frame As IntPtr)
	End Sub

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function GetObjectList(engine As IntPtr, ByRef count As Integer) As IntPtr
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Sub DeleteObjectList(list As IntPtr)
	End Sub

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Sub DestroyTracker(engine As IntPtr, currentFrame As IntPtr, x As Integer, y As Integer)
	End Sub

End Class
Friend Structure MatProps
	Public IsContinuous As Integer
	Public Data As IntPtr
	Public DataEnd As IntPtr
	Public Height As Integer
	Public nChannels As Integer
	Public [Step] As Integer
	Public IsSubmat As Integer
	Public Width As Integer
End Structure