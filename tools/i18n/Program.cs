using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;

var root = Path.GetFullPath(args[0]);
var values = new List<object>();
foreach (var file in Directory.EnumerateFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories)
    .Where(p => !p.Contains("/obj/") && !p.Contains("/bin/")).Order())
{
    var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
    if (tree.GetDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error)) throw new InvalidDataException("Invalid C# syntax: " + file);
    foreach (var node in tree.GetRoot().DescendantNodes().Where(n =>
        n.IsKind(SyntaxKind.StringLiteralExpression) || n is InterpolatedStringExpressionSyntax))
    {
        var interpolation = node as InterpolatedStringExpressionSyntax;
        var chunks = interpolation?.Contents.Select(c => c switch
        {
            InterpolatedStringTextSyntax text => new { text = (string?)text.TextToken.ValueText, expression = (string?)null, format = (string?)null },
            InterpolationSyntax slot => new { text = (string?)null, expression = (string?)slot.Expression.ToString(), format = slot.FormatClause?.FormatStringToken.ValueText },
            _ => throw new InvalidOperationException()
        }).ToArray();
        values.Add(new {
            file = Path.GetRelativePath(root, file).Replace('\\', '/'),
            line = tree.GetLineSpan(node.Span).StartLinePosition.Line + 1,
            start = node.SpanStart, length = node.Span.Length, code = node.ToString(),
            text = node is LiteralExpressionSyntax literal ? literal.Token.ValueText : null,
            interpolated = interpolation is not null, chunks,
            nested = node.Ancestors().Any(a => a is InterpolatedStringExpressionSyntax),
            context = node.Ancestors().OfType<MemberDeclarationSyntax>().FirstOrDefault()?.ToString().Split('\n')[0]
        });
    }
}
File.WriteAllText(args[1], JsonSerializer.Serialize(values, new JsonSerializerOptions { WriteIndented = true }));
