using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinForms = System.Windows.Forms;

namespace FixSubtitles
{

	public partial class Root : Window
	{
		public Root() => InitializeComponent();
		private void OnFixSubtitles(object sender, RoutedEventArgs e) => FixSubtitles();

		private void FixSubtitles()
		{
			string folder_path;

			var dialog = new WinForms.FolderBrowserDialog();
			if (dialog.ShowDialog() == WinForms.DialogResult.OK)
			{
				folder_path = dialog.SelectedPath;
			}
			else
			{
				return;
			}

			var isRecursive = (bool)chkIsRecursive.IsChecked;
			var rootDir = new DirectoryInfo(folder_path);
			ProcessDir(rootDir, isRecursive);
			MessageBox.Show("Well done!");
		}

		private void ProcessDir(DirectoryInfo dir, bool isRecursive)
		{
			foreach (var file in dir.EnumerateFiles("*.srt"))
			{
				var text = File.ReadAllText(file.FullName);
				var newText = Regex.Replace(text, @"(?<=[^\r\n])\r\n\d+(?=\r\n)", string.Empty);
				File.WriteAllText(file.FullName, newText);
			}
			if (isRecursive)
			{
				var subDirs = dir.EnumerateDirectories();
				if (subDirs.Any())
				{
					foreach (var subDir in subDirs)
					{
						ProcessDir(subDir, isRecursive: true);
					}
				}
			}
		}
	}
}