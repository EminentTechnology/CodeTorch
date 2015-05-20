using System;
using System.Configuration;
using System.IO;

namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Summary description for TemplateFolder.
	/// </summary>
	public class TemplateFolder
	{
		private TemplateFolderCollection _Folders = new TemplateFolderCollection();
		private TemplateCollection _Templates  = new TemplateCollection();

		public string FolderName = "";

		public TemplateFolder()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public TemplateFolder(string DirectoryPath)
		{
			//does folder exists
			DirectoryInfo di = new DirectoryInfo(DirectoryPath);
			string TemplateFileExtension = ConfigurationManager.AppSettings["TemplateFileExtension"];
			//string TemplateFileExtension = "cst";
			
			if(di.Exists)
			{
				FolderName = di.Name;
				// get a reference to each template in that directory
				FileInfo[] fiArr = di.GetFiles("*." + TemplateFileExtension);

				// add template to templates collection of this folder
				foreach (FileInfo fi in fiArr)
				{
					this._Templates.Add( new Template(fi));
				}

				// get a reference to each directory in that directory
				DirectoryInfo[] diArr = di.GetDirectories();

				// add directory to folders collection
				foreach (DirectoryInfo dri in diArr)
				{
					this._Folders.Add(new TemplateFolder(dri.FullName));
				}
				

				

			}


			//loop through folder for all files that are of teplate file extension type

			//populate template with file

			//get list of all folders and call this function on it recursively
		}

		

		public TemplateFolderCollection Folders
		{
			get
			{
				return _Folders;
			}
			
		}

		public TemplateCollection Templates
		{
			get
			{
				return _Templates;
			}
			
		}
	}
}
