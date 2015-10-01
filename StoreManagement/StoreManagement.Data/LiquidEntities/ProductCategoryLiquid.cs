﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class ProductCategoryLiquid : BaseDrop
    {
        public ProductCategory ProductCategory { get; set; }



        public ProductCategoryLiquid(ProductCategory productCategory)
        {
            this.ProductCategory = productCategory;
 
        }


        public String DetailLink
        {
            get
            {
                return LinkHelper.GetProductCategoryLink(this.ProductCategory);
            }
        }
        public int Count { get; set; }
    }
}
