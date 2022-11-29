Imports System
Imports System.IO
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.UI
Imports DevExpress.Web.ASPxUploadControl
Imports DevExpress.Web.Mvc
Imports Sample.Models

Namespace Sample.Controllers

    Public Class HomeController
        Inherits Controller

        Public Function Index() As ActionResult
            Return View([Select]())
        End Function

        Public Function GridViewPartial() As ActionResult
            Return PartialView([Select]())
        End Function

        <HttpPost, ValidateInput(False)>
        Public Function AddNew(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal user As User) As ActionResult
            If ModelState.IsValid Then
                Insert(user)
            Else
                ViewData("UserInfo") = user
            End If

            Return PartialView("GridViewPartial", [Select]())
        End Function

        <HttpPost, ValidateInput(False)>
        Public Function Update(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal user As User) As ActionResult
            If ModelState.IsValid Then
                UserProvider.Update(user)
            Else
                ViewData("UserInfo") = user
            End If

            Return PartialView("GridViewPartial", [Select]())
        End Function

        <HttpPost, ValidateInput(False)>
        Public Function Delete(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal user As User) As ActionResult
            UserProvider.Delete(user)
            Return PartialView("GridViewPartial", [Select]())
        End Function

        Public Function ImageUpload() As ActionResult
            Call UploadControlExtension.GetUploadedFiles("uploadControl", UploadControlHelper.ValidationSettings, New EventHandler(Of FileUploadCompleteEventArgs)(AddressOf UploadControlHelper.uploadControl_FileUploadComplete))
            Return Nothing
        End Function
    End Class

    Public Class UploadControlHelper

        Public Shared ReadOnly ValidationSettings As ValidationSettings = New ValidationSettings With {.AllowedFileExtensions = New String() {".jpg", ".jpeg", ".jpe"}, .MaxFileSize = 4000000}

        Public Shared Sub uploadControl_FileUploadComplete(ByVal sender As Object, ByVal e As FileUploadCompleteEventArgs)
            If e.UploadedFile.IsValid Then
                Dim resultFilePath As String = "~/Content/Avatars/" & String.Format("Avatar{0}{1}", Convert.ToString(HttpContext.Current.Session("UserID")), Path.GetExtension(e.UploadedFile.FileName))
                e.UploadedFile.SaveAs(HttpContext.Current.Request.MapPath(resultFilePath))
                Dim urlResolver As IUrlResolutionService = TryCast(sender, IUrlResolutionService)
                If urlResolver IsNot Nothing Then e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath) & "?refresh=" & Guid.NewGuid().ToString()
            End If
        End Sub
    End Class
End Namespace
