using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Autodesk.Forge.DesignAutomation.Inventor.Utils;
using Inventor;
using Shared;

namespace ExportSTEPPlugin
{
    [ComVisible(true)]
    public class ExportSTEPAutomation : AutomationBase
    {
        public ExportSTEPAutomation(InventorServer inventorApp) : base(inventorApp) { }

        public override void ExecWithArguments(Document doc, NameValueMap map)
        {
            LogTrace("Processing " + doc.FullFileName);

            try
            {
                ExportToSTEP(doc);
            }
            catch (Exception e)
            {
                LogError("Export to STEP failed: " + e.ToString());
            }
        }

        private void ExportToSTEP(Document doc)
        {
            using (new HeartBeat())
            {
                LogTrace("** Exporting to STEP");

                try
                {
                    var stepAddin = _inventorApplication
                        .ApplicationAddIns
                        .Cast<ApplicationAddIn>()
                        .FirstOrDefault(item => item.ClassIdString == "{90AF7F40-0C01-11D5-8E83-0010B541CD80}");

                    if (stepAddin == null)
                    {
                        LogError("STEP translator add-in not found");
                        return;
                    }

                    var translator = (TranslatorAddIn)stepAddin;
                    LogTrace("STEP Translator add-in is available");

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
                        "output.step"
                    );
                    dataMedium.FileName = outputPath;

                    // Configure STEP export options
                    // ApplicationProtocolType:
                    // 0 = AP203 (Configuration Controlled Design)
                    // 3 = AP214 (Automotive Design) - includes colors and layers
                    options.Value["ApplicationProtocolType"] = 3;

                    // ExportFileStructure:
                    // 0 = Export as single file
                    // 1 = Export as assembly structure
                    options.Value["ExportFileStructure"] = 0;

                    LogTrace("STEP export options configured: AP214, single file");

                    translator.SaveCopyAs(doc, context, options, dataMedium);
                    
                    LogTrace($"** Successfully exported to STEP: {outputPath}");

                    if (!System.IO.File.Exists(outputPath))
                    {
                        throw new System.IO.FileNotFoundException("STEP file was not created");
                    }

                    var fileInfo = new System.IO.FileInfo(outputPath);
                    LogTrace($"STEP file size: {fileInfo.Length} bytes");
                }
                catch (Exception e)
                {
                    LogError($"STEP export failed: {e.Message}");
                    throw;
                }
            }
        }
    }
}

