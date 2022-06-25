namespace MoreStructures.SuffixTrees.Builders.Ukkonen;

/// <summary>
/// An object which keeps the state of execution of the Ukkonen algorithm, and enforce its coherency.
/// </summary>
internal class IterationState
{
    /// <summary>
    /// Builds a fresh <see cref="IterationState"/> object, to execute the Ukkonen algorithm on the provided 
    /// <paramref name="text"/>.
    /// </summary>
    /// <param name="text">The text, to build the Suffix Tree of.</param>
    /// <remarks>
    ///     <para id="algorithm">
    ///     ALGORITHM
    ///     <br/>
    ///     - Creates a <see cref="Root"/> object, which will be used as root for the entire execution of the 
    ///       algorithm.
    ///       <br/>
    ///     - The <see cref="Root"/> node is also set as current <see cref="ActiveNode"/>, while 
    ///       <see cref="ActiveEdgeStart"/> is set to a negative value and <see cref="ActiveLength"/> to 0 (as there 
    ///       is no a valid <see cref="ActiveEdge"/> yet - it is going to be set when Rule 3 is triggered).
    ///       <br/>
    ///     - <see cref="Phase"/> is initially set to a negative value: the first phase has to be explicitely 
    ///       started with <see cref="StartPhaseIncreasingRemainingAndGlobalEnd"/>.
    ///     </para>
    /// </remarks>
    public IterationState(TextWithTerminator text)
    {
        NextLeafStart = 0;
        NextNodeId = 0;

        Text = text;
        Root = new(NextNodeId++, null, null, null); // The root is never a leaf and has no incoming edge

        Phase = -1;
        RemainingSuffixes = 0;
        PreviousInternalNodeInTheSamePhase = null;
        GlobalEnd = new(-1);

        ActiveNode = Root;
        ActiveEdgeStart = -1;
        ActiveLength = 0;
    }

    #region General readonly properties

    /// <summary>
    /// The <see cref="TextWithTerminator"/> to build the <see cref="SuffixTreeNode"/> structure of.
    /// </summary>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// The root node of the Suffix Tree, temporarily used during tree construction.
    /// </summary>
    public MutableNode Root { get; }

    #endregion

    #region Cursors

    /// <summary>
    /// The top-level iteration. There are as many phases as chars in <see cref="Text"/>.
    /// </summary>
    private int Phase;

    /// <summary>
    /// How many suffixes still need to be created within the current phase.
    /// </summary>
    /// <remarks>
    /// Every time a leaf is created, a suffix has been fully processed, so <see cref="RemainingSuffixes"/> is reduced 
    /// by one, whereas when the active edge and length are changed, because RULE 3 EXTENSION is trigger, the tree is
    /// in implicit state, i.e. there is no edge with termination character and no internal node with only one edge 
    /// going out of it.
    /// </remarks>
    private int RemainingSuffixes;

    /// <summary>
    /// The internal node created in the previous iteration of the same phase, or <see langword="null"/>, if 
    /// there hasn't been a previous iteration in the same phase where an internal node has been created.
    /// </summary>
    private MutableNode? PreviousInternalNodeInTheSamePhase;

    /// <summary>
    /// The end of the text, which is a moving target - it gets increased every time a new char from 
    /// <see cref="Text"/> is processed.
    /// </summary>
    /// <remarks>
    /// Since it is kept by reference from edges, increasing ends at 
    /// <see cref="StartPhaseIncreasingRemainingAndGlobalEnd"/> performs the RULE 1 EXTENSION on all leafs at no 
    /// additional cost.
    /// <br/> 
    /// This is one of the mechanisms that make time Ukkonen algorithm complexity linear.
    /// </remarks>
    private readonly MovingEnd GlobalEnd;

    private int NextLeafStart;
    private int NextNodeId;

    /// <summary>
    /// The char in <see cref="Text"/> at index <see cref="Phase"/>.
    /// </summary>
    private char CurrentChar
    {
        get
        {
            if (Phase < 0)
                throw new InvalidOperationException("No phase started yet");
            return Text[Phase];
        }
    }

    #endregion

    #region Active Point

