﻿using QM.Log;
using QM.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class Application
    {
        private List<IComponent> components;
        private ApplicationState state;
        private ILog log;
        private string serverId;
        //最大连接数
        public int maxConnectCount;

        public Application()
        {
            components = new List<IComponent>();
            state = ApplicationState.Init;
        }

        public void Start()
        {
            var beginTime = TimeUtils.GetUnixTimestampMilliseconds();
            if (state >= ApplicationState.Start)
            {
                log.Error("已经初始化");
                return;
            }

            foreach (var component in components)
            {
                component.Start();
            }
            state = ApplicationState.Start;
            AfterStart();
            var endTime = TimeUtils.GetUnixTimestampMilliseconds();
            log.Debug($"Start 执行时间:{endTime - beginTime}ms");
        }

        private void AfterStart()
        {
            var beginTime = TimeUtils.GetUnixTimestampMilliseconds();
            if (state >= ApplicationState.AfterStart)
            {
                log.Error("已经执行AfterStart");
                return;
            }

            foreach (var component in components)
            {
                component.AfterStart();
            }
            state = ApplicationState.AfterStart;
            var endTime = TimeUtils.GetUnixTimestampMilliseconds();
            log.Debug($"AfterStart 执行时间:{endTime - beginTime}ms");
        }

        public void Stop()
        {
            var beginTime = TimeUtils.GetUnixTimestampMilliseconds();
            if (state >= ApplicationState.Stop)
            {
                log.Error("已经执行Stop");
                return;
            }
            foreach (var component in components)
            {
                component.Stop();
            }
            var endTime = TimeUtils.GetUnixTimestampMilliseconds();
            log.Debug($"Stop 执行时间:{endTime - beginTime}ms");
        }
    }
}
