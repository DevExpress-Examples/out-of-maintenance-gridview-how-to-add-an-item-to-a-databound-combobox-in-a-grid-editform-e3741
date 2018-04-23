using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace AddComboItemPopup.Controllers {
    public class HomeController : Controller {
        public static OrderItemsProvider orderItemsProvider;

        public ActionResult Index() {
            orderItemsProvider = new OrderItemsProvider();
            return GridView();
        }
        public ActionResult GridView() {
            ViewData["Products"] = orderItemsProvider.GetProducts();
            return View("Index", orderItemsProvider.GetItems());
        }
        public ActionResult GridViewPartial() {
            ViewData["Products"] = orderItemsProvider.GetProducts();
            return PartialView("_GridView", orderItemsProvider.GetItems());
        }

        public ActionResult gridComboBoxPartial() {
            return PartialView("_EditComboBox", orderItemsProvider.GetProducts());
        }

        #region Main grid ppdate procedures
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] OrderItem item) {
            if (ModelState.IsValid) {
                try {
                    orderItemsProvider.ItemInsert(item.ProductId, item.Quantity);
                }
                catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = ModelState["ProductId"].Errors[0].ErrorMessage;

            ViewData["Products"] = orderItemsProvider.GetProducts();
            return PartialView("_GridView", orderItemsProvider.GetItems());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdatePartial([ModelBinder(typeof(DevExpressEditorsBinder))] OrderItem item) {
            if (ModelState.IsValid) {
                try {
                    orderItemsProvider.ItemUpdate(item.ProductId, item.Quantity, item.Id);
                }
                catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = ModelState["ProductId"].Errors[0].ErrorMessage;

            ViewData["Products"] = orderItemsProvider.GetProducts();
            return PartialView("_GridView", orderItemsProvider.GetItems());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial(int Id) {
            if (Id >= 0) {
                try {
                    orderItemsProvider.ItemDelete(Id);
                }
                catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridView", orderItemsProvider.GetItems());
        }
        #endregion

        #region ComboBox grid ppdate procedures
        [HttpPost, ValidateInput(false)]
        public ActionResult AddNewComboBoxPartial([ModelBinder(typeof(DevExpressEditorsBinder))] OrderProduct product) {
            if (ModelState.IsValid) {
                try {
                    orderItemsProvider.ProductInsert(product.Name, product.Price);
                }
                catch (Exception e) {
                    ViewData["EditgridComboBoxError"] = e.Message;
                }
            }
            else
                ViewData["EditgridComboBoxError"] = ModelState["Id"].Errors[0].ErrorMessage;

            return PartialView("_EditComboBox", orderItemsProvider.GetProducts());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateComboBoxPartial([ModelBinder(typeof(DevExpressEditorsBinder))] OrderProduct product) {
            if (ModelState.IsValid) {
                try {
                    orderItemsProvider.ProductUpdate(product.Name, product.Price, product.Id);
                }
                catch (Exception e) {
                    ViewData["EditgridComboBoxError"] = e.Message;
                }
            }
            else
                ViewData["EditgridComboBoxError"] = ModelState["Id"].Errors[0].ErrorMessage;

            return PartialView("_EditComboBox", orderItemsProvider.GetProducts());
        }

        #endregion
    }
}
