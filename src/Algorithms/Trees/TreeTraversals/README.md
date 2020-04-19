# Tree Traversals

The tree traversal implementations here follow a visitor pattern,
where a tree applies a traversal to its nodes in order to
enumerate them.

An example for such an operation would be

```csharp
var traversal = new InOrderBinaryTreeTraverser<T>();
foreach (var value in traversal.Traverse(_rootNode))
{
    // process the node's value
}
```
