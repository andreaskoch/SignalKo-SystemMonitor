﻿namespace SignalKo.SystemMonitor.Agent.Core.Queueing
{
    public interface IMessageQueueFeeder<T>
    {
        void Start(IMessageQueue<T> workQueue);

        void Stop();
    }
}