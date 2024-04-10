using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using BD5.Pages;

namespace BD5.Roles
{
    public class Admin : Singleton, IRole
    {
        private Admin() { }

        public static Admin Instance => Instance<Admin>();


        public List<TabInfo> Tabs => new List<TabInfo>()
            {
                new TabInfo("Персонал", new StaffPage()),
                new TabInfo("Роли", new RolesPage()),
                new TabInfo("Доступные роли", new AvailableRolesPage())
            };

        public int ID_role => new ClubEntities().roles
                .Where(x => x.rolename == "Админ")
                .First()
                .ID_roles;

        public List<TabInfo> Authorization(string login, string password)
        {
            // ПРОВЕРКА (логин, пароль)
            var _staff = new ClubEntities().staff;
            var _availableRoles = new ClubEntities().availableroles;
            foreach (var staff in _staff)
            {
                if (staff.username == login
                    && ApplicationGlobalMethods.Instance.ComputeSHA256Hash(password) == staff.passwd)
                {
                    var admin = _availableRoles
                        .Where(x => x.staff_ID == staff.ID_staff && x.roles_ID == ID_role)
                        .FirstOrDefault();
                    if (admin != null)
                    {
                        return Tabs;
                    }
                }
            }
            return new List<TabInfo>();
        }
    }
}
