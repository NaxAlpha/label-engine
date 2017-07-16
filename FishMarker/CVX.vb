Imports System.Runtime.InteropServices

Friend Class CVX

	Private Const DLLPath As String = "C:\Users\Nax\Documents\Visual Studio 2017\Projects\cvx\x64\Release\cvx.dll"

	' Mat APIs

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function CreateFrame() As IntPtr
	End Function

	<DllImport(DLLPath, CallingConvention:=CallingConvention.StdCall)>
	Public Shared Function GetFrameProps(frame As IntPtr) As MatData
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
	Public Shared Function CreateTracker(engine As IntPtr, currentFrame As IntPtr, region As CvRect) As IntPtr
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
	Public Shared Function DestroyTracker(engine As IntPtr, currentFrame As IntPtr, x As Integer, y As Integer) As IntPtr
	End Function

End Class
<StructLayout(LayoutKind.Sequential)>
Friend Structure MatData
	Public IsContinuous As Integer
	Public Data As IntPtr
	Public DataEnd As IntPtr
	Public Height As Integer
	Public nChannels As Integer
	Public [Step] As Integer
	Public IsSubmat As Integer
	Public Width As Integer
End Structure
<StructLayout(LayoutKind.Sequential)>
Friend Structure CvRect
	Public X As Integer
	Public Y As Integer
	Public W As Integer
	Public H As Integer
	Public Shared Widening Operator CType(this As CvRect) As Rectangle
		Return New Rectangle(this.X, this.Y, this.W, this.H)
	End Operator
	Public Shared Narrowing Operator CType(this As Rectangle) As CvRect
		Return New CvRect With {.X = this.X, .Y = this.Y, .W = this.Width, .H = this.Height}
	End Operator
End Structure