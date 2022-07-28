namespace MoreStructures.SuffixArrays.LongestCommonPrefix;

/// <summary>
/// The LCP Array of a string S of length n is defined as an array LCP of length n - 1 such that LCP[i] is the
/// length of the prefix in common between the suffixes starting at SA[i] and SA[i + 1], where SA is the Suffix 
/// Array of S, for each i from 0 to n - 1 excluded.
/// </summary>
/// <remarks>
/// The LCP Array of a string of length n has n - 1 items.
/// </remarks>
/// <example>
/// Given the string <c>S = "mississippi$"</c> with terminator char <c>'$'</c>, S has Suffix Array 
/// <c>SA = { 11, 10, 7, 4, ... }</c>.
/// <br/>
/// - The 1-st item of the LCP Array of S is the length of the prefix in common between the suffix starting at 
///   SA[0], <c>"$"</c>, and the one starting at SA[1], <c>"i$"</c>. Such prefix is the empty string, therefore 
///   <c>LCP[0] = 0</c>.
///   <br/>
/// - The 2-nd item of the LCP Array of S is the length of the prefix in common between the suffix starting at 
///   SA[1], <c>"i$"</c>, and the one starting at SA[2], <c>"ippi$"</c>. Such prefix is the string <c>"i"</c>, 
///   therefore <c>LCP[1] = 1</c>.
///   <br/>
/// - The 3-rd item of the LCP Array of S is the length of the prefix in common between the suffix starting at 
///   SA[2], <c>"ippi$"</c>, and the one starting at SA[3], <c>"issippi$"</c>. Such prefix is again the string 
///   <c>"i"</c>, therefore <c>LCP[2] = 1</c>.
///   <br/>
/// - etc...
/// </example>
public record struct LcpArray(IEnumerable<int> Lengths);
