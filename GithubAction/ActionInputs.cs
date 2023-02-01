using CommandLine;
using System.Runtime.InteropServices;

namespace GithubAction
{
    public class ActionInputs
    {
        string _repositoryName = null!;

        [Option('n', "name",
            HelpText = "The repository name, for example: \"samples\". Assign from `github.repository`.")]
        public string Name
        {
            get => _repositoryName;
            set => ParseAndAssign(value, str => _repositoryName = str);
        }

        static void ParseAndAssign(string? value, Action<string> assign)
        {
            if (value is { Length: > 0 } && assign is not null)
            {
                assign(value.Split("/")[^1]);
            }
        }
    }
}
