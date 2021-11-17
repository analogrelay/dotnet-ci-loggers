using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnalogRelay.GitHubActionsLogger
{
    public class WorkflowCommandWriter
    {
        private readonly TextWriter _writer;

        public WorkflowCommandWriter(TextWriter writer)
        {
            _writer = writer;
        }
        
        public virtual async Task WriteCommandAsync(string command, string value, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var paramString = new StringBuilder();
            foreach (var (key, val) in parameters)
            {
                paramString.Append($"{key}={val},");
            }

            var output = new StringBuilder();
            output.Append($"::{command}");

            if (paramString.Length > 1)
            {
                paramString.Length -= 1;
                output.Append($" {paramString}");
            }

            output.Append("::");
            if (!string.IsNullOrEmpty(value))
            {
                output.Append(value);
            }

            await _writer.WriteLineAsync(output.ToString());
        }

        public virtual Task WriteErrorAsync(string message, string file = null, int? line = null,
            int? endLine = null, string title = null)
        {
            var parameters = new Dictionary<string, string>();
            SetSourceLocationParameters(file, line, endLine, title, parameters);

            return WriteCommandAsync("error", message, parameters);
        }

        public virtual Task WriteWarningAsync(string message, string file = null, int? line = null,
            int? endLine = null, string title = null)
        {
            var parameters = new Dictionary<string, string>();
            SetSourceLocationParameters(file, line, endLine, title, parameters);

            return WriteCommandAsync("warning", message, parameters);
        }

        private static void SetSourceLocationParameters(string file, int? line, int? endLine, string title,
            Dictionary<string, string> parameters)
        {
            if (!string.IsNullOrEmpty(file))
            {
                parameters["file"] = file;
            }

            if (line != null)
            {
                parameters["line"] = line.Value.ToString();
            }

            if (endLine != null)
            {
                parameters["endLine"] = endLine.Value.ToString();
            }

            if (!string.IsNullOrEmpty(title))
            {
                parameters["title"] = title;
            }
        }
    }
}