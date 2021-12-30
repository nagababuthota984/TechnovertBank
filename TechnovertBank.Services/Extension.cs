﻿using System;

namespace TechnovertBank.Services
{
    public static class Extension
    {
        public static bool EqualInvariant(this string inputString, string compareString)
        {
            if (inputString == null || compareString == null) return false;
            return inputString.Equals(compareString, StringComparison.OrdinalIgnoreCase);
        }
    }
}
