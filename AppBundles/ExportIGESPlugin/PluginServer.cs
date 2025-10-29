using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Inventor;

namespace ExportIGESPlugin
{
    [Guid("e6b2d3c4-a5f6-47e8-b9c0-1d2e3f4a5b6c")]
    public class PluginServer : ApplicationAddInServer
    {
        private InventorServer _inventorServer;
        public dynamic Automation { get; private set; }

        public void Activate(ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            Trace.TraceInformation(": ExportIGESPlugin (" + Assembly.GetExecutingAssembly().GetName().Version.ToString(4) + "): initializing... ");
            _inventorServer = addInSiteObject.InventorServer;
            Automation = new ExportIGESAutomation(_inventorServer);
        }

        public void Deactivate()
        {
            Trace.TraceInformation(": ExportIGESPlugin: deactivating... ");
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

