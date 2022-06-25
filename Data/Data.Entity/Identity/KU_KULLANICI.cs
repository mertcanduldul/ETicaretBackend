﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entity.Identity
{
    public class KU_KULLANICI
    {
        [Key]
        public int ID_KULLANICI { get; set; }

        public string AD_SOYAD { get; set; }

        public string E_MAIL { get; set; }

        public string SIFRE { get; set; }

        public bool ACCOUNT_VERIFIED { get; set; }

        public DateTime? CREDATE { get; set; }

        public bool DELETED { get; set; }
    }
}

