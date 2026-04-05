namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    private static List<string> Numberseparator(string infix)
    {
        var separator = new List<string>();
        var number = "";

        foreach (var c in infix)
        {
            if (char.IsDigit(c) || c == '.')
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
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

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

    private static bool IsOperator(string item)
    {
        return item.Length == 1 && "+-*/^()".Contains(item);
    } 
}