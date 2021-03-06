﻿using FastSocket.Options;
using FastSocket.Server;
using System;

namespace FastSocket.Build
{
    public class FastSocketBuild : IFastSocketBuild
    {
        public void ConfigureDefaultOptions(FastSocketBuildOption options)
        {
            fastSocketBuildOption = options;
        }

        public void ConfigFastSocketService(Action<IFastSocketService> action)
        {
            Action_FastSocketService = action;
        }

        FastSocketBuildOption fastSocketBuildOption = null;
        Action<IFastSocketService> Action_FastSocketService = new Action<IFastSocketService>(p => { });

        public IFastSocket Build()
        {
            var result = fastSocketBuildOption.IsConfigSuccess();
            var configResult = new
            {
                IsSuccess = result.Item1,
                ErrorException = result.Item2
            };
            if (!configResult.IsSuccess)
            {
                Console.WriteLine($"配置错误({configResult.ErrorException.Message})({configResult.ErrorException.StackTrace})");
                throw configResult.ErrorException;
            }

            IFastSocketService fastSocketService = new FastSocketService();
            Action_FastSocketService(fastSocketService);
            IFastSocket fastSocket = new FastSocket(fastSocketBuildOption, fastSocketService);
            return fastSocket;
        }
    }
}
