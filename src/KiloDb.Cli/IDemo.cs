namespace KiloDb.Cli;

public interface IDemo
{
    string Command {get;}
    string Description {get; }
    void Run(string[] args);
}