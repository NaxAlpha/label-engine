'File:			Pointer.vb
'Author:		Nauman Mustafa
'License:		MIT
'Modified:		14-July-2017
'Description:	Basic implementation of C++ pointers in VB.NET, is used for unsafe C# code conversion via Telerik Code Converter (Unofficial)
'				If you convert c# unsafe code via Telerik code converter, pointers in C# get reference to Pointer(Of T) structure
'				It can't be exactly as C# one because there is no function to get pointer of .net variable.

Imports System.Runtime.InteropServices

Public Structure Pointer(Of T As Structure)

	Private Shared TypeSize As Integer = Marshal.SizeOf(Of T)

	Private ptr As IntPtr

	Public Sub New(obj As T)
		Throw New Exception("Cannot get pointer of .Net object")
	End Sub

	Default Public Property Value(idx As IntPtr) As T
		Get
			Return Marshal.PtrToStructure(Of T)(ptr + CLng(idx) * TypeSize)
		End Get
		Set(value As T)
			Marshal.StructureToPtr(Of T)(value, ptr + CLng(idx) * TypeSize, False)
		End Set
	End Property

	Public Property Target As T
		Get
			Return Value(0)
		End Get
		Set(value As T)
			Me.Value(0) = value
		End Set
	End Property

	Public Function ToPointer() As IntPtr
		Return ptr
	End Function

	Public Shared Narrowing Operator CType(this As Pointer(Of T)) As T
		Return this.Target
	End Operator

	Public Shared Operator +(this As Pointer(Of T), that As Integer) As Pointer(Of T)
		Return this.ptr + that * TypeSize
	End Operator

	Public Shared Operator -(this As Pointer(Of T), that As Integer) As Pointer(Of T)
		Return this.ptr - that * TypeSize
	End Operator

	Public Shared Narrowing Operator CType(this As Pointer(Of T)) As IntPtr
		Return this.ptr
	End Operator

	Public Shared Widening Operator CType(this As IntPtr) As Pointer(Of T)
		Return New Pointer(Of T)() With {.ptr = this}
	End Operator

End Structure
