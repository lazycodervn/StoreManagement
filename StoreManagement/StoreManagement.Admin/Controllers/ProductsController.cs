﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {



        public ActionResult Index(int storeId = 0, String search = "", int categoryId = 0)
        {
            List<Product> resultList = new List<Product>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = ProductRepository.GetProductByTypeAndCategoryId(storeId, StoreConstants.ProductType, categoryId, search);
            }

            var contentsAdminViewModel = new ProductsAdminViewModel();
            contentsAdminViewModel.Products = resultList;
            contentsAdminViewModel.Categories = ProductCategoryRepository.GetProductCategoriesByStoreIdFromCache(storeId, StoreConstants.ProductType);
            return View(contentsAdminViewModel);
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product content = ProductRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            if (!CheckRequest(content))
            {
                return RedirectToAction("NoAccessPage", "Home", new { id = content.StoreId });
            }
            return View(content);
        }

        //
        // GET: /Product/Create

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0, int selectedCategoryId = 0)
        {


            var content = new Product();
            content.ProductCategoryId = selectedCategoryId;  
            content.StoreId = GetStoreId(selectedStoreId); 

            var labels = new List<LabelLine>();
            var fileManagers = new List<FileManager>();
            if (id == 0)
            {
                content.Type = StoreConstants.ProductType;
                content.CreatedDate = DateTime.Now;
                content.State = true;
            }
            else
            {

                content = ProductRepository.GetSingleIncluding(id, r => r.ProductFiles.Select(r1 => r1.FileManager));
                if (!CheckRequest(content)) //security for right store and its item.
                {
                    return HttpNotFound("Not Found");
                }
                content.UpdatedDate = DateTime.Now;
                labels = LabelLineRepository.GetLabelLinesByItem(id, StoreConstants.ProductType);

                fileManagers = content.ProductFiles.Select(r => r.FileManager).ToList();
            }
            ViewBag.LabelLines = labels;
            ViewBag.FileManagers = fileManagers;
            return View(content);
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrEdit(Product product, int[] selectedFileId = null, int[] selectedLabelId = null)
        {
            if (ModelState.IsValid)
            {
                if (!CheckRequest(product))
                {
                    return new HttpNotFoundResult("Not Found");
                }




                if (product.ProductCategoryId == 0)
                {
                    List<FileManager> fileManagers = new List<FileManager>();
                    var labelList = new List<LabelLine>();
                    if (product.Id > 0)
                    {
                        var content = ProductRepository.GetSingleIncluding(product.Id, r => r.ProductFiles.Select(r1 => r1.FileManager));
                        fileManagers = content.ProductFiles.Select(r => r.FileManager).ToList();
                        labelList = LabelLineRepository.GetLabelLinesByItem(product.Id, StoreConstants.ProductType);

                    }
                    ViewBag.FileManagers = fileManagers;
                    ViewBag.LabelLines = labelList;
                    ModelState.AddModelError("ProductCategoryId", "You should select category from category tree.");
                    return View(product);
                }


                product.Description = GetCleanHtml(product.Description);
                if (product.Id == 0)
                {
                    ProductRepository.Add(product);
                }
                else
                {
                    ProductRepository.Edit(product);
                }

                ProductRepository.Save();
                int contentId = product.Id;
                if (selectedFileId != null)
                {
                    ProductFileRepository.SaveProductFiles(selectedFileId, contentId);
                }


                LabelLineRepository.SaveLabelLines(selectedLabelId, contentId, StoreConstants.ProductType);


                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = product.StoreId, categoryId = product.ProductCategoryId });
                }
                else
                {
                    return RedirectToAction("Index", new { categoryId = product.ProductCategoryId });
                }
            }

            return View(product);
        }



        //
        // GET: /Product/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id = 0)
        {
            Product content = ProductRepository.GetSingle(id);
            if (!CheckRequest(content))
            {
                return new HttpNotFoundResult("Not Found");
            }
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }
        public ActionResult StoreDetails(int id = 0)
        {
            Product product = ProductRepository.GetSingle(id);
            Store s = StoreRepository.GetSingle(product.StoreId);
            Category cat = CategoryRepository.GetSingle(product.ProductCategoryId);
            var productDetailLink = LinkHelper.GetProductLink(product, cat.Name);
            String detailPage = String.Format("http://{0}{1}", s.Domain, productDetailLink);
            return Redirect(detailPage);

        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product content = ProductRepository.GetSingle(id);
            if (!CheckRequest(content))
            {
                return new HttpNotFoundResult("Not Found");
            }

            ProductRepository.Delete(content);
            ProductRepository.Save();


            if (IsSuperAdmin)
            {
                return RedirectToAction("Index", new { storeId = content.StoreId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


    }
}