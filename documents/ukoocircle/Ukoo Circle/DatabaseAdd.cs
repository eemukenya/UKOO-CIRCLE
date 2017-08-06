using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ukoo_Circle
{
    class DatabaseAdd
    {
        public void AddUser(String name, String pass)
        {
            using (UkooDataContext context = new UkooDataContext(UkooDataContext.DBConnectionString))
            {
                User_details du = new User_details();
                du.user_name = name;
                du.user_pass = pass;
                context.Users.InsertOnSubmit(du);
                context.SubmitChanges();
            }
        }
    }
}
