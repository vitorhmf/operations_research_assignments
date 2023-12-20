using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Questao5.Models
{
    public class Grafico
    {
        public string option {get;set;}
        public IEnumerable<Variables> variable {get;set;}
        public string NumerodeRestricoes {get;set;}
        public IEnumerable<Restricao> Restricoes {get;set;}
    }
}