namespace SampleApp.Shared.Infrastructure.Blobs.Orders
{
    using SampleApp.Orders.Client;

    public class OrdersAzureStorageBlobRepository : AzureStorageBlobRepository, IOrdersBlobRepository
    {
    }
}
