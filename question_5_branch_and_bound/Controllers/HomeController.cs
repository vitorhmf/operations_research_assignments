using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Questao5.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Google.OrTools.LinearSolver;


namespace Questao5.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    

   [HttpGet]
    public IActionResult Index()
    {
        ViewData["restricao"] = "1";
        ViewData["nvariables"] = "2";
        return View();
    }

    [HttpPost]
    public IActionResult Restricao(string nvariables="2",string restricao="1")
    {
        ViewData["restricao"] = restricao;
        ViewData["nvariables"] = nvariables;
        Console.WriteLine("{0} {1}",nvariables,restricao);
        return View("Index");
    }


    [HttpPost]
    public IActionResult Equacoes(Grafico grafic)
    {
        string wrtu= grafic.option + " " + "Z =";
       int cont = 0;
       foreach (var item in grafic.variable)
       {
           
            if(cont==0){
            wrtu += item.X1+"X"+Convert.ToString(cont+1);}
            else{
                wrtu += " + "+item.X1+"X"+Convert.ToString(cont+1);
            }
            cont++;
       }

       double[] funcaoobjetivo = new double[cont];
       double[] z = new double[cont];
       int conta = 0;
       foreach (var item in grafic.variable)
       {
            Console.WriteLine(item.X1);
           
            funcaoobjetivo[conta] = Convert.ToDouble(item.X1);
            conta++;
       }

        ViewData["Equacao"] = wrtu; 
        Solver solver = Solver.CreateSolver("SCIP");
        

        // Create the variables x and y.
        Variable[] x = new Variable[cont];
        for (int j = 0; j < cont; j++)
        {
            x[j] = solver.MakeIntVar(0.0, double.PositiveInfinity, $"x_{j}");
            
        }
        
        
        int contador=0;
        
        foreach(var item in grafic.Restricoes){
        Constraint constraint;
       if(string.Compare(item.Sinal,">=")==0){  
        constraint = solver.MakeConstraint(Convert.ToDouble(item.Resultado),double.PositiveInfinity ,"");
       }
       
       else{
         constraint = solver.MakeConstraint(0, Convert.ToDouble(item.Resultado), "");
       }

        contador=0;
        foreach (var item2 in item.variables)
        {
         
          constraint.SetCoefficient(x[contador], Convert.ToDouble(item2.X1));
          
          contador++;   
        }
        
               
        }
        Console.WriteLine("Number of constraints = " + solver.NumConstraints());
        
        cont=0;   
        Objective objective = solver.Objective();
        
        foreach (var item3 in grafic.variable)
       {
            
            objective.SetCoefficient(x[cont], Convert.ToDouble(item3.X1));
            //Console.WriteLine("{0} {1}",cont,item3.X1);
            cont++;
       }
        
     
       
        ViewData["erro"]="";
        
        ViewData["interation"]=0;
        ViewData["Numerobranchandbounds"]=0;
        ViewData["final"]=0.0;
        ViewData["Time"] =0.0;
        ViewData["array"] = funcaoobjetivo;
        if(string.Compare(grafic.option,"Max")==0){ 
         objective.SetMaximization();
        } 
        else{
            objective.SetMinimization();
        } 
        
         Solver.ResultStatus resultStatus = solver.Solve();

        // Check that the problem has an optimal solution.
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
        {
            //Console.WriteLine("The problem does not have an optimal solution!");
            ViewData["erro"] = "O problema não tem uma solução ótima!"; 
        }
        else{
        Console.WriteLine("Solution:");
        Console.WriteLine("Objective value = " + solver.Objective().Value());
        Console.WriteLine("Solution:");
        ViewData["final"] =  solver.Objective().Value();
        Console.WriteLine("Optimal objective value = " + solver.Objective().Value());
        Console.WriteLine("Optimal objective value = " + solver.Objective().BestBound());
        double soma=0.0;
        for (int j = 0; j < cont; j++)
        {
            soma+= x[j].SolutionValue()*funcaoobjetivo[j];
            z[j] = x[j].SolutionValue();
            Console.WriteLine("x[" + j + "] = " + x[j].SolutionValue());
        }
        ViewData["array"] = z;
        Console.WriteLine(soma);
        Console.WriteLine("\nAdvanced usage:");
        ViewData["Time"] = solver.WallTime();
        Console.WriteLine("Problem solved in " + solver.WallTime() + " milliseconds");
        ViewData["interation"] = solver.Iterations();
        Console.WriteLine("Problem solved in " + solver.Iterations() + " iterations");
        ViewData["Numerobranchandbounds"] = solver.Nodes();
        Console.WriteLine("Problem solved in " + solver.Nodes() + " branch-and-bound nodes");
    
        }
 
        return View(grafic);
    }

   

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Privacy(Grafico grafic)
    {
        foreach (var item in grafic.Restricoes)
        {
            
            
            foreach (var item2 in item.variables)
            {
              Console.WriteLine(item2.X1);    
            }
            
        }
        return View(grafic);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
