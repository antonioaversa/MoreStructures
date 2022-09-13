using MoreStructures.SuffixTrees.Builders.Ukkonen;

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
        var state = new IterationState(new("ab"));

        state.Root.Children[new(0, new(2))] = new MutableNode(1, 0, null);
        state.Root.Children[new(1, new(2))] = new MutableNode(2, 1, null);
        state.Root.Children[new(2, new(2))] = new MutableNode(3, 2, null);

        state.InitializeActiveEdgeAndLength(new MutableEdge(0, new(2)));
        Assert.ThrowsException<InvalidOperationException>(() => state.ActivePointFollowedByCurrentChar());
    }

    [TestMethod]
    public void InitializeActiveEdgeAndLength_ThrowsExceptionWhenInvalidActivePoint()
    {
        var state = new IterationState(new("ab"));
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        Assert.ThrowsException<InvalidOperationException>(() =>
            state.InitializeActiveEdgeAndLength(new MutableEdge(0, new(0))));
    }

    [TestMethod]
    public void ActivePointFollowedByCurrentChar_WhenActivePointIsDefined_TrueCases()
    {
        var state = new IterationState(new("abcdef"));

        state.Root.Children[new(0, new(6))] = new MutableNode(1, 0, null);
        state.Root.Children[new(1, new(6))] = new MutableNode(2, 1, null);
        state.Root.Children[new(2, new(6))] = new MutableNode(3, 2, null);
        state.Root.Children[new(3, new(6))] = new MutableNode(4, 3, null);
        state.Root.Children[new(4, new(6))] = new MutableNode(5, 4, null);
        state.Root.Children[new(5, new(6))] = new MutableNode(6, 5, null);
        state.Root.Children[new(6, new(6))] = new MutableNode(7, 6, null);

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.InitializeActiveEdgeAndLength(new MutableEdge(0, new(2)));
        Assert.IsTrue(state.ActivePointFollowedByCurrentChar());

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.IncrementActiveLength();
        Assert.IsTrue(state.ActivePointFollowedByCurrentChar());

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        state.IncrementActiveLength();
        state.IncrementActiveLength();
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
        var child3 = new MutableNode(3, 3, null);

        state.Root.Children[new(0, new(0))] = child1;
        state.Root.Children[new(1, new(3))] = child2;
        state.Root.Children[new(3, new(3))] = child3;

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        // No active point yet => ActivePointFollowedByCurrentChar is false
        Assert.IsFalse(state.ActivePointFollowedByCurrentChar()); 

        state.InitializeActiveEdgeAndLength(new MutableEdge(0, new(0)));
        // InitializeActiveEdgeAndLength on Active Edge of length 1 => auto-jump of Active Node to child1
        Assert.IsFalse(state.ActivePointFollowedByCurrentChar()); 

        state.InitializeActiveEdgeAndLength(new MutableEdge(1, new(3)));
        // InitializeActiveEdgeAndLength => Active Point moved to char b of child 1 => Following char is 'a', which is
        // equal to current char (Phase 0 => current char is the first 'a')
        Assert.IsTrue(state.ActivePointFollowedByCurrentChar());

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        Assert.IsFalse(state.ActivePointFollowedByCurrentChar());

        state.StartPhaseIncreasingRemainingAndGlobalEnd();
        Assert.IsTrue(state.ActivePointFollowedByCurrentChar());

        state.IncrementActiveLength();
        Assert.IsFalse(state.ActivePointFollowedByCurrentChar());
    }
}
