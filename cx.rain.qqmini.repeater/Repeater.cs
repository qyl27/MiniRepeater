using QQMini.PluginSDK.Core;
using QQMini.PluginSDK.Core.Model;
using System;
using System.Collections.Generic;

namespace cx.rain.qqmini.repeater
{
    public class Repeater : PluginBase
    {
        private static readonly Dictionary<Group, Tuple<QQ, Message>> LastMessages = new Dictionary<Group, Tuple<QQ, Message>>();
        private static readonly Dictionary<Group, HashSet<QQ>> MessageSenders = new Dictionary<Group, HashSet<QQ>>();
        private static readonly Dictionary<Group, Message> RepeatedMessages = new Dictionary<Group, Message>();

        public override PluginInfo PluginInfo => new PluginInfo()
        {
            Author = "秋雨落",
            Description = "一个简单的群聊复读机插件。",
            Name = "Mini复读机",
            PackageId = "cx.rain.qqmini.repeater",
            Version = new Version(1, 0, 0, 0)
        };

        public override void OnInitialize()
        {
            // Todo: Configuration files.
        }

        public override void OnUninitialize()
        {
            // Todo: Configuration files.
        }

        public override QMEventHandlerTypes OnReceiveGroupMessage(QMGroupMessageEventArgs e)
        {
            if (!LastMessages.ContainsKey(e.FromGroup))
            {
                LastMessages.Add(e.FromGroup, new Tuple<QQ, Message>(e.FromQQ, e.Message));
            }
            else
            {
                LastMessages[e.FromGroup] = new Tuple<QQ, Message>(e.FromQQ, e.Message);
            }

            if (!MessageSenders.ContainsKey(e.FromGroup))
            {
                MessageSenders[e.FromGroup] = new HashSet<QQ>();
            }

            MessageSenders[e.FromGroup].Add(e.FromQQ);

            if (!RepeatedMessages.ContainsKey(e.FromGroup))
            {
                RepeatedMessages.Add(e.FromGroup, null);
            }

            if (RepeatedMessages[e.FromGroup] != null && e.Message == RepeatedMessages[e.FromGroup])
            {
                MessageSenders[e.FromGroup].Clear();
                return QMEventHandlerTypes.Continue;
            }

            if (e.Message == LastMessages[e.FromGroup].Item2)
            {
                MessageSenders[e.FromGroup].Clear();
                return QMEventHandlerTypes.Continue;
            }

            if (MessageSenders[e.FromGroup].Count >= 3)
            {
                QMApi.SendGroupMessage(e.RobotQQ, e.FromGroup, e.Message);
                RepeatedMessages[e.FromGroup] = e.Message;
                MessageSenders[e.FromGroup].Clear();
            }

            return QMEventHandlerTypes.Continue;
        }
    }
}
