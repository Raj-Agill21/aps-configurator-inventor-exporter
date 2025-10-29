using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Inventor;

namespace ExportSTLPlugin
{
    [Guid("c8d4f5e6-b7a8-49fa-cbd2-3f4a5b6c7d8e")]
    public class PluginServer : ApplicationAddInServer
    {
        private InventorServer _inventorServer;
        public dynamic Automation { get; private set; }

        public void Activate(ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            Trace.TraceInformation(": ExportSTLPlugin (" + Assembly.GetExecutingAssembly().GetName().Version.ToString(4) + "): initializing... ");
            _inventorServer = addInSiteObject.InventorServer;
            Automation = new ExportSTLAutomation(_inventorServer);
        }

        public void Deactivate()
        {
            Trace.TraceInformation(": ExportSTLPlugin: deactivating... ");
            Marshal.ReleaseComObject(_inventorServer);
            _inventorServer = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void ExecuteCommand(int CommandID)
        {
            // obsolete
        }
    }
}

