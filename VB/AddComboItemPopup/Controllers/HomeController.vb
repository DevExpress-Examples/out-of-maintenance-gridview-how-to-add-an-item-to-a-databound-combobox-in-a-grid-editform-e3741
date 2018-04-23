Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc

Namespace AddComboItemPopup.Controllers
	Public Class HomeController
		Inherits Controller
		Public Shared orderItemsProvider As OrderItemsProvider

		Public Function Index() As ActionResult
			orderItemsProvider = New OrderItemsProvider()
			Return GridView()
		End Function
		Public Function GridView() As ActionResult
			ViewData("Products") = orderItemsProvider.GetProducts()
			Return View("Index", orderItemsProvider.GetItems())
		End Function
		Public Function GridViewPartial() As ActionResult
			ViewData("Products") = orderItemsProvider.GetProducts()
			Return PartialView("_GridView", orderItemsProvider.GetItems())
		End Function

		Public Function gridComboBoxPartial() As ActionResult
			Return PartialView("_EditComboBox", orderItemsProvider.GetProducts())
		End Function

		#Region "Main grid ppdate procedures"
		<HttpPost, ValidateInput(False)> _
		Public Function AddNewPartial(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal item As OrderItem) As ActionResult
			If ModelState.IsValid Then
				Try
					orderItemsProvider.ItemInsert(item.ProductId, item.Quantity)
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			Else
				ViewData("EditError") = ModelState("ProductId").Errors(0).ErrorMessage
			End If

			ViewData("Products") = orderItemsProvider.GetProducts()
			Return PartialView("_GridView", orderItemsProvider.GetItems())
		End Function
		<HttpPost, ValidateInput(False)> _
		Public Function UpdatePartial(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal item As OrderItem) As ActionResult
			If ModelState.IsValid Then
				Try
					orderItemsProvider.ItemUpdate(item.ProductId, item.Quantity, item.Id)
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			Else
				ViewData("EditError") = ModelState("ProductId").Errors(0).ErrorMessage
			End If

			ViewData("Products") = orderItemsProvider.GetProducts()
			Return PartialView("_GridView", orderItemsProvider.GetItems())
		End Function
		<HttpPost, ValidateInput(False)> _
		Public Function DeletePartial(ByVal Id As Integer) As ActionResult
			If Id >= 0 Then
				Try
					orderItemsProvider.ItemDelete(Id)
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			End If
			Return PartialView("_GridView", orderItemsProvider.GetItems())
		End Function
		#End Region

		#Region "ComboBox grid ppdate procedures"
		<HttpPost, ValidateInput(False)> _
		Public Function AddNewComboBoxPartial(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal product As OrderProduct) As ActionResult
			If ModelState.IsValid Then
				Try
					orderItemsProvider.ProductInsert(product.Name, product.Price)
				Catch e As Exception
					ViewData("EditgridComboBoxError") = e.Message
				End Try
			Else
				ViewData("EditgridComboBoxError") = ModelState("Id").Errors(0).ErrorMessage
			End If

			Return PartialView("_EditComboBox", orderItemsProvider.GetProducts())
		End Function
		<HttpPost, ValidateInput(False)> _
		Public Function UpdateComboBoxPartial(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal product As OrderProduct) As ActionResult
			If ModelState.IsValid Then
				Try
					orderItemsProvider.ProductUpdate(product.Name, product.Price, product.Id)
				Catch e As Exception
					ViewData("EditgridComboBoxError") = e.Message
				End Try
			Else
				ViewData("EditgridComboBoxError") = ModelState("Id").Errors(0).ErrorMessage
			End If

			Return PartialView("_EditComboBox", orderItemsProvider.GetProducts())
		End Function

		#End Region
	End Class
End Namespace
