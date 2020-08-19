﻿namespace SampleApp.Orders.Client.Records
{
    using System;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using SampleApp.Shared.Abstractions;

    public class OrderAuditRecord : RecordBase
    {
        public OrderShadow Record { get; set; }

        public RecordTransactionType TransactionType { get; set; }

        public string TransactionTypeName
        {
            get { return TransactionType.ToString(); }
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }
    }

    [Owned]
    [AutoMap(typeof(OrderRecord))]
    public class OrderShadow
    {
        public DateTime? CreatedOn { get; set; }

        public Guid? Id { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long Number { get; set; }

        public string PartitionKey { get; set; }

        public decimal Total { get; set; }
    }

    public enum RecordTransactionType
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
}
