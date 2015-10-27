﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class RetailerService : BaseService, IRetailerService
    {
        public RetailerService(string webServiceAddress) : base(webServiceAddress)
        {
        }

        protected override string ApiControllerName
        {
            get { return "Retailers"; }
        }

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public Task<List<Retailer>> GetRetailersAsync(int storeId, int? take, bool isActive)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetRetailersAsync?storeId={2}&take={3}&isActive={4}",
                    WebServiceAddress, ApiControllerName, storeId, take, isActive);
                return HttpRequestHelper.GetUrlResultsAsync<Retailer>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;

            }
        }

        public Task<Retailer> GetRetailerAsync(int retailerId)
        {

            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetRetailerAsync?retailerId={2}",
                    WebServiceAddress, ApiControllerName, retailerId);
                return HttpRequestHelper.GetUrlResultAsync<Retailer>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;

            }
        }
    }
}