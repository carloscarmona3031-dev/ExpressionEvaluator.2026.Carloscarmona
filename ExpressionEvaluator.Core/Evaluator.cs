namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    //este metodo separa toda la expresion matematica en "tokens" (numeros y operadores)
    //tambien permite evaluar numeros negativos mirando si el "-" hace parte de una operacion o del resultado como tal

    private static List<string> Numberseparator(string infix)
    {
        var separator = new List<string>();
        var number = "";

        for (int i = 0; i < infix.Length; i++)
        {
            var c = infix[i];

            if (char.IsDigit(c) || c == '.')
            {
                number += c;
            }
            else if (c == '-' && (i == 0 || "+-*/^(".Contains(infix[i - 1])))
            {
                number += c;
            }
            else
            {
                if (!string.IsNullOrEmpty(number))
                {
                    separator.Add(number);
                    number = "";
                }

                if (!char.IsWhiteSpace(c))
                    separator.Add(c.ToString());
            }
        }

        if (!string.IsNullOrEmpty(number))
            separator.Add(number);

        return separator;
    }

    //convierte la expresion de infija a posfija
    private static List<string> InfixToPostfix(string infix)
    {
        var separator = Numberseparator(infix);
        var output = new List<string>();
        var stack = new Stack<string>();

        foreach (var separate in separator)
        {
            if (!IsOperator(separate))
            {
                output.Add(separate);
            }
            else if (separate == "(")
            {
                stack.Push(separate);
            }
            else if (separate == ")")
            {
                while (stack.Peek() != "(") output.Add(stack.Pop());
                stack.Pop();
            }
            else
            {
                while (stack.Count > 0 && 
                    PriorityStack(stack.Peek()) >= PriorityInfix(separate)) 
                {
                    output.Add(stack.Pop());
                }
                stack.Push(separate);
            }
        }

        while (stack.Count > 0) 
            output.Add(stack.Pop());

        return output;
            
    }
    //estos metodos fueron cambiados para que ya no evaluaran las expresiones individiales en "char" si no en grupos o "tokens" "String"
    private static int PriorityStack(string item) => item switch
    {
        "^" => 3,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 0,
        _ => throw new Exception("Sintax error."),
    };
    
    private static int PriorityInfix(string item) => item switch
    {
        "^" => 4,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 5,
        _ => throw new Exception("Sintax error."),
    };
    //evalua la expresion en notacion posfija, haciendo los calculos y sacando o añadiendo al stack
    private static double EvaluatePostfix(List<string> postfix)
    {
        var stack = new Stack<double>();

        foreach (var separate in postfix)
        {
            if (!IsOperator(separate))
            {
                stack.Push(double.Parse(separate));
            }
            else
            {
                var b = stack.Pop();
                var a = stack.Pop();

                stack.Push(separate switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    "^" => Math.Pow(a, b),_ => throw new Exception("Sintax error.")
                });
            }
        }

        return stack.Pop();   
    }
    //este determina si un grupo o "token" es operador o no
    private static bool IsOperator(string item)
    {
        return item.Length == 1 && "+-*/^()".Contains(item);
    } 
}