﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class UsuarioDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
