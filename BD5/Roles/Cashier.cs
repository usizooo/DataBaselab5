using System;
using System.Collections.Generic;
using System.Linq;
using BD5.Pages;

namespace BD5.Roles
{
    public class Cashier : Singleton, IRole
    {
        private Cashier() { }

        public static Cashier Instance => Instance<Cashier>();


        public List<TabInfo> Tabs => new List<TabInfo>()
            {
                new TabInfo("Заказы", new OrdersPage()),
                new TabInfo("Акции", new PromotionPage()),
                new TabInfo("Участники акции", new PromotionParticipantPage()),
                new TabInfo("Покупатели", new BuyersPage()),
                new TabInfo("Длина сессии", new SessionLengthsPage()),
            };

        public int ID_role => new ClubEntities().roles
                .Where(x => x.rolename == "Кассир")
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
                    var cashier = _availableRoles
                        .Where(x => x.staff_ID == staff.ID_staff && x.roles_ID == ID_role)
                        .FirstOrDefault();
                    if (cashier != null)
                    {
                        return Tabs;
                    }
                }
            }
            return new List<TabInfo>();
        }
    }
}
