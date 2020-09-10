﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class ConnectionDataBaseDto
    {
        public string DataBaseName { get; set; }
        public string ConnectionString { get; set; }
        public int Product { get; set; }

        public virtual ParametersPontofopagDto ParametersPontofopag { get; set; }
    }
}