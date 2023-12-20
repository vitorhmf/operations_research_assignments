import lippy as lp
from scipy.optimize import linprog
import numpy as np

def Restricao(numerodevariaveis,numerorestricoes):
    restricao = []
    resultado = []
    A = np.array([])
    b = np.array([])

    print("Informe as restrições:")
    for i in range(numerorestricoes):
        l = []
        print("Restrição "+str(i+1))
        for j in range(numerodevariaveis+1):
        
            if(j!=numerodevariaveis):
                print("Informe o valor da variável X"+str(j+1)+ " da Restrição  "+str(i+1))
                x= float(input())
                l.append(x)
            else:
                print("Informe o resultado:")
                x= float(input())
                resultado.append(x)
                b = np.append(b,x)
        restricao.append(l)
    A = np.array(restricao)
    return restricao,resultado,A,b

numerodevariaveis = int(input("Numéro de variáveis: "))
numerorestricoes = int(input("Números de restrições: "))

objetivefunction = []
c = np.array([])

print("Informe a função objetivo")
for i in range(numerodevariaveis):
    print("Informe o valor da variável X"+str(i+1) + " da função objetivo")
    x= float(input())
    c = np.append(c,-1.0*x)
    objetivefunction.append(x)

restricao,resultado,A,b= Restricao(numerodevariaveis,numerorestricoes)
    
simplex = lp.SimplexMethod(objetivefunction, restricao, resultado,log_mode=lp.LogMode.FULL_LOG)
solution, func_value = simplex.solve()
print(solution,func_value)

print("Simplex Dual:")


objetivefunction, restricao, resultado = lp.primal_to_dual_lp(objetivefunction, restricao, resultado)
simplex = lp.SimplexMethod(objetivefunction, restricao, resultado, log_mode=lp.LogMode.FULL_LOG)
solution, func_value = simplex.solve()
print(solution,func_value)

print("Preço Sombra")
#Resolve Shadow prices
dual_c = b
dual_b = -1.0 * c

dual_A = -1.0 * np.transpose(A)
result = linprog(b,dual_A,c)

# Extract shadow prices from the dual solution
#shadow_prices = result.slack

#print("Shadow Prices:", shadow_prices)
print(result.x)