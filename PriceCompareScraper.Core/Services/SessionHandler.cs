using Microsoft.AspNetCore.Http;
using PriceCompareScraper.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PriceCompareScraper.Core.Services
{
    public class SessionHandler
    {
        private readonly HttpContext _context;

        public SessionHandler(HttpContext context)
        {
            _context = context;
        }

        public void Set(eSessionKeys key, string json)
        {
            var byteJson = Encoding.UTF8.GetBytes(json);
            _context.Session.Set(key.ToString(), byteJson);
        }

        public bool TryGet(eSessionKeys key, out string output)
        {
            var ok = false;
            output = string.Empty;
            if (ok = _context.Session.TryGetValue(key.ToString(), out byte[] val))
                output = Encoding.UTF8.GetString(val);
            return ok;
        }
    }
}
