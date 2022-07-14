using System.Text;

namespace LeetCode_10_RegEx
{
    public class Program
    {
        static void Main(string[] args)
        {
            IsMatch("abbb", ".b*b");
        }

        public static bool IsMatch(string s, string p)
        {
            /*
            **  Given an input string s and a pattern p, implement regular expression matching with support for '.' and '*' where:
            **
            **  '.' Matches any single character.​​​​
            **  '*' Matches zero or more of the preceding element.
            **  The matching should cover the entire input string(not partial).
            */

            /*
            **  Constraints:
            **
            **  1 <= s.length <= 20
            **  1 <= p.length <= 30
            **  s contains only lowercase English letters.
            **  p contains only lowercase English letters, '.', and '*'.
            **  It is guaranteed for each appearance of the character '*', there will be a previous valid character to match.
            */
            CheckConstraints(s, p);

            bool match = true;

            int pPos = 0;
            int sPos = 0;
            char prevPChar = '\0';
            char nextPChar = '\0';
            char currPChar = '\0';
            char currSChar = '\0';
            char prevSChar = '\0';
            char nextSChar = '\0';
            char starMatchChar = '\0';
            bool workingWithAStarPattern = false;
            int numSequentialStarMatchesInPattern = 0;

            // To check the regular expression, we'll iterate through all the characters in both strings.
            // We'll treat the special case of the single character string here separately though.
            if ((p.Length == 1) || (s.Length == 1))
            {
                if ((p.Length == 1) && (s.Length == 1))
                {
                    if (p[0] == '.')
                    {
                        // If p and s are both one character and p is '.', then we have a match.
                        return true;
                    }
                    else if (p[0] == s[0])
                    {
                        // If p and s are both one character and are identical, then we have a match.
                        return true;
                    }
                    else
                    {
                        // For all other cases where p and s are both one character, then we don't have a match.
                        return false;
                    }
                }
                else if (p.Length == 1)
                {
                    // If the pattern is just one character but the string isn't, then we can't have a match.
                    return false;
                }
                else
                {
                    // If the string is just one character but the pattern isn't, then the only way we can have a match
                    // is if we have an appropriate star pattern (e.g., a*)

                }
            }
            else
            {
                // The strings have more than one character each, so iterate through the characters to 
                // decide if we have a RegEx match.
                while ((pPos < p.Length) && (sPos < s.Length))
                {
                    currPChar = p[pPos];
                    currSChar = s[sPos];

                    if (pPos + 1 < p.Length)
                    {
                        nextPChar = p[pPos + 1];
                        if (nextPChar == '*')
                        {
                            starMatchChar = currPChar;
                            workingWithAStarPattern = true;

                            // Go one more position past the star pattern to see if there's another character that's the same
                            // as the starMatchChar. If so, then we won't let the "hungry" star pattern eat up all the matching
                            // chars (we'll leave at least one for the next part of the pattern to have)
                            int pPos2 = pPos + 2;
                            numSequentialStarMatchesInPattern = 0;
                            while (pPos2 < p.Length)
                            {
                                if (p[pPos2] == starMatchChar)
                                {
                                    pPos2++;
                                    numSequentialStarMatchesInPattern++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (!workingWithAStarPattern)
                    {
                        if (currPChar == '.')
                        {
                            // If we encounter a '.' in the pattern that isn't followed by a '*', then it matches
                            // any character in the string, so keep processing.
                        }
                        else if (currPChar != currSChar)
                        {
                            // If we aren't dealing with a star pattern, the next character in the pattern
                            // must match the next character in the string in order to still possibly have
                            // a match. If the next characters don't match, stop processing.
                            return false;
                        }
                    }
                    else
                    {
                        if (currPChar == '.')
                        {
                            // If we're working with a star pattern based on '.', then this will match the
                            // current character in the string (as many times as it occurs in sequence).
                            while (sPos < s.Length)
                            {
                                if (sPos + 1 < s.Length)
                                {
                                    nextSChar = s[sPos + 1];

                                    if (nextSChar == currSChar)
                                    {
                                        prevSChar = currSChar;
                                        currSChar = nextSChar;
                                    }
                                    else
                                    {
                                        // If we get to the end of where the .* pattern successfully matches, break out of the
                                        // check to finish processing the rest of the string and pattern.
                                        break;
                                    }

                                    sPos++;
                                }
                                else
                                {
                                    // If we get to this point then we reached the end of the string.
                                    // If we do have any more pattern, the only way we can still have a 
                                    //  successful match is if all that's left in the pattern is star patterns 
                                    //  (since they can match 0 characters).
                                    return RestOfPatternIsJustStarPatterns(p, pPos);
                                }
                            }
                        }
                        else
                        {
                            // We're working with a star pattern based on a lowercase letter, so look for that letter as many times as
                            // it occurs in sequence (even zero times).
                            //
                            // The caveat is that we need to be aware of cases where we need to "save" some characters for the rest of the 
                            // pattern. For example, if our string is "bbbbb" and our pattern is "b*b", we need to "save" one of the "b"
                            // characters at the end of the string to match the last "b" in the pattern. This is because the *b is "hungry"
                            // and would "eat up" all the "b" characters, if allowed. Earlier we set the "numSequentialStarMatchesInPattern" counter 
                            // specifically for this purpose.
                            int numSequentialStarMatchesInString = 0;
                            while ((currSChar == currPChar) && (sPos < s.Length))
                            {
                                numSequentialStarMatchesInString++;

                                if (sPos + 1 < s.Length)
                                {
                                    nextSChar = s[sPos + 1];

                                    if (nextSChar == currSChar)
                                    {
                                        //numSequentialStarMatchesInString++;
                                        prevSChar = currSChar;
                                        currSChar = nextSChar;
                                    }
                                    else
                                    {
                                        // If we get to the end of where the .* pattern successfully matches, break out of the
                                        // check to finish processing the rest of the string and pattern.
                                        break;
                                    }

                                    sPos++;
                                }
                                else
                                {
                                    // If we get to this point then we reached the end of the string.
                                    //
                                    // If we do have any more pattern, the only way we can still have a 
                                    //  successful match is if all that's left in the pattern is star patterns 
                                    //  (since they can match 0 characters).
                                    //
                                    // There's one other case, and that's where we had a star pattern with more of the same type of 
                                    //  character immediately following it (e.g., "b*b"). In this case, we have to check if all the
                                    //  string characters are accounted for by the pattern.
                                    if ((numSequentialStarMatchesInPattern == 0) && (numSequentialStarMatchesInString == 0))
                                    {
                                        // This case is acceptable for a match, so keep processing
                                        return RestOfPatternIsJustStarPatterns(p, pPos);
                                    }
                                    else if ((numSequentialStarMatchesInPattern > 0) && (numSequentialStarMatchesInString == 0))
                                    {
                                        return false;
                                    }
                                    else if ((numSequentialStarMatchesInPattern == 0) && (numSequentialStarMatchesInString > 0))
                                    {
                                        return RestOfPatternIsJustStarPatterns(p, pPos);
                                    }
                                    else if ((numSequentialStarMatchesInPattern > 0) && (numSequentialStarMatchesInString > 0))
                                    {
                                        if (numSequentialStarMatchesInPattern <= numSequentialStarMatchesInString)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    prevPChar = currPChar;
                    prevSChar = currSChar;

                    if (workingWithAStarPattern)
                    {
                        // When working with a star pattern (e.g., a*), after we're done with it we want
                        // to move ahead two positions in the pattern (instead of just one) to get past both
                        // characters in the star pattern.
                        pPos++;
                        pPos++;
                    }
                    else
                    {
                        pPos++;
                    }
                    sPos++;

                    // Reset the flags for the next part of the pattern
                    starMatchChar = '\0';
                    workingWithAStarPattern = false;
                }

                // If we still have characters left over in the string for which there is no match in the
                // pattern, then we don't have a match.
                if (sPos < s.Length)
                {
                    return false;
                }
            }

            return match;
        }

        private static bool RestOfPatternIsJustStarPatterns(string p, int pPos)
        {
            if ((pPos + 1) >= p.Length)
            {
                // If we don't have any more pattern, we have a successful match.
                return true;
            }
            else
            {
                if ((p.Length - pPos) % 2 != 0)
                {
                    // If we do have more pattern, we must have an even number of 
                    // characters left (in order for it to be all star patterns)
                    return false;
                }
                else
                {
                    while (pPos < p.Length)
                    {
                        if (p[pPos + 1] == '*')
                        {
                            // We found another star pattern, so keep going.
                            pPos += 2;
                        }
                        else
                        {
                            // We found something that wasn't a star pattern, so we can't have a match.
                            return false;
                        }
                    }
                    // If we get to this point then we're out of pattern and we thus have a match.
                    return true;
                }
            }
        }

        private static void CheckConstraints(string s, string p)
        {
            //  1 <= s.length <= 20
            //  1 <= p.length <= 30
            if (
                (s.Length < 1) || (s.Length > 20)
                || (p.Length < 1) || (p.Length > 30)
                )
            {
                throw new Exception("Constraints violated");
            }

            //  s contains only lowercase English letters.
            foreach (char c in s)
            {
                if (!Char.IsLower(c))
                {
                    throw new Exception("Constraints violated");
                }
            }

            //  p contains only lowercase English letters, '.', and '*'.
            foreach (char c in p)
            {
                if (!Char.IsLower(c) && (c != '.') && (c != '*'))
                {
                    throw new Exception("Constraints violated");
                }
            }

            //  It is guaranteed for each appearance of the character '*', there will be a previous valid character to match.
            char previousChar = '\0';
            foreach (char c in p)
            {
                if ((c == '*') && !((Char.IsLower(previousChar) || (previousChar == '.'))))
                {
                    throw new Exception("Constraints violated");
                }
                previousChar = c;
            }
        }
    }
}