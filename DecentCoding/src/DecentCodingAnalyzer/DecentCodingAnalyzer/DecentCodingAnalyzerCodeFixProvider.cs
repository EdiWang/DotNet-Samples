using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace DecentCodingAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DecentCodingAnalyzerCodeFixProvider)), Shared]
    public class DecentCodingAnalyzerCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Rename dirty word";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(new[]
        {
            DiagnosticIdConsts.TypeName,
            DiagnosticIdConsts.PropertyName,
            DiagnosticIdConsts.FieldName
        });

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            if (context.Diagnostics.Any())
            {
                foreach (var diagnostic in context.Diagnostics)
                {
                    var diagnosticSpan = diagnostic.Location.SourceSpan;
                    switch (diagnostic.Descriptor.Id)
                    {
                        case DiagnosticIdConsts.TypeName:
                            {
                                var typeDecl = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
                                if (null != typeDecl)
                                {
                                    context.RegisterCodeFix(
                                        CodeAction.Create(
                                            title: Title + " in type name",
                                            createChangedSolution: c => MakeDecentAsync(context.Document, typeDecl, c),
                                            equivalenceKey: Title + " in type name"),
                                        diagnostic);
                                }

                                break;
                            }

                        case DiagnosticIdConsts.PropertyName:
                            {
                                var propDecl = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<PropertyDeclarationSyntax>().FirstOrDefault();
                                if (null != propDecl)
                                {
                                    context.RegisterCodeFix(
                                        CodeAction.Create(
                                            title: Title + " in property name",
                                            createChangedSolution: c => MakeDecentAsync(context.Document, propDecl, c),
                                            equivalenceKey: Title + " in property name"),
                                        diagnostic);
                                }
                                break;
                            }

                        case DiagnosticIdConsts.FieldName:
                            {
                                var fieldDecl = root.FindToken(diagnostic.Location.SourceSpan.Start);
                                context.RegisterCodeFix(
                                    CodeAction.Create(
                                        title: Title + " in field name",
                                        createChangedSolution: c => MakeFieldDecentAsync(context.Document, fieldDecl, c),
                                        equivalenceKey: Title + " in field name"),
                                    diagnostic);
                                break;
                            }
                    }
                }
            }
        }

        private async Task<Solution> MakeFieldDecentAsync(Document document, SyntaxToken declaration,
            CancellationToken cancellationToken)
        {
            var newName = DirtyWordMagician.DirtyToDecent(declaration.ValueText);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var symbol = semanticModel.GetDeclaredSymbol(declaration.Parent, cancellationToken);
            var solution = document.Project.Solution;
            return await Renamer.RenameSymbolAsync(solution, symbol, newName, solution.Workspace.Options, cancellationToken).ConfigureAwait(false);
        }

        private async Task<Solution> MakeDecentAsync(Document document, MemberDeclarationSyntax memberDecl, CancellationToken cancellationToken)
        {
            string identifierTokenText = null;
            switch (memberDecl)
            {
                case TypeDeclarationSyntax ts:
                    identifierTokenText = ts.Identifier.Text;
                    break;
                case PropertyDeclarationSyntax ps:
                    identifierTokenText = ps.Identifier.Text;
                    break;
            }

            var newName = DirtyWordMagician.DirtyToDecent(identifierTokenText);

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(memberDecl, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }
    }
}
