using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Questão2.Models
{
    public class Grafico
    {
        public string option {get;set;}
        public string X1 {get;set;}
        public string X2 {get;set;}
        public string NumerodeRestricoes {get;set;}
        public IEnumerable<Restricao> Restricoes {get;set;}
    }
}