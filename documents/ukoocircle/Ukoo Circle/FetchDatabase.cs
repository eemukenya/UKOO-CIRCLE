using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ukoo_Circle
{
    class Users
    {
        public string id { get; set; }
        public string name { get; set; }
        public string pass { get; set; }
    }
    class FetchDatabase
    {
        public IList<User_details> GetAllUsers()
        {
            IList<User_details> list = null;
            using (UkooDataContext context = new UkooDataContext(UkooDataContext.DBConnectionString))
            {
                IQueryable<User_details> query = from c in context.Users select c;
                list = query.ToList();
            }
            return list;
        }

        public List<Users> getUsers()
        {
            IList<User_details> usrs = this.GetAllUsers();
            List<Users> allmsgs = new List<Users>();
            foreach (User_details m in usrs)
            {
                Users n = new Users();
                n.id = m.ID.ToString();
                n.name = m.user_name;
                n.pass = m.user_pass;
                allmsgs.Add(n);
            }
            return allmsgs;
        }

        public string getNumber()
        {
            IList<User_details> usrs = this.GetAllUsers();
            List<Users> allmsgs = new List<Users>();
            User_details last = usrs.Last();
            foreach (User_details m in usrs)
            {
                if (m.Equals(last))
                {
                    return m.user_name;  //return last logged in user
                }
            }
            return "false";
        }
    }
}
