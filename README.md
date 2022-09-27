# Data structures and algorithms for .NET6

![License](https://img.shields.io/github/license/antonioaversa/MoreStructures)

MoreStructures is a library of classical algorithms and data structures, written 100% in (safe, managed) C# 10, for .NET 6 and above.

Built for fun and education, it has been implemented with __readability__ and __maintenability__ in mind, minimizing external dependencies and trying to make it easy for everybody to modify and extend.

All its public API are [documented](https://antonioaversa.github.io/MoreStructures/dotnet-docs-api/api/) and it is fully [unit tested](https://antonioaversa.github.io/MoreStructures/dotnet-tests-report/), with more than 2400 test scenarios and 100% line and branch [coverage](https://antonioaversa.github.io/MoreStructures/dotnet-coverage-report/).

Data structures implemented include:
- __Stacks__: array-list, linked-list;
- __Queues__: array-list, linked-list;
- __Priority Queues__: array-list, binary heaps, binomial heaps, Fibonacci heaps;
- __Disjoint Sets__: quick find, quick union, weighted, path compression;
- __Trees__: recursive immutable n-ary trees;
- __Graphs__: edge list, adjacency list, adjacency matrix;
- __Suffix Trees__: edge compression;
- __Suffix Tries__: standard.

Algorithms implemented include:
- __String sorting__: counting sort, quicksort;
- __List searching__: linear, binary;
- __List sorting__: selection, insertion, Shell, heap;
- __String matching__: Burrows-Wheeler transform, suffix arrays, Knuth-Morris-Pratt;
- __Tree augmentation__: counting;
- __Tree visit__: DFS, BFS, in iterative and recursive forms;
- __Graphs visits__: DFS, BFS, in iterative and recursive forms;
- __Graphs minimum spanning tree__: Kruskal, Prim;
- __Graphs shortest distance__: Dijkstra, Bellman-Ford, BFS-based;
- __Graphs shortest path__: BFS-based;
- __Graphs topological sorting__: DFS on each vertex, any path to sink, single DFS sink-based;
- __Graphs strongly connected components__: sink-based;
- __Suffix Trees construction__: naive, Ukkonen, LCP-based;
- __Suffix Trees matching__: exact pattern matching, shortest non-shared substring;
- __Suffix Tries construction__: naive;
- __Suffix Tries matching__: exact pattern matching.

## Install
[![.NET6 Nuget](https://img.shields.io/nuget/v/MoreStructures)](https://www.nuget.org/packages/MoreStructures/)
[![.NET6 Nuget Pre](https://img.shields.io/nuget/vpre/MoreStructures)](https://www.nuget.org/packages/MoreStructures/)

Via NuGet Package Manager:
```console
Install-Package MoreStructures
```

Via `dotnet` CLI:
```console
dotnet add package MoreStructures
```

## Build
![.NET6 Build and Test](https://img.shields.io/github/workflow/status/antonioaversa/MoreStructures/.NET%20Build%20and%20Test)
![.NET6 Coverage](https://antonioaversa.github.io/MoreStructures/dotnet-coverage-badge/badge.svg)

[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=antonioaversa_mooc&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=antonioaversa_mooc)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=antonioaversa_mooc&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=antonioaversa_mooc)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=antonioaversa_mooc&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=antonioaversa_mooc)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=antonioaversa_mooc&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=antonioaversa_mooc)

The latest detailed Tests Report for the .NET Libraries is available [here](https://antonioaversa.github.io/MoreStructures/dotnet-tests-report/).

The latest detailed Code Coverage Report for the .NET Libraries is available [here](https://antonioaversa.github.io/MoreStructures/dotnet-coverage-report/).

## Status
![Issues](https://img.shields.io/github/issues/antonioaversa/MoreStructures)
![Pull Requests](https://img.shields.io/github/issues-pr/antonioaversa/MoreStructures)
![Pull requests](https://img.shields.io/github/issues-pr-closed/antonioaversa/MoreStructures)
![Downloads](https://img.shields.io/github/downloads/antonioaversa/MoreStructures/total)

## Documentation
The latest documentation for the .NET Libraries is available [here](https://antonioaversa.github.io/MoreStructures/dotnet-docs-api/api/).
