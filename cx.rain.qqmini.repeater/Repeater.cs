using QQMini.PluginSDK.Core;
using QQMini.PluginSDK.Core.Model;
using System;
using System.Collections.Generic;

namespace cx.rain.qqmini.repeater
{
    public class Repeater : PluginBase
    {
        private static readonly Dictionary<Group, List<KeyValuePair<QQ, Message>>> groupLog = new Dictionary<Group, List<KeyValuePair<QQ, Message>>>();

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
            //添加没有记录的新群
            if (groupLog.ContainsKey(e.FromGroup) == false)
            {
                groupLog.Add(e.FromGroup, new List<KeyValuePair<QQ, Message>>(3));
            }

            //防止单人复读
            if (groupLog[e.FromGroup][2].Key == e.FromQQ &&
                groupLog[e.FromGroup][2].Value == e.Message)
            {
                return QMEventHandlerTypes.Continue;
            }

            //添加消息记录
            groupLog[e.FromGroup].Add(new KeyValuePair<QQ, Message>(e.FromQQ, e.Message));
            groupLog[e.FromGroup].RemoveAt(0);

            //如果当前消息和倒数第二条记录一样，且不和第三条记录相同就复读
            if (e.Message == groupLog[e.FromGroup][1].Value &&
                e.Message != groupLog[e.FromGroup][0].Value)
            {
                //复读！ 
                QMApi.SendGroupMessage(e.RobotQQ, e.FromGroup, e.Message);
            }

            return QMEventHandlerTypes.Continue;
        }
    }
}
