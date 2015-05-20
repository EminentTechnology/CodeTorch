using System;

namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Summary description for TemplateFolderCollection.
	/// </summary>
	public class TemplateFolderCollection : System.Collections.CollectionBase
	{
		public TemplateFolderCollection() 
		{
			//
			// TODO: Add constructor logic here
			//
		}

		

		public TemplateFolder this[int index]
		{
			get
			{
				return (TemplateFolder) List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public void Add(TemplateFolder templateFolderItem)
		{
			List.Add(templateFolderItem);
		}

		public void Remove(int index)
		{
			// Check to see if there is a widget at the supplied index.
			if (index > Count - 1 || index < 0)
				// If no widget exists, a messagebox is shown and the operation 
				// is cancelled.
			{
				//System.Windows.Forms.MessageBox.Show("Index not valid!");
			}
			else
			{
				List.RemoveAt(index); 
			}
		}

	}
}
