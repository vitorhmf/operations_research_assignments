using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Questão2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Google.OrTools.LinearSolver;


namespace Questão2.Controllers;

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
        
        return View();
    }

    [HttpPost]
    public IActionResult Restricao(string restricao="1")
    {
        ViewData["restricao"] = restricao;
        Console.WriteLine(restricao);
        return View("Index");
    }


    [HttpPost]
    public IActionResult Equacoes(Grafico grafic)
    {
        //List<string> list = new List<string>();
        //var allCounts = grafic.Restricoes.Select(c => c.Sinal);
        
        ViewData["Equacao"] = grafic.option + " " + "Z = "+ grafic.X1+ "X1 + "+ grafic.X2 + "X2";
        
        Solver solver = Solver.CreateSolver("GLOP");
        

        // Create the variables x and y.
        Variable xx = solver.MakeNumVar(0.0, double.PositiveInfinity, "x");
        Variable yy = solver.MakeNumVar(0.0, double.PositiveInfinity, "y");
        
        
        Graph u = new Graph();
        int cont = 1;
        int testador =0;
        Graph d = new Graph();
        d.id = "Graph" + Convert.ToString(cont);
        foreach(var item in grafic.Restricoes){
                
                if(testador==0){
                
                d.latex = item.X1res+"x + "+item.X2res+"y "+ item.Sinal + " " + item.Resultado;
              
                }
                else{
                
                d.latex += " \\left\\{"+item.X1res+"x + "+item.X2res+"y "+ item.Sinal + " " + item.Resultado+" \\right\\}";
                
                }
                testador+=1;
        }
        d.latex+=" \\left\\{x>=0 \\right\\} \\left\\{ y>=0 \\right\\}";
        Dados.Adiciona(d);
        cont+=1;
        testador=0;
      

        foreach(var item in grafic.Restricoes){
                if(testador!=0){
                Graph j = new Graph();
                j.id = "Graph" + Convert.ToString(cont);
                j.latex = item.X1res+"x + "+item.X2res+"y = " + item.Resultado;
                Dados.Adiciona(j);
                cont+=1;}
         
        if(String.Compare(item.Sinal,"<=")==0){
            
            solver.Add(Convert.ToDouble(item.X1res)*xx + Convert.ToDouble(item.X2res)*yy <= Convert.ToDouble(item.Resultado));
        }

        

        else if(string.Compare(item.Sinal,">=")==0){
            solver.Add(Convert.ToDouble(item.X1res)*xx + Convert.ToDouble(item.X2res)*yy >= Convert.ToDouble(item.Resultado));
        }

        else{
            solver.Add(Convert.ToDouble(item.X1res)*xx + Convert.ToDouble(item.X2res)*yy == Convert.ToDouble(item.Resultado));
        }
                testador+=1;
        }

        ViewData["erro"]="";
        ViewData["Otimox"]=0.0;
        ViewData["Otimoy"]=0.0;
        ViewData["final"]=0.0;
        if(string.Compare(grafic.option,"Max")==0){ 
         
        solver.Maximize(Convert.ToDouble(grafic.X1)*xx + Convert.ToDouble(grafic.X2)*yy);
        } 
        else{
            
            solver.Minimize(Convert.ToDouble(grafic.X1)*xx + Convert.ToDouble(grafic.X2)*yy);
            Console.WriteLine("teste");
        }   
         Solver.ResultStatus resultStatus = solver.Solve();

        // Check that the problem has an optimal solution.
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
        {
            Console.WriteLine("The problem does not have an optimal solution!");
            ViewData["erro"] = "O problema não tem uma solução ótima!"; 
        }
        else{
        Console.WriteLine("Solution:");
        Console.WriteLine("Objective value = " + solver.Objective().Value());
        Console.WriteLine("x = " + xx.SolutionValue());
        Console.WriteLine("y = " + yy.SolutionValue());
        ViewData["Otimox"] = xx.SolutionValue();
        ViewData["Otimoy"] = yy.SolutionValue();
        ViewData["final"] = solver.Objective().Value();
    
        }
        List<Graph> x = Dados.Listar();
        
       
        var stringjson = JsonConvert.SerializeObject(x);
        Dados.Delete();
        ViewData["Desmos"] = stringjson;
        return View(grafic);
    }

   

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
