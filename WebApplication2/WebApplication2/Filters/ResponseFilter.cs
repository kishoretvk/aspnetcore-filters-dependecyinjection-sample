using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.service;

namespace WebApplication2.Filters
{
    public class ResponseFilter : Attribute, IResourceFilter
    {
        private ISimpleRepo _isimpleRepo;

        public ResponseFilter(ISimpleRepo repo)
        {
            _isimpleRepo = repo;
            
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
          //  throw new NotImplementedException();
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.RouteData.Values.Add("token", _isimpleRepo.complexObj("welcome"));
            context.HttpContext.Items["tokens"] = _isimpleRepo.complexObj("welcome");
        }
    }
}