    /// <summary>
    /// The node in the tree currently being built, where to start from when looking for <see cref="CurrentChar"/>.
    /// </summary>
    /// <remarks>
    /// At the beginning it is set to the root of the about-to-be-built tree. The root doesn't have a Suffix Link.
    /// Unlike <see cref="ActiveEdge"/> and <see cref="ActiveLength"/>, <see cref="ActiveNode"/> is always defined.
    /// <br/>
    /// It's changed by <see cref="JumpActivePointIfNecessary"/>, when the current <see cref="ActiveEdge"/> has been
    /// fully traversed. 
    /// <br/>
    /// It is also changed by <see cref="CreateLeafAndPossiblyIntermediateAndDecrementRemainingSuffixes"/> in Rule 
    /// 2 Extension, and set to its <see cref="MutableNode.SuffixLink"/>, when an internal node has been created
    /// and the <see cref="ActiveNode"/> is not the <see cref="Root"/> node.
    /// </remarks>
    private MutableNode ActiveNode;

    /// <summary>
    /// The index in the text, of the 1st char identifying the edge starting from <see cref="ActiveNode"/>, where 
    /// to start from when looking for <see cref="CurrentChar"/>. It's -1 if no edge has been selected yet.
    /// </summary>
    /// <remarks>
    /// For example, if the edge is labelled as "abc" and the 1st "a" corresponds to the 1st char of the text 
    /// "abcxyx$", the active edge is 0. At the beginning no edge has been selected from the active node, so 
    /// there is no active edge, and the value -1 is set. In this scenario <see cref="ActiveLength"/> is also 
    /// reset to 0.
    /// </remarks>
    private int ActiveEdgeStart;

    /// <summary>
    /// The amount of chars which have been processed on the <see cref="ActiveEdge"/>.
    /// </summary>
    /// <remarks>
    /// At the beginning, no edge has been selected from the active node, so its value is 0.
    /// </remarks>
    private int ActiveLength;

    /// <summary>
    /// The edge coming out of <see cref="ActiveNode"/> which starts with <see cref="ActiveEdgeStart"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If <see cref="ActiveEdgeStart"/> is invalid in the current context (i.e. there is no active point).
    /// </exception>
    private MutableEdge ActiveEdge
    {
        get
        {
            var activeEdgeOrDefault = ActiveNode.Children.Keys.SingleOrDefault(
                edge => Text[edge.Start] == Text[ActiveEdgeStart]);
            if (activeEdgeOrDefault is not MutableEdge activeEdge)
                throw new InvalidOperationException($"No edge from {ActiveNode} starting with {ActiveEdgeStart}");
            return activeEdge;
        }
    }

    #endregion

    /// <summary>
    /// Checks whether there are still more top-level iterations to execute, or not.
    /// </summary>
    /// <returns>True if there are still phases to go, false otherwise.</returns>
    public bool IsThereANextPhase() => Phase < Text.Length - 1;

    /// <summary>
    /// Start a new phase, i.e. to the next char in <see cref="Text"/>, increasing <see cref="RemainingSuffixes"/> 
    /// and <see cref="GlobalEnd"/> and resetting <see cref="PreviousInternalNodeInTheSamePhase"/>.
    /// </summary>
    public void StartPhaseIncreasingRemainingAndGlobalEnd()
    {
        Phase++;
        RemainingSuffixes++;
        GlobalEnd.Value++;
        PreviousInternalNodeInTheSamePhase = null;
    }

    /// <summary>
    /// Whether there still are suffixes to be processed in the current <see cref="Phase"/>.
    /// </summary>
    /// <returns>True if there are, false otherwise.</returns>
    public bool StillRemainingSuffixesInCurrentPhase()
    {
        if (Phase < 0)
            throw new InvalidOperationException("No phase started yet");
        return RemainingSuffixes > 0;
    }

    /// <summary>
    /// If no active point is currently defined (non-positive <see cref="ActiveLength"/>), and there is an
    /// edge coming out of the <see cref="ActiveNode"/> which starts with <see cref="CurrentChar"/>, gives back
    /// that edge. 
    /// </summary>
    /// <returns>The edge, if found. Otherwise null.</returns>
    public MutableEdge? NoActivePointAndEdgeStartingFromActiveNodeWithCurrentChar()
    {
        if (ActiveLength > 0)
            return null;

        return ActiveNode.Children.Keys.SingleOrDefault(edge => Text[edge.Start] == CurrentChar);
    }

    /// <summary>
    /// Sets the <see cref="ActiveEdge"/>, by actually storing its <see cref="ActiveEdgeStart"/>. It also set
    /// <see cref="ActiveLength"/> to 1, since activating an edge means consuming the 1st char of the edge, as it
    /// corresponds to the current char of <see cref="Text"/>.
    /// </summary>
    /// <param name="edge">The new <see cref="ActiveEdge"/> to set.</param>
    public void InitializeActiveEdgeAndLength(MutableEdge edge)
    {
        ActiveEdgeStart = edge.Start;
        ActiveLength = 1;

        JumpActivePointIfNecessary(edge);
    }

