using BD5.Roles;
using System.Windows;

namespace BD5
{
    public class WindowManager : Singleton
    {
        public static WindowManager Instance => Instance<WindowManager>();

        private WindowManager() { }

        public void OpenNewWindow(Window to, Window from)
        {
            to.Closing += delegate { from.Show(); };
            to.Show();
            from.Hide();
        }
    }
}
