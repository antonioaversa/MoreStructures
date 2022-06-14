﻿using StringAlgorithms.SuffixStructures;

namespace StringAlgorithms.SuffixTries;

public class SuffixTrieBuilder
    : ISuffixStructureBuilder<SuffixTrieEdge, SuffixTrieNode, SuffixTriePath, SuffixTrieBuilder>
{
    public SuffixTriePath EmptyPath() =>
        new (Enumerable.Empty<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>>());

    public SuffixTriePath SingletonPath(SuffixTrieEdge edge, SuffixTrieNode node) =>
        new(Enumerable.Repeat(KeyValuePair.Create(edge, node), 1));

    public SuffixTriePath MultistepsPath(params (SuffixTrieEdge edge, SuffixTrieNode node)[] pathNodes) =>
        new(pathNodes.Select(pathNode => KeyValuePair.Create(pathNode.edge, pathNode.node)));

    public SuffixTriePath MultistepsPath(IEnumerable<KeyValuePair<SuffixTrieEdge, SuffixTrieNode>> pathNodes) =>
        new(pathNodes);

    public SuffixTrieNode BuildTree(TextWithTerminator text)
    {
        SuffixTrieNode root = new SuffixTrieNode.Leaf(0);

        for (var suffixIndex = 0; suffixIndex < text.Length; suffixIndex++)
            root = IncludeSuffixIntoTrie(text, suffixIndex, suffixIndex, root);

        return root;
    }

    private static SuffixTrieNode.Intermediate IncludeSuffixIntoTrie(
        TextWithTerminator text, int suffixCurrentIndex, int suffixIndex, SuffixTrieNode node)
    {
        var nodeChildren = new Dictionary<SuffixTrieEdge, SuffixTrieNode>(node.Children);
        var childEdgeAndNode = nodeChildren
            .SingleOrDefault(c => text[c.Key] == text[new SuffixTrieEdge(suffixCurrentIndex)]);

        if (childEdgeAndNode is (SuffixTrieEdge edge, SuffixTrieNode child))
        {
            nodeChildren[edge] = IncludeSuffixIntoTrie(text, suffixCurrentIndex + 1, suffixIndex, child);
        }
        else
        {
            child = new SuffixTrieNode.Leaf(suffixIndex);
            for (int i = text.Length - 1; i >= suffixCurrentIndex + 1; i--)
            {
                child = new SuffixTrieNode.Intermediate(
                    new Dictionary<SuffixTrieEdge, SuffixTrieNode> { [new(i)] = child });
            }
            nodeChildren[new(suffixCurrentIndex)] = child;
        }

        return new SuffixTrieNode.Intermediate(nodeChildren);
    }
}