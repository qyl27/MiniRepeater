using QQMini.PluginSDK.Core;
using QQMini.PluginSDK.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cx.rain.qqmini.repeater
{
    public class Repeater : PluginBase
    {
        public override PluginInfo PluginInfo => new PluginInfo()
        {
            Author = "秋雨落",
            Description = "一个简单的群聊复读机插件。",
            Name = "Mini复读机",
            PackageId = "cx.rain.qqmini.repeater",
            Version = new Version(1, 0, 0, 0)
        };
    }
}
