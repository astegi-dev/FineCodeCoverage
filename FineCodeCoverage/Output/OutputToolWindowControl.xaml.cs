﻿using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using EnvDTE;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;

namespace FineCodeCoverage.Output
{
	/// <summary>
	/// Interaction logic for OutputToolWindowControl.
	/// </summary>
	public partial class OutputToolWindowControl : UserControl
	{
		private static DTE DTE;
		private static ScriptManager ScriptManager;
		private static OutputToolWindowControl Instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="OutputToolWindowControl"/> class.
		/// </summary>
		[SuppressMessage("Usage", "VSTHRD104:Offer async methods")]
		public OutputToolWindowControl()
		{
			Instance = this;
			
			InitializeComponent();

			ThreadHelper.JoinableTaskFactory.Run(async () =>
			{
				await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
				DTE = (DTE)await OutputToolWindowCommand.Instance.ServiceProvider.GetServiceAsync(typeof(DTE));
			});

			ScriptManager = new ScriptManager(DTE);
			SummaryBrowser.ObjectForScripting = ScriptManager;
			CoverageBrowser.ObjectForScripting = ScriptManager;
			RiskHotspotsBrowser.ObjectForScripting = ScriptManager;

			BtnBeer.MouseLeftButtonDown += (s, e) => System.Diagnostics.Process.Start("https://paypal.me/FortuneNgwenya");
			BtnIssue.MouseLeftButtonDown += (s, e) => System.Diagnostics.Process.Start("https://github.com/FortuneN/FineCodeCoverage/issues");
			BtnReview.MouseLeftButtonDown += (s, e) => System.Diagnostics.Process.Start("https://marketplace.visualstudio.com/items?itemName=FortuneNgwenya.FineCodeCoverage&ssr=false#review-details");
		}

		[SuppressMessage("Usage", "VSTHRD102:Implement internal logic asynchronously")]
		private static void SetUrl(WebBrowser browser, string filePath)
		{
			ThreadHelper.JoinableTaskFactory.Run(async () =>
			{
				await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

				var fileUrl = $"file://127.0.0.1/{filePath.Replace(':', '$').Replace('\\', '/')}?t={DateTime.Now.Ticks}";
				browser.Source = new Uri(fileUrl);
			});
		}

		public static void SetFilePaths(string summaryHtmlFilePath, string coverageHtmlFilePath, string riskHotspotsHtmlFilePath)
		{
			SetUrl(Instance.SummaryBrowser, summaryHtmlFilePath);
			SetUrl(Instance.CoverageBrowser, coverageHtmlFilePath);
			SetUrl(Instance.RiskHotspotsBrowser, riskHotspotsHtmlFilePath);
		}
	}

	[ComVisible(true)]
	public class ScriptManager
	{
		private readonly DTE _dte;

		public ScriptManager(DTE dte)
		{
			_dte = dte;
		}

		[SuppressMessage("Usage", "VSTHRD104:Offer async methods")]
		[SuppressMessage("Style", "IDE0060:Remove unused parameter")]
		public void OpenFile(string htmlFilePath, int file, int line)
		{
			ThreadPool.QueueUserWorkItem(state =>
			{
				// get .cs source file

				var csFilesRowXml = File.ReadAllLines(htmlFilePath).First(x => x.IndexOf("<tr><th>File(s):", StringComparison.OrdinalIgnoreCase) != -1);
				var csFilesArray = XElement.Parse(csFilesRowXml).Descendants().Where(x => x.Name.LocalName.Equals("a", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value.Trim()).ToArray();

				// load source file into IDE

				ThreadHelper.JoinableTaskFactory.Run(async () =>
				{
					await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

					_dte.MainWindow.Activate();

					foreach (var csFile in csFilesArray)
					{
						_dte.ItemOperations.OpenFile(csFile, Constants.vsViewKindCode);
						
						if (line != 0)
						{
							((TextSelection)_dte.ActiveDocument.Selection).GotoLine(line, false);
						}
					}
				});
			});
		}
	}
}