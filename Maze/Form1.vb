Public Class Form1
    Private Labirint As Bitmap
    Private bFinish As Boolean
    Private bSound As Boolean
    Private OldPointX As Integer
    Private OldPointY As Integer
    Private startSoundPlayer As System.Media.SoundPlayer
    Private finishSoundPlayer As System.Media.SoundPlayer

    ' Для инкрементного алгоритма в Panel1_MouseMove
    Private xerr As Integer
    Private yerr As Integer
    Private d As Integer
    Private dx As Integer
    Private dy As Integer
    Private incX As Integer
    Private incY As Integer
    Private x As Integer
    Private y As Integer
    Private pixelColor As Color
    Private i As Integer

    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()
        ' Добавьте все инициализирующие действия после вызова InitializeComponent().
        Labirint = New Bitmap("labirint.bmp")
        ' размер labirint.bmp строго 797x597 при размере Panel1 800x600 и свойстве Panel1 BorderStyle = Fixed3D
        Panel1.BackgroundImage = Labirint

        bSound = False
        ' This SoundPlayer plays a sound whenever the player hits a wall.
        startSoundPlayer = New System.Media.SoundPlayer("C:\Windows\Media\chord.wav")
        ' This SoundPlayer plays a sound when the player finishes the game.
        finishSoundPlayer = New System.Media.SoundPlayer("C:\Windows\Media\tada.wav")

        bFinish = False
        MoveToStart()
    End Sub

    ''' <summary>
    ''' Move the mouse pointer to start
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MoveToStart()
        If bSound Then startSoundPlayer.Play()
        Dim startingPoint = Panel1.Location
        startingPoint.Offset(18, 15)
        OldPointX = 18
        OldPointY = 15
        Cursor.Position = PointToScreen(startingPoint)
    End Sub

    Private Sub Panel1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        ' Инкрементный восьмисвязный алгоритм Брезенхэма
        xerr = 0
        yerr = 0

        dx = e.X - OldPointX
        dy = e.Y - OldPointY

        If dx > 0 Then incX = 1
        If dx = 0 Then incX = 0
        If dx < 0 Then incX = -1

        If dy > 0 Then incY = 1
        If dy = 0 Then incY = 0
        If dy < 0 Then incY = -1

        'dx = Math.Abs(dx)
        'dy = Math.Abs(dy)
        If dx < 0 Then dx = dx - dx - dx
        If dy < 0 Then dy = dy - dy - dy

        If dx > dy Then
            d = dx
        Else : d = dy
        End If

        x = OldPointX
        y = OldPointY

        i = d
        While i
            xerr = xerr + dx
            yerr = yerr + dy

            If xerr > d Then
                xerr = xerr - d
                x = x + incX
            End If

            If yerr > d Then
                yerr = yerr - d
                y = y + incY
            End If

            pixelColor = Labirint.GetPixel(x, y)

            If pixelColor.R = 65 And pixelColor.G = 105 And pixelColor.B = 225 Then
                ' RoyalBlue ARGB = (255,65,105,255)
                MoveToStart()
                Return
            End If

            If (pixelColor.R = 127 And pixelColor.G = 255 And pixelColor.B = 212) Or (pixelColor.R = 0 And pixelColor.G = 0 And pixelColor.B = 0) Then
                ' Aquamarine ARGB = (255,127,255,212)
                ' Black ARGB = (255,0,0,0)
                If bFinish Then
                    If bSound Then finishSoundPlayer.Play()
                    ' Show a congratulatory MessageBox, then close the form.
                    MessageBox.Show("Congratulations!")
                    Close()
                Else
                    MoveToStart()
                    bFinish = True
                End If
                Return
            End If

            i = i - 1
        End While

        OldPointX = x
        OldPointY = y
    End Sub

    Private Sub Panel1_MouseLeave() Handles Panel1.MouseLeave
        bFinish = False
    End Sub

    Private Sub Panel1_MouseEnter() Handles Panel1.MouseEnter
        ' первым для Panel1(как и для любого элемента) всегда происходит
        ' событие MouseEnter, потом событие MouseMove, а потом, самым последним,
        ' происходит событие MouseLeave(по документации MSDN)
        If bFinish = False Then
            MoveToStart()
            bFinish = True
        End If
    End Sub

    Private Sub Button1_Click() Handles Button1.Click
        If bSound = False Then
            Button1.Text = "Звук включён!"
            bSound = True
        Else
            Button1.Text = "Звук выключен"
            bSound = False
        End If
    End Sub
End Class
