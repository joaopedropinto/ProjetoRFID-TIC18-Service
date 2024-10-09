using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal_WebAPI.WebAPI.Models
{
    public class Result
    {
        /// <summary>
        /// Boolean que indica se houve sucesso na requisição
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Mensagem adicional com detalhes sobre o processo
        /// </summary>
        public string Message { get; set; }
    }
}