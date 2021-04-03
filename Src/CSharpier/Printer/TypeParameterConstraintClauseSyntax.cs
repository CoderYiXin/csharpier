using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterConstraintClauseSyntax(
            TypeParameterConstraintClauseSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(
                    node.WhereKeyword,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Name),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(
                    node.ColonToken,
                    afterTokenIfNoTrailing: " "
                ),
                Indent(
                    this.PrintSeparatedSyntaxList(
                        node.Constraints,
                        this.Print,
                        Line
                    )
                )
            );
        }
    }
}
