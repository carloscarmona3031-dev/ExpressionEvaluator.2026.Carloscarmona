using ExpressionEvaluator.Core;

try
{
    
    var result2 = Evaluator.Evaluate("2*7/4-(8-9^(1/2))+6");
    var result3 = Evaluator.Evaluate("4*(5+6-(8/2^3)-7)-1");

    
    Console.WriteLine(result2);
    Console.WriteLine(result3);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}