import numpy as np
from scipy.optimize import linprog

# Objective function coefficients (c)
c = np.array([-23, -32])

# Coefficient matrix of the inequalities (A)
A = np.array([[10, 6], [5, 10],[1,2]])

# Right-hand side of the inequalities (b)
b = np.array([2500,2000,500])

# Solve the linear programming problem
result = linprog(c, A_ub=A, b_ub=b, method='highs')

# Print the results
print("Status:", result.message)
print("Optimal values:", result.x)
print("Objective value:", result.fun*-1)


dual_c = b
dual_b = -1.0 * c
dual_A = -1.0 * np.transpose(A)
result = linprog(b,dual_A,c)

# Extract shadow prices from the dual solution
#shadow_prices = result.slack

#print("Shadow Prices:", shadow_prices)
print(result.x)
