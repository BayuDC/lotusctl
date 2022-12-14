using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CliWrap;
using CliWrap.Buffered;

namespace lotusctl.Library {
    public static class Systemd {
        public async static Task Load(List<Service> services) {
            var serviceList = services.Aggregate("", (acc, service) => $"{acc} {service.CodeName}");
            var result = await Cli.Wrap("systemctl")
                .WithArguments("is-active" + serviceList)
                .WithValidation(CommandResultValidation.None)
                .ExecuteBufferedAsync();
            var status = result.StandardOutput.Split('\n');

            for (var i = 0; i < services.Count; i++) {
                services[i].IsActive = status[i] == "active";
            }
        }
        public async static Task<string> Start(string serviceName)
            => await RunBasicCommand("start", serviceName);
        public async static Task<string> Stop(string serviceName) {
            var a =
                await RunBasicCommand("stop", serviceName);
            return a;
        }
        public async static Task<string> Restart(string serviceName)
            => await RunBasicCommand("restart", serviceName);
        public async static Task<string> Status(string serviceName)
            => await RunBasicCommand("status", serviceName);

        private async static Task<string> RunBasicCommand(string command, string argument) {
            var result = await Cli.Wrap("systemctl")
                .WithArguments($"{command} {argument}")
                .WithValidation(CommandResultValidation.None)
                .ExecuteBufferedAsync();
            return result.StandardOutput + result.StandardError;
        }
    }
}