namespace SiteFactory.Orchestrator;

/// <summary>
/// Logger estilizado para o terminal do SiteFactory.
/// </summary>
public static class ConsoleLogger
{
    private static readonly object Lock = new();

    public static void Banner()
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
   ____ _ _       _____           _                     
  / ___(_) |_ ___|  ___|_ _  ___| |_ ___  _ __ _   _   
  \___ \| | __/ _ \ |_ / _` |/ __| __/ _ \| '__| | | |  
   ___) | | ||  __/  _| (_| | (__| || (_) | |  | |_| |  
  |____/|_|\__\___|_|  \__,_|\___|\__\___/|_|   \__, |  
                                                 |___/   
          Orquestrador de Sites Multi-Agentes (V1)");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Supervisão Humana + Automação de Agentes de IA");
            Console.WriteLine("  -------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public static void ProjectHeader(string clientName)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[SiteFactory]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Projeto: {clientName}\n");
            Console.ResetColor();
        }
    }

    public static void AgentStart(int stepNumber, int totalSteps, string agentName)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{stepNumber}/{totalSteps}] {agentName}...");
            Console.ResetColor();
        }
    }

    public static void AgentFinished(string agentName)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {agentName} concluído\n");
            Console.ResetColor();
        }
    }

    public static void Step(string message)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("    • ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public static void ArtifactCreated(string relativePath)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("      ✔ Artefato: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(relativePath);
            Console.ResetColor();
        }
    }

    public static void Info(string message)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[i] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public static void Success(string message)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[✔] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public static void Warning(string message)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[⚠] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public static void Error(string message)
    {
        lock (Lock)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[✖] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public static void Header(string title)
    {
        lock (Lock)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================================================================================");
            Console.WriteLine($"  {title}");
            Console.WriteLine("================================================================================");
            Console.ResetColor();
        }
    }
}
