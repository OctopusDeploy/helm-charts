using System;
using System.Text;
using Halibut;
using KubernetesAgent.Integration.Setup.Common;
using KubernetesAgent.UpgradeManager;
using Octopus.Tentacle.Client;
using Octopus.Tentacle.Client.Scripts.Models;
using Octopus.Tentacle.Client.Scripts.Models.Builders;
using Octopus.Tentacle.Contracts;
using Octopus.Versioning;
using File = KubernetesAgent.UpgradeManager.File;

namespace KubernetesAgent.Integration.Utils
{
    public class UpgradeManagerWrapper
    {
        readonly KubernetesAgentUpgradeManager upgradeManager;
        readonly TentacleClient client;
        readonly ILogger logger;

        readonly Func<string, string> setOctopusVariableFunctionInjector = content =>
            $$"""
              function encode_servicemessagevalue
              {
                  echo -n "$1" | openssl enc -base64 -A;
              }
              function set_octopusvariable
              {
                  MESSAGE="##octopus[setVariable";
                  if [ -n "$1" ]; then
                      MESSAGE="$MESSAGE name='$(encode_servicemessagevalue "$1")'";
                  fi
                  if [ -n "$2" ]; then
                      MESSAGE="$MESSAGE value='$(encode_servicemessagevalue "$2")'";
                  fi
                  MESSAGE="$MESSAGE]";
                  echo $MESSAGE;
              }
              {{content}}
              """;

        public UpgradeManagerWrapper(KubernetesAgentUpgradeManager upgradeManager, TentacleClient client, ILogger logger)
        {
            this.upgradeManager = upgradeManager;
            this.client = client;
            this.logger = logger;
        }

        public async Task UpgradeAgent(
            string releaseName,
            string @namespace,
            IVersion currentVersion,
            IVersion newVersion,
            File package,
            CancellationToken cancellationToken)
        {
            await upgradeManager.UpgradeAgent(releaseName, @namespace, currentVersion, newVersion, async (script, files, ct) => await RunScript(script, files.Append(package).ToArray(), ct), cancellationToken);
        }

        async Task<ScriptResult> RunScript(string script, File[] files, CancellationToken _)
        {
            var bootstrapScript = setOctopusVariableFunctionInjector(
                """
                chmod +x script.sh
                output=$(./script.sh)
                echo "Output is: '$output'"
                set_octopusvariable "ScriptOutput" "$output"
                """);
            var commandBuilder = new ExecuteKubernetesScriptCommandBuilder(Guid.NewGuid().ToString())
                .IsRawScript()
                .WithScriptBody(bootstrapScript)
                .WithScriptFile(new ScriptFile("script.sh", DataStream.FromBytes(Encoding.ASCII.GetBytes(script))));
            foreach (var file in files)
            {
                commandBuilder.WithScriptFile(new ScriptFile(file.Name, DataStream.FromBytes(file.Content)));
            }

            var command = commandBuilder.Build();

            var serviceMessages = new List<ServiceMessage>();
            var serviceMessageParser = new ServiceMessageParser(output => logger.Information("{Output}", output), serviceMessage => serviceMessages.Add(serviceMessage));

            var testLogger = new TestLogger(logger);
            var result = await client.ExecuteScript(command, OnScriptStatusResponseReceived, OnScriptCompleted, testLogger, CancellationToken.None);

            var outputs = serviceMessages.FirstOrDefault(m => m.Name == "setVariable" && m.Properties.TryGetValue("name", out var name) && name == "ScriptOutput")?.Properties.TryGetValue("value", out var value) == true ? value : string.Empty;

            return new ScriptResult(result.ExitCode, outputs);

            async Task OnScriptCompleted(CancellationToken t)
            {
                await Task.CompletedTask;
            }

            void OnScriptStatusResponseReceived(ScriptExecutionStatus res)
            {
                foreach (var log in res.Logs)
                {
                    serviceMessageParser.Append(log.Text);
                }
            }
        }
    }
}
