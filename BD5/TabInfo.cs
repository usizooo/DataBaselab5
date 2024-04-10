using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BD5.Pages;

namespace BD5
{
    public struct TabInfo
    {
        public readonly string TableName;
        public readonly Page Page;
        public TabInfo(string tableName, Page page)
        {
            TableName = tableName;
            Page = page;
        }
    }
}
