Imports System.Runtime.CompilerServices
Imports OpenCvSharp

Module PointExtensions

	<Extension>
	Public Function ToPoint2f(this As Drawing.Point) As Point2f
		Return New Point2f(this.X, this.Y)
	End Function

	<Extension>
	Public Function ToPoint(this As Point2f) As Drawing.Point
		Return New Drawing.Point(this.X, this.Y)
	End Function

	<Extension>
	Public Function ToRect(this As Rectangle) As Rect
		Return New Rect(this.X, this.Y, this.Width, this.Height)
	End Function

	<Extension>
	Public Function ToRectangle(this As Rect) As Rectangle
		Return New Rectangle(this.X, this.Y, this.Width, this.Height)
	End Function

	<Extension>
	Public Function Shift(this As Point2f, delta As Point2f) As Point2f
		Return this - delta
	End Function

	<Extension>
	Public Function Rotate(this As Point2f, angle As Single) As Point2f
		Return New Point2f(this.X * Math.Cos(angle) - this.Y * Math.Sin(angle),
						   this.X * Math.Sin(angle) + this.Y * Math.Cos(angle))
	End Function



End Module