using System;
using System.Linq;
using System.Collections.Generic;
using CliWrap;
using CliWrap.Buffered;

namespace lotusctl.Library {
    public static class Systemd {
        public async static void Status(List<Service> services) {
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
        public async static void Start(string serviceName) {
            var result = await GetBasicCommand("start", serviceName)
                .ExecuteAsync();
        }
        public async static void Stop(string serviceName) {
            var result = await GetBasicCommand("stop", serviceName)
                .ExecuteAsync();
        }
        public async static void Restart(string serviceName) {
            var result = await GetBasicCommand("restart", serviceName)
                .ExecuteAsync();
        }

        private static Command GetBasicCommand(string command, string argument) {
            return Cli.Wrap("systemctl")
                .WithArguments($"{command} {argument}")
                .WithValidation(CommandResultValidation.None);
        }
    }
}