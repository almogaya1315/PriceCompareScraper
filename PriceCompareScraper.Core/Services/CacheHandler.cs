using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.CodeAnalysis.CSharp;
using PriceCompareScraper.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PriceCompareScraper.Core.Services
{
    public class CacheHandler
    {
        private readonly HttpContext _context;

        public CacheHandler(HttpContext context)
        {
            _context = context;
        }

        public void Set(eCacheKeys key, string json, params eCacheType[] cacheTypes)
        {
            foreach (var type in cacheTypes)
            {
                var byteJson = Encoding.UTF8.GetBytes(json);

                switch (type)
                {
                    case eCacheType.Session:
                        SetSession(key, byteJson);
                        break;
                    case eCacheType.Cookies:
                        SetCookies(key, json);
                        break;
                    case eCacheType.All:
                        SetSession(key, byteJson);
                        SetCookies(key, json);
                        break;
                }
            }
        }
        private void SetSession(eCacheKeys key, byte[] byteJson)
        {
            _context.Session.Set(key.ToString(), byteJson);
        }
        private void SetCookies(eCacheKeys key, string json)
        {
            _context.Response.Cookies.Append(key.ToString(), json);
        }

        public bool TryGet(eCacheKeys key, out string output, params eCacheType[] cacheTypes)
        {
            var ok = false;
            output = string.Empty;

            foreach (var type in cacheTypes)
            {
                switch (type)
                {
                    case eCacheType.Session:
                        output = GetSession(ref ok, key);
                        break;
                    case eCacheType.Cookies:
                        output = GetCookies(ref ok, key);
                        break;
                }
            }
            return ok;
        }
        private string GetSession(ref bool ok, eCacheKeys key)
        {
            if (ok = _context.Session.TryGetValue(key.ToString(), out byte[] val))
                return Encoding.UTF8.GetString(val);
            else return string.Empty;
        }
        private string GetCookies(ref bool ok, eCacheKeys key)
        {
            if (ok == _context.Request.Cookies.TryGetValue(key.ToString(), out string val))
                return val;
            else return string.Empty;
        }
    }
}
