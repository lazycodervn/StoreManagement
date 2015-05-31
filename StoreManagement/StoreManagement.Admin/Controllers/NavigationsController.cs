﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class NavigationsController : BaseController
    {
      


        public NavigationsController(IStoreContext dbContext,
       ISettingRepository settingRepository)
            : base(dbContext, settingRepository)
        {
   
        }

        //
        // GET: /Navigations/

        public ViewResult Index(int storeId=0, String search="")
        {
            List<Navigation> resultList = new List<Navigation>();
            if (storeId == 0)
            {
                resultList = navigationRepository.GetAll().ToList();
            }
            else
            {
                resultList = navigationRepository.GetStoreNavigations(storeId);
            }
            if (!String.IsNullOrEmpty(search))
            {
                resultList =
                    resultList.Where(r => r.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            ViewBag.Search = search;
            return View(resultList);
        }

        //
        // GET: /Navigations/Details/5

        public ViewResult Details(int id)
        {
            Navigation navigation = navigationRepository.GetSingle(id);
            return View(navigation);
        }

        //
        // GET: /Navigations/Create

        public ActionResult SaveOrEdit(int id=0)
        {
            var item = new Navigation();
            if (id == 0)
            {
                item.ParentId = 0;
                item.CreatedDate = DateTime.Now;
            }
            else
            {
                item = navigationRepository.GetSingle(id);
            }
            return View(item);
        }

        //
        // POST: /Navigations/Create

        [HttpPost]
        public ActionResult SaveOrEdit(Navigation navigation)
        {
           // if (ModelState.IsValid)
            {

                if (navigation.Id == 0)
                {
                    navigationRepository.Add(navigation);
                    navigationRepository.Save();
                }
                else
                {
                    navigationRepository.Edit(navigation);
                    navigationRepository.Save();
                }

                return RedirectToAction("Index");
            }

            return View(navigation);
        }
 

       
        //
        // GET: /Navigations/Delete/5

        public ActionResult Delete(int id)
        {
            Navigation navigation = navigationRepository.GetSingle(id);
            return View(navigation);
        }

        //
        // POST: /Navigations/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Navigation navigation = navigationRepository.GetSingle(id);
            navigationRepository.Delete(navigation);
            navigationRepository.Save();
            return RedirectToAction("Index");
        }

    }
}