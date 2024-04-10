using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD5.Roles
{
    public interface IRole
    {
        List<TabInfo> Tabs { get; }
        int ID_role { get; }

        public List<TabInfo> Authorization(string login, string password);
    }
}
