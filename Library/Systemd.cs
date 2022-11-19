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
    }
}