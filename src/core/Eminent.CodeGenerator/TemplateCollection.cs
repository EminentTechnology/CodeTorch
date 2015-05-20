using System;

namespace Eminent.CodeGenerator
{
	/// <summary>
	/// Summary description for TemplateCollection.
	/// </summary>
	public class TemplateCollection : System.Collections.CollectionBase
	{
		public TemplateCollection() 
		{
			//
			// TODO: Add constructor logic here
			//
		}

		

		public Template this[int index]
		{
			get
			{
				return (Template) List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public void Add(Template templateItem)
		{
			List.Add(templateItem);
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
