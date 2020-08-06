namespace Sample.Abstractions
{
    public interface IRecordCompactible
    {
        public object ToCompact();

        public T ToCompact<T>() where T : class, new();
    }
}
