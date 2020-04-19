namespace Widemeadows.Algorithms.Tests.Data.Heap
{
    /// <summary>
    /// An operation on a heap.
    /// </summary>
    public readonly struct HeapOperation<T>
        where T : notnull
    {
        public readonly HeapOperationType Type;
        public readonly T Value;

        public HeapOperation(HeapOperationType type, T value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() =>
            Type != HeapOperationType.PullTop
                ? $"{Type} {Value}"
                : $"{Type}";
    }
}
