namespace SampleApp.Shared.Infrastructure.Blob.Orders
{
    using SampleApp.Orders.Client.Data;

    public class OrdersBlobRepository : FileSystemBlobRepository, IOrdersBlobRepository
    {
    }
}
