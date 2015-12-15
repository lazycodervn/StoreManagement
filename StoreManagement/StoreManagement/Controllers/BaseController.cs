﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using Ninject;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Helper;
using StoreManagement.Models;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static readonly Logger BaseLogger = LogManager.GetCurrentClassLogger();

        [Inject]
        public IStoreService StoreService { set; get; }

        [Inject]
        public IItemFileService ItemFileService { set; get; }

        [Inject]
        public ISettingService SettingService { set; get; }

        [Inject]
        public IFileManagerService FileManagerService { get; set; }

        [Inject]
        public IContentFileService ContentFileService { set; get; }

        [Inject]
        public ICommentService CommentService { set; get; }

        [Inject]
        public IContentService ContentService { set; get; }

        [Inject]
        public ICategoryService CategoryService { set; get; }

        [Inject]
        public INavigationService NavigationService { set; get; }

        [Inject]
        public IPageDesignService PageDesignService { set; get; }

        [Inject]
        public IStoreUserService StoreUserService { set; get; }

        [Inject]
        public IEmailSender EmailSender { set; get; }


        [Inject]
        public IActivityService ActivityService { set; get; }

        [Inject]
        public IRetailerService RetailerService { set; get; }

        [Inject]
        public IProductService ProductService { set; get; }

        [Inject]
        public IProductAttributeService ProductAttributeService { set; get; }

        [Inject]
        public IProductAttributeRelationService ProductAttributeRelationService { set; get; }

        [Inject]
        public IProductFileService ProductFileService { set; get; }

        [Inject]
        public IProductCategoryService ProductCategoryService { set; get; }

        [Inject]
        public IBrandService BrandService { set; get; }

        [Inject]
        public ILocationService LocationService { set; get; }

        [Inject]
        public IContactService ContactService { set; get; }


        [Inject]
        public ILabelService LabelService { set; get; }


        protected Store MyStore { set; get; }
        protected int StoreId { get; set; }
        protected String StoreName { get; set; }
 


        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            GetStoreByDomain(requestContext);


            ViewBag.MetaDescription = GetSettingValue(StoreConstants.MetaTagDescription);
            ViewBag.MetaKeywords = GetSettingValue(StoreConstants.MetaTagKeywords);

        }
        private void GetStoreByDomain(RequestContext requestContext)
        {
            var sh = new StoreHelper();
            var store = sh.GetStoreByDomain(StoreService, requestContext.HttpContext.Request);
            this.MyStore = store;
            this.StoreId = store.Id;
            this.StoreName = store.Name;

            if (store == null)
            {
                throw new Exception("Store cannot be NULL");
            }
        }

        protected new HttpNotFoundResult HttpNotFound(string statusDescription = null)
        {
            return new HttpNotFoundResult(statusDescription);
        }
        protected bool IsModulActive(String controllerName)
        {
            var navigations = NavigationService.GetStoreActiveNavigations(MyStore.Id);
            var item = navigations.Any(r => r.ControllerName.ToLower().StartsWith(controllerName.ToLower()));
            return item;
        }
        protected bool CheckRequest(BaseEntity entity)
        {
            return entity.StoreId == MyStore.Id;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            ViewData[StoreConstants.MetaTagKeywords] = GetSettingValue(StoreConstants.MetaTagKeywords, "");
            ViewData[StoreConstants.MetaTagDescription] = GetSettingValue(StoreConstants.MetaTagDescription, "");
            ViewData[StoreConstants.CanonicalUrl] = GetSettingValue(StoreConstants.CanonicalUrl, "");
            ViewData["StoreName"] = MyStore.Name;

            SetStoreCache();
            base.OnActionExecuting(filterContext);
        }
        protected bool IsCacheEnable { get; set; }
        protected void SetStoreCache()
        {
            if (StoreService == null)
            {
                BaseLogger.Trace("StoreService is null");
                return;
            }
            var isCacheEnable = StoreService.GetStoreCacheStatus(StoreId);
            this.IsCacheEnable = isCacheEnable;
            // Logger.Trace("StoreId =" + StoreId + " " + isCacheEnable);
            SettingService.IsCacheEnable = isCacheEnable;
            SettingService.CacheMinute = GetSettingValueInt("SettingService_CacheMinute", 200);


            NavigationService.IsCacheEnable = isCacheEnable;
            NavigationService.CacheMinute = GetSettingValueInt("NavigationService_CacheMinute", 200);

            ProductCategoryService.IsCacheEnable = isCacheEnable;
            ProductCategoryService.CacheMinute = GetSettingValueInt("ProductCategoryService_CacheMinute", 200);

            ProductFileService.IsCacheEnable = isCacheEnable;
            ProductFileService.CacheMinute = GetSettingValueInt("ProductFileService_CacheMinute", 200);

            ProductService.IsCacheEnable = isCacheEnable;
            ProductService.CacheMinute = GetSettingValueInt("ProductService_CacheMinute", 200);


            StoreUserService.IsCacheEnable = isCacheEnable;
            StoreUserService.CacheMinute = GetSettingValueInt("StoreUserService_CacheMinute", 200);

            PageDesignService.IsCacheEnable = isCacheEnable;
            PageDesignService.CacheMinute = GetSettingValueInt("PageDesignService_CacheMinute", 200);

            CategoryService.IsCacheEnable = isCacheEnable;
            CategoryService.CacheMinute = GetSettingValueInt("CategoryService_CacheMinute", 200);

            ContentService.IsCacheEnable = isCacheEnable;
            ContentService.CacheMinute = GetSettingValueInt("ContentService_CacheMinute", 200);

            ContentFileService.IsCacheEnable = isCacheEnable;
            ContentFileService.CacheMinute = GetSettingValueInt("ContentFileService_CacheMinute", 200);

            FileManagerService.IsCacheEnable = isCacheEnable;
            FileManagerService.CacheMinute = GetSettingValueInt("FileManagerService_CacheMinute", 200);


            BrandService.IsCacheEnable = isCacheEnable;
            BrandService.CacheMinute = GetSettingValueInt("BrandService_CacheMinute", 200);

            LocationService.IsCacheEnable = isCacheEnable;
            LocationService.CacheMinute = GetSettingValueInt("LocationService_CacheMinute", 200);

            ContactService.IsCacheEnable = isCacheEnable;
            ContactService.CacheMinute = GetSettingValueInt("ContactService_CacheMinute", 200);

            StoreService.IsCacheEnable = isCacheEnable;
            StoreService.CacheMinute = GetSettingValueInt("StoreService_CacheMinute", 200);
        }
      

        protected bool GetSettingValueBool(String key, bool defaultValue)
        {
            String d = defaultValue ? bool.TrueString : bool.FalseString;
            return GetSettingValue(key, d).ToBool();
        }
        protected int GetSettingValueInt(String key, int defaultValue)
        {
            String d = defaultValue + "";
            return GetSettingValue(key, d).ToInt();
        }
        protected String GetSettingValue(String key, String defaultValue)
        {
            var value = GetSettingValue(key);
            if (String.IsNullOrEmpty(value))
            {
                BaseLogger.Trace("Store Default Setting= " + StoreId + " Key=" + key + " defaultValue=" + defaultValue);
                return ProjectAppSettings.GetWebConfigString(key, defaultValue);
            }
            else
            {
                return value;
            }
        }
        protected String GetSettingValue(String key)
        {
            try
            {
                if (StoreId == 0)
                {
                    return "";
                }
                var item = GetStoreSettings().FirstOrDefault(r => r.SettingKey.RemoveTabNewLines().Equals(key.RemoveTabNewLines(), StringComparison.InvariantCultureIgnoreCase));

                if (item != null)
                {
                    return item.SettingValue;
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {

                BaseLogger.Error(ex, "Store= " + StoreId + " Key=" + key, key);

                return "";
            }
        }
        private readonly TypedObjectCache<List<Setting>> _settingStoreCache = new TypedObjectCache<List<Setting>>("SettingsCache");
        private readonly TypedObjectCache<List<FileManager>> _imagesStoreCache = new TypedObjectCache<List<FileManager>>("FileManagersCache");

        protected List<Setting> GetStoreSettings()
        {
            String key = String.Format("GetStoreSettingsFromCacheAsync-{0}", StoreId);
            _settingStoreCache.IsCacheEnable = true;
            List<Setting> items = null;
            _settingStoreCache.TryGet(key, out items);
            if (items == null)
            {
                var itemsAsyn = SettingService.GetStoreSettingsFromCache(StoreId);

                items = itemsAsyn;
                _settingStoreCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Setting_CacheAbsoluteExpiration_Minute", 10)));

            }
            return items;

        }

    }
}