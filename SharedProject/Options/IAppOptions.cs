﻿namespace FineCodeCoverage.Options
{
    internal interface IFCCCommonOptions
    {
        bool Enabled { get; set; }
        bool DisabledNoCoverage { get; set; }
        bool IncludeTestAssembly { get; set; }
        bool IncludeReferencedProjects { get; set; }

        string[] ExcludeAssemblies { get; set; }
        string[] IncludeAssemblies { get; set; }
    }

    internal interface IMsCodeCoverageIncludesExcludesOptions
    {
        string[] ModulePathsExclude { get; set; }
        string[] ModulePathsInclude { get; set; }
        string[] CompanyNamesExclude { get; set; }
        string[] CompanyNamesInclude { get; set; }
        string[] PublicKeyTokensExclude { get; set; }
        string[] PublicKeyTokensInclude { get; set; }
        string[] SourcesExclude { get; set; }
        string[] SourcesInclude { get; set; }
        string[] AttributesExclude { get; set; }
        string[] AttributesInclude { get; set; }
        string[] FunctionsInclude { get; set; }
        string[] FunctionsExclude { get; set; }
    }
    internal interface IMsCodeCoverageOptions : IMsCodeCoverageIncludesExcludesOptions, IFCCCommonOptions { }
    internal enum RunMsCodeCoverage { No, IfInRunSettings, Yes }

    internal interface IOpenCoverCoverletExcludeIncludeOptions
    {
        string[] Exclude { get; set; }
        string[] ExcludeByAttribute { get; set; }
        string[] ExcludeByFile { get; set; }
        string[] Include { get; set; }
    }

    internal enum OpenCoverRegister { Default,NoArg, User, Path32, Path64}

    internal interface IOpenCoverOptions
    {
        string OpenCoverCustomPath { get; set; }
        OpenCoverRegister OpenCoverRegister { get; set; }
        string OpenCoverTarget { get; set; }
        string OpenCoverTargetArgs { get; set; }
    }
    internal interface IAppOptions : IMsCodeCoverageOptions, IOpenCoverCoverletExcludeIncludeOptions, IFCCCommonOptions, IOpenCoverOptions
    {
        bool RunInParallel { get; set; }
        int RunWhenTestsExceed { get; set; }
        string ToolsDirectory { get; set; }
        bool RunWhenTestsFail { get; set; }
        bool RunSettingsOnly { get; set; }
        bool CoverletConsoleGlobal { get; set; }
        string CoverletConsoleCustomPath { get; set; }
        bool CoverletConsoleLocal { get; set; }
        string CoverletCollectorDirectoryPath { get; set; }
        
        string FCCSolutionOutputDirectoryName { get; set; }
        int ThresholdForCyclomaticComplexity { get; set; }
        int ThresholdForNPathComplexity { get; set; }
        int ThresholdForCrapScore { get; set; }
        bool CoverageColoursFromFontsAndColours { get; set; }
        bool ShowCoverageInOverviewMargin { get; set; }
        bool ShowCoveredInOverviewMargin { get; set; }
        bool ShowUncoveredInOverviewMargin { get; set; }
        bool ShowPartiallyCoveredInOverviewMargin { get; set; }
        bool StickyCoverageTable { get; set; }
        bool NamespacedClasses { get; set; }
        bool HideFullyCovered { get; set; }
        bool Hide0Coverable { get; set; }
        bool Hide0Coverage { get; set; }
        bool AdjacentBuildOutput { get; set; }
        RunMsCodeCoverage RunMsCodeCoverage { get; set; } 
        bool ShowToolWindowToolbar { get; set; }

        NamespaceQualification NamespaceQualification { get; set; }
    }

    internal enum NamespaceQualification
    {
        FullyQualified,
        AlwaysUnqualified,
        UnqualifiedByNamespace,
        QualifiedByNamespaceLevel
    }
}