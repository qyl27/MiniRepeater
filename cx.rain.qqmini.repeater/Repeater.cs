using QQMini.PluginSDK.Core;
using QQMini.PluginSDK.Core.Model;
using System;
using System.Collections.Generic;

namespace cx.rain.qqmini.repeater
{
    public class Repeater : PluginBase
    {
        private static readonly Dictionary<long, Tuple<long, string>> LastMessages = new Dictionary<long, Tuple<long, string>>();
        private static readonly Dictionary<long, HashSet<long>> MessageSenders = new Dictionary<long, HashSet<long>>();
        private static readonly Dictionary<long, string> RepeatedMessages = new Dictionary<long, string>();

        public override PluginInfo PluginInfo => new PluginInfo()
        {
            Author = "秋雨落",
            Description = "一个简单的群聊复读机插件。",
            Name = "Mini复读机",
            PackageId = "cx.rain.qqmini.repeater",
            Version = new Version(1, 1, 0, 0)
        };

        public override void OnInitialize()
        {
            QMLog.Info("[Mini复读机] 加载成功！");
            // Todo: Configuration files.
        }

        public override void OnUninitialize()
        {
            QMLog.Info("[Mini复读机] 卸载成功！");
            // Todo: Configuration files.
        }

        public override QMEventHandlerTypes OnReceiveGroupMessage(QMGroupMessageEventArgs e)
        {
            if (!LastMessages.ContainsKey(e.FromGroup.Id))
            {
                LastMessages.Add(e.FromGroup.Id, new Tuple<long, string>(e.FromQQ.Id, e.Message.Text));
            }
            else
            {
                LastMessages[e.FromGroup.Id] = new Tuple<long, string>(e.FromQQ.Id, e.Message.Text);
            }

            if (!MessageSenders.ContainsKey(e.FromGroup.Id))
            {
                MessageSenders[e.FromGroup.Id] = new HashSet<long>();
            }

            MessageSenders[e.FromGroup.Id].Add(e.FromQQ.Id);

            if (!RepeatedMessages.ContainsKey(e.FromGroup.Id))
            {
                RepeatedMessages.Add(e.FromGroup.Id, null);
            }

            if (RepeatedMessages[e.FromGroup.Id] != null && e.Message.Text == RepeatedMessages[e.FromGroup.Id])
            {
                MessageSenders[e.FromGroup.Id].Clear();
                return QMEventHandlerTypes.Continue;
            }

            if (MessageSenders[e.FromGroup.Id].Count >= 3)
            {
                QMApi.SendGroupMessage(e.RobotQQ, e.FromGroup, e.Message.Text);
                RepeatedMessages[e.FromGroup.Id] = e.Message.Text;
                MessageSenders[e.FromGroup.Id].Clear();
            }

            return QMEventHandlerTypes.Continue;
        }
    }
}
