using Serilog;

namespace Ajuna.SubstrateSeminarDemos.Console;

public interface IDemoModule
{
    Task ExecuteAsync(ILogger logger);
}