﻿using TechnovertBank.Data;

namespace TechnovertBank.Models
{
    public static class SessionContext
    {
        public static Bank Bank { get; set; }
        public static Account Account { get; set; }
        public static Employee Employee { get; set; }
    }
}
