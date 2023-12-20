using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questão2.Models
{
    public class Dados
    {
       private static List<Graph> grafico = new List<Graph>();


        public static void Adiciona(Graph p){

                grafico.Add(p);
        }

         public static List<Graph> Listar()
        {
            return grafico;
        }

        public static void Delete(){
                
                grafico.Clear();
        }
    }
}