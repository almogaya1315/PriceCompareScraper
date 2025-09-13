using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCompareScraper.Core.Enums
{
    public enum eProducts
    {
        [Description("מדיח")]
        Dishwasher = 1,
        [Description("מיקרוגל")]
        Microwave = 2,
        [Description("נינגה")]
        Ninja = 3,
        [Description("תנור")]
        Oven = 4,
        [Description("מקרר")]
        Refrigirator = 5
    }
}
