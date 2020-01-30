using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DecentCodingAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DecentCodingAnalyzer : DiagnosticAnalyzer
    {
        private const string Category = "Naming";

        #region String Resources

        private static readonly LocalizableString TypeNameTitle = 
            new LocalizableResourceString(nameof(Resources.TypeNameAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString TypeNameMessageFormat = 
            new LocalizableResourceString(nameof(Resources.TypeNameAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString TypeNameDescription = 
            new LocalizableResourceString(nameof(Resources.TypeNameAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString PropertyNameTitle = 
            new LocalizableResourceString(nameof(Resources.PropertyNameAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString PropertyNameMessageFormat = 
            new LocalizableResourceString(nameof(Resources.PropertyNameAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString PropertyNameDescription = 
            new LocalizableResourceString(nameof(Resources.PropertyNameAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        #endregion

        private static readonly DiagnosticDescriptor TypeNameRule = 
            new DiagnosticDescriptor(DiagnosticIdConsts.TypeName, TypeNameTitle, TypeNameMessageFormat, Category, DiagnosticSeverity.Warning, true, TypeNameDescription);

        private static readonly DiagnosticDescriptor PropertyNameRule = 
            new DiagnosticDescriptor(DiagnosticIdConsts.PropertyName, PropertyNameTitle, PropertyNameMessageFormat, Category, DiagnosticSeverity.Warning, true, PropertyNameDescription);

        private static readonly DiagnosticDescriptor FieldNameRule = 
            new DiagnosticDescriptor(DiagnosticIdConsts.FieldName, PropertyNameTitle, PropertyNameMessageFormat, Category, DiagnosticSeverity.Warning, true, PropertyNameDescription);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(TypeNameRule, PropertyNameRule, FieldNameRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeProperty, SymbolKind.Property);
            context.RegisterSymbolAction(AnalyzeField, SymbolKind.Field);
        }

        static bool CheckDirtyWord(string input)
        {
            return DirtyWordMagician.IsFullDirtyWord(input)
                   || DirtyWordMagician.HasDirtyWord(input);
        }

        private static void AnalyzeField(SymbolAnalysisContext context)
        {
            var field = (IFieldSymbol)context.Symbol;

            if (CheckDirtyWord(field.Name))
            {
                var diagnostic = Diagnostic.Create(FieldNameRule, context.Symbol.Locations[0], field.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeProperty(SymbolAnalysisContext context)
        {
            if (CheckDirtyWord(context.Symbol.Name))
            {
                var diagnostic = Diagnostic.Create(PropertyNameRule, context.Symbol.Locations[0], context.Symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            if (context.Symbol is INamedTypeSymbol && CheckDirtyWord(context.Symbol.Name))
            {
                var diagnostic = Diagnostic.Create(TypeNameRule, context.Symbol.Locations[0], context.Symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
