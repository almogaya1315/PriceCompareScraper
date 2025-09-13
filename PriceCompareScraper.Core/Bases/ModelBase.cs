using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PriceCompareScraper.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCompareScraper.Core.Bases
{
    public abstract class ModelBase<T> : PageModel
    {
        protected readonly ILogger<T> _logger;
        protected SessionHandler _session;

        public ModelBase(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}
