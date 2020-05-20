using System;
using FluentValidation;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace NerdStore.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }

    }
}
