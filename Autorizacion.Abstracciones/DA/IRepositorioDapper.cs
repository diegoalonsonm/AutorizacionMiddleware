﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorizacion.Abstracciones.DA
{
    public interface IRepositorioDapper
    {
        SqlConnection ObtenerRepositorioDapper();
    }
}