using PP_Teplokor_IPsp121.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_Teplokor_IPsp121.Helper
{
    public class DataService
    {
        private readonly MyDbContext _context;

        public DataService(MyDbContext context)
        {
            _context = context;
        }

        public List<CategoryError> GetCategoryErrors()
        {
            return _context.CategoryError.ToList();
        }

        public List<Client> GetClients()
        {
            return _context.Client.ToList();
        }

        public List<Address> GetAddressesByClientId(int clientId)
        {
            return _context.Address.Where(a => a.ClientID == clientId).ToList();
        }
    }

}
