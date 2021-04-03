using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLetClauseSyntax(LetClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(
                    node.LetKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.Identifier,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(
                    node.EqualsToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Expression)
            );
        }
    }
}
