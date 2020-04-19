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
        public readonly int Index;

        public HeapOperation(HeapOperationType type, int index, T value)
        {
            Type = type;
            Index = index;
            Value = value;
        }

        public override string ToString() =>
            Type != HeapOperationType.Extract
                ? $"{Type} {Value}"
                : $"{Type}";
    }
}
