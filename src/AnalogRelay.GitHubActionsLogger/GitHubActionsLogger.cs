using System;
using System.IO;
using Microsoft.Build.Framework;

namespace AnalogRelay.GitHubActionsLogger
{
    public class GitHubActionsLogger: ILogger
    {
        private readonly WorkflowCommandWriter _writer;

        public GitHubActionsLogger() : this(new WorkflowCommandWriter(Console.Out))
        {
        }

        public GitHubActionsLogger(WorkflowCommandWriter writer)
        {
            _writer = writer;
        }
        
        public void Initialize(IEventSource eventSource)
        {
            eventSource.WarningRaised += eventSource_WarningRaised;
            eventSource.ErrorRaised += eventSource_ErrorRaised;
        }

        public void Shutdown()
        {
        }

        public LoggerVerbosity Verbosity { get; set; }
        public string Parameters { get; set; }

        private async void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            await _writer.WriteErrorAsync(e.Message, e.File, e.LineNumber, e.EndLineNumber);
        }

        private async void eventSource_WarningRaised(object sender, BuildWarningEventArgs e)
        {
            await _writer.WriteWarningAsync(e.Message, e.File, e.LineNumber, e.EndLineNumber);
        }
    }
}
