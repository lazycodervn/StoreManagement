﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class FileManager : IEntity
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Guid { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public Boolean State { get; set; }
        public int Ordering { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ContentLength { get; set; }
        public string GoogleImageId { get; set; }
    }
}
