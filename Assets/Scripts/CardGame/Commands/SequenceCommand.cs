// using System.Collections.Generic;
//
// public class SequenceCommand : Command
// {
//     private readonly List<Command> _commands;
//
//     public SequenceCommand(IEnumerable<Command> commands)
//     {
//         _commands = new List<Command>(commands);
//     }
//
//     public override void StartCommandExecution()
//     {
//         // Wstrzyknij pod-komendy na początek kolejki (w zachowanej kolejności)
//         Command.PrependToQueue(_commands);
//
//         // I zakończ tę "komendę-kontener"
//         CommandExecutionComplete();
//     }
// }