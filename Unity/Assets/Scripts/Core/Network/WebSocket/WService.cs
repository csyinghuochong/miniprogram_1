#if UNITY
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ET
{
    public class WService: AService
    {
        private long idGenerater = 200000000;
        
        private readonly Dictionary<long, WChannel> channels = new();

        public override void Dispose()
        {
            if (this.IsDisposed())
            {
                return;
            }
            
            base.Dispose();

            this.Id = 0;

            foreach (var kv in this.channels.ToArray())
            {
                kv.Value.Dispose();
            }
        }

        public WService(AddressFamily addressFamily)
        {
            this.ServiceType = ServiceType.Outer;
        }
        
        public WService(IPEndPoint ipEndPoint)
        {
            this.ServiceType = ServiceType.Outer;
        }

        private long GetId
        {
            get
            {
                return ++this.idGenerater;
            }
        }
        
        public override void Create(long id, IPEndPoint ipEndPoint)
        {
            WChannel channel = new(id, ipEndPoint, this);
            this.channels[channel.Id] = channel;
        }

        public override void Remove(long id, int error = 0)
        {
            WChannel channel;
            if (!this.channels.TryGetValue(id, out channel))
            {
                return;
            }
            channel.Error = error;

            this.channels.Remove(id);
            channel.Dispose();
        }

        protected void Get(long id, IPEndPoint ipEndPoint)
        {
            if (!this.channels.TryGetValue(id, out _))
            {
                this.Create(id, ipEndPoint);
            }
        }
        
        public WChannel Get(long id)
        {
            WChannel channel = null;
            this.channels.TryGetValue(id, out channel);
            return channel;
        }

        public override void Send(long channelId, MemoryBuffer memoryBuffer)
        {
            this.channels.TryGetValue(channelId, out WChannel channel);
            if (channel == null)
            {
                return;
            }
            channel.Send(memoryBuffer);
        }

        public override void Update()
        {
        }
        
        public override bool IsDisposed()
        {
            return this.Id == 0;
        }
    }
}
#endif