﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bumblebee.Plugins
{

    public interface IPluginStatus
    {
        bool Enabled { get; set; }
    }

    public interface IPluginInfo
    {
        string IconUrl { get; }

        string EditorUrl { get; }

        string InfoUrl { get; }
    }

    public enum PluginLevel : int
    {
        High9 = 19,
        High8 = 18,
        High7 = 17,
        High6 = 16,
        High5 = 15,
        High4 = 14,
        High3 = 13,
        High2 = 12,
        High1 = 11,
        None = 10,
        Low1 = 9,
        Low2 = 8,
        Low3 = 7,
        Low4 = 6,
        Low5 = 5,
        Low6 = 4,
        Low7 = 3,
        Low8 = 2,
        Low9 = 1,

    }


    public interface IPlugin
    {
        string Name { get; }

        void Init(Gateway gateway, System.Reflection.Assembly assembly);

        PluginRunInfo RunInfo(int p, int ps)
        {
            return new PluginRunInfo();
        }

        string Description { get; }

        void LoadSetting(JToken setting);

        Object SaveSetting();

        PluginLevel Level { get; }

    }

    public enum PluginType
    {
        Requesting,
        Requested,
        ResponseError,
        Loader,
        HeaderWriting,
        GetAgentServer,
        AgentRequesting,
        Responding
    }

    public class PluginInfo
    {

        public PluginInfo(IPlugin plugin)
        {
            if (plugin is IRequestedHandler)
                Type = PluginType.Requested.ToString();
            if (plugin is IRequestingHandler)
                Type = PluginType.Requesting.ToString();
            if (plugin is IGatewayLoader)
                Type = PluginType.Loader.ToString();
            if (plugin is IHeaderWritingHandler)
                Type = PluginType.HeaderWriting.ToString();
            if (plugin is IGetServerHandler)
                Type = PluginType.GetAgentServer.ToString();
            if (plugin is IAgentRequestingHandler)
                Type = PluginType.AgentRequesting.ToString();
            if (plugin is IRespondingHandler)
                Type = PluginType.Responding.ToString();
            Name = plugin.Name;
            var copyright = (from a in plugin.GetType().Assembly.CustomAttributes
                             where a.AttributeType == typeof(System.Reflection.AssemblyCopyrightAttribute)
                             select a).FirstOrDefault();
            if (copyright != null)
                Copyright = (string)copyright.ConstructorArguments[0].Value;
            Version = plugin.GetType().Assembly.GetName().Version.ToString();
            Assembly = plugin.GetType().Assembly.GetName().Name;
            Description = plugin.Description;
            Level = plugin.Level.ToString();
            if (plugin is IPluginStatus status)
            {
                Status = true;
                Enabled = status.Enabled;
            }
            else
            {
                Enabled = true;
            }
            if (plugin is IPluginInfo info)
            {
                EditorUrl = info.EditorUrl;
                IconUrl = info.IconUrl;
                InfoUrl = info.InfoUrl;
            }
        }

        public string Copyright { get; set; }

        public string Level { get; set; }

        public string EditorUrl { get; set; }

        public string InfoUrl { get; set; }

        public string IconUrl { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public bool Enabled { get; set; }

        public string Version { get; set; }

        public string Assembly { get; set; }

        public string Description { get; set; }
    }

    public class PluginRunInfo
    {
        public List<PluginRunInfoColumn> Columns { get; set; } = new List<PluginRunInfoColumn>();
        public PluginRunInfoPage Data { get; set; } = new PluginRunInfoPage();
    }

    public class PluginRunInfoPage
    {

        public int Count { get; set; } = 1;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public object Data { get; set; } = new List<object>();
    }

    public class PluginRunInfoColumn
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
