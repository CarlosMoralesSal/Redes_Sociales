using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class GooglePlusAccessToken
    {
        public String access_token { get; set; }
        public String token_type { get; set; }
        public Int32 expires_in { get; set; }
        public String id_token { get; set; }
        public String refresh_token { get; set; }
    }
}
