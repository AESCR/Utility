using System;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Common.Service;
using Common.Utility.AOP;
using Common.Utility.CustomException;
using Common.Utility.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace Ulink.Core.WebAPI.Controllers.Error
{
    /// <summary>
    ///     错误测试
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public ITestService TestService { get; set; }
        /// <summary>
        /// 错误测试
        /// </summary>
        /// <returns></returns>
        [HttpGet] 
        
        public async Task<IActionResult> ErrorTest()
        {
            try
            {
                TestService.TestEroor();
            }
            catch (Exception e)
            {
                throw new CustomException(ResponseEnum.Validation);
            }

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> OkTest()
        {
            return Ok();
        }
    }
}