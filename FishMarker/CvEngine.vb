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

	Public Function GetCurrentFrame() As Bitmap
		If Not IsOpened Then Return Nothing

		Dim src = CVX.GetFrameProps(currentFrame)

		Dim pf As PixelFormat
		Select Case src.nChannels
			Case 1
				pf = PixelFormat.Format8bppIndexed
				Exit Select
			Case 3
				pf = PixelFormat.Format24bppRgb
				Exit Select
			Case 4
				pf = PixelFormat.Format32bppArgb
				Exit Select
			Case Else
				Throw New ArgumentException("Number of channels must be 1, 3 or 4.", NameOf(src))
		End Select

		Dim dst As New Bitmap(src.Width, src.Height, pf)

		If pf = PixelFormat.Format8bppIndexed Then
			Dim plt As ColorPalette = dst.Palette
			For x As Integer = 0 To 255
				plt.Entries(x) = Color.FromArgb(x, x, x)
			Next
			dst.Palette = plt
		End If

		Dim w As Integer = src.Width
		Dim h As Integer = src.Height
		Dim rect As New Rectangle(0, 0, w, h)
		Dim bd As BitmapData = Nothing

		Dim submat As Boolean = src.IsSubmat
		Dim continuous As Boolean = src.IsContinuous

		Try
			bd = dst.LockBits(rect, ImageLockMode.[WriteOnly], pf)

			Dim srcData As IntPtr = src.Data
			Dim pSrc As Pointer(Of Byte) = srcData
			Dim pDst As Pointer(Of Byte) = bd.Scan0
			Dim ch As Integer = src.nChannels
			Dim sstep As Integer = src.Step
			Dim dstep As Integer = CInt(((src.Width * ch) + 3) / 4) * 4
			Dim stride As Integer = bd.Stride

			Select Case pf
				Case PixelFormat.Format1bppIndexed
					If True Then
						If submat Then
							Throw New NotImplementedException("submatrix not supported")
						End If
						Dim x As Integer = 0
						Dim y As Integer
						Dim bytePos As Integer
						Dim mask As Byte
						Dim b As Byte = 0
						Dim i As Integer
						For y = 0 To h - 1
							For bytePos = 0 To stride - 1
								If x < w Then
									For i = 0 To 7
										mask = CByte(&H80 >> i)
										If x < w AndAlso pSrc(sstep * y + x) = 0 Then
											b = b And CByte(mask Xor &HFF)
										Else
											b = b Or mask
										End If

										x += 1
									Next
									pDst(bytePos) = b
								End If
							Next
							x = 0
							pDst += stride
						Next
						Exit Select
					End If
				Case PixelFormat.Format8bppIndexed, PixelFormat.Format24bppRgb, PixelFormat.Format32bppArgb
					If sstep = dstep AndAlso Not submat AndAlso continuous Then
						Dim imageSize As UInteger = src.DataEnd.ToInt64() - src.Data.ToInt64()
						CopyMemory(pDst, pSrc, imageSize)
					Else
						For y As Integer = 0 To h - 1
							Dim offsetSrc As Long = (y * sstep)
							Dim offsetDst As Long = (y * dstep)
							CopyMemory(pDst + offsetDst, pSrc + offsetSrc, w * ch)
						Next
					End If
					Exit Select
				Case Else
					Throw New NotImplementedException()
			End Select
		Finally
			dst.UnlockBits(bd)
		End Try
		Return dst
	End Function

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
