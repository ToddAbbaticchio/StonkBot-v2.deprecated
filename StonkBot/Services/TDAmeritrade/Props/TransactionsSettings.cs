﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StonkBot.StonkBot.Services.TDAmeritrade.Props
{
    public class TransactionsSettings
    {
        #pragma warning disable IDE1006 // Naming Styles
        public long accountId { get; set; }
        public string? type { get; set; }
        public string? symbol { get; set; }
        /// <summary>
        /// Valid format: yyyy-MM-dd
        /// </summary>
        public string? startDate { get; set; }
        /// <summary>
        /// Valid format: yyyy-MM-dd
        /// </summary>
        public string? endDate { get; set; }
    }
}