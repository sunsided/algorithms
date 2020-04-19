# Changelog

All notable changes to this project will be documented in this file.
This project uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

### Added

- Quickselect for arbitrary elements with an `IComparer<TElement>`.
- Naive binary tree implementation.
- Non-recursive in-, level-, post- and pre-order traversals for
  the binary tree.
- recursive in-, post- and pre-order traversals for
  the binary tree.
- Min Heap and Max Heap data structures.

### Internal

- Targeting .NET Standard 2.1.
- Using nullable reference types.
- Using embedded `JetBrains.Annotations` (2020.1.0)
  for improved code contracts.
- Using [ErrorProne.NET](https://github.com/SergeyTeplyakov/ErrorProne.NET)
  in order to improve code correctness.
- Added SourceLink against the GitHub repo.
