using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Inventor;

namespace ExportDWGPlugin
{
    [Guid("d7c3e4f5-b6a7-48e9-bad1-2e3f4a5b6c7d")]
    public class PluginServer : ApplicationAddInServer
    {
        private InventorServer _inventorServer;
        public dynamic Automation { get; private set; }

        public void Activate(ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            Trace.TraceInformation(": ExportDWGPlugin (" + Assembly.GetExecutingAssembly().GetName().Version.ToString(4) + "): initializing... ");
            _inventorServer = addInSiteObject.InventorServer;
            Automation = new ExportDWGAutomation(_inventorServer);
        }

        public void Deactivate()
        {
            Trace.TraceInformation(": ExportDWGPlugin: deactivating... ");
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

