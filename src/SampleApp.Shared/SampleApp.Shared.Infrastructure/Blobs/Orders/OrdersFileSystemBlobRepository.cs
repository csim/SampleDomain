namespace SampleApp.Shared.Infrastructure.Blobs.Orders
{
    using SampleApp.Orders.Client.Data;

    public class OrdersFileSystemBlobRepository : FileSystemBlobRepository, IOrdersBlobRepository
    {
    }
}
