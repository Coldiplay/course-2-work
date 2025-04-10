using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bruh.VMTools
{
    public static class ExeptionHandler
    {
        public static bool Try(Action action)
        {
            bool result = false;

            try
            {
                action();
                result = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);            
            }

            return result;
        }
    }
}
