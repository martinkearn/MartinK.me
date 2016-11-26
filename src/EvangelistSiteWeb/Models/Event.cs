﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvangelistSiteWeb.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Conference { get; set; }
        public string Talk { get; set; }
        public string ExternalUrl { get; set; }
        public string InternalUrl { get; set; }

        public DateTime Date { get; set; }
    }
}
