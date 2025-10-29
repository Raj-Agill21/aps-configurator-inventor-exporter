using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Autodesk.Forge.DesignAutomation.Inventor.Utils;
using Inventor;
using Shared;

namespace ExportIGESPlugin
{
    [ComVisible(true)]
    public class ExportIGESAutomation : AutomationBase
    {
        public ExportIGESAutomation(InventorServer inventorApp) : base(inventorApp) { }

        public override void ExecWithArguments(Document doc, NameValueMap map)
        {
            LogTrace("Processing " + doc.FullFileName);

            try
            {
                ExportToIGES(doc);
            }
            catch (Exception e)
            {
                LogError("Export to IGES failed: " + e.ToString());
            }
        }

        private void ExportToIGES(Document doc)
        {
            using (new HeartBeat())
            {
                LogTrace("** Exporting to IGES");

                try
                {
                    var igesAddin = _inventorApplication
                        .ApplicationAddIns
                        .Cast<ApplicationAddIn>()
                        .FirstOrDefault(item => item.ClassIdString == "{533E9A98-FC3B-11D4-8E7E-0010B541CD80}");

                    if (igesAddin == null)
                    {
                        LogError("IGES translator add-in not found");
                        return;
                    }

                    var translator = (TranslatorAddIn)igesAddin;
                    LogTrace("IGES Translator add-in is available");

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
                        "output.iges"
                    );
                    dataMedium.FileName = outputPath;

                    // Configure IGES export options
                    // ExportFileStructure:
                    // 0 = Export as single file
                    // 1 = Export as assembly structure
                    options.Value["ExportFileStructure"] = 0;

                    LogTrace("IGES export options configured: single file");

                    translator.SaveCopyAs(doc, context, options, dataMedium);
                    
                    LogTrace($"** Successfully exported to IGES: {outputPath}");

                    if (!System.IO.File.Exists(outputPath))
                    {
                        throw new System.IO.FileNotFoundException("IGES file was not created");
                    }

                    var fileInfo = new System.IO.FileInfo(outputPath);
                    LogTrace($"IGES file size: {fileInfo.Length} bytes");
                }
                catch (Exception e)
                {
                    LogError($"IGES export failed: {e.Message}");
                    throw;
                }
            }
        }
    }
}

