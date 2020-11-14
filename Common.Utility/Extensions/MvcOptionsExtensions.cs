using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Common.Utility.Extensions
{
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="opts"> </param>
        /// <param name="routeAttribute"> 自定的前缀内容 </param>
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
    }

    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        public void Apply(ApplicationModel application)
        {
            //遍历所有的 Controller
            foreach (var controller in application.Controllers)
            {
                // 已经标记了 RouteAttribute 的 Controller
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                    foreach (var selectorModel in matchedSelectors)
                        // 在 当前路由上 再 添加一个 路由前缀
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(
                            _centralPrefix,
                            selectorModel.AttributeRouteModel);

                // 在 当前路由上 不再 添加任何路由前缀
                //selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;

                // 没有标记 RouteAttribute 的 Controller
                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                    foreach (var selectorModel in unmatchedSelectors)
                        // 添加一个 路由前缀
                        //selectorModel.AttributeRouteModel = _centralPrefix;

                        // 不添加前缀(说明：不使用全局路由，重构action，实现自定义、特殊的action路由地址)
                        selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
            }
        }
    }
}