using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questao5.Models
{
    public class Restricao
    {
        public string Sinal {get;set;}
        public IEnumerable<Variables> variables {get;set;}
        public string Resultado {get;set;}
    }
}