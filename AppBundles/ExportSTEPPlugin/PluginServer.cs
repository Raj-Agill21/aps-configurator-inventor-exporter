using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Inventor;

namespace ExportSTEPPlugin
{
    [Guid("f5a3c1e0-8b1d-4e5f-9a2c-3d4e5f6a7b8c")]
    public class PluginServer : ApplicationAddInServer
    {
        private InventorServer _inventorServer;
        public dynamic Automation { get; private set; }

        public void Activate(ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            Trace.TraceInformation(": ExportSTEPPlugin (" + Assembly.GetExecutingAssembly().GetName().Version.ToString(4) + "): initializing... ");
            _inventorServer = addInSiteObject.InventorServer;
            Automation = new ExportSTEPAutomation(_inventorServer);
        }

        public void Deactivate()
        {
            Trace.TraceInformation(": ExportSTEPPlugin: deactivating... ");
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

