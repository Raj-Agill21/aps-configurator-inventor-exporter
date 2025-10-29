using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Autodesk.Forge.DesignAutomation.Inventor.Utils;
using Inventor;
using Shared;

namespace ExportDWGPlugin
{
    [ComVisible(true)]
    public class ExportDWGAutomation : AutomationBase
    {
        public ExportDWGAutomation(InventorServer inventorApp) : base(inventorApp) { }

        public override void ExecWithArguments(Document doc, NameValueMap map)
        {
            LogTrace("Processing " + doc.FullFileName);

            try
            {
                ExportToDWG(doc);
            }
            catch (Exception e)
            {
                LogError("Export to DWG failed: " + e.ToString());
            }
        }

        private void ExportToDWG(Document doc)
        {
            using (new HeartBeat())
            {
                LogTrace("** Exporting to DWG");

                try
                {
                    var dwgAddin = _inventorApplication
                        .ApplicationAddIns
                        .Cast<ApplicationAddIn>()
                        .FirstOrDefault(item => item.ClassIdString == "{C24E3AC4-122E-11D5-8E91-0010B541CD80}");

                    if (dwgAddin == null)
                    {
                        LogError("DWG translator add-in not found");
                        return;
                    }

                    var translator = (TranslatorAddIn)dwgAddin;
                    LogTrace("DWG Translator add-in is available");

                    TranslationContext context = _inventorApplication
                        .TransientObjects
                        .CreateTranslationContext();
                    context.Type = IOMechanismEnum.kFileBrowseIOMechanism;

                    NameValueMap options = _inventorApplication
                        .TransientObjects
                        .CreateNameValueMap();

                    DataMedium dataMedium = _inventorApplication
                        .TransientObjects
                        .CreateDataMedium();

                    string outputPath = System.IO.Path.Combine(
                        System.IO.Directory.GetCurrentDirectory(),
                        "output.dwg"
                    );
                    dataMedium.FileName = outputPath;

                    LogTrace("DWG export options configured");

                    translator.SaveCopyAs(doc, context, options, dataMedium);
                    
                    LogTrace($"** Successfully exported to DWG: {outputPath}");

                    if (!System.IO.File.Exists(outputPath))
                    {
                        throw new System.IO.FileNotFoundException("DWG file was not created");
                    }

                    var fileInfo = new System.IO.FileInfo(outputPath);
                    LogTrace($"DWG file size: {fileInfo.Length} bytes");
                }
                catch (Exception e)
                {
                    LogError($"DWG export failed: {e.Message}");
                    throw;
                }
            }
        }
    }
}

