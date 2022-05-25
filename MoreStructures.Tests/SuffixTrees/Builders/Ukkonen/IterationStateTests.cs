using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.SuffixTrees.Builders.Ukkonen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Tests.SuffixTrees.Builders.Ukkonen;

[TestClass]
public class IterationStateTests
{
    [TestMethod]
    public void Text_IsSetOnConstruction()
    {
        var text = new TextWithTerminator("abc");
        var state = new IterationState(text);
        Assert.AreEqual(text, state.Text);
    }

    [TestMethod]
    public void Root_IsSetOnConstruction()
    {
        var state = new IterationState(new("abc"));
        Assert.IsNotNull(state.Root);
    }

    [TestMethod]
    public void IsThereANextPhase_IsCorrect()
    {
        var state = new IterationState(new("ab"));
        Assert.IsTrue(state.IsThereANextPhase());
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        Assert.IsTrue(state.IsThereANextPhase());
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        Assert.IsTrue(state.IsThereANextPhase());
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        Assert.IsFalse(state.IsThereANextPhase());
    }

    [TestMethod]
    public void StillRemainingSuffixesInCurrentPhase_AccessProtectedWhenNotInPhase()
    {
        var state = new IterationState(new("ab"));
        Assert.ThrowsException<InvalidOperationException>(() => state.StillRemainingSuffixesInCurrentPhase());
    }

    [TestMethod]
    public void ActivePointFollowedByCurrentChar_NullAtStart()
    {
        var state = new IterationState(new("ab"));
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        Assert.IsFalse(state.ActivePointFollowedByCurrentChar());
    }

    [TestMethod]
    public void ActivePointFollowedByCurrentChar_ThrowsExceptionBeforeStartingAPhase()
    {
        var state = new IterationState(new("aa"));

        var child1 = new MutableNode(1, null, null);
        child1.Children[new(1, new(2))] = new MutableNode(2, 0, null);
        child1.Children[new(2, new(2))] = new MutableNode(3, 1, null);

        state.Root.Children[new(0, new(0))] = child1;
        state.Root.Children[new(2, new(2))] = new MutableNode(4, 0, null);

        state.InitializeActiveEdgeAndLength(new MutableEdge(1, new(0)));
        Assert.ThrowsException<InvalidOperationException>(() => state.ActivePointFollowedByCurrentChar());
    }

    [TestMethod]
    public void ActivePointFollowedByCurrentChar_ThrowsExceptionWhenInvalidActivePoint()
    {
        var state = new IterationState(new("ab"));
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.InitializeActiveEdgeAndLength(new MutableEdge(0, new(0)));
        Assert.ThrowsException<InvalidOperationException>(() =>
            state.ActivePointFollowedByCurrentChar());
    }

    [TestMethod]
    public void ActivePointFollowedByCurrentChar_WhenActivePointIsDefined_TrueCases()
    {
        var state = new IterationState(new("aa"));

        var child1 = new MutableNode(1, null, null);
        child1.Children[new(1, new(2))] = new MutableNode(2, 0, null); 
        child1.Children[new(2, new(2))] = new MutableNode(3, 1, null);

        state.Root.Children[new(0, new(0))] = child1;
        state.Root.Children[new(2, new(2))] = new MutableNode(4, 0, null);

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.InitializeActiveEdgeAndLength(new MutableEdge(1, new(0)));
        Assert.IsTrue(state.ActivePointFollowedByCurrentChar());
    }

    [TestMethod]
    public void ActivePointFollowedByCurrentChar_WhenActivePointIsDefined_FalseCases()
    {
        var state = new IterationState(new("aba"));

        var child1 = new MutableNode(1, null, null);
        child1.Children[new(1, new(3))] = new MutableNode(2, 0, null);
        child1.Children[new(3, new(3))] = new MutableNode(3, 2, null);

        var child2 = new MutableNode(4, null, null);
        child2.Children[new(2, new(3))] = new MutableNode(5, 1, null);

        var child3 = new MutableNode(3, 3, null);

        state.Root.Children[new(0, new(0))] = child1;
        state.Root.Children[new(1, new(1))] = child2;
        state.Root.Children[new(3, new(3))] = child3;

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.InitializeActiveEdgeAndLength(new MutableEdge(0, new(0)));
        Assert.IsFalse(state.ActivePointFollowedByCurrentChar());

        state.InitializeActiveEdgeAndLength(new MutableEdge(1, new(1)));
        Assert.IsTrue(state.ActivePointFollowedByCurrentChar());
    }
}
