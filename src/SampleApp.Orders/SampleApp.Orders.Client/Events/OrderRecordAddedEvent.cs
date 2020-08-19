﻿namespace SampleApp.Orders.Client.Events
{
    using SampleApp.Orders.Client.Records;
    using SampleApp.Shared.Abstractions;

    public class OrderRecordAddedEvent : RecordTransactionEvent<OrderRecord>
    {
    }
}
