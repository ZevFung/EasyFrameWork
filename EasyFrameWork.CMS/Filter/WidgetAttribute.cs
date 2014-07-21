﻿using Easy.CMS.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Easy;
using Easy.CMS.Page;
using Easy.CMS.Layout;

namespace Easy.CMS.Filter
{
    public class WidgetAttribute : FilterAttribute, IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ZoneWidgetCollection zones = new ZoneWidgetCollection();

            //Page
            string path = filterContext.RequestContext.HttpContext.Request.Path;
            PageService pageService = new PageService();
            List<PageEntity> pages = pageService.Get(new Data.DataFilter().Where<PageEntity>(m => m.Url, Constant.DataEnumerate.OperatorType.Equal, path));
            LayoutEntity layout = null;
            if (pages.Any())
            {
                PageEntity page = pages.First();
                LayoutService layoutService = new LayoutService();
                layout = layoutService.Get(page.LayoutId);
                layout.Page = page;
                WidgetService widgetService = new WidgetService();
                List<WidgetBase> widgets = widgetService.Get(new Data.DataFilter().Where<WidgetBase>(m => m.PageId, Constant.DataEnumerate.OperatorType.Equal, page.PageId));

                widgets.ForEach(m =>
                {
                    IWidgetPartDriver partDriver = Loader.CreateInstance<IWidgetPartDriver>(m.AssemblyName, m.FullTypeName);
                    WidgetPart part = partDriver.Display(partDriver.GetWidget(m.WidgetId), filterContext.HttpContext);
                    if (zones.ContainsKey(part.ZoneId))
                    {
                        zones[part.ZoneId].Add(part);
                    }
                    else
                    {
                        WidgetCollection partCollection = new WidgetCollection();
                        partCollection.Add(part);
                        zones.Add(part.ZoneId, partCollection);
                    }
                });

                layout.ZoneWidgets = zones;
                ViewResult viewResult = (filterContext.Result as ViewResult);
                if (viewResult != null)
                {
                    //viewResult.MasterName = "~/Modules/Easy.CMS.Page/Views/Shared/_Layout.cshtml";
                }
                viewResult.ViewData[LayoutEntity.LayoutKey] = layout;
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(404);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }

}