    /// <summary>
    /// Whether there is an active point (active node + edge + length), followed by the <see cref="CurrentChar"/> 
    /// being processed in the current <see cref="Phase"/>.
    /// </summary>
    /// <returns>True if there is an active point which respects the condition, false otherwise.</returns>
    /// <exception cref="InvalidOperationException">If the active point state is inconsistent.</exception>
    public bool ActivePointFollowedByCurrentChar()
    {
        if (ActiveEdgeStart < 0)
            return false;

        return Text[ActiveEdge.Start + ActiveLength] == CurrentChar;
    }

    /// <summary>
    /// Increment <see cref="ActiveLength"/>, updating the Active Point if necessary. 
    /// Used when <see cref="ActivePointFollowedByCurrentChar"/> is satified, to trigger Rule 3 Extension, which is 
    /// a showstopper for the current phase.
    /// </summary>
    public void IncrementActiveLength()
    {
        ActiveLength++;
        JumpActivePointIfNecessary(ActiveEdge);
    }

    /// <summary>
    /// Makes the active point properties consistent with each other, jumping <see cref="ActiveNode"/> if necessary
    /// and resetting <see cref="ActiveEdgeStart"/> and <see cref="ActiveLength"/> when appropriate. 
    /// </summary>
    /// <remarks>
    ///     <para id="algo">
    ///     ALGORITHM
    ///     <br/>
    ///     Performed at the end of every iteration.
    ///     <br/>
    ///     - If there is no Active Point, i.e. negative <see cref="ActiveEdgeStart"/> or zero 
    ///       <see cref="ActiveLength"/>, there's nothing to update. Therefore, exit.
    ///       <br/>
    ///     - Scenario 1: If there are still chars not traversed on the current <see cref="ActiveEdge"/>, the Active 
    ///       Point is still valid. Therefore, exit. Otherwise, perform the update. 
    ///       <br/>
    ///    <br/>
    ///    There are two scenarios when all chars have been consumed on the current Active Edge, and an update of the 
    ///    Active Point is required.
    ///    <br/>
    ///     - Scenario 2: the Active Point points to the last char of the <see cref="ActiveEdge"/>. This can happen 
    ///       when Rule 3 Extension is applied and <see cref="ActiveLength"/> is increased by 1, reaching the end of
    ///       the current <see cref="ActiveEdge"/>. In this scenario, change <see cref="ActiveNode"/>, jumping to the 
    ///       node pointed by the current <see cref="ActiveEdge"/>, and reset the Active Point.
    ///       <br/>
    ///     - Scenario 3: the Active Point overflows, with the ActiveLength going beyond the <see cref="ActiveEdge"/>.
    ///       This can happen when Rule 2 Extension is applied and <see cref="ActiveEdgeStart"/> is increased by 1,
    ///       changing active edge, and <see cref="ActiveLength"/> is decreased by 1, but the edge which was unique on 
    ///       the previous branch is split into a sequence of edges on the new branch. In this scenario, change 
    ///       <see cref="ActiveNode"/>, jumping to the node pointed by the current <see cref="ActiveEdge"/>, and set 
    ///       the Active Point accordingly.
    ///     </para>
    /// </remarks>
    public void JumpActivePointIfNecessary(MutableEdge? previousActiveEdge)
    {
        while (true)
        {
            if (ActiveEdgeStart < 0 || ActiveLength <= 0)
            {
                ActiveEdgeStart = -1;
                ActiveLength = 0;
                return;
            }

            var currentActiveEdge = ActiveEdge;
            var remainingCharsOnActiveEdgeAfterActivePoint =
                currentActiveEdge.End.Value - (currentActiveEdge.Start + ActiveLength - 1);

            // Scenario 1
            if (remainingCharsOnActiveEdgeAfterActivePoint > 0)
            {
                return;
            }

            // Scenario 2
            if (remainingCharsOnActiveEdgeAfterActivePoint == 0)
            {
                ActiveNode = ActiveNode.Children[currentActiveEdge];
                ActiveEdgeStart = -1;
                ActiveLength = 0;
                return;
            }

            // Scenario 3
            var firstCharOverflowingCurrentActiveEdge = previousActiveEdge!.Start + currentActiveEdge.Length;
            ActiveNode = ActiveNode.Children[currentActiveEdge];
            ActiveEdgeStart = ActiveNode.Children.Keys.Single(
                edge => Text[edge.Start] == Text[firstCharOverflowingCurrentActiveEdge]).Start;
            ActiveLength = -remainingCharsOnActiveEdgeAfterActivePoint;

            previousActiveEdge = currentActiveEdge;
        }
    }

