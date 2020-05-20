using MediatR;
using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.DominObjects
{
    public class DomainNotification : Message, INotification
    {
        public DateTime Timestamp { get; set; }
        public Guid DomainNotificationId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int Version { get; set; }

        public DomainNotification(string key, string value)
        {
            Timestamp = DateTime.Now;
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
        }

    }
}
