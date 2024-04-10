using System;
using System.Collections.Generic;
using System.Linq;
using BD5.Pages;

namespace BD5.Roles
{
    public class Engineer : Singleton, IRole
    {
        private Engineer() { }

        public static Engineer Instance => Instance<Engineer>();

        public List<TabInfo> Tabs => new List<TabInfo>()
            {
                new TabInfo("Технические проблемы", new TechnicalProblemsPage()),
                new TabInfo("Тип проблемы", new TypeOfProblemPage()),
                new TabInfo("Склад", new WarehousePage()),
                new TabInfo("Починка", new FixPage()),
                new TabInfo("Места", new PlacePage()),
                new TabInfo("Девайсы", new DevicesPage()),
                new TabInfo("Комплектующие", new SpecificationsPage()),
                new TabInfo("Игры", new GamesPage()),
                new TabInfo("Доступные игры", new AvailableGamesPage())
            };

        public int ID_role => new ClubEntities().roles
                .Where(x => x.rolename == "Инжeнер")
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
                    var engineer = _availableRoles
                        .Where(x => x.staff_ID == staff.ID_staff && x.roles_ID == ID_role)
                        .FirstOrDefault();
                    if (engineer != null)
                    {
                        return Tabs;
                    }
                }
            }
            return new List<TabInfo>();
        }
    }
}