    /// <summary>
    /// Performs Rule 2 Extension.
    /// </summary>
    public void CreateLeafAndPossiblyIntermediateAndDecrementRemainingSuffixes()
    {
        var leafEdge = new MutableEdge(Phase, GlobalEnd);
        var leafNode = new MutableNode(NextNodeId++, NextLeafStart++, null, leafEdge); // Leaves don't have Suffix Link

        MutableNode internalNode;

        if (ActiveLength == 0)
        {
            // No current active edge => branch out from the active node directly
            ActiveNode.Children[leafEdge] = leafNode;

            // Stop edge to ActiveNode from tracking the end of the text, as ActiveNode transitions from being a leaf
            // to an intermediate node
            if (ActiveNode.IncomingEdge != null) // False on the root
            {
                ActiveNode.IncomingEdge.End = new(ActiveNode.IncomingEdge.End.Value);
            }

            internalNode = ActiveNode;
        }
        else
        {
            // There is an active edge => split the active edge into two, with an intern node in the middle
            var activeEdge = ActiveEdge;
            MutableEdge sharedEdge = new(activeEdge.Start, new(activeEdge.Start + ActiveLength - 1));
            MutableEdge reminderEdge = new(activeEdge.Start + ActiveLength, activeEdge.End);

            internalNode = new(NextNodeId++, null, Root, sharedEdge);
            internalNode.Children[reminderEdge] = ActiveNode.Children[activeEdge];
            internalNode.Children[leafEdge] = leafNode;

            ActiveNode.Children[sharedEdge] = internalNode;
            ActiveNode.Children.Remove(activeEdge);
        }

        // If an internal node has been created in a previous Rule 2 Extension of the current phase, update
        // the SuffixLink of the previous internal node to be the just created internal node, and store the
        // just create internal store PreviousInternalNodeInTheSamePhase, to be able to set the Suffix Link
        // on next internal node if a new Rule 2 Extension happens after this one, in the same phase
        if (PreviousInternalNodeInTheSamePhase is MutableNode previousInternalNode)
            previousInternalNode.SuffixLink = internalNode;
        PreviousInternalNodeInTheSamePhase = internalNode;

        var previousActiveEdge = ActiveEdgeStart >= 0 ? ActiveEdge : null;

        // Now that the leaf x has been created, and possibly the internal node abc..z (length = RemainingSuffixes)
        // has also been created, and the previous internal node updated (if any), there are still RemainingSuffixes
        // suffixes to be processed: bc..zx, c..zx, ... x. Those can be easily found in the tree by using Suffix Links.
        if (ActiveNode == Root && ActiveEdgeStart >= 0)
        {
            // To move from abc..zx to bc..zx when the active node is the root, increment ActiveEdgeStart
            // (which means that the start of the new ActiveEdge is now b) and decrement ActiveLength (because
            // now the ActiveEdge is one char shorter.
            // The edge bc..z is guaranteed to exist already because bc..z it's a suffix already processed in
            // previous iterations and Rule 1 Extension implicitely extend bc..z to bc..zx.
            ActiveEdgeStart++;
            ActiveLength--;
        }
        else
        {
            // To move from abc..zx to bc..zx when the active node is an internal node N, move the active node
            // to the Suffix Link of N, which is by definition the node having path bc..zx. Active edge and
            // length don't change, because the new active node has the suffix bc..zx.
            ActiveNode = ActiveNode.SuffixLink ?? Root;
        }

        JumpActivePointIfNecessary(previousActiveEdge);

        // Because a leaf has been created, a suffix has been processed, and the number of remaining suffixes
        // (which is > 1 in case previous phases have terminated with a showstopper due to Rule 3 Extension)
        // is decreased by 1.
        RemainingSuffixes--;
    }

    /// <summary>
    /// <inheritdoc/>
    /// Visualization of the main state variables, such as <see cref="Phase"/>, <see cref="RemainingSuffixes"/>
    /// and active point data.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public override string ToString() =>
        string.Join(", ",
            $"{nameof(Phase)} = {Phase,-2}",
            $"{nameof(CurrentChar)} = {CurrentChar}",
            $"{nameof(RemainingSuffixes)} = {RemainingSuffixes,-2}",
            $"{nameof(ActiveNode)} = {ActiveNode,-2}",
            $"{nameof(ActiveEdgeStart)} = {ActiveEdgeStart,-2}",
            $"{nameof(ActiveLength)} = {ActiveLength,-2}") + Environment.NewLine +
        $"Tree = {Root.Dump(new(Text, GlobalEnd))}";
}
