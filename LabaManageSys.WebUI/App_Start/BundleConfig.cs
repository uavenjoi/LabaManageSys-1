﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace LabaManageSys.WebUI.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.1.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/taskrating").Include(
                        "~/Scripts/JTaskRatingScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/journal").Include(
                        "~/Scripts/JJournalScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                        "~/Scripts/datepicker.min.js",
                        "~/Scripts/JDatePickerScript.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css", 
                        "~/Content/bootstrap.min.css", 
                        "~/Content/globalstyle.css", 
                        "~/Content/datepicker.min.css",
                        "~/Content/StarsStyle.css"));
        }
    }
}