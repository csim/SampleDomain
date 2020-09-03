namespace SampleApp.Shared.Infrastructure.Blob.Orders
{
    using SampleApp.Orders.Client.Data;

    public class OrdersFileSystemBlobRepository : FileSystemBlobRepository, IOrdersBlobRepository
    {
    }
}
