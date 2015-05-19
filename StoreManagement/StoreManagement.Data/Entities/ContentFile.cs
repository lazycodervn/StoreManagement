﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class ContentFile : IEntity
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int FileManagerId { get; set; }

        //public virtual ICollection<FileManager> FileManagers { get; set; }
        //public virtual ICollection<Content> Contents { get; set; }

        public FileManager FileManager { get; set; }
        public Content Content { get; set; }


    }
}
