Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Web
Imports System.Web.SessionState

Namespace Sample.Models

    Public Class User

        Private idField As Integer

        Private nicknameField As String

        Private avatarUrlField As String

        Public Sub New()
            ID = 0
            NickName = String.Empty
            AvatarUrl = String.Empty
        End Sub

        <Key>
        Public Property ID As Integer
            Get
                Return idField
            End Get

            Set(ByVal value As Integer)
                idField = value
            End Set
        End Property

        <Required(ErrorMessage:="NickName is required")>
        Public Property NickName As String
            Get
                Return nicknameField
            End Get

            Set(ByVal value As String)
                nicknameField = value
            End Set
        End Property

        Public Property AvatarUrl As String
            Get
                Return avatarUrlField
            End Get

            Set(ByVal value As String)
                avatarUrlField = value
            End Set
        End Property

        Public Sub Assign(ByVal source As User)
            NickName = source.NickName
            AvatarUrl = source.AvatarUrl
        End Sub
    End Class

    Public Module UserProvider

        Const Key As String = "UserProvider"

        Private ReadOnly Property Session As HttpSessionState
            Get
                Return HttpContext.Current.Session
            End Get
        End Property

        Private ReadOnly Property Data As List(Of User)
            Get
                If Session(Key) Is Nothing Then Call Restore()
                Return TryCast(Session(Key), List(Of User))
            End Get
        End Property

        Public Function [Select]() As IEnumerable(Of User)
            Return Data
        End Function

        Public Sub Insert(ByVal item As User)
            item.ID = Data.Count + 1
            Data.Add(item)
        End Sub

        Public Sub Update(ByVal item As User)
            Dim storedItem As User = FindItem(item.ID)
            storedItem.Assign(item)
        End Sub

        Public Sub Delete(ByVal item As User)
            Dim storedItem As User = FindItem(item.ID)
            Data.Remove(storedItem)
        End Sub

        Public Sub Restore()
            Session(Key) = New List(Of User)()
        End Sub

        Private Function FindItem(ByVal id As Integer) As User
            For Each item As User In Data
                If item.ID = id Then Return item
            Next

            Return Nothing
        End Function
    End Module
End Namespace
