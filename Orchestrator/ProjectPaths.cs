namespace SiteFactory.Orchestrator;

/// <summary>
/// Gerencia a estrutura de pastas e caminhos de um projeto de cliente específico.
/// </summary>
public class ProjectPaths
{
    public string RootPath { get; }
    public string SrcPath { get; }
    public string SrcStylesPath { get; }
    public string SrcScriptsPath { get; }
    public string PublicPath { get; }
    public string TestsPath { get; }
    public string DocsPath { get; }
    public string AgentsPath { get; }
    public string ProjectJsonPath { get; }

    public ProjectPaths(string baseProjectsDirectory, string projectSlug)
    {
        RootPath = Path.Combine(baseProjectsDirectory, projectSlug);
        SrcPath = Path.Combine(RootPath, "src");
        SrcStylesPath = Path.Combine(SrcPath, "styles");
        SrcScriptsPath = Path.Combine(SrcPath, "scripts");
        PublicPath = Path.Combine(RootPath, "public");
        TestsPath = Path.Combine(RootPath, "tests");
        DocsPath = Path.Combine(RootPath, "docs");
        AgentsPath = Path.Combine(RootPath, ".agents");
        ProjectJsonPath = Path.Combine(RootPath, "project.json");
    }

    /// <summary>
    /// Garante que todos os diretórios do projeto existam no disco.
    /// </summary>
    public void EnsureDirectoriesExist()
    {
        Directory.CreateDirectory(RootPath);
        Directory.CreateDirectory(SrcPath);
        Directory.CreateDirectory(SrcStylesPath);
        Directory.CreateDirectory(SrcScriptsPath);
        Directory.CreateDirectory(PublicPath);
        Directory.CreateDirectory(TestsPath);
        Directory.CreateDirectory(DocsPath);
        Directory.CreateDirectory(AgentsPath);
    }
}